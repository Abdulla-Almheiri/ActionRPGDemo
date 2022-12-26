using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterVFXController : MonoBehaviour
    {
        public List<CharacterVFXLocationEntry> VFXSpawnPoints;

        public Transform GetVFXSpawnTransform(CharacterVFXSpawnLocationType characterVFXSpawnType)
        {
            return VFXSpawnPoints.Find(x => x.CharacterVFXSpawnLocationType == characterVFXSpawnType).GameObjectSpawnPoint;
        }
    }

    [System.Serializable]
    public class CharacterVFXLocationEntry
    {
        public CharacterVFXSpawnLocationType CharacterVFXSpawnLocationType;
        public Transform GameObjectSpawnPoint;
        //public float ScaleMultiplier = 1f;
    }
}