using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory
{
    public IEnemyFactory CreateFactory(EnemyLevel level)
    {
        switch (level)
        {
            case EnemyLevel.LEVEL1:
                return EnemyLevel1.Instance;
            default:
                return null;
        }
    }
}
