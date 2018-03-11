using AdaptiveCards.Rendering.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AdaptiveCards.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OnRenderAdaptiveCard(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            string json = await client.GetStringAsync(new Uri("https://adaptivecard.azurewebsites.net/api/AppConsultAdaptiveCards?code=AzSEpdNE/P0c9OFIBjro2vSKwGIlLdBWdc53/jmR7Y9PX2l1Ks0/nQ=="));
            AdaptiveCardParseResult card = AdaptiveCard.FromJsonString(json);

            AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();
            var renderResult = renderer.RenderAdaptiveCard(card.AdaptiveCard);

            if (renderResult != null)
            {
                MainPanel.Children.Add(renderResult.FrameworkElement);
            }
        }
    }
}
