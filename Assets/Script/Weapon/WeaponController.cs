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
    public Joystick joystick;
	
	Vector2 GameobjectRotation;
	private float GameobjectRotation2;


	void Update()
	{
		elaspedSpawnTime += Time.deltaTime;
		//Gets the input from the jostick
		if (Mathf.Abs(joystick.Horizontal) > 0.1f || Mathf.Abs(joystick.Vertical) > 0.1f)
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
            
            if (elaspedSpawnTime > spawnDuration)
            {
                SoundController.instance.PlaySoundWeapon(weapons);
                GameObject bullet = Instantiate<GameObject>(bulletPrefap, bulletPoint.transform.position, bulletPoint.transform.rotation);
				bullet.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
				bullet.gameObject.GetComponent<Bullet>().damage = bulletDamge;
				Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
				bullet_body.AddForce((bulletPoint.transform.position - transform.position) * bulletSpeed, ForceMode2D.Impulse);
				elaspedSpawnTime = 0f;
			}
			GameobjectRotation = new Vector2(joystick.Horizontal, joystick.Vertical);
			if (joystick.Horizontal > 0.02)
			{
				//Rotates the object if the player is facing right
				GameobjectRotation2 = GameobjectRotation.x + GameobjectRotation.y * 90;
				transform.rotation = Quaternion.Euler(0f, 0f, GameobjectRotation2);
			}
			else if (joystick.Horizontal < -0.02)
			{
				//Rotates the object if the player is facing left
				GameobjectRotation2 = GameobjectRotation.x + GameobjectRotation.y * -90;
				transform.rotation = Quaternion.Euler(0f, 180f, -GameobjectRotation2);
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

}
