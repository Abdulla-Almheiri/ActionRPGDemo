using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Chaos.Gameplay.VFX
{
    [CreateAssetMenu(fileName = "new screen effect", menuName = "Content/Visual Effects/Screen Volume Effect")]
    public class ScreenVolumeVisualEffect : ScriptableObject
    {
        public VolumeProfile VolumeProfile;
        public AnimationCurve EaseInAndOutCurve;

    }
}