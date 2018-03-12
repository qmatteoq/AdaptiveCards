using AdaptiveCards.Rendering.Wpf;
using DesktopBridge;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using Windows.ApplicationModel.UserActivities;
using Windows.UI.Shell;

namespace AdaptiveCards.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserActivityChannel _userActivityChannel;
        private UserActivity _userActivity;
        private UserActivitySession _userActivitySession;
        private Helpers _desktopBridgeHelpers;

        string json;

        public MainWindow()
        {
            InitializeComponent();
            _desktopBridgeHelpers = new Helpers();
        }

        private async void OnRenderAdaptiveCard(object sender, RoutedEventArgs e)
        {
            AdaptiveCardRenderer renderer = new AdaptiveCardRenderer();

            //string result = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //int index = result.LastIndexOf("\\");
            //string jsonPath = $"{result.Substring(0, index)}\\adaptivecard.json";
            //TextReader tr = new StreamReader(jsonPath);
            //string json = await tr.ReadToEndAsync();

            HttpClient client = new HttpClient();
            json = await client.GetStringAsync("https://adaptivecard.azurewebsites.net/api/AppConsultAdaptiveCards?code=AzSEpdNE/P0c9OFIBjro2vSKwGIlLdBWdc53/jmR7Y9PX2l1Ks0/nQ==");
            AdaptiveCardParseResult card = AdaptiveCard.FromJson(json);

            var renderResult = renderer.RenderCard(card.Card);
            renderResult.OnAction += RenderResult_OnAction;

            if (renderResult != null)
            {
                MainPanel.Children.Add(renderResult.FrameworkElement);
            }
        }

        private async void RenderResult_OnAction(RenderedAdaptiveCard sender, AdaptiveActionEventArgs e)
        {
            if (e.Action.Type == "Action.OpenUrl")
            {
                if (_desktopBridgeHelpers.IsRunningAsUwp())
                {
                    _userActivity.ActivationUri = new Uri("adaptivecards://openLastPost");
                    _userActivity.VisualElements.DisplayText = "Windows AppConsult blog";
                    _userActivity.VisualElements.Content = AdaptiveCardBuilder.CreateAdaptiveCardFromJson(json);

                    await _userActivity.SaveAsync();
                    _userActivitySession?.Dispose();
                    _userActivitySession = _userActivity.CreateSession();
                }

                var action = e.Action as AdaptiveOpenUrlAction;
                Process.Start(action.Url.ToString());
            }
        }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (_desktopBridgeHelpers.IsRunningAsUwp())
            {
                _userActivityChannel = UserActivityChannel.GetDefault();
                _userActivity = await _userActivityChannel.GetOrCreateUserActivityAsync("NewBlogPost");
            }
        }
    }
}
