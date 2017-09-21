using Dawa_Api_Phone.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Contacts;
using Newtonsoft.Json.Linq;


namespace Dawa_Api_Phone
{

    public sealed partial class MapPage : Page
    {
        private static HttpClient _client;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ZipAndAdresseInfo zipAdresseInfo;

        // Page Flow: MainPage --> ZipPage --> StreetNamePage --> HouseNumberingPage --> *MapPage*

        public MapPage()
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
            // Get Values
            zipAdresseInfo = e.NavigationParameter as ZipAndAdresseInfo;

            GetLocationFromValues(zipAdresseInfo);
        }

        private void GetLocationFromValues(ZipAndAdresseInfo zipAndAdresseInfo)
        {
            try
            {
                string url = "adresser?vejnavn=" + zipAndAdresseInfo.Adress + "&postnr=" + zipAndAdresseInfo.Zip + "&husnr=" + zipAndAdresseInfo.HouseNr + "&struktur=mini";
                HttpResponseMessage response = _client.GetAsync(url).Result;
                string responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic location = JArray.Parse(responseBody); // Maybe not JArray
                // Get x & y coordinate, then make it to a BasicGeoposition
                BasicGeoposition MapGeoposition = new BasicGeoposition();
                foreach (var place in location)
                {
                    MapGeoposition = GetBasicGeopsition(place);
                }
                    UseBasicGeopositionOnMap(MapGeoposition);
 
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void UseBasicGeopositionOnMap(BasicGeoposition mapGeoposition)
        {
            Geopoint location = new Geopoint(mapGeoposition);

            var mapIcon = new MapIcon
            {
                Location = location,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = zipAdresseInfo.Adress + " " + zipAdresseInfo.HouseNr, // Titel på Ikon
                ZIndex = 1,
                Visible = true,
            };
            // Add the MapIcon to the map.
            Map.MapElements.Add(mapIcon);

            // Center the map over the POI.
            Map.Center = location;
            Map.ZoomLevel = 16;
            Map.LandmarksVisible = true;
        }

        /// <summary>
        /// Format the x & y coordinate to double, Saves ZipCityName to zipAdresseInfo object
        /// </summary>
        /// <param name="location">JSON Input</param>
        /// <returns></returns>
        private BasicGeoposition GetBasicGeopsition(dynamic location)
        {
            double latitude = double.Parse(string.Format($"{location.y}"));
            double longitude = double.Parse(string.Format($"{location.x}"));
            zipAdresseInfo.ZipCityName = string.Format($"{location.postnrnavn}"); // ZipName

            BasicGeoposition localGeo = new BasicGeoposition() { Latitude = latitude, Longitude = longitude };
            return localGeo;
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

        private void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void button_Mail_Click(object sender, RoutedEventArgs e)
        {
            ComposeEmail();
        }

        private async void ComposeEmail()
        {
            var contactPicker = new ContactPicker();
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
            Contact recipent = await contactPicker.PickContactAsync();
            if (recipent != null)
            {
                this.AppendContactFieldValues(recipent.Emails);
            }
            else
            {
            }
        }

        private async void AppendContactFieldValues<T>(IList<T> fields)
        {
            if (fields.Count > 0)
            {
                if (fields[0].GetType() == typeof(ContactEmail))
                {
                    foreach (ContactEmail email in fields as IList<ContactEmail>)
                    {
                        var emailMessage = new Windows.ApplicationModel.Email.EmailMessage();
                        emailMessage.Body = string.Format($"Navn: {zipAdresseInfo.Name} \nTelefon: {zipAdresseInfo.PhoneNumber} \nEmail: {zipAdresseInfo.Email} \nAdresse: {zipAdresseInfo.Adress} {zipAdresseInfo.HouseNr} \nPostnr & By: {zipAdresseInfo.Zip} {zipAdresseInfo.ZipCityName}"); // Brødtekst
                        emailMessage.Subject = "Dawa App Windows Phone 8.1"; // Titel
                        var emailRecipent = new EmailRecipient(email.Address);
                        emailMessage.To.Add(emailRecipent);
                        await EmailManager.ShowComposeNewEmailAsync(emailMessage);
                        break;
                    }
                }
            }
            else
            {
            }
        }
    }
}
