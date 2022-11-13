using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealKitController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(PlayerController.Instance.currentHealth != 100)
            {
                if(PlayerController.Instance.currentHealth + 10 > 100)
                {
                    PlayerController.Instance.currentHealth = 100;
                }
                else
                {
                    PlayerController.Instance.currentHealth += 10;
                }
            }
            Destroy(gameObject);
        }
    }
}
