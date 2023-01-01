using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.Skills
{
    public class SkillPrefabCombatController : MonoBehaviour
    {
        public GameUIController GameUIControllerTest;
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
            other.gameObject.GetComponent<CharacterCombatController>()?.TakeDamage(Random.Range(1f, 50f));
            GameUIControllerTest.SpawnDamageTextAtScreenPositionTest(Random.Range(1f, 50f), UnityEngine.Camera.main.WorldToScreenPoint(other.gameObject.transform.position));
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("COLLISION ENTERED : " + collision);
            collision.gameObject.GetComponent<CharacterCombatController>()?.TakeDamage(Random.Range(1f, 50f));
        }*/

    }
}