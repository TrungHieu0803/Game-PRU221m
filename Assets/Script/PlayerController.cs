using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public float speed;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private Image healthBarSprite;
    public GameObject weapon;
    private float maxHealth;
    public float currentHealth;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        weapon = GameObject.FindGameObjectWithTag("Weapon");
    }


    private void Update()
    {
        Vector2 dir = Vector2.zero;
        if (joystick.Horizontal < -0.5f)
        {
            dir.x = -1;

            weapon.GetComponent<WeaponController>().Rotation(-1);
        //    WeaponController.instance.Rotation(-1);
            animator.SetInteger("Direction", -1);

        }
        else if (joystick.Horizontal > 0.5f)
        {
            dir.x = 1;
            weapon.GetComponent<WeaponController>().Rotation(1);
            // WeaponController.instance.Rotation(1);
            animator.SetInteger("Direction", 1);

        }
        else
        {
            weapon.GetComponent<WeaponController>().Rotation(1);
            //  WeaponController.instance.Rotation(1);
            animator.SetInteger("Direction", 0);
        }

        if (joystick.Vertical > 0.5f)
        {
            dir.y = 1;

        }
        else if (joystick.Vertical < -0.5f)
        {
            dir.y = -1;

        }

        dir.Normalize();
        GetComponent<Rigidbody2D>().velocity = speed * dir;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBarSprite.fillAmount = currentHealth / maxHealth;
    }
}
