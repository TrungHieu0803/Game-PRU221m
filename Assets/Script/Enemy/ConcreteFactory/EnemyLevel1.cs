using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyLevel1 : MonoBehaviour, IEnemyFactory
{
    public static EnemyLevel1 Instance { get; private set; }
    [SerializeField]
    private GameObject meleeEnemy;
    [SerializeField]
    private GameObject rangeEnemy;
    private ObjectPool<GameObject> poolMelee;
    private ObjectPool<GameObject> poolRange;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        poolMelee = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(meleeEnemy);
        }, enemy =>
        {
            enemy.SetActive(true);
        }, enemy =>
        {
            enemy.SetActive(false);
        }, enemy =>
        {
            Destroy(enemy);
        }, false, 10, 20
        );

        poolRange = new ObjectPool<GameObject>(() =>
        {
            return Instantiate(rangeEnemy);
        }, enemy =>
        {
            enemy.SetActive(true);
        }, enemy =>
        {
            enemy.SetActive(false);
        }, enemy =>
        {
            Destroy(enemy);
        }, false, 10, 20
);
    }



    public GameObject MeleeEnemy(Vector3 position)
    {
        var enemy = poolMelee.Get();
        enemy.transform.position = position;
        return enemy;
    }

    public GameObject RangeEnemy(Vector3 position)
    {
        var enemy = poolRange.Get();
        enemy.transform.position = position;
        return enemy;
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
