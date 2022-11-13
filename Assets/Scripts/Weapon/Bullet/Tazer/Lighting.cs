using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{

    public float lightingDamage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1f);
        StartCoroutine(SetActive());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SetActive()
    {
        yield return new WaitForSeconds(0.45f);
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            SoundController.instance.PlaySoundTazerLighting();
            Helper.EnemyReceiveDamage(lightingDamage, collision);        
        }
    }
}
