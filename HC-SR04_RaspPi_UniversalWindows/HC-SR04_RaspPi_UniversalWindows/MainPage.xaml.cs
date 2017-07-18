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
using Windows.Devices.Gpio;
using Windows.Web.Http;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HC_SR04_RaspPi_UniversalWindows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static string errorMessage;
        public MainPage()
        {
            this.InitializeComponent();
            errorMessage = "";
        }

        private async void ClickMe_Click(object sender, RoutedEventArgs e)
        {
            Task<string> returnedMessage = postData();
            string message = await returnedMessage;

            this.HelloMessage.Text = message;  
        }
        public static async Task<string> postData()
        {
            Uri uri = new Uri("https://api.powerbi.com/beta/72862590-c7d7-4a14-9db8-a58e052d0001/datasets/b631142f-602e-45a7-b160-f579a6d4b850/rows?key=q7bOLIcHBLtKW0GQ%2Bxxt2iieDgNxhUpw%2Fi4bEeuYJw2wW1MGw0NyEvL4bubcLLC1qNiyduJdaRexUkRRQLhX%2Bg%3D%3D");

            HttpClient client = new HttpClient();
            var headers = client.DefaultRequestHeaders;

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string currentDateTime = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
            string httpRequestBody = "[\r\n{\r\n\"Sensor\" :\"Cube\",\r\n\"DateTime\" :\"" + currentDateTime + "\",\r\n\"Reading\" :10\r\n}\r\n]";
            HttpStringContent httpRequestContent = new HttpStringContent(httpRequestBody);
            string httpResponseBody = "";

            try
            {
                httpResponse = await client.PostAsync(uri, httpRequestContent);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                return "Yay!";
            }
            catch (Exception ex)
            {
                errorMessage = "An Error Has Occured! " + ex.Message;
                return "Boo :(";
            }
        }
    }
}
