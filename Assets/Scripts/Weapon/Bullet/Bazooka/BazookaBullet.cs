using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaBullet : Bullet
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float explosionDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            GameObject explosion =  Instantiate<GameObject>(explosionPrefab, transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().explosionDamage = explosionDamage;
            Helper.EnemyReceiveDamage(damage, collision);
        }
    }
}
