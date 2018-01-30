# Dawa-API App Windows Phone 8.1
Windows Phone 8.1 using the dawa-api, with map features &amp; Email 📧📜📲📄🌍

### API-Page: https://dawa.aws.dk/

### Sketch: https://xd.adobe.com/view/47368fca-5b8c-4818-bd5f-d8e27e7db7c0/

# Hello World! Coders! 💻

Greetings all sentient beings! Okay, let's do it! As you can see, it is an app for Windows Phone 8.1 written in C# code.
You can find every danish adress with this app. 

First of all. 
I want to say, that if you use an emulator, you cannot use the E-mail part, unless you have an account with some few contacts.
In the code I have made a little map embedded into the //Comment, to help you to find out, what is the flow of the program.


Here is an example at line 17 from "MainPage.xaml.cs": 👀
```csharp
// Page Flow: *MainPage* --> ZipPage --> StreetNamePage --> HouseNumberingPage --> MapPage
```
The asterisk-stars (*Page*), tells what is the current page you are on, the arrow points the next Page.


## Navigation  🌍

How I go from MainPage to ZipPage is very simple. I use Frame.Navigate(typeof(PageName), objectIWantToSend);


Here is an example at line 56 from "MainPage.xaml.cs": 👀
```csharp
Frame.Navigate(typeof(ZipPage), zipAdresseInfo);
```
## Final Note! 📜📌

As A final note, I want to say that my app is potent with RegularExpressions.
```csharp
Regex regexPhoneNumber = new Regex("^[0-9]+$");
Regex regexName = new Regex("[A-Za-z.'\\-\\p{L}\\p{Zs}\\p{Lu}\\p{Ll}\']+$");

string strRegexEmail = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" + @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" + @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
Regex regexEmail = new Regex(strRegexEmail);
```
So you need to write a valid Name, PhoneNumber & Email to pass on to the next page.
Do not worry, nothing is being saved, it's not a scam :-)
