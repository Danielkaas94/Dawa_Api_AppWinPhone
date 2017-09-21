using Dawa_Api_Phone.Common;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace Dawa_Api_Phone
{

    public sealed partial class StreetNamePage : Page
    {
        private static HttpClient _client;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ZipAndAdresseInfo zipAdresseInfo;

        // Page Flow: MainPage --> ZipPage --> *StreetNamePage* --> HouseNumberingPage --> MapPage

        public StreetNamePage()
        {
            zipAdresseInfo = new ZipAndAdresseInfo();

            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);

            _client = new HttpClient();
            _client.Timeout = new TimeSpan(0, 20, 0);
            _client.BaseAddress = new Uri("https://dawa.aws.dk/");

            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private void LoadStreetNameFromZipCode(string zipcodeValue) // 
        {
            try
            {
                string url = "vejnavne?postnr=" + zipcodeValue;
                HttpResponseMessage response = _client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic adresser = JArray.Parse(responseBody);
                foreach (dynamic adresse in adresser)
                {
                    ListBoxItem ListboxItem_StreetName = new ListBoxItem();
                    ListboxItem_StreetName.Content = FormatAdresse(adresse);
                    ListboxItem_StreetName.FontSize = 15;
                    listBox_StreetNames.Items.Add(ListboxItem_StreetName);

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string FormatAdresse(dynamic adresse)
        {
            return string.Format($"{adresse.navn}");
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            zipAdresseInfo = e.NavigationParameter as ZipAndAdresseInfo;

            LoadStreetNameFromZipCode(zipAdresseInfo.Zip);
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        /// <summary>
        /// The value being tapped is being saves in zipAdresseInfo object and sended to the next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_StreetNames_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (listBox_StreetNames.SelectedItem != null)
            {
                ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
                zipAdresseInfo.Adress = lbi.Content.ToString();
            }
            Frame.Navigate(typeof(HouseNumberingPage), zipAdresseInfo);
        }
    }
}
