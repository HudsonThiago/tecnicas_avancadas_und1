using System.Runtime;
using NUnit.Framework;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float backspinDrag;
    public float joules = 1000;

    public float shootCooldown;
    private float nextShootTime;

    //------------------Shotgun Shoot-------------------
    public int shotgunBullets;
    public float spreadAngle;

    public GameObject noAmmoLabel;
    [SerializeField] Ammo Ammo;

    public bool isShootgun = false;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(Time.time >= nextShootTime)
            {
                if (Ammo.ammoCurrent > 0)
                {
                    if (isShootgun == true)
                    {
                        shootShotgun();
                    }
                    else
                    {
                        shoot();
                    }
                }
                else
                {
                    ShowAmmoMessage();
                }
            }
        }
    }


    void shoot()
    {
        // Ray a partir da crosshair (imagem no centro da tela)
        Camera camera = Camera.main;
        Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = camera.ScreenPointToRay(center);

        // Direção do cano até o ponto mirado (ou “reto” se não acertar nada)
        Vector3 direction;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, ~0, QueryTriggerInteraction.Ignore))
            direction = (hit.point - bulletSpawn.position).normalized;
        else
            direction = ray.direction.normalized;

        GameObject bulletObj = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.LookRotation(direction));

        if (bulletObj.TryGetComponent<Rigidbody>(out var rb))
        {
            float speed = joules > 0f ? Mathf.Sqrt((2f * joules) / rb.mass) : 60f;
            rb.linearVelocity = direction * speed;

            float magnusForce = Mathf.Sqrt(speed) * backspinDrag;
            rb.AddForce(Vector3.up * magnusForce, ForceMode.Impulse);
        }

        Ammo.ammoCurrent--;
        nextShootTime = Time.time + shootCooldown;
        Destroy(bulletObj, 10f);
    }

    void shootShotgun()
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

            Ammo.ammoCurrent--;
            nextShootTime = Time.time + shootCooldown;
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
}