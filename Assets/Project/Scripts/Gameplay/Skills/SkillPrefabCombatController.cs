using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using Chaos.Gameplay.Systems;
using Chaos.Gameplay.Player;

namespace Chaos.Gameplay.Skills
{
    public class SkillPrefabCombatController : MonoBehaviour
    {
        public SkillActionElement ElementTest;
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
            //Debug.Log("TRIGGER ENTERED : " + other);
            if(other.gameObject.GetComponent<PlayerController>() == true)
            { 
                return;
            }
            other.gameObject.GetComponent<CharacterCombatController>()?.TakeDamage(Random.Range(1, 50));
            other.gameObject.GetComponent<CharacterCombatController>()?.TriggerHitByElement(ElementTest);
            GameUIControllerTest.SpawnDamageTextAtScreenPositionTest(Random.Range(1, 50).ToString(), other.gameObject.transform);
        }

        /*private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("COLLISION ENTERED : " + collision);
            collision.gameObject.GetComponent<CharacterCombatController>()?.TakeDamage(Random.Range(1f, 50f));
        }*/

    }
}