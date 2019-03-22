using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.sbp.ai
{
    /// <summary>
    /// Interface for NextTo
    /// </summary>
    public interface ITransition : IDisposable
    {
        State GetNextState ();
    }

    /// <summary>
    /// For Generic
    /// </summary>
    public interface ITransition<TFrom> : ITransition
        where TFrom : State
    {

    }

    /// <summary>
    /// Conditional Transition
    /// </summary>
    public class ConditionalTransition<TFrom> : ITransition<TFrom>
        where TFrom : State
    {
        State next;

        Func<bool> condition;

        public ConditionalTransition (State next, Func<bool> condition)
        {
            this.next = next;
            this.condition = condition != null
                ? condition
                : () => false;
        }

        public State GetNextState ()
        {
            return condition() ? next : null;
        }

        public void Dispose ()
        {
            this.next = null;
            this.condition = null;
        }
    }

    public static class ConditionalTransitionExtensions
    {
        public static TFrom SetTransition<TFrom> (this TFrom state, State next, Func<bool> condition)
            where TFrom : State
        {
            return state.SetTransition(new ConditionalTransition<TFrom>(next, condition)) as TFrom;
        }

        public static TFrom SetTransition<TFrom> (this TFrom state, State next)
            where TFrom : State
        {
            return state.SetTransition(next, () => true);
        }
    }

    /// <summary>
    /// Interruptible Transition
    /// </summary>
    public class InterruptibleTransition<TFrom> : ITransition<TFrom>
        where TFrom : State
    {
        State from;

        Func<State> getter;

        bool canTransitionToSelf;

        public InterruptibleTransition (State from, Func<State> getter, bool canTransitionToSelf)
        {
            this.from = from;
            this.getter = getter;
            this.canTransitionToSelf = canTransitionToSelf;
        }

        public State GetNextState ()
        {
            var next = this.getter();
            if (canTransitionToSelf) {
                return next;
            } else {
                if (next == from) {
                    return null;
                } else {
                    return next;
                }
            }
        }

        public void Dispose ()
        {
            from = null;
            getter = null;
        }
    }

    public static class InterruptibleTransitionExtensions
    {
        public static TFrom SetTransition<TFrom> (this TFrom from, Func<State> getter, bool canTransitionToSelf = false)
            where TFrom : State
        {
            return from.SetTransition(new InterruptibleTransition<TFrom>(from, getter, canTransitionToSelf)) as TFrom;
        }
    }
    
    /// <summary>
    /// Fsm State
    /// </summary>
    public abstract class State : IDisposable
    {
        protected List<ITransition> transitions = new List<ITransition>();

        public State ()
        {
        }

        public abstract void OnStateEnter (State from, ITransition via);

        public abstract void OnStateUpdate ();

        public abstract void OnStateExit (State to, ITransition via);

        public bool GetNextState (out State to, out ITransition via)
        {
            to = null;
            via = null;

            for (int i = 0; i < transitions.Count; ++i)
            {
                var transition = transitions[i];
                var next = transition.GetNextState();
                if (next != null) {
                    to = next;
                    via = transition;
                    return true;
                }
            }

            return false;
        }

        public State SetTransition (ITransition transition)
        {
            transitions.Add(transition);
            return this;
        }
         
        public State SetTransition (IEnumerable<ITransition> transitionsAdd)
        {
            transitions.AddRange(transitionsAdd);
            return this;
        }

        public virtual void Dispose ()
        {
            foreach (var transition in transitions) {
                transition.Dispose();
            }
            transitions.Clear();
        }

    }
}