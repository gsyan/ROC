using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CustomTimeline
{
    [TrackColor(0.4448276f, 0f, 1f)]
    [TrackClipType(typeof(MethodCall_Asset))]
    [TrackBindingType(typeof(GameObject))]
    public class MethodCall_Track : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            var director = go.GetComponent<PlayableDirector>();
            var trackTargetObject = director.GetGenericBinding(this) as GameObject;

            foreach (var clip in GetClips())
            {
                var playableAsset = clip.asset as MethodCall_Asset;

                if (playableAsset)
                {
                    if (trackTargetObject)

                    {
                        playableAsset.TrackTargetObject = trackTargetObject;
                    }
                }
            }

            var scriptPlayable = ScriptPlayable<MethodCall_MixerBehaviour>.Create(graph, inputCount);
            return scriptPlayable;
        }
    }
}


