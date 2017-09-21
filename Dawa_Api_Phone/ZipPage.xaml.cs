using Dawa_Api_Phone.Common;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Dawa_Api_Phone
{

    public sealed partial class ZipPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ZipAndAdresseInfo zipAdresseInfo;
        private bool zipCodeIsLegit;

        // Page Flow: MainPage --> *ZipPage* --> StreetNamePage --> HouseNumberingPage --> MapPage

        public ZipPage()
        {

            zipAdresseInfo = new ZipAndAdresseInfo();

            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
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

        private void Search_button_Click(object sender, RoutedEventArgs e)
        {

            zipAdresseInfo.Zip = textBox_ZipCode.Text;

            if (zipCodeIsLegit == true)
            {
                Frame.Navigate(typeof(StreetNamePage), zipAdresseInfo);
            }
        }

        /// <summary>
        /// Input will be checked with Regular Expression(Numbers), color indicator(Red/Green)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_ZipCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            string zipcode_Text = textBox_ZipCode.Text;
            Regex regexZipcode = new Regex("^[0-9]+$");

            if (zipcode_Text.Length == 4 && regexZipcode.IsMatch(zipcode_Text))
            {
                textBox_ZipCode.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Green);
                textBox_ZipCode.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                zipCodeIsLegit = true;
            }
            else
            {
                textBox_ZipCode.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
                textBox_ZipCode.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                zipCodeIsLegit = false;
            }
        }
    }
}
