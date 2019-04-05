using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CustomTimeline
{
    [Serializable]
    public class MethodCall_Asset : PlayableAsset, ITimelineClipAsset
    {
        public MethodCall_Behaviour template = new MethodCall_Behaviour();
        public GameObject TrackTargetObject { get; set; }

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<MethodCall_Behaviour>.Create(graph, template);
            MethodCall_Behaviour clone = playable.GetBehaviour();
            clone.targetObject = TrackTargetObject;
            return playable;
        }
    }
}


