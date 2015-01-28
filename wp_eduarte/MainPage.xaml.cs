using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

namespace wp_eduarte
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
            this.NavigationCacheMode = NavigationCacheMode.Required;
            this.wvBrowser.Source = new Uri("https://go-selfservice.educus.nl/app/login/Gilde%20Opleidingen?2");
            this.wvBrowser.Focus(FocusState);
            // Kijken of de gebruiker is ingelogd
            logincheck();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        // Terug knop
        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            // Na iedere afhandeling e.Handled = true; zetten!!!!!!!
            if (wvBrowser.CanGoBack == true)
            {
                wvBrowser.GoBack();
                e.Handled = true;
            }
            else
            {

            }
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }        

        private async void btn_main_signin_Click(object sender, RoutedEventArgs e)
        {
            // INLOGGEN
            // Gegevens stringen
            string un = tb_main_un.Text;
            string pw = tb_main_pw.Password;

            // HTML binnenhalen
            string html = Convert.ToString(htmlDownload("https://go-selfservice.educus.nl/app/login/Gilde%20Opleidingen?2"));

//#############
            // JS in <head> injecteren
            //html = html.Replace("<head>", "<head><script>document.getElementById(\"username\").value = \"" + un + "\";document.getAttribute(\"password\").value = \"" + pw + "\"; var e = document.getElementById('loginForm_hf_0'); e.name = 'aanmelden'; e.value = 'x'; var f = document.getElementById('loginForm'); var ff = f; if (ff.onsubmit != undefined) { if (ff.onsubmit() == false) return false; } f.submit(); e.value = ''; e.name = ''; return false; ");
            html = html.Replace("<head>", "<head><script>document.getElementById(\"username\").value = \"" + un + "\";document.getAttribute(\"password\").value = \"" + pw + "\";</script> ");

            // WebView maken
            WebView wv = new WebView();
            wv.NavigateToString(html);

            wvBrowser.NavigateToString(html);

            // Kijken of de gebruiker wordt doorgesluisd naar juiste pagina
            if (tb_main_un.Text == "" || tb_main_pw.Password == "")
            {
                MessageDialog msg = new MessageDialog("Een of meerdere velden zijn niet ingevuld.");
                await msg.ShowAsync();
            }
            else
            {
                try
                {

                    if (wv.Source.ToString() == "https://go-selfservice.educus.nl/app/Home/Start?2")
                    {
                        // WOOOOOOW!!! INGELOGD!!!!
                        // \/ Weghalen! \/
                        btn_main_signin.Content = "Succes";
                    }
                    MessageDialog msg = new MessageDialog(wv.Source.ToString());
                    await msg.ShowAsync();
                }
                catch (Exception)
                {
                    MessageDialog msg = new MessageDialog("Onjuiste inloggegevens, probeer het opnieuw.");
                    await msg.ShowAsync();
                }
            }
            
            
//#############
        }

        private void logincheck()
        {
            // Alle Pivots verbergen
            foreach (PivotItem pvt in pvt_master.Items)
            {
                // Zorgen dat de standaard niet wordt verborgen
                if (pvt.Name == "pvt_welkom")
                {
                    // Niks doen
                }
                else
                {
                    // Verbergen
// !->              pvt.Visibility = Visibility.Collapsed;
                }
            }

            // Kijken of de gebruiker is ingelogd
            /*if(blablabla == blablabla)
            {
                // Ingelogd -> Alles weergeven
                foreach (PivotItem pvt_item in pvt_master.Items)
                {
                    // Weergeven
                    pvt_item.Visibility = Visibility.Visible;
                }

                // Welkom grid weergeven
                grd_main.Visibility = Visibility.Visible;
                grd_signin.Visibility = Visibility.Visible;

            }*/
        }

        private async Task<string> htmlDownload(string url)
        {
            // Client handler maken
            HttpClientHandler httph = new HttpClientHandler { UseDefaultCredentials = true, AllowAutoRedirect = true };
            HttpClient httpc = new HttpClient(httph);

            // Http message maken
            HttpResponseMessage respmsg = await httpc.GetAsync(url);
            respmsg.EnsureSuccessStatusCode();

            // Html terugsturen
            return await respmsg.Content.ReadAsStringAsync();
        }

        private void btnGetCalendar_Click(object sender, RoutedEventArgs e)
        {
            wvCalendar.Navigate(new Uri("https://go-selfservice.educus.nl/app/Agenda/Dagrooster"));
        }

        private void btnResults_Click(object sender, RoutedEventArgs e)
        {
            wvResults.Navigate(new Uri("https://go-selfservice.educus.nl/app/Resultaten/RecenteResultaten"));
        }

        private void btnInternship_Click(object sender, RoutedEventArgs e)
        {
            wvInternship.Navigate(new Uri("https://go-selfservice.educus.nl/app/Stage/StagesOverzicht"));
        }

        private void btnAanwezig_Click(object sender, RoutedEventArgs e)
        {
            wvAttending.Navigate(new Uri("https://go-selfservice.educus.nl/app/Studie/Aanwezigheid"));
        }

        private void btnInformation_Click(object sender, RoutedEventArgs e)
        {
            wvInformation.Navigate(new Uri("https://go-selfservice.educus.nl/app/Mijngegevens/Personalia"));
        }
    }
}
