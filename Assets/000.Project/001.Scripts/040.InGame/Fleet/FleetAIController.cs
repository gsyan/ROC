using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    public class FleetAIController : AIController
    {
        private Fleet _fleet;
        
        public override void StartAI()
        {
            _fleet = gameObject.GetComponent<Fleet>();

            State spawn = new FleetStateSpawn(this);
            State move = new FleetStateMove(this);

            spawn
                .SetTransition(move, () => ((FleetStateSpawn)spawn).IsGoMove);

            move
                .SetTransition(GetInterupptibleState);

            fsm.AddState(spawn);
            fsm.AddState(move);
            
            base.StartAI();
        }

        protected virtual BaseState GetInterupptibleState()
        {
            BaseState ret = default(BaseState);

            if (Input.GetKey(KeyCode.Alpha2))
            {
                //return new FleetStateDisappear(this);
                return new FleetStateDisappear(this);
            }

            return ret;
        }

        //오늘의 할일 리스폰을 어떤 식으로 제작해야 할까 ..... 리스폰 코드가 내부에 있다 해도 죽은 오브젝트가 스스로 살아나는것은 안될듯.
        public void ReSpawn()
        {
            gameObject.SetActive(true);
            fsm.StartFSM();//이게 어떤 스테이트를 호출할지 테스트 해봐야함
        }
    }
}

