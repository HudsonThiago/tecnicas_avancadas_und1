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

    [Header("Point UI")]
    public TextMeshProUGUI playerPoint;

    [Header("Others")]
    public Entity entity;
    public MenuManager menuManager;
    public Timer timer;


    void Update()
    {
        if (entity.health <= 0 || !timer.isRunning)
        {
            menuManager.OpenGameOverScreen();
        }
    }
}
