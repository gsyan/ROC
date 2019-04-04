using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CustomTimeline
{
    [TrackClipType(typeof(Light_Asset))]
    [TrackBindingType(typeof(Light))]
    public class Light_Track : TrackAsset
    {
        //clip 추가시 호출됨
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<Light_Behaviour>.Create(graph, inputCount);
        }
    }
}


