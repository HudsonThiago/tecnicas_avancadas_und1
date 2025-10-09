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
    public Text ammoText;
    void Start()
    {
        ammoCurrent = ammoMax;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
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
