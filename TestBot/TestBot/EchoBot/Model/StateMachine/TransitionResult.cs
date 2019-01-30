namespace EchoBot.Model.StateMachine
{
    /// <summary>
    /// A struct used to aid in the transition of processes.
    /// </summary>
    /// <typeparam name="TState">The type of state to be processed.</typeparam>
    public struct TransitionResult<TState>
    {
        public TState CurrentState { get; private set; }

        public bool IsValid { get; private set; }

        public TransitionResult(TState state, bool valid)
        {
            CurrentState = state;
            IsValid = valid;
        }
    }
}