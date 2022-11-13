using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Item
{
    public GameObject Prefab;
    [Range(0f, 100f)] public float chance = 100f;
    public double _weight;
}
public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;
    [SerializeField]
    public Item[] items;
    private double accumulatedWeights;
    private System.Random rand = new System.Random();
    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        CalculateWeights();
    }

    public void SpawnRandomItem(Vector2 position)
    {
        var randomIndex = GetRandomItemIndex();
        
        if(randomIndex != 1)
        {
            Item randomItem = items[randomIndex];
            Instantiate<GameObject>(randomItem.Prefab, position, Quaternion.identity, transform);
        }
        
    }

    public void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (Item a in items)
        {
            accumulatedWeights += a.chance;
            a._weight = accumulatedWeights;
        }
    }

    private int GetRandomItemIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;
        for (int i = 0; i < items.Length; i++)
            if (items[i]._weight >= r)
                return i;

        return 1;
    }

    public void SpawnItemWithIndex(Vector2 position, int itemIndex)
    {
        Item randomAmmo = items[itemIndex];
        Instantiate<GameObject>(randomAmmo.Prefab, position, Quaternion.identity, transform);
    }
}
