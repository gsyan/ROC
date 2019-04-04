using UnityEngine;
using UnityEngine.Playables;


namespace CustomTimeline
{
    public class Light_Asset : PlayableAsset
    {
        public Light_Behaviour template;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<Light_Behaviour>.Create(graph, template);
            return playable;
        }
    }
}



