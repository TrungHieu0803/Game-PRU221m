using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	public static WeaponController instance;
	[SerializeField]
	GameObject bulletPrefap;
	GameObject bulletPoint;
	public bool isShoot;
	private float elaspedSpawnTime;
    private void Awake()
    {
		instance = this;
    }

    private void Start()
    {
		isShoot = false;
		bulletPoint = GameObject.FindGameObjectWithTag("Gun");
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
			if(elaspedSpawnTime > 0.5f)
            {
				GameObject bullet = Instantiate<GameObject>(bulletPrefap, bulletPoint.transform.position, bulletPoint.transform.rotation);
				bullet.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
				Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
				bullet_body.AddForce((bulletPoint.transform.position - transform.position) * 5, ForceMode2D.Impulse);
				elaspedSpawnTime = 0f;
			}
			GameobjectRotation = new Vector2(joystick.Horizontal, joystick.Vertical);
			if (joystick.Horizontal > 0.01)
			{
				//Rotates the object if the player is facing right
				GameobjectRotation2 = GameobjectRotation.x + GameobjectRotation.y * 90;
				transform.rotation = Quaternion.Euler(0f, 0f, GameobjectRotation2);
			}
			else if (joystick.Horizontal < -0.01)
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
