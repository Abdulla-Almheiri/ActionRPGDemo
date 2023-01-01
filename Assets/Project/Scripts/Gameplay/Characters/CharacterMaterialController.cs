using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Characters
{
    public class CharacterMaterialController : MonoBehaviour
    {
        private Material _material;
        private List<Material> _materials = new List<Material>();
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
                UpdateHighlight(1f);
            }

            if (Input.GetKeyUp(KeyCode.N) == true)
            {
                UpdateHighlight(0f);
            }
        }

        public void Initialize()
        {
            _material = GetComponentInChildren<SkinnedMeshRenderer>().material;
            SkinnedMeshRenderer[] renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach(SkinnedMeshRenderer renderer in renderers)
            {
                _materials.Add(renderer.material);
            }
        }

        public void UpdateHighlight(float value)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            _material.SetFloat("_HighlightAmount", value);
            foreach(Material mat in _materials)
            {
                mat.SetFloat("_HighlightAmount", value);
            }

            Debug.Log(_material.name);
        }

        public void OnMouseEnter()
        {
            UpdateHighlight(1f);
        }

        public void OnMouseExit()
        {
            UpdateHighlight(0f);
        }
    }
}