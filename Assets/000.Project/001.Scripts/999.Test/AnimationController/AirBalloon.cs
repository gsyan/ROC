using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class AirBalloon : MonoBehaviour
    {
        private AirBalloonAniController _aniController;
        
        private void Awake()
        {
            _aniController = gameObject.GetComponent<AirBalloonAniController>();
        }

        private void Start()
        {
#if UNITY_EDITOR
            transform.Find("ForFX/Camera").localPosition = new Vector3(0, 1.7f, -0.016f);
#endif

            if (_aniController != null)
            {
                _aniController.StartAI();
            }


            AddAnimationEvent();


        }
        private void AddAnimationEvent()
        {
            Animator _animator = GetComponent<Animator>();
            AnimationClip clip = _animator.runtimeAnimatorController.animationClips[1];

            AnimationEvent aniEvent = new AnimationEvent();
            aniEvent.functionName = "Shake";
            aniEvent.time = 6.0f;
            clip.AddEvent(aniEvent);

            aniEvent = new AnimationEvent();
            aniEvent.functionName = "LightControl";
            aniEvent.time = 7.0f;
            clip.AddEvent(aniEvent);

            aniEvent = new AnimationEvent();
            aniEvent.functionName = "Phytoncide";
            aniEvent.time = 10.0f;
            clip.AddEvent(aniEvent);


            // 증산 작용 포스트 이팩트 필요?



            clip = _animator.runtimeAnimatorController.animationClips[2];



            clip = _animator.runtimeAnimatorController.animationClips[3];

            aniEvent = new AnimationEvent();
            aniEvent.functionName = "Baobab";
            aniEvent.time = 50.0f;
            clip.AddEvent(aniEvent);



            clip = _animator.runtimeAnimatorController.animationClips[4];


            clip = _animator.runtimeAnimatorController.animationClips[5];




        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Shake();
                //LightControl();
            }
        }


        
        // shake balloon
        public void Shake()
        {
            if (!isShaking)
            {
                StartCoroutine(ShakeCor(3.0f, 50.0f, 0.3f));
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

        
        // point light range control
        public void LightControl()
        {
            if (!isLightControlling)
            {
                StartCoroutine(LightControlCor(3.0f, 100.0f, 10.0f, 50.0f));
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


        // Phytoncide
        public void Phytoncide()
        {
            if (!isPhytoncideControlling)
            {
                StartCoroutine(PhytoncideCor());
            }
        }
        bool isPhytoncideControlling = false;
        private IEnumerator PhytoncideCor()
        {
            
            yield return null;    
        }




        //Baobab
        public void Baobab()
        {
            
        }



    }
}


