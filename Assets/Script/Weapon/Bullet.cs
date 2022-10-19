using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    Timer bullet_timer;
    public float Impulse = 100f;
    private string layerName;
    void Start()
    {
        layerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
        bullet_timer = gameObject.AddComponent<Timer>();
        bullet_timer.Duration = 6;
        bullet_timer.Run();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (layerName == "Layer 1" && collision.gameObject.tag == "Walls")
        {
            Destroy(gameObject);
        } 
        else if (layerName == "Layer 2" && collision.gameObject.tag == "Walls2")
        {
            Destroy(gameObject);
        }
        else if (layerName == "Layer 3" && collision.gameObject.tag == "Walls3")
        {
            Destroy(gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
