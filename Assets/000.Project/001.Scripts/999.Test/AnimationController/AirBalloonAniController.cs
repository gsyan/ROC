using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    [System.Serializable]
    public class AirBalloonStep
    {
        public string stepName;
        public float stepValue;
    }

    public class AirBalloonAniController : AIController//MonoBehaviour
    {
        public bool bAutoStart = false;// 자동 스타트?

        public float aniSpeed = 1.0f;

        public float startFrame = 0.0f;// 처음부터 or 중간부터

        public float currentFrame = 0.0f;// 현재 프래임 표시

        public string currentClip = "";// 현재 플레이 중인 애니 클립

        public float currentClipTime = 0.0f;// 현재 플레이 중인 애니 클립 의 진행 현황 (초 = sec)

        public float currentClipTimeTotal = 0.0f;// 현재 플레이 중인 애니 클립 의 토탈 시간 (초 = sec)
        

        [HideInInspector]
        public float jumpToFrame = -1.0f;// 점프로 이동할 프래임


        public AnimationClip[] aniClips;
        public float[] aniLengthStack;
        
        /// <summary>
        /// 0.0 ~ 1.0 까지 허용
        /// </summary>
        public AirBalloonStep[] steps;//임의로 정해준 지점(기점?)
        [HideInInspector] public int stepIndex;
        
        
        new public void StartAI()
        {
            CalculateAniClipLength();

            State spawn = new AirBalloonAniStateSpawn(this);
            State move = new AirBalloonAniStateMove(this);

            spawn
                .SetTransition(move, () => ((AirBalloonAniStateSpawn)spawn).IsGoing);

            move
                .SetTransition(GetInterupptibleState);

            fsm.AddState(spawn);
            fsm.AddState(move);

            base.StartAI();
        }
        private void CalculateAniClipLength()
        {
            aniLengthStack = new float[aniClips.Length];
            
            for (int i = 0; i < aniClips.Length; ++i)
            {
                if (i == 0)
                {
                    aniLengthStack[i] = aniClips[i].length;
                }
                else
                {
                    aniLengthStack[i] = aniLengthStack[i - 1] + aniClips[i].length;
                }
            }
        }


        protected virtual BaseState GetInterupptibleState()
        {
            BaseState ret = default(BaseState);

            // if LookAt me (x)sec
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("AirBalloonAniController / GetInterupptibleState if space");
            }
            return ret;
        }

        public void UpdateStepIndex()
        {

        }

        public void JumpToClip(int index)
        {
            Animator _animator = GetComponent<Animator>();
            _animator.Play(aniClips[index].name, 0, 0.0f);

        }


        public void ChangeJumpToFrame(float frameNormalized)
        {
            jumpToFrame = frameNormalized;
        }

        public void FindClipAndNormalizedFrame(float normalizedTotalFrame, ref string clipName, ref float normalizedClipFrame)
        {
            float findingFrame = aniLengthStack[aniLengthStack.Length - 1] * normalizedTotalFrame;

            for(int i =0; i< aniLengthStack.Length; ++i)
            {
                if(findingFrame > aniLengthStack[i])
                {
                    continue;
                }
                else
                {
                    clipName = aniClips[i].name;

                    if (i == 0)
                    {
                        normalizedClipFrame = findingFrame / aniClips[i].length;
                    }
                    else
                    {
                        float inClipFrame = (findingFrame - aniLengthStack[i - 1]);
                        normalizedClipFrame = inClipFrame / aniClips[i].length;
                    }
                    break;
                }
            }
        }


    }
}


