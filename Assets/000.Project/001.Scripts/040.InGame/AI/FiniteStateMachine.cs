using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace com.sbp.ai
{
    /// <summary>
    /// 
    /// </summary>
    public class FiniteStateMachine : IDisposable
    {
        public State CurrentState
        {
            get
            {
                return this.currentState;
            }
        }

        protected State currentState;

        protected State nextState;

        ITransition transitionToNextState;

        List<State> states = new List<State>();

        bool started = false;

        bool updating = false;

        /// <summary>
        /// Add to State
        /// </summary>
        /// <param name="state">State.</param>
        public void AddState(State state)
        {
            if (this.states.Contains(state))
            {
                return;
            }

            this.states.Add(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entryState">Entry state.</param>
        public void StartFSM(State entryState)
        {
            if (!this.started)
            {
                if (!this.states.Contains(entryState))
                {
                    throw new Exception("Not Found EntryState");
                }

                this.nextState = entryState;
                this.started = true;
                UpdateState();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void StartFSM()
        {
            if(this.states.Count > 0)
            {
                StartFSM(this.states[0]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateState()
        {
            if (!this.started)
                return;

            if (this.updating)
                return;

            this.updating = true;

            UpdateCore();

            this.updating = false;
        }

        void UpdateCore()
        {
            if (this.nextState != null)
            {
                var from = this.currentState;
                this.currentState = this.nextState;
                this.currentState.OnStateEnter(from, this.transitionToNextState);
                OnStateEnter(this.currentState);
                this.nextState = null;
                this.transitionToNextState = null;
            }

            this.currentState.OnStateUpdate();
            OnStateUpdate(this.currentState);

            var willNextState = default(State);
            var transition = default(ITransition);
            if (this.currentState.GetNextState(out willNextState, out transition))
            {
                this.currentState.OnStateExit(willNextState, transition);
                OnStateExit(this.currentState);
                this.nextState = willNextState;
                this.transitionToNextState = transition;
            }
        }

        protected virtual void OnStateEnter(State state)
        {

        }

        protected virtual void OnStateUpdate(State state)
        {

        }

        protected virtual void OnStateExit(State state)
        {

        }

        public virtual void Dispose()
        {
            foreach (var state in this.states)
            {
                state.Dispose();
            }
            this.states.Clear();
        }
    }
}