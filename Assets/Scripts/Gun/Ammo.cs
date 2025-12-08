using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int ammoMax;
    public int ammoCurrent;
    public int magazine;
    public int magazineType;
    public Text ammoText;

    public GameObject bulletPrefab;
    public Gun gun;

    [SerializeField] private float bulletWeight;
    void Awake()
    {
        if (magazineType == 0)
        {
            ammoMax = 60;
        }
        else if (magazineType == 1)
        {
            ammoMax = 30;
        }
        else if (magazineType == 2)
        {
            ammoMax = 15;
        }
        else
        {
            ammoMax = 8;
        }

        ammoCurrent = ammoMax;

        if (bulletPrefab.TryGetComponent(out Rigidbody rb))
        {
            bulletWeight = rb.mass;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            gun.updateBullet();
        }

        //ammoText.text = ammoCurrent.ToString() + " / " + magazine.ToString();
    }
    void Reload()
    {
        if (magazine > 0)
        {
            ammoCurrent = ammoMax;
            magazine--;
        }
    }
}
