using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Skills
{
    public class SkillExpansionController : MonoBehaviour
    {
        public Vector3 ExpansionVelocity;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            ProcessExpansion();
        }

        private void ProcessExpansion()
        {
            transform.localScale += ExpansionVelocity * Time.deltaTime;
        }
    }
}