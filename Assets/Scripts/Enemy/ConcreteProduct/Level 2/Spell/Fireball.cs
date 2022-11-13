using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    Timer spell_timer;
    public float Impulse = 100f;
   
    public float damage;
    void Start()
    {
        Destroy(gameObject, 2.9f);
        
        spell_timer = gameObject.AddComponent<Timer>();
        spell_timer.Duration = 6;
        spell_timer.Run();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            PlayerController.Instance.currentHealth -= damage;
            Vibration.Vibrate(50);
        }
    }
}
