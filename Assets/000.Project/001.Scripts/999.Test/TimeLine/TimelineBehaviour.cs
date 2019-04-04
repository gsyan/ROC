using System;
using UnityEngine;
using UnityEngine.Playables;

namespace CustomTimeline
{
    [Serializable]
    public class TimelineBehaviour : PlayableBehaviour
    {
        public Color color = Color.white;
        public float intensity = 1f;
        public float range = 50f;


    }
}


