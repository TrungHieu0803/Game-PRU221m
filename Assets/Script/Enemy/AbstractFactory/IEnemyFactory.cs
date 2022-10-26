using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyFactory
{
    public GameObject RangeEnemy(Vector3 position);

    public GameObject MeleeEnemy(Vector3 position);
}
