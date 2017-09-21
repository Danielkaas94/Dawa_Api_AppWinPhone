using Dawa_Api_Phone.Common;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Dawa_Api_Phone
{

    public sealed partial class HouseNumberingPage : Page
    {

        private static HttpClient _client;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ZipAndAdresseInfo zipAdresseInfo;

        // Page Flow: MainPage --> ZipPage --> StreetNamePage --> *HouseNumberingPage* --> MapPage

        public HouseNumberingPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);

            _client = new HttpClient();
            _client.Timeout = new TimeSpan(0,20,0);
            _client.BaseAddress = new Uri("https://dawa.aws.dk/");

            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
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

            LoadHouseNumbersFromValues(zipAdresseInfo);
        }



        // Test URL = https://dawa.aws.dk/adresser?vejnavn=Absalonsvej&postnr=8800&struktur=mini
        private void LoadHouseNumbersFromValues(ZipAndAdresseInfo zipAndAdresseInfo) // Testing
        {
            try
            {
                string url = "adresser?vejnavn=" + zipAndAdresseInfo.Adress + "&postnr=" + zipAndAdresseInfo.Zip + "&struktur=mini";
                HttpResponseMessage response = _client.GetAsync(url).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic houseNumbers = JArray.Parse(responseBody);
                foreach (dynamic houseNr in houseNumbers)
                {
                    ListBoxItem listboxItem_HouseNumber = new ListBoxItem();
                    listboxItem_HouseNumber.Content = FormatHouseNumber(houseNr);
                    listboxItem_HouseNumber.FontSize = 15;
                    listBoxHouseNumbers.Items.Add(listboxItem_HouseNumber);
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        private string FormatHouseNumber(dynamic houseNr)
        {
            return string.Format($"{houseNr.husnr}");
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
        private void listBoxHouseNumbers_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (listBoxHouseNumbers.SelectedItem != null)
            {
                //zipAdresseInfo.adress = listBox_StreetNames.SelectedItem.ToString(); // Not working
                ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
                zipAdresseInfo.HouseNr = lbi.Content.ToString();
            }
            Frame.Navigate(typeof(MapPage), zipAdresseInfo);
        }
    }
}
