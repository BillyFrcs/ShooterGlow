using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sounds.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager Instance { get; set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
