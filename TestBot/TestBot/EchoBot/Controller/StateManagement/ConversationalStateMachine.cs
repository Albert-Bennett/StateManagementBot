using EchoBot.Model.StateMachine;

namespace EchoBot.Controller.StateManagement
{
    public class ConversationalStateMachine : FiniteStateMachine<ProcessStates, ProcessCommands>
    {
        public ConversationalStateMachine() : base(ProcessStates.Name)
        {
            AddState(ProcessStates.Name, ProcessStates.Name, ProcessCommands.Start);
            AddState(ProcessStates.Name, ProcessStates.Feeling, ProcessCommands.Feeling);
            AddState(ProcessStates.Feeling, ProcessStates.Facts, ProcessCommands.Fact);

            AddState(ProcessStates.Facts, ProcessStates.Facts, ProcessCommands.Fact);
            AddState(ProcessStates.Facts, ProcessStates.Feeling, ProcessCommands.Feeling);
        }
    }
}
