using EchoBot.Controller.StateManagement;
using EchoBot.Model.StateMachine;
using Newtonsoft.Json;
using System;

namespace EchoBot
{
    public class LuisParsing
    {
        static ConversationalStateMachine stateMachine = new ConversationalStateMachine();

        string userName;
        string currentFeeling;

        public string ParseLuis(string luisResult)
        {
            RecognizerResult recognizerResult = JsonConvert.DeserializeObject<RecognizerResult>(luisResult);

            string result = string.Empty;

            switch (recognizerResult.Intent.Intent)
            {
                case "FindNames":
                    {
                        TransitionResult<ProcessStates> res = stateMachine.MoveToNextState(ProcessCommands.Start);

                        if (res.IsValid)
                            result = SetFirstName(recognizerResult.Entities);
                        else
                            result = "Come on now, we have already been through this.";
                    }
                    break;

                case "FindFeeling":
                    {
                        TransitionResult<ProcessStates> res = stateMachine.MoveToNextState(ProcessCommands.Feeling);

                        if (res.IsValid)
                            result = ProcessFeeling(recognizerResult.Entities);
                        else
                            result = "Come on now. Names first, introductions are important these days.";
                    }
                    break;

                case "SayFact":
                    {
                        TransitionResult<ProcessStates> res = stateMachine.MoveToNextState(ProcessCommands.Fact);

                        if (res.IsValid)
                            result = GetRandomFact();
                        else
                            result = "Nope, names first!!";
                    }
                    break;

                default:
                    result = "Have you tried typing something. You know... something that I can understand.";
                    break;
            }

            string state = Enum.GetName(typeof(ProcessStates), stateMachine.CurrentState);


            return "[" + state + "]: " + result;
        }

        string ProcessFeeling(RecognizerEntity[] entities)
        {
            currentFeeling = "undecided";

            if (entities.Length > 0)
            {
                currentFeeling = entities[0].Entity;

                switch (entities[0].Type)
                {
                    case "positiveFeeling":
                        return "Whao, whao, whao... too much info.";

                    case "neturalFeeling":
                        return "Yeah, I know the feeling...";

                    case "negativeFeeling":
                        return "Awww, you poor creature.";
                }
            }

            return "Sharing is caring.";
        }

        string SetFirstName(RecognizerEntity[] entities)
        {
            string res = "Sorry, what was your name again?";

            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i].Type == "firstName")
                {
                    userName = entities[i].Entity;
                    res = "Hi, " + entities[i].Entity + ". How are you today?";
                    break;
                }
            }

            return res;
        }

        string GetRandomFact()
        {
            string[] facts = new string[]
            {
                "Sheep hibernate underground in the winter.",
                "Horses are a type of Dog.",
                "Fishes, are actually afraid of grass.",
                "The internet is a black box with a blinking red light on it.",
                "Water isn't always wet.",
                "You can drink lava, but only once."
            };

            int index = new Random().Next(0, facts.Length);

            return facts[index];
        }
    }
}
