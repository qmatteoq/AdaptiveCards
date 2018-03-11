using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdaptiveCards.Rendering.Html;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdaptiveCards.Web.Pages
{
    public class AdaptiveCardModel : PageModel
    {
        public HtmlString AdaptiveCard;

        public async Task OnGet()
        {
            HttpClient client = new HttpClient();
            string json = await client.GetStringAsync("https://adaptivecard.azurewebsites.net/api/AppConsultAdaptiveCards?code=AzSEpdNE/P0c9OFIBjro2vSKwGIlLdBWdc53/jmR7Y9PX2l1Ks0/nQ==");
            AdaptiveCardParseResult card = AdaptiveCards.AdaptiveCard.FromJson(json);
            AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();
            RenderedAdaptiveCard renderedCard = renderer.RenderCard(card.Card);

            // Get the output HTML 
            AdaptiveCard = new HtmlString(renderedCard.Html.ToString());
        }
    }
}