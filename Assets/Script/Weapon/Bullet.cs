using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    Timer bullet_timer;
    public float Impulse = 100f;
    void Start()
    {
        bullet_timer = gameObject.AddComponent<Timer>();
        bullet_timer.Duration = 6;
        bullet_timer.Run();
    }


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.tag == "Walls")
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
