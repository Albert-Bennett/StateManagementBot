using System;

namespace EchoBot.Model.StateMachine
{
    /// <summary>
    /// A helper for managing the transition of processes.
    /// </summary>
    public class StateTransition<TState, TCommand>
        where TState : struct, IConvertible, IComparable
        where TCommand : struct, IConvertible, IComparable
    {
        /// <summary>
        /// The current processing state of the transition.
        /// </summary>
        TState currentState;

        /// <summary>
        /// The current command state being processed.
        /// </summary>
        TCommand command;

        /// <summary>
        /// Creates a new StateTransition.
        /// </summary>
        /// <param name="currentState">The current processing state.</param>
        /// <param name="command">The command being processed.</param>
        public StateTransition(TState currentState, TCommand command)
        {
            if (!typeof(TState).IsEnum || !typeof(TCommand).IsEnum)
                throw new InvalidOperationException("TState and TCommand must be enums.");

            this.currentState = currentState;
            this.command = command;
        }

        public override int GetHashCode()
        {
            return 17 + (23 * currentState.GetHashCode()) + (31 * command.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            StateTransition<TState, TCommand> o = obj as StateTransition<TState, TCommand>;
        
            return o != null &&
                currentState.CompareTo(o.currentState) == 0 &&
                command.CompareTo(o.command) == 0;
        }
    }
}
