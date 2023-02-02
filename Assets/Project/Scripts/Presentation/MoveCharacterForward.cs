using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Presentation
{
    public class MoveCharacterForward : MonoBehaviour
    {
        public Vector3 Speed;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            transform.Translate(Speed * Time.deltaTime);
        }
    }
}