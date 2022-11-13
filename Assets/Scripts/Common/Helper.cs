using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static Vector2 getVector2DByDegree(float degree)
    {
        float radian = degree * Mathf.Deg2Rad;
        float sinD = Mathf.Sin(radian);
        float cosD = Mathf.Cos(radian);
        float x = sinD;
        float y = cosD;
        return new Vector2(x, y);
    }

    public static Quaternion GetRotation(Vector3 from, Vector3 to)
    {
        Vector2 direction = to - from;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotaiton = Quaternion.AngleAxis(angle, Vector3.forward);
        return rotaiton;
    }

    public static void EnemyReceiveDamage(float damage, Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Melee melee))
        {
            melee.ReceiveDamage(damage);
        }
        else if (collision.gameObject.TryGetComponent(out Range range))
        {
            range.ReceiveDamage(damage);
        }
    }
}
