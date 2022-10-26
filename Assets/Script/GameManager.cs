using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] altar;
    private float elapsedSpawnTime;
    private EnemyFactory enemyFactory;
    void Start()
    {
        enemyFactory = new EnemyFactory();
        elapsedSpawnTime = 0;
        altar = GameObject.FindGameObjectsWithTag("Altar");

    }

    // Update is called once per frame
    void Update()
    {
        elapsedSpawnTime += Time.deltaTime;
        if(elapsedSpawnTime >= 3)
        {
            int randomAltar = Random.Range(0, altar.Length);
            
            enemyFactory.CreateFactory(EnemyLevel.LEVEL1).MeleeEnemy(altar[randomAltar].transform.position);
            enemyFactory.CreateFactory(EnemyLevel.LEVEL1).RangeEnemy(altar[randomAltar].transform.position);    
            elapsedSpawnTime = 0f;
        }
    }
}
