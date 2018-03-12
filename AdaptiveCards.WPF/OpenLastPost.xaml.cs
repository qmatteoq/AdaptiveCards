using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace AdaptiveCards.WPF
{
    /// <summary>
    /// Interaction logic for OpenLastPost.xaml
    /// </summary>
    public partial class OpenLastPost : Window
    {
        public OpenLastPost()
        {
            InitializeComponent();
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            string json = await client.GetStringAsync("https://adaptivecard.azurewebsites.net/api/AppConsultAdaptiveCards?code=AzSEpdNE/P0c9OFIBjro2vSKwGIlLdBWdc53/jmR7Y9PX2l1Ks0/nQ==");
            AdaptiveCard card = AdaptiveCard.FromJson(json).Card;

            var action = card.Actions.FirstOrDefault();
            if (action != null)
            {
                if (action.Type == "Action.OpenUrl")
                {
                    var urlAction = action as AdaptiveOpenUrlAction;
                    Process.Start(urlAction.Url.ToString());
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
