using AdaptiveCards.Rendering.Uwp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.UserActivities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Shell;
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
        private UserActivityChannel _userActivityChannel;
        private UserActivity _userActivity;
        private UserActivitySession _userActivitySession;
        string json;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            _userActivityChannel = UserActivityChannel.GetDefault();
            _userActivity = await _userActivityChannel.GetOrCreateUserActivityAsync("NewBlogPost");

            if (e.Parameter != null)
            {
                HttpClient client = new HttpClient();
                json = await client.GetStringAsync(new Uri("https://adaptivecard.azurewebsites.net/api/AppConsultAdaptiveCards?code=AzSEpdNE/P0c9OFIBjro2vSKwGIlLdBWdc53/jmR7Y9PX2l1Ks0/nQ=="));
                AdaptiveCard card = AdaptiveCard.FromJsonString(json).AdaptiveCard;

                var action = card.Actions.FirstOrDefault();
                if (action != null)
                {
                    if (action.ActionType == ActionType.OpenUrl)
                    {
                        var urlAction = action as AdaptiveOpenUrlAction;
                        await Launcher.LaunchUriAsync(urlAction.Url);
                    }
                }
            }
        }

        private async void OnRenderAdaptiveCard(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            json = await client.GetStringAsync(new Uri("https://adaptivecard.azurewebsites.net/api/AppConsultAdaptiveCards?code=AzSEpdNE/P0c9OFIBjro2vSKwGIlLdBWdc53/jmR7Y9PX2l1Ks0/nQ=="));
            AdaptiveCardParseResult card = AdaptiveCard.FromJsonString(json);

            AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();
            var renderResult = renderer.RenderAdaptiveCard(card.AdaptiveCard);
            renderResult.Action += RenderResult_OnAction;

            if (renderResult != null)
            {
                MainPanel.Children.Add(renderResult.FrameworkElement);
            }
        }

        private async void RenderResult_OnAction(RenderedAdaptiveCard sender, AdaptiveActionEventArgs e)
        {
            if (e.Action.ActionType == ActionType.OpenUrl)
            {
                _userActivity.ActivationUri = new Uri("adaptivecards://openLastPost");
                _userActivity.VisualElements.DisplayText = "Windows AppConsult blog";
                _userActivity.VisualElements.Content = AdaptiveCardBuilder.CreateAdaptiveCardFromJson(json);

                await _userActivity.SaveAsync();
                _userActivitySession?.Dispose();
                _userActivitySession = _userActivity.CreateSession();

                var action = e.Action as AdaptiveOpenUrlAction;
                await Launcher.LaunchUriAsync(action.Url);
            }
        }
    }
}
