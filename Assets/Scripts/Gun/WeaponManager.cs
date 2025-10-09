using System.Diagnostics;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int selectedWeapon = 0;
    public int selecterWeaponBackup;
    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            selectedWeapon = 3;
        }

        if (selectedWeapon != selecterWeaponBackup)
        {
            SelectWeapon();
            selecterWeaponBackup = selectedWeapon;
        }
        
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform gun in transform)
        {
            if (i == selectedWeapon)
            {
                gun.gameObject.SetActive(true);
            }
            else
            {
                gun.gameObject.SetActive(false);
            }
            i++;
        }
    }
}