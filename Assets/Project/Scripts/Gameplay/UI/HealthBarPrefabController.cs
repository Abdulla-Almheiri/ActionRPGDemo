using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Chaos.Gameplay.UI
{
    public class HealthBarPrefabController : MonoBehaviour
    {
        private Image _image;
        private float _originalHeight = 10f;
        private UnityEngine.Camera _cachedCamera;
        private Transform _initialWorldPoint;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            ProcessScreenPosition();
        }

        public void SetHealthPercentage(float value)
        {
            _image.rectTransform.sizeDelta = new Vector2(value, _originalHeight);
        }

        private void ProcessScreenPosition()
        {
            var newPoint = _cachedCamera.WorldToScreenPoint(_initialWorldPoint.position);
            newPoint.x -= 50f;
            _image.rectTransform.SetPositionAndRotation(newPoint, Quaternion.identity);
        }
        public void Initialize(Transform location)
        {
            _initialWorldPoint = location;
            _cachedCamera = UnityEngine.Camera.main;
            _image = GetComponent<Image>();
        }
    }
}