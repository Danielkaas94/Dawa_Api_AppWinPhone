using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Dawa_Api_Phone
{
    public sealed partial class MainPage : Page
    {
        private ZipAndAdresseInfo zipAdresseInfo;

        private bool nameIsLegit;
        private bool phoneNumberIsLegit;
        private bool emailIsLegit;

        // Page Flow: *MainPage* --> ZipPage --> StreetNamePage --> HouseNumberingPage --> MapPage

        public MainPage()
        {
            zipAdresseInfo = new ZipAndAdresseInfo();

            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        /// <summary>
        /// if bools are true, the values is being saves in zipAdresseInfo object and sended to the next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Next_Click(object sender, RoutedEventArgs e)
        {
            if (phoneNumberIsLegit == true && nameIsLegit == true && emailIsLegit == true)
            {
                zipAdresseInfo.Name = textBox_Name.Text;
                zipAdresseInfo.PhoneNumber = textBox_Cellphone.Text;
                zipAdresseInfo.Email = textBox_Email.Text;
                Frame.Navigate(typeof(ZipPage), zipAdresseInfo); // Navigate to the next page.
            }
            else
            {
                PaintAllBordersRed();
            }
        }

        /// <summary>
        /// Input will be checked with Regular Expression(Name), color indicator(Red/Green)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Name_TextChanged(object sender, TextChangedEventArgs e) // Navn
        {
            string Name_Text = textBox_Name.Text;
            Regex regexName = new Regex("[A-Za-z.'\\-\\p{L}\\p{Zs}\\p{Lu}\\p{Ll}\']+$");

            if (regexName.IsMatch(Name_Text))
            {
                ChangeNameBorderGreen();
            }
            else
            {
                ChangeNameBorderRed();
            }
        }

        /// <summary>
        /// Input will be checked with Regular Expression(Numbers), color indicator(Red/Green)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Cellphone_TextChanged(object sender, TextChangedEventArgs e) // Telefon nr.
        {
            string cellPhone_Text = textBox_Cellphone.Text;
            Regex regexPhoneNumber = new Regex("^[0-9]+$");

            if (cellPhone_Text.Length == 8 && regexPhoneNumber.IsMatch(textBox_Cellphone.Text))
            {
                ChangePhoneBorderGreen();
            }
            else
            {
                ChangePhoneBorderRed();
            }
        }

        /// <summary>
        /// Input will be checked with Regular Expression(Email), color indicator(Red/Green)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_Email_TextChanged(object sender, TextChangedEventArgs e) // E-mail
        {
            string email_Text = textBox_Email.Text;

            string strRegexEmail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

            Regex regexEmail = new Regex(strRegexEmail);

            if (regexEmail.IsMatch(email_Text))
            {
                ChangeEmailBorderGreen();
            }
            else
            {
                ChangeEmailBorderRed();
            }
        }

        #region ChangeBorderColor


        private void PaintAllBordersRed()
        {
            textBox_Name.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            textBox_Cellphone.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            textBox_Email.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        }

        /* Name */

        private void ChangeNameBorderGreen()
        {
            textBox_Name.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Green);
            textBox_Name.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            nameIsLegit = true;
        }

        private void ChangeNameBorderRed()
        {
            textBox_Name.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            textBox_Name.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            nameIsLegit = false;
        }

        /* Phone */

        private void ChangePhoneBorderGreen()
        {
            textBox_Cellphone.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Green);
            textBox_Cellphone.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            phoneNumberIsLegit = true;
        }

        private void ChangePhoneBorderRed()
        {
            textBox_Cellphone.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            textBox_Cellphone.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            phoneNumberIsLegit = false;
        }

        /* Email */

        private void ChangeEmailBorderGreen()
        {
            textBox_Email.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Green);
            textBox_Email.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
            emailIsLegit = true;
        }

        private void ChangeEmailBorderRed()
        {
            textBox_Email.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            textBox_Email.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
            emailIsLegit = false;
        }


        #endregion
    }
}
