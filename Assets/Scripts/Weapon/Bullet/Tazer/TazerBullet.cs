using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TazerBullet : Bullet
{
    [SerializeField]
    private GameObject lightingPrefab;
    [SerializeField]
    private float lightingDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            GameObject explosion = Instantiate<GameObject>(lightingPrefab, collision.gameObject.transform.position, Quaternion.identity);
            explosion.GetComponent<Lighting>().lightingDamage = lightingDamage;
            Helper.EnemyReceiveDamage(damage, collision);
        }
    }
}
