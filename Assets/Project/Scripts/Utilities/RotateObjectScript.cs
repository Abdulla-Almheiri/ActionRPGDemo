using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Utilities
{
    public class RotateObjectScript : MonoBehaviour
    {
        public Vector3 Rotation;
        void Update()
        {
            transform.Rotate( Rotation * Time.deltaTime);
        }
    }
}