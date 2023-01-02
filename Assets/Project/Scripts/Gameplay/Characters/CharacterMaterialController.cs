using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Systems;
using Chaos.Gameplay.VFX;
using Chaos.Gameplay.Skills;

namespace Chaos.Gameplay.Characters
{
    public class CharacterMaterialController : MonoBehaviour
    {
        public GameMaterialController GameMaterialController;
        private ObjectsMaterialProfile _objectsMaterialProfile;
        private Color _originalColor;
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
                var curve = _objectsMaterialProfile.MaterialHighlightColorsTriggeredbyElements[0].HighlightPowerCurve;
                var color = _objectsMaterialProfile.MaterialHighlightColorsTriggeredbyElements[0].MaterialHighlightColor;
                AnimateHighlightByCurve(curve, color, 1f);
            }
        }

        public void Initialize()
        {
            _objectsMaterialProfile = GameMaterialController.MaterialProfile;
            _material = GetComponentInChildren<Renderer>().material;
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach(Renderer renderer in renderers)
            {
                _materials.Add(renderer.material);
            }
        }

        public void TriggerHighlightOnMouseHover(float value, Color color)
        {
            value = Mathf.Clamp(value, 0f, 1f);
            _material.SetFloat("_HighlightAmount", value);
            foreach(Material mat in _materials)
            {
                mat.SetFloat("_HighlightAmount", value);
                mat.SetColor("_HighlightColor", color);
            }

           // Debug.Log(_material.name);
        }
        public void TriggerHitEffectBySkillActionElement(SkillActionElement skillActionElement)
        {
            
            var highlightData = _objectsMaterialProfile.MaterialHighlightColorsTriggeredbyElements.Find(x => x.Element == skillActionElement);
            if(highlightData == null)
            {
                
                return;
            }

            var curve = highlightData.HighlightPowerCurve;
            if(curve == null)
            {
                
                return;
            }

            var color = highlightData.MaterialHighlightColor;


            AnimateHighlightByCurve(curve, color, 1f);
            

        }
        public void OnMouseEnter()
        {
            StopAllCoroutines();
            var color = _objectsMaterialProfile.NeutralHighlightColor;
            TriggerHighlightOnMouseHover(1f, color);
        }

        public void OnMouseExit()
        {
            ResetToDefault();
        }

        public void AnimateHighlightByCurve(AnimationCurve animationCurve, Color newColor, float duration)
        {
            foreach (Material mat in _materials)
            {
                mat.SetColor("_HighlightColor", newColor);
            }

            StartCoroutine(AnimateHighlightByCurveCO(animationCurve, duration));
        }

        private IEnumerator AnimateHighlightByCurveCO(AnimationCurve animationCurve, float duration)
        {
            var wait = new WaitForEndOfFrame();
            var increment = Time.deltaTime;
            float progress = 0;
            while(progress <= 1f + increment)
            {
                foreach (Material mat in _materials)
                {
                    mat.SetFloat("_HighlightAmount", animationCurve.Evaluate(progress));
                }
                progress += increment;
                increment = Time.deltaTime;
                yield return wait;
            }
            ResetToDefault();
        }

        private void ResetToDefault()
        {
            foreach (Material mat in _materials)
            {
                mat.SetFloat("_HighlightAmount", 0f);
                mat.SetColor("_HighlightColor", Color.clear);
            }
        }
    }
}