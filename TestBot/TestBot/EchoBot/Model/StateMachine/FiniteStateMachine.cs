using System;
using System.Collections.Generic;

namespace EchoBot.Model.StateMachine
{
    public class FiniteStateMachine<TState, TCommand>
            where TState : struct, IConvertible, IComparable
            where TCommand : struct, IConvertible, IComparable
    {
        Dictionary<StateTransition<TState, TCommand>, TState> transitions;

        /// <summary>
        /// The current state.
        /// </summary>
        public TState CurrentState { get; private set; }

        /// <summary>
        /// The previous state.
        /// </summary>
        public TState PreviousState { get; private set; }

        /// <summary>
        /// Creates a new FiniteStateMachine.
        /// </summary>
        /// <param name="initialState">The initial state of the state machine.</param>
        public FiniteStateMachine(TState initialState)
        {
            if (!typeof(TState).IsEnum || !typeof(TCommand).IsEnum)
                throw new InvalidOperationException("TState and TCommand need to be of type enum.");

            CurrentState = initialState;

            transitions = new Dictionary<StateTransition<TState, TCommand>, TState>();
        }

        /// <summary>
        /// Adds a new state for the FiniteStateMachine to manage.
        /// </summary>
        /// <param name="begin">The begining state.</param>
        /// <param name="end">The ending state.</param>
        /// <param name="command">The command to be executed.</param>
        protected void AddState(TState begin, TState end, TCommand command)
        {
            transitions.Add(new StateTransition<TState, TCommand>(begin, command), end);
        }

        /// <summary>
        /// Gets the next processing state.
        /// </summary>
        /// <param name="command">The command to be processed.</param>
        /// <returns>The next processing state.</returns>
        public TransitionResult<TState> GetNextState(TCommand command)
        {
            StateTransition<TState, TCommand> transition = 
                new StateTransition<TState, TCommand>(CurrentState, command);

            return transitions.TryGetValue(transition, out TState nextState) ? 
                new TransitionResult<TState>(nextState, true) :
                new TransitionResult<TState>(CurrentState, false);
        }

        /// <summary>
        /// Moves this process on to the next state.
        /// </summary>
        /// <param name="command">The next command to be processed.</param>
        /// <returns>The next processing state.</returns>
        public TransitionResult<TState> MoveToNextState(TCommand command)
        {
            TransitionResult<TState> result = GetNextState(command);

            if (result.IsValid)
                ChangeState(result.CurrentState);

            return result;
        }

        void ChangeState(TState stateChange)
        {
            PreviousState = CurrentState;
            CurrentState = stateChange;
        }
    }
}
