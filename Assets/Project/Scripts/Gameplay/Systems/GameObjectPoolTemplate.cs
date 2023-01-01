using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Systems
{
    [CreateAssetMenu(fileName = "new gameObject pool template", menuName = "Content/Systems/GameObject Pool Template")]
    public class GameObjectPoolTemplate : ScriptableObject
    {
        public GameObjectPoolController PoolablePrefab;
        public bool HideInactiveInHierarchy = false;
        public bool HideActiveInHierarchy = false;
        public bool PreAllocateObjects = false;
        public bool SetActiveAfterAllocation = false;
        public bool RequiresCanvas = false;
        public int MaxSize = 12;

    }
}