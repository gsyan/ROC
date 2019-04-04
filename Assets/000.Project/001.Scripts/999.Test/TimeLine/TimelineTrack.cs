using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CustomTimeline
{
    [TrackClipType(typeof(TimelineAsset))]
    [TrackBindingType(typeof(Light))]
    public class TimelineTrack : TrackAsset
    {
        //clip 추가시 호출됨
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<TimelineMixerBehaviour>.Create(graph, inputCount);
        }
    }
}


