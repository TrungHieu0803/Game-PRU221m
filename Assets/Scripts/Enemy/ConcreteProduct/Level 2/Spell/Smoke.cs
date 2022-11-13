using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    // Start is called before the first frame update
    private float spellDamage;
    void Start()
    {
        Destroy(gameObject, 1.7f);
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController.Instance.currentHealth -= spellDamage;
            Vibration.Vibrate(10);
        }
    }

    public void SetSpellDamage(float spellDamage)
    {
        this.spellDamage = spellDamage;
    }
}
