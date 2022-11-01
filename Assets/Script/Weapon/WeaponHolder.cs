using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponHolder : MonoBehaviour
{
    public static WeaponHolder Instance;
    int totalWeapons = 1;
    private int currentWeaponIndex;
    private GameObject[] weapons;
    [SerializeField]
    private TextMeshProUGUI textAmmo;
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

    private void Update()
    {
        textAmmo.text = weapons[currentWeaponIndex].GetComponent<WeaponController>().bulletStock.ToString();
    }

    // Update is called once per frame

    public void ChangeWeapon(int indexWeapon)
    {
        weapons[currentWeaponIndex].SetActive(false);
        weapons[indexWeapon].SetActive(true);
        currentWeaponIndex = indexWeapon;
        PlayerController.Instance.weapon = weapons[indexWeapon];
        textAmmo.text = weapons[currentWeaponIndex].GetComponent<WeaponController>().bulletStock.ToString();
    }

    public GameObject GetWeapon(int indexWeapon)
    {
        return weapons[indexWeapon];
    }

    public void PickAmmo(int indexWeapon, int bullets)
    {
        weapons[indexWeapon].GetComponent<WeaponController>().SetBulletStock(bullets);
    }
}
