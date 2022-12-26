using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    public class SkillPrefabCombatController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("TRIGGER ENTERED : " + other);
            other.gameObject.GetComponent<CombatController>()?.TakeDamage(Random.Range(1f, 50f));
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("COLLISION ENTERED : " + collision);
            collision.gameObject.GetComponent<CombatController>()?.TakeDamage(Random.Range(1f, 50f));
        }

    }
}