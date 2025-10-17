using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Bullet UI")]
    public TextMeshProUGUI bullet;
    public TextMeshProUGUI bulletPack;

    [Header("Health UI")]
    public TextMeshProUGUI healthLabel;
    public Image healthBar;

    public Entity entity;

    public MenuManager menuManager;

    void Update()
    {
        if(entity.health <= 0)
        {
            menuManager.OpenGameOverScreen();
            Debug.Log("Mrreu");
        }
    }
}
