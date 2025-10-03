using Assets.scripts;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, Movement
{
    [Header("Movement")]
    public Rigidbody rb;
    public Vector2 direction { get; set; }
    public Vector3 moveDirection { get; set; }
    public bool isRunning { get; set; }
    public bool isMoving;


    private void Awake()
    {
        isMoving = true;
        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            this.rb = rb;
        }
        rb.freezeRotation = true;
    }

    public void setDirection(Vector2 direction)
    {
        this.direction = direction;
    }

    public void WalkAction(Entity entity)
    {
        if (isRunning && isMoving)
        {
            moveDirection = transform.forward * direction.y + transform.right * direction.x;
            rb.AddForce(moveDirection.normalized * entity.moveSpeed * 10f, ForceMode.Force);
        }
    }

    public void speedControl(Entity entity)
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > entity.moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * entity.moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}
