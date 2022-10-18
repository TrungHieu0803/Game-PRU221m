using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class PlayerController : MonoBehaviour
    {
        public float speed;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private Joystick joystick;
        private void Start()
        {
            animator = GetComponent<Animator>();
        }


        private void Update()
        {
            Vector2 dir = Vector2.zero;
            if (joystick.Horizontal < -0.5f)
            {
                dir.x = -1;

                WeaponController.instance.Rotation(-1);
                animator.SetInteger("Direction", -1);
                
            }
            else if (joystick.Horizontal > 0.5f)
            {
                dir.x = 1;
                WeaponController.instance.Rotation(1);
                animator.SetInteger("Direction", 1);
               
            }
            else
            {
                WeaponController.instance.Rotation(1);
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
        }
    }
}
