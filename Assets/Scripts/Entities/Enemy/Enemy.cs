using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public Entity entity;
    public float stopDistance = 1.5f;      // Dist�ncia m�nima para parar
    public GameObject projectilePrefab;    // Prefab do proj�til
    public Transform shootPoint;           // Ponto de onde o tiro sai
    public float projectileSpeed = 10f;    // Velocidade do proj�til
    public float projectileDamage = 10f;   // Dano causado ao player

    private Transform player;
    private Rigidbody rb;
    private bool isChanneling = false;     // Se est� canalizando o tiro
    private bool canShoot = true;          // Controle do tempo de recarga

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Encontra o jogador pela tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // Refer�ncia ao componente Entity (caso exista)
        if (TryGetComponent(out Entity entity))
            this.entity = entity;
    }

    void FixedUpdate()
    {
        if (!isChanneling)
            Movement();

        LookAtPlayer();
    }

    private void Movement()
    {
        if (player == null)
            return;

        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector3 movePosition = transform.position + direction * entity.moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(movePosition);
        }
        else if (canShoot && !isChanneling)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    private void LookAtPlayer()
    {
        if (player == null)
            return;

        Vector3 lookDir = (player.position - transform.position);
        lookDir.y = 0;

        if (lookDir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, 10f * Time.fixedDeltaTime));
        }
    }

    private IEnumerator ShootRoutine()
    {
        isChanneling = true;
        canShoot = false;

        // Canaliza o tiro por 1s
        yield return new WaitForSeconds(1f);

        // Atira o proj�til
        Shoot();

        // Aguarda 2s antes de poder atirar novamente
        yield return new WaitForSeconds(2f);

        isChanneling = false;
        canShoot = true;
    }

    private void Shoot()
    {
        if (projectilePrefab == null || player == null)
        {
            Debug.LogWarning("Enemy: Sem proj�til ou player n�o encontrado!");
            return;
        }

        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position + transform.forward * 1f;

        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector3 dir = ((player.position+Vector3.up * 1f) - spawnPos).normalized;

        if (proj.TryGetComponent(out Rigidbody projRb))
        {
            projRb.linearVelocity = dir * projectileSpeed;
        }

        proj.transform.rotation = Quaternion.LookRotation(dir);

        // Envia o dano para o proj�til
        if (proj.TryGetComponent(out EnemyProjectile projectile))
        {
            projectile.damage = projectileDamage;
        }

        Destroy(proj, 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        int segments = 40;
        float angleStep = 360f / segments;
        Vector3 prevPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle) * stopDistance;
            float z = Mathf.Cos(angle) * stopDistance;

            Vector3 nextPoint = transform.position + new Vector3(x, 0.01f, z);

            if (i > 0)
                Gizmos.DrawLine(prevPoint, nextPoint);
            else
                firstPoint = nextPoint;

            prevPoint = nextPoint;
        }

        Gizmos.DrawLine(prevPoint, firstPoint);
    }
}