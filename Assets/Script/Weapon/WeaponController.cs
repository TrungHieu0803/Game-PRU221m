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
	public Joystick joystick;
	private float elaspedSpawnTime;
	private bool isShoot;
	
	
    private void Awake()
    {
        
    }

    private void Start()
    {
		isShoot = false;
	}
    // Start is called before the first frame update
    
	

	void Update()
	{

		
		elaspedSpawnTime += Time.deltaTime;
		//Gets the input from the jostick
		if (Mathf.Abs(joystick.Horizontal) > 0.5f || Mathf.Abs(joystick.Vertical) > 0.5f)
		{

			
            isShoot = true;
			Rotation(0);
			
		}
		else
        {
			isShoot = false;
		}
		
	}

	public void Rotation( int direction)
    {
		
        if (isShoot)
        {
			
			Vector3 moveVector = Vector3.up * joystick.Horizontal + Vector3.left * joystick.Vertical;
			if(joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
				transform.rotation = Quaternion.LookRotation(Vector3.forward, moveVector);
            }
            if (elaspedSpawnTime > spawnDuration)
            {
                SpawnBullet();
            }
           
		}
		else
        {
			if(direction == -1)
            {
				transform.rotation = Quaternion.Euler(0f, 180f, -0f);
			}
			else
            {
				transform.rotation = Quaternion.Euler(0f, 0f, -0f);
			}
        }
    }

	private void SpawnBullet()
    {
		if(bulletStock > 0)
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
		if(bulletStock > 999)
        {
			bulletStock = 999;
        }
    }
}
