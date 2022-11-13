using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEnemyRange1 : MonoBehaviour
{
    private bool isHit;
    public float damage;
    
    void Start()
    {
        isHit = false;
        StartCoroutine(Active());
        Destroy(gameObject, 3.13f);
        
    }

    // Update is called once per frame
    void Update()
    {

       
    }

    private IEnumerator Active()
    {
        yield return new WaitForSeconds(2.3f);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isHit )
        {
            Vibration.Vibrate(50);
            PlayerController.Instance.currentHealth -= damage;
            isHit = true;
            
        }
    }
}
