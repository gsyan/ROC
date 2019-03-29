using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace com.sbp.ai
{
    public class KE_1_1 : BaseState
    {
        private KEventController _eventController;
        private AirBalloonAniController _aniController;
        private string _currentStep = "";

        public KE_1_1(AIController controller) : base(controller)
        {
            _eventController = (KEventController)controller;
            _aniController = _eventController.GetComponent<AirBalloonAniController>();
        }

        public override void OnStateEnter(State from, ITransition via)
        {
            base.OnStateEnter(from, via);

            _currentStep = this.GetType().ToString();
            _currentStep = _currentStep.Replace(this.GetType().Namespace + ".", "");
            Debug.Log(_currentStep + " Enter");
        }

        public override void OnStateUpdate()
        {
            base.OnStateUpdate();

            string stepName = _aniController.steps[_aniController.stepIndex].stepName;

            if( string.Compare(stepName, _currentStep) != 0 )
            {
                string className = stepName;

                Assembly assembly = Assembly.GetExecutingAssembly();

                Type t = assembly.GetType(this.GetType().Namespace + "." + className);

                System.Object obj = Activator.CreateInstance(t, new object[] { _eventController });

                this.SetTransition((obj as KE_1_2));
            }

            Debug.Log(_currentStep + " Update");
        }

        public override void OnStateExit(State to, ITransition via)
        {
            base.OnStateExit(to, via);

            Debug.Log(_currentStep + " Exit");
        }


    }
}
    
