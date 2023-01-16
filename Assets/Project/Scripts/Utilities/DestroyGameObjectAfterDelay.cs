using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Utilities
{
    public class DestroyGameObjectAfterDelay : MonoBehaviour
    {
        [field:SerializeField]
        public float Delay { get; private set; } = 3f;

        void Start()
        {
            Destroy(gameObject, Delay);
        }
    }
}