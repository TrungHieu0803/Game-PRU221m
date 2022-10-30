using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEnemyRange1 : MonoBehaviour
{
    private bool isHit;
    
    void Start()
    {
        isHit = false;
        StartCoroutine(Active());
        Destroy(gameObject, 4.4f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Active()
    {
        yield return new WaitForSeconds(3.3f);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !isHit )
        {
            
            isHit = true;
        }
    }
}
