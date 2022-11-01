using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    // Start is called before the first frame update
    public int weaponIndex;
    [SerializeField]
    private int bullets;
    void Start()
    {
        Destroy(gameObject, 8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            WeaponHolder.Instance.PickAmmo(weaponIndex, bullets);
        }
    }
}
