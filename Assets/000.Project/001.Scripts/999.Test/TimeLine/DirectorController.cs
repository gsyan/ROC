using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CustomTimeline
{
    /// <summary>
    /// 여러 타임라인을 등록해서 순차적으로 플레이가 필요할때 사용
    /// </summary>
    public class DirectorController : MonoBehaviour
    {
        public PlayableDirector currentDirector;
        int _index = 0;

        public PlayableDirector[] playableDirectors;


        private void Start()
        {
            currentDirector = playableDirectors[_index];
            currentDirector.Play();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Next();
            }


            if(currentDirector.state != PlayState.Playing)
            {
                Next();
            }
            else
            {
                Debug.Log(currentDirector.name + ": " + currentDirector.time + " / " + currentDirector.duration);
            }
            




        }

        public void Next()
        {
            _index++;
            if( _index < playableDirectors.Length)
            {
                currentDirector = playableDirectors[_index];
                currentDirector.Play();
            }
        }





    }
}


