using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletPrefap;
    [SerializeField]
    private GameObject bulletPoint;
    [SerializeField]
    private float spawnDuration;
    [SerializeField]
    private float bulletDamge;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private Weapons weapons;
    [SerializeField]
    public int bulletStock;
    private float elaspedSpawnTime;
    public bool isShot;

    private void Start()
    {

    }
    // Start is called before the first frame update



    void Update()
    {
        elaspedSpawnTime += Time.deltaTime;
        if (isShot)
        {
            var closestEnemy = PlayerController.Instance.GetClosestEnemy();
            if(closestEnemy != null)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Helper.GetRotation(transform.position, closestEnemy.transform.position), 1000);
            }
            SpawnBullet();
        }
    }

    public void Rotation(int direction)
    {
        if (!isShot)
        {
            if (direction == -1)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, -0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -0f);
            }
        }
    }

    public void SpawnBullet()
    {
        if (elaspedSpawnTime > spawnDuration && bulletStock > 0)
        {
            SoundController.instance.PlaySoundWeapon(weapons);
            GameObject bullet = Instantiate<GameObject>(bulletPrefap, bulletPoint.transform.position, bulletPoint.transform.rotation);
            bullet.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
            bullet.gameObject.GetComponent<Bullet>().damage = bulletDamge;
            Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
            bullet_body.AddForce((bulletPoint.transform.position - transform.position) * bulletSpeed, ForceMode2D.Impulse);
            elaspedSpawnTime = 0f;
            bulletStock--;
        }


    }

    public void SetBulletStock(int bullets)
    {
        bulletStock += bullets;
        if (bulletStock > 999)
        {
            bulletStock = 999;
        }
    }

    public void LoadBullet(Vector3 position)
    {
        GameObject bullet = Instantiate<GameObject>(bulletPrefap, position, Quaternion.identity);
        bullet.gameObject.GetComponent<Bullet>().damage = bulletDamge;
        Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
        bullet_body.AddForce((bulletPoint.transform.position - transform.position) * bulletSpeed, ForceMode2D.Impulse);
    }

}
