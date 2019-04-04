using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class AirBalloonAniStateMove : BaseState
    {
        private AirBalloonAniController _aniController;
        private Animator _animator;


        public AirBalloonAniStateMove(AIController controller) : base(controller)
        {
            _aniController = (AirBalloonAniController)controller;

            _animator = _aniController.GetComponent<Animator>();
        }

        public override void OnStateEnter(State from, ITransition via)
        {
            base.OnStateEnter(from, via);

            if (_animator)
            {
                _animator.speed = _aniController.aniSpeed;

                if (_aniController.startFrame >= 0.0f)
                {
                    string clipName = "";
                    float normalizedClipFrame = 0.0f;
                    _aniController.FindClipAndNormalizedFrame(_aniController.startFrame, ref clipName, ref normalizedClipFrame);
                    _animator.Play(clipName, 0, normalizedClipFrame);

                }
            }

        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();

            UpdateAniTotalNormalizedCurrentFrame();
            UpdateAniStepIndex();
            JumpTo();
        }
        private void UpdateAniTotalNormalizedCurrentFrame()
        {
            float frameSum = 0.0f;

            AnimatorStateInfo asi = _animator.GetCurrentAnimatorStateInfo(0);
            AnimatorClipInfo[] myAnimatorClip = _animator.GetCurrentAnimatorClipInfo(0);

            if (myAnimatorClip.Length > 0)
            {
                for(int i = 0; i < _aniController.aniClips.Length; ++i)
                {
                    if (myAnimatorClip[0].clip == _aniController.aniClips[i])
                    {
                        frameSum += _aniController.aniClips[i].length * asi.normalizedTime;
                        //string s = string.Format("{0}/ Length: {1} / curInClip: {2}", myAnimatorClip[0].clip.name, myAnimatorClip[0].clip.length, _aniController.aniClips[i].length * asi.normalizedTime);
                        _aniController.currentClip = myAnimatorClip[0].clip.name;
                        _aniController.currentClipTime = _aniController.aniClips[i].length * asi.normalizedTime;
                        _aniController.currentClipTimeTotal = _aniController.aniClips[i].length;
                        break;
                    }
                    else
                    {
                        frameSum += _aniController.aniClips[i].length;
                        
                    }
                }

                _aniController.currentFrame = frameSum / _aniController.aniLengthStack[_aniController.aniLengthStack.Length - 1];
            }


        }
        private void UpdateAniStepIndex()
        {
            for(int i=0; i< _aniController.steps.Length; ++i)
            {
                if (_aniController.currentFrame > _aniController.steps[i].stepValue)
                {
                    _aniController.stepIndex = i;
                }
            }
        }
        private void JumpTo()
        {
            if (_animator)
            {
                if(_aniController.jumpToFrame < 0.0f)
                {
                    return;
                }

                string clipName = "";
                float normalizedClipFrame = 0.0f;
                _aniController.FindClipAndNormalizedFrame(_aniController.jumpToFrame, ref clipName, ref normalizedClipFrame);
                _animator.Play(clipName, 0, normalizedClipFrame);
                _aniController.jumpToFrame = -1.0f;
            }
        }

        

        private void SpeedControl()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _aniController.aniSpeed = _animator.speed += 0.1f;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _aniController.aniSpeed = _animator.speed -= 0.1f;
            }
        }



    }
}


