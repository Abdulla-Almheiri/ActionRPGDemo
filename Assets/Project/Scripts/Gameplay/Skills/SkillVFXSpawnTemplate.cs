using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.Skills
{
    [CreateAssetMenu(fileName = "new skillVFX spawn", menuName = "Content/Skills/SkillVFX Spawn Template")]
    public class SkillVFXSpawnTemplate : ScriptableObject
    {
        public CharacterVFXSpawnLocationType VFXSpawnLocationType;
        public SkillVFXPrefabScript SkillVFXPrefab;

    }
}