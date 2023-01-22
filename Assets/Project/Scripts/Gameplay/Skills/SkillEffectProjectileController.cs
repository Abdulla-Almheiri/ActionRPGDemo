using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    public class SkillEffectProjectileController : MonoBehaviour
    {
        public Vector3 Velocity;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            ProcessProjectileMovement();
        }

        private void ProcessProjectileMovement()
        {
            var delta = Velocity * Time.deltaTime;
            transform.Translate(delta);
        }
    }
}