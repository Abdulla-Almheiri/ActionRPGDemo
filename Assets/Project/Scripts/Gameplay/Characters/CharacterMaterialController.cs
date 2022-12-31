using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterMaterialController : MonoBehaviour
    {
        private Material _material;
        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.M) == true)
            {
                UpdateHighlight();
            }
        }

        public void Initialize()
        {
            _material = GetComponentInChildren<SkinnedMeshRenderer>().material;
        }

        public void UpdateHighlight()
        {
            _material.SetFloat("_HighlightAmount", 1f);
            Debug.Log(_material.name);
        }
    }
}