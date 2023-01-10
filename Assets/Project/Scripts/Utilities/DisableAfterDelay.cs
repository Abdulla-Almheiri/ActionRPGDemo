using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Utilities.GameObjects
{
    public class DisableAfterDelay : MonoBehaviour
    {
        public float Delay = 3f;
        private float _elapsedTime = 0f;
        void Start()
        {
            //Destroy(gameObject, 10f);
        }

        public void Update()
        {
            ProcessElapsedTime();
        }

        private void ProcessElapsedTime()
        {
            _elapsedTime += Time.deltaTime;
            if(_elapsedTime >= Delay)
            {
                _elapsedTime = 0f;
                gameObject.SetActive(false);
            }
        }
        public void OnEnable()
        {
            
        }
    }
}