using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Chaos.Gameplay.Systems;

namespace Chaos.Gameplay.UI
{
    public class FloatingCombatTextAnimationController : MonoBehaviour
    {
        public FloatingCombatTextTemplate FloatingCombatTextTemplate;
        private GameObjectPoolController _gameObjectPoolController;
        private TMP_Text _TMPText;
        private Vector3 _initialPosition;
        private float _elapsedTime = 0f;
        private float _maxDurationInSeconds = 3f;

        public void Awake()
        {
            _gameObjectPoolController = GetComponent<GameObjectPoolController>(); ;
        }
        // Start is called before the first frame update
        void Start()
        {
            //Initialize(new Vector3(400f,400f,0f));
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyUp(KeyCode.C))
            {
                Play();
            }
            ProcessDisablingAfterElapsedTime();
        }
        private void ProcessDisablingAfterElapsedTime()
        {
            if(gameObject.activeSelf == false)
            {
                return;
            }

            _elapsedTime += Time.deltaTime;
            if(_elapsedTime >= _maxDurationInSeconds)
            {
                gameObject.SetActive(false);
            }
        }
        public void Initialize(Vector3 initialPosition)
        {
            _TMPText = GetComponent<TMP_Text>();
            _initialPosition = initialPosition;
            _TMPText.rectTransform.SetPositionAndRotation(_initialPosition, Quaternion.identity);
            Play();
        }

        public void Play()
        {
            if(FloatingCombatTextTemplate == null || _TMPText == null)
            {
                return;
            }
            foreach(FloatingCombatTextAnimationData animationData in FloatingCombatTextTemplate.PropertiesToAnimate)
            {
                StartCoroutine(AnimatePropertyWithAnimationCurveCO(animationData));
            }


        }

        private IEnumerator AnimatePropertyWithAnimationCurveCO(FloatingCombatTextAnimationData animationData)
        {
            float increment = 1f / 30f;

            var wait = new WaitForSeconds(increment);
            float progress = 0f;
            while(progress <= 1f+increment)
            {
                SetPropertyValue(animationData.PropertyToAnimate, animationData.CurveMultiplier * animationData.AnimationCurve.Evaluate(progress));
                progress += increment;
                yield return wait;
            }
        }

        private void SetPropertyValue(FloatingCombatTextAnimationDataPropertyType propertyType, float newValue)
        {

            if(propertyType == FloatingCombatTextAnimationDataPropertyType.RelativePositionX)
            {
                Vector3 newPosition = _initialPosition + (new Vector3(newValue, 0f, 0f));
                _TMPText.rectTransform.SetPositionAndRotation(newPosition, Quaternion.identity);
                return;
            }

            if (propertyType == FloatingCombatTextAnimationDataPropertyType.RelativePositionY)
            {
                Vector3 newPosition = _initialPosition + (new Vector3(0f, newValue, 0f));
                _TMPText.rectTransform.SetPositionAndRotation(newPosition, Quaternion.identity);
                return;
            }

            if (propertyType == FloatingCombatTextAnimationDataPropertyType.Scale)
            {
                _TMPText.transform.localScale = new Vector3(newValue, newValue, newValue);
                return;
            }

            if (propertyType == FloatingCombatTextAnimationDataPropertyType.Transparency)
            {
                var oldColor = _TMPText.color;
                var newColor = oldColor;
                newColor.a = newValue;
                _TMPText.color = newColor;
                return;
            }
        }


        public void OnEnable()
        {
            Play();
        }

        public void OnDisable()
        {
            _gameObjectPoolController.ReturnToPool();
        }

    }
}