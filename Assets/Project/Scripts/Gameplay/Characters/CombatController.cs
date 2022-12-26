using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Skills;

namespace Chaos.Gameplay.Characters
{

    public class CombatController : MonoBehaviour
    {
        public void TakeDamage(float value)
        {
            Debug.Log("Damage taken:   " + value);
        }

       public void ApplySkillAction(SkillAction skillAction)
        {

        }
    }
}