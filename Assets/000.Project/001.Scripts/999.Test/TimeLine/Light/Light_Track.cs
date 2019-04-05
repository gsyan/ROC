using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CustomTimeline
{
    [TrackClipType(typeof(Light_Asset))]
    [TrackBindingType(typeof(Light))]
    public class Light_Track : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<Light_MixerBehaviour>.Create(graph, inputCount);
        }
    }
}


