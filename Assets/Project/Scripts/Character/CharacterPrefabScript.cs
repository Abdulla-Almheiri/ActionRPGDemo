using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Character
{
    public class CharacterPrefabScript : MonoBehaviour
    {
        public Material Material;
        public Transform Root;
        public Bounds Bounds;
        public Mesh Mesh;
        public GameObject Prefab;
        public SkinnedMeshRenderer SkinnedMesh;
        private SkinnedMeshRenderer _skinnedMeshRenderer;
        private Transform _root;

        private void Start()
        {
            Initialize();
       
        }

        public void Initialize()
        {
            _skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            if(_skinnedMeshRenderer == null)
            {
                return;
            }

            if(Root != null)
            {
                var spawnedRoot = Instantiate(Root, transform);
                
            }
            _skinnedMeshRenderer.bones = SkinnedMesh.bones;
            _skinnedMeshRenderer.sharedMaterial = Material;
            _skinnedMeshRenderer.sharedMesh = Mesh;
            _skinnedMeshRenderer.bounds = Prefab.GetComponent<SkinnedMeshRenderer>().localBounds;
        }
    }
}
