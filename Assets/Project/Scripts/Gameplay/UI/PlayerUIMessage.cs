using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.UI
{
    [CreateAssetMenu(fileName = "new player UI message", menuName = "Content/UI/Player UI Message")]
    public class PlayerUIMessage : ScriptableObject
    {
        [TextArea()]
        public string Text;
        public Color Color;
        public AudioClip SoundEffect;
    }
}