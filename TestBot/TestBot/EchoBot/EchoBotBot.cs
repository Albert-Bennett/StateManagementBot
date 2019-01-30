using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EchoBot
{
    public class EchoBotBot : IBot
    {
        EchoBotAccessors accessors;
        ILogger log;

        LuisParsing luisParsing = new LuisParsing();

        public EchoBotBot(EchoBotAccessors accessors, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            log = loggerFactory.CreateLogger<EchoBotBot>();
            log.LogTrace("Turn start.");
            this.accessors = accessors ?? throw new ArgumentNullException(nameof(accessors));
        }

        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                CounterState state = await accessors.CounterState.GetAsync(turnContext, () => new CounterState());
                state.TurnCount++;

                await accessors.CounterState.SetAsync(turnContext, state);
                await accessors.ConversationState.SaveChangesAsync(turnContext);

                string responseMessage = await MakeRequest(turnContext.Activity.Text);

                await turnContext.SendActivityAsync(responseMessage);
            }
            else
            {
                await turnContext.SendActivityAsync($"{turnContext.Activity.Type} event detected");
            }
        }

        async Task<string> MakeRequest(string textToAnalyze)
        {
            HttpClient client = new HttpClient();
            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);

            string endpointKey = "4a492486d9e2470aba2a4995d434b6c0";

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", endpointKey);

            queryString["q"] = textToAnalyze;

            string endpointUri = "https://westeurope.api.cognitive.microsoft.com/luis/v2.0/apps/537785b3-dae5-4095-8e5d-14ebfca59b5f?verbose=true&timezoneOffset=60&subscription-key=4a492486d9e2470aba2a4995d434b6c0&" + queryString;
            HttpResponseMessage response = await client.GetAsync(endpointUri);

            string jsonContent = await response.Content.ReadAsStringAsync();

            return luisParsing.ParseLuis(jsonContent);
        }
    }
}