using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    //let camera follow target
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float rightLimit;
        [SerializeField]
        private float leftLimit;
        [SerializeField]
        private float upLimit;
        [SerializeField]
        private float downLimit;


        private void Update()
        {
            if (target == null) return;
            transform.position = new Vector3(Mathf.Clamp(target.position.x, leftLimit, rightLimit), Mathf.Clamp(target.position.y, downLimit, upLimit), transform.position.z);
        
        }

    }
}
