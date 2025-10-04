using UnityEngine;
using TMPro;

public class Shotgun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Range(0f, 0.50f)]
    public float backspinDrag;
    public float joules = 1.49f;

    public int shotgunBullets;

    //[Range(0f, 0.50f)]
    public float spreadAngle;

    public int ammunitionMax;
    private int ammunition;
    public GameObject noAmmoLabel;

    public float shootCooldown;
    private float nextShootTime;

    void Start()
    {
        ammunition = ammunitionMax;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        // respeita o cooldown
            if (Time.time >= nextShootTime)
            {
                if (ammunition > 0)
                {
                    shoot();
                    ammunition--;
                    nextShootTime = Time.time + shootCooldown;
                }
                else
                {
                    ShowAmmoMessage();
                }
            }
        }    

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }


    void shoot()
    {
        Camera camera = Camera.main;
        Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = camera.ScreenPointToRay(center);

        Vector3 direction;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, ~0, QueryTriggerInteraction.Ignore))
            direction = (hit.point - bulletSpawn.position).normalized;
        else
            direction = ray.direction.normalized;

        for (int i = 0; i < shotgunBullets; i++)
        {

            float yaw = Random.Range(-spreadAngle, spreadAngle);
            float pitch = Random.Range(-spreadAngle, spreadAngle);

            Vector3 dir =
                Quaternion.AngleAxis(yaw, camera.transform.up) *
                Quaternion.AngleAxis(pitch, camera.transform.right) *
                direction;

            GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.LookRotation(dir));

            if (bulletObj.TryGetComponent<Rigidbody>(out var rb))
            {
                float speed = joules > 0f ? Mathf.Sqrt((2f * joules) / rb.mass) : 60f;
                rb.linearVelocity = dir * speed;
            }

            Destroy(bulletObj, 10f);
        }
    }

    public void ShowAmmoMessage()
    {
        if (!noAmmoLabel) return;
        noAmmoLabel.SetActive(true);
        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), 2f);
    }

    void Hide() => noAmmoLabel.SetActive(false);

    void Reload()
    {
        ammunition = ammunitionMax;
    }

}
