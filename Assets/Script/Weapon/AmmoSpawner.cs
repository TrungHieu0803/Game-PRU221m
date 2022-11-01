using UnityEngine;

[System.Serializable]
public class Ammo
{
    public int weaponIndex;
    // for debug :
    public GameObject Prefab;
    [Range(0f, 100f)] public float Chance = 100f;

    [HideInInspector] public double _weight;
}


public class AmmoSpawner : MonoBehaviour
{
    public static AmmoSpawner Instance;
    [SerializeField] private Ammo[] ammo;

    private double accumulatedWeights;
    private System.Random rand = new System.Random();


    private void Awake()
    {
        Instance = this;
        CalculateWeights();
    }


    public void SpawnRandomAmmo(Vector2 position)
    {
        Ammo randomAmmo = ammo[GetRandomAmmoIndex()];
        
       if(randomAmmo.weaponIndex != -1)
        {
            GameObject ammo =  Instantiate<GameObject>(randomAmmo.Prefab, position, Quaternion.identity, transform);
            ammo.GetComponent<AmmoController>().weaponIndex = randomAmmo.weaponIndex;
        }
       
    }

    private int GetRandomAmmoIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for (int i = 0; i < ammo.Length; i++)
            if (ammo[i]._weight >= r)
                return i;

        return 0;
    }

    private void CalculateWeights()
    {
        accumulatedWeights = 0f;
        foreach (Ammo a in ammo)
        {
            accumulatedWeights += a.Chance;
            a._weight = accumulatedWeights;
        }
    }
}