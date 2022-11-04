using UnityEngine;

[System.Serializable]
public class Ammo
{
    public int weaponIndex;
    public GameObject Prefab;
    [Range(0f, 100f)] public float startChance = 100f;
    [Range(0f, 100f)] public float finalChance = 100f;
    [Range(0f, 100f)] public float chance = 100f;
    [HideInInspector] public double _weight;
}


public class AmmoSpawner : MonoBehaviour
{
    public static AmmoSpawner Instance;
    [SerializeField] 
    public Ammo[] ammoes;
    private double accumulatedWeights;
    private System.Random rand = new System.Random();


    private void Awake()
    {
        Instance = this;
        
    }

    private void Start()
    {
        for (int i = 0; i < ammoes.Length; i++)
        {
            ammoes[i].chance = ammoes[i].startChance;
        }
        
    }


    public void SpawnRandomAmmo(Vector2 position)
    {
        CalculateWeights();
        Ammo randomAmmo = ammoes[GetRandomAmmoIndex()];
        
       if(randomAmmo.weaponIndex != -1)
        {
            GameObject ammo =  Instantiate<GameObject>(randomAmmo.Prefab, position, Quaternion.identity, transform);
            ammo.GetComponent<AmmoController>().weaponIndex = randomAmmo.weaponIndex;
        }
       
    }

    private int GetRandomAmmoIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < ammoes.Length; i++)
            if (ammoes[i]._weight >= r)
                return i;

        return 0;
    }

    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (Ammo a in ammoes)
        {
            accumulatedWeights += a.chance;
            a._weight = accumulatedWeights;
        }
    }
}