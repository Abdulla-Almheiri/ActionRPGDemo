using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;

namespace Chaos.Gameplay.VFX
{
    [CreateAssetMenu(fileName = "new material profile", menuName = "Content/Visual Effects/Objects Material Profile")]
    public class ObjectsMaterialProfile : ScriptableObject
    {
        [Tooltip("Last value on curve will remain until mouse exits object.")]
        public AnimationCurve MouseHoverHighlightBehaviourCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public Color NeutralHighlightColor = Color.white;
        public Color HostileHighlightColor = Color.red;
        public Color FriendlyHighlightColor = Color.green;
        public Material FlatColorHitMaterial;
        [Space(10)]
        [Header("Hit Frame Details")]
        public AnimationCurve HitFrameAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 0);
        //public CharacterAnimationData

        [Tooltip("Triggered When hit by skillAction with specified element. Highlight amount defaults to 0 after duration is elapsed.")]
        public List<ObjectsMaterialProfileData> MaterialHighlightColorsTriggeredbyElements = new List<ObjectsMaterialProfileData>();
    }
}