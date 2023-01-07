using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Chaos.Test
{
    public class DictTextTestScript : MonoBehaviour
    {
        public TestDictionaryScript ScriptableObject;
        private TMP_Text textComponent;
        // Start is called before the first frame update
        void Start()
        {
            textComponent = GetComponent<TMP_Text>();
            textComponent.text = ScriptableObject.GetValue(2).ToString();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}