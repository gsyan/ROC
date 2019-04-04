using UnityEngine;
using UnityEngine.Playables;


namespace CustomTimeline
{
    public class TimelineAsset : PlayableAsset
    {
        public TimelineBehaviour template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TimelineBehaviour>.Create(graph, template);
            return playable;
        }
    }
}



