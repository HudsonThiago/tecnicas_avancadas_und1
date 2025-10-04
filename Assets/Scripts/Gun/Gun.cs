using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    [Range(0f, 0.50f)]
    public float backspinDrag;
    public float joules = 1.49f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = botão esquerdo
        {
            shoot();
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

    Destroy(bulletObj, 10f);
}


    /*public void shoot(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            GameObject bb = Instantiate(bullet, bulletSpawn.position, Quaternion.identity, transform);
            if (bb.TryGetComponent(out Rigidbody rigidbody))
            {
                float speed = Mathf.Sqrt((2f * joules) / rigidbody.mass);

                rigidbody.linearVelocity = Vector3.right * speed;

                float magnusForce = Mathf.Sqrt(speed) * backspinDrag;
                rigidbody.AddForce(Vector3.up * magnusForce, ForceMode.Impulse);
            }
        }
    }*/


}
