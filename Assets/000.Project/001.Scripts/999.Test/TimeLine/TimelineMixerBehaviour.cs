using UnityEngine;
using UnityEngine.Playables;

namespace CustomTimeline
{
    public class TimelineMixerBehaviour : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Light trackBinding = playerData as Light;
            float finalIntensity = 0f;
            Color finalColor = Color.black;
            float finalRange = 0f;


            if (!trackBinding)
                return;

            int inputCount = playable.GetInputCount(); //get the number of all clips on this track

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                ScriptPlayable<TimelineBehaviour> inputPlayable = (ScriptPlayable<TimelineBehaviour>)playable.GetInput(i);
                TimelineBehaviour input = inputPlayable.GetBehaviour();

                // Use the above variables to process each frame of this playable.
                finalIntensity += input.intensity * inputWeight;
                finalColor += input.color * inputWeight;
                finalRange += input.range * inputWeight;
            }

            //assign the result to the bound object
            trackBinding.intensity = finalIntensity;
            trackBinding.color = finalColor;
            trackBinding.range = finalRange;

        }
    }

}

