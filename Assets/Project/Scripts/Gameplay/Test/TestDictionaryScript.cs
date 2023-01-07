using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Test
{
    [CreateAssetMenu(fileName = "new dict test", menuName = "Content/Test/Dict Test")]
    public class TestDictionaryScript : ScriptableObject
    {

        public List<float> list = new List<float>();
        private Dictionary<int, float> dict = new Dictionary<int, float>();
        private void OnEnable()
        {
            for(int i =0; i<list.Count; i++)
            {
                dict[i] = list[i];
            }
        }

        public float GetValue(int index)
        {
            if(dict.TryGetValue(index, out float value))
            {
                return value;
            }
            return -1f;
        }
    }
}