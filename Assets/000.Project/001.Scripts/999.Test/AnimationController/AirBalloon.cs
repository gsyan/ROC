using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class AirBalloon : MonoBehaviour
    {
        private AirBalloonAniController _aniController;
        //private KEventController _kEventController;

        private void Awake()
        {
            _aniController = gameObject.GetComponent<AirBalloonAniController>();
            //_kEventController = gameObject.GetComponent<KEventController>();
        }

        private void Start()
        {
            if (_aniController != null)
            {
                _aniController.StartAI();
            }

            //if (_kEventController != null)
            //{
            //    _kEventController.StartAI();
            //}

            Animator _animator = GetComponent<Animator>();
            //myAnim. runtimeAnimatorController.animationClips[0];
            AnimationClip clip = _animator.runtimeAnimatorController.animationClips[1];

            AnimationEvent aniEvent = new AnimationEvent();
            aniEvent.functionName = "LightControl";
            aniEvent.time = 10.0f;

            clip.AddEvent(aniEvent);



        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Shake();
                //LightControl();
            }
        }


        /// <summary>
        /// shake balloon
        /// </summary>
        /// <param name="forSec"></param>
        /// <param name="speed"></param>
        /// <param name="amount"></param>
        public void Shake(float forSec = 3.0f, float speed = 50.0f, float amount = 0.3f)
        {
            if (!isShaking)
            {
                StartCoroutine(ShakeCor(forSec, speed, amount));
            }
        }
        bool isShaking = false;
        private IEnumerator ShakeCor(float forSec, float speed, float amount)
        {
            float elipse = 0.0f;
            isShaking = true;
            
            Transform ForFX = transform.Find("ForFX");

            while (elipse < forSec)
            {
                Vector3 position = ForFX.localPosition;
                position += ForFX.right * Mathf.Sin(elipse * speed) * amount;
                ForFX.localPosition = position;
                elipse += Time.deltaTime;
                
                yield return null;
            }

            elipse = 0.0f;
            while (elipse < 0.5f)
            {
                ForFX.localPosition = Vector3.Lerp(ForFX.localPosition, Vector3.zero, elipse * 2.0f);
                elipse += Time.deltaTime;
                yield return null;
            }

            isShaking = false;
            yield return null;
        }

        /// <summary>
        /// point light range control
        /// </summary>
        /// <param name="forSec"></param>
        /// <param name="speed"></param>
        /// <param name="wayValue"></param>
        /// <param name="destinationValue"></param>
        public void LightControl()
        {
            if (!isLightControlling)
            {
                StartCoroutine(LightControlCor(2.0f, 100.0f, 10.0f, 50.0f));
            }
        }
        public void LightControl(float forSec = 2.0f, float speed = 100.0f, float wayValue = 10.0f, float destinationValue = 50.0f)
        {
            if (!isLightControlling)
            {
                StartCoroutine(LightControlCor(forSec, speed, wayValue, destinationValue));
            }
        }
        bool isLightControlling = false;
        private IEnumerator LightControlCor(float forSec, float speed, float wayValue, float destinationValue)
        {
            float elipse = 0.0f;
            isLightControlling = true;

            Transform pointLight = transform.Find("ForFX").Find("Point Light");
            Light light = pointLight.GetComponent<Light>();

            while (elipse < 1.0f)
            {
                light.range -= Time.deltaTime * speed;
                if(light.range < wayValue)
                {
                    light.range = wayValue;
                }

                elipse += Time.deltaTime;

                yield return null;
            }


            elipse = 0.0f;
            while (elipse < forSec)
            {
                elipse += Time.deltaTime;
                yield return null;
            }


            elipse = 0.0f;
            while (elipse < 1.0f)
            {
                light.range += Time.deltaTime * speed;
                if (light.range > destinationValue )
                {
                    light.range = destinationValue;
                }

                elipse += Time.deltaTime;

                yield return null;
            }


            isLightControlling = false;
            yield return null;
        }

    }
}


