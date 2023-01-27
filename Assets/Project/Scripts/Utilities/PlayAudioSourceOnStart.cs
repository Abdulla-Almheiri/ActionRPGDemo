using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chaos.Gameplay.Utilities
{
    public class PlayAudioSourceOnStart : MonoBehaviour
    { 
        void Start()
        {
            var audioSource = GetComponent<AudioSource>();
            if(audioSource != null)
            {
                audioSource.Play();
            }
        }

    }
}