using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using AdaptiveCards.Parser;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AdaptiveCards.Function
{
    public static class AdaptiveCardsFunction
    {
        [FunctionName("AppConsultAdaptiveCards")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            FeedParser feedParser = new FeedParser();
            List<Item> items = feedParser.Parse("https://blogs.msdn.microsoft.com/appconsult/feed/", FeedType.RSS);

            var lastNews = items.OrderByDescending(x => x.PublishDate).FirstOrDefault();

            AdaptiveCard card = new AdaptiveCard();

            AdaptiveTextBlock title = new AdaptiveTextBlock
            {
                Text = lastNews.Title,
                Size = AdaptiveTextSize.Medium,
                Wrap = true
            };

            AdaptiveColumnSet columnSet = new AdaptiveColumnSet();

            AdaptiveColumn photoColumn = new AdaptiveColumn
            {
                Width = "auto"
            };
            AdaptiveImage image = new AdaptiveImage
            {
                Url = new Uri("https://pbs.twimg.com/profile_images/587911661526327296/ZpWZRPcp_400x400.jpg"),
                Size = AdaptiveImageSize.Small,
                Style = AdaptiveImageStyle.Person
            };
            photoColumn.Items.Add(image);

            AdaptiveTextBlock name = new AdaptiveTextBlock
            {
                Text = "Matteo Pagani",
                Weight = AdaptiveTextWeight.Bolder,
                Wrap = true
            };

            AdaptiveTextBlock date = new AdaptiveTextBlock
            {
                Text = lastNews.PublishDate.ToShortDateString(),
                IsSubtle = true,
                Spacing = AdaptiveSpacing.None,
                Wrap = true
            };

            AdaptiveColumn authorColumn = new AdaptiveColumn
            {
                Width = "stretch"
            };
            authorColumn.Items.Add(name);
            authorColumn.Items.Add(date);

            columnSet.Columns.Add(photoColumn);
            columnSet.Columns.Add(authorColumn);

            AdaptiveTextBlock body = new AdaptiveTextBlock
            {
                Text = $"{lastNews.Content.Substring(0, 150)}...",
                Wrap = true
            };

            AdaptiveOpenUrlAction action = new AdaptiveOpenUrlAction
            {
                Url = new Uri(lastNews.Link),
                Title = "View"
            };

            card.Body.Add(title);
            card.Body.Add(columnSet);
            card.Body.Add(body);
            card.Actions.Add(action);

            string json = card.ToJson();

            return new OkObjectResult(json);
        }
    }
}
