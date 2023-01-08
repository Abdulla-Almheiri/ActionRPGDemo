using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chaos.Gameplay.Characters;
using UnityEditor;

namespace Chaos.Editor
{
    [CustomEditor(typeof(CharacterAnimation))]
    public class CharacterAnimationEditor : UnityEditor.Editor
    {
        private CharacterAnimation _characterAnimation;
        private void OnEnable()
        {
            _characterAnimation = target as CharacterAnimation;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}