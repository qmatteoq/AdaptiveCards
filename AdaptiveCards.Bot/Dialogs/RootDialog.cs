using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace AdaptiveCards.Bot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            HttpClient client = new HttpClient();
            string json = await client.GetStringAsync("https://adaptivecard.azurewebsites.net/api/AppConsultAdaptiveCards?code=AzSEpdNE/P0c9OFIBjro2vSKwGIlLdBWdc53/jmR7Y9PX2l1Ks0/nQ==");

            Attachment attachment = new Attachment();
            attachment.ContentType = AdaptiveCard.ContentType;
            attachment.Content = AdaptiveCard.FromJson(json).Card;


            var returnMessage = context.MakeMessage();
            returnMessage.Attachments.Add(attachment);

            await context.PostAsync(returnMessage);

            context.Wait(MessageReceivedAsync);
        }
    }
}