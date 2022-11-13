using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;
    [SerializeField] 
    public EnemyInfo[] enemies;
    private double accumulatedWeights;
    private System.Random rand = new System.Random();
    private EnemyFactory enemyFactory;
    private GameObject[] altar;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        enemyFactory = new EnemyFactory();
        altar = GameObject.FindGameObjectsWithTag("Altar");
        if (!StartMenuController.Instance.isLoad)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].chance = enemies[i].startChance;
            }
        }
        CalculateWeights();
    }

    public void Spawn()
    {
        int randomAltar = Random.Range(0, altar.Length);
        EnemyInfo randomEnemy = enemies[GetRandomAmmoIndex()];
        switch (randomEnemy.enemy)
        {
            case Enemy.MELEE1:
                enemyFactory.CreateFactory(EnemyLevel.LEVEL1).MeleeEnemy(altar[randomAltar].transform.position);
                break;
            case Enemy.RANGE1:
                enemyFactory.CreateFactory(EnemyLevel.LEVEL1).RangeEnemy(altar[randomAltar].transform.position);
                break;
            case Enemy.MELEE2:
                enemyFactory.CreateFactory(EnemyLevel.LEVEL2).MeleeEnemy(altar[randomAltar].transform.position);
                break;
            case Enemy.RANGE2:
                enemyFactory.CreateFactory(EnemyLevel.LEVEL2).RangeEnemy(altar[randomAltar].transform.position);
                break;
            default:
                break;
        }
    }

    private int GetRandomAmmoIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < enemies.Length; i++)
            if (enemies[i]._weight >= r)
                return i;

        return GetRandomAmmoIndex();
    }

    public void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (EnemyInfo enemy in enemies)
        {
            accumulatedWeights += enemy.chance;
            enemy._weight = accumulatedWeights;
        }
    }
}


[System.Serializable]
public class EnemyInfo
{
    public Enemy enemy;
    [Range(0f, 100f)] public float startChance = 100f;
    [Range(0f, 100f)] public float finalChance = 100f;
    [Range(0f, 100f)] public float chance = 100f;
    [HideInInspector] public double _weight;
}
