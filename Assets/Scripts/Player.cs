using Assets.scripts;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public Entity entity;
    public PlayerMovement playerMovement;

    private void Awake()
    {
        if (TryGetComponent(out PlayerMovement playerMovement))
        {
            this.playerMovement = playerMovement;
        }
        if (TryGetComponent(out Entity entity))
        {
            this.entity = entity;
        }
    }

    private void Update()
    {
        playerMovement.adjustDrag();
    }

    private void FixedUpdate()
    {
        playerMovement.WalkAction(entity);
    }

}
