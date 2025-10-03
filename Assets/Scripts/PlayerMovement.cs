using Assets.scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour, Movement
{
    [Header("Movement")]
    public Rigidbody rb;
    public Vector2 direction { get; set; }
    public Vector2 moveDirection { get; set; }
    public Transform orientation { get; set; }
    public bool isRunning { get; set; }
    public bool isMoving;
    public bool isGrounded;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    bool readyToJump;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;


    private void Awake()
    {
        isMoving = true;
        isGrounded = true;
        if (gameObject.TryGetComponent(out Rigidbody rb))
        {
            this.rb = rb;
        }
        if (transform.Find("Orientation").TryGetComponent(out Transform orientation))
        {
            this.orientation = orientation;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
    }

    public void WalkAction(Entity entity)
    {
        if (isRunning && isMoving)
        {
            // calculate movement direction
            moveDirection = orientation.forward * direction.y + orientation.right * direction.x;
            Debug.Log(moveDirection);
            rb.AddForce(moveDirection.normalized * entity.moveSpeed * 10f, ForceMode.Force);

            //// on ground
            //if (isGrounded)
            //    rb.AddForce(moveDirection.normalized * entity.moveSpeed * 10f, ForceMode.Force);

            //// in air
            //else if (!isGrounded)
            //    rb.AddForce(moveDirection.normalized * entity.moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    public void adjustDrag()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    public void setDirection(Vector2 direction)
    {
        this.direction = direction;
    }
}
