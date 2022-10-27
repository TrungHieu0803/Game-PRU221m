using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public static WeaponHolder Instance;
    int totalWeapons = 1;
    private int currentWeaponIndex;
    private GameObject[] weapons;
    // Start is called before the first frame update


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        totalWeapons = gameObject.transform.childCount;
        weapons = new GameObject[totalWeapons];

        for(int i = 0; i < totalWeapons; i++)
        {
            weapons[i] = gameObject.transform.GetChild(i).gameObject;
            weapons[i].SetActive(false);
        }
        currentWeaponIndex = 0;
        weapons[currentWeaponIndex].SetActive(true);
    }

    // Update is called once per frame

    public void ChangeWeapon(int indexWeapon)
    {
        weapons[currentWeaponIndex].SetActive(false);
        weapons[indexWeapon].SetActive(true);
        currentWeaponIndex = indexWeapon;
        PlayerController.Instance.weapon = GameObject.FindGameObjectWithTag("Weapon");
    }
}
