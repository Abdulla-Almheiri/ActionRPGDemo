using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Utilities.GameObjects
{
    public class DestroyAfterDelay : MonoBehaviour
    {
        public float Delay = 3f;
        void Start()
        {
            Destroy(gameObject, Mathf.Max(0f, Delay));
        }

    }
}