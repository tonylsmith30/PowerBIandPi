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
        private GpioPin pin;
        private GpioPinValue pinValue;

        public MainPage()
        {
            errorMessage = "";
            this.InitializeComponent();
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

        private const int LED_PIN = 12;
        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pin = null;
                errorMessage = "There is no GPIO controller on this device.";
                return;
            }

            pin = gpio.OpenPin(LED_PIN);
            pinValue = GpioPinValue.High;
            pin.Write(pinValue);
            pin.SetDriveMode(GpioPinDriveMode.Output);

            errorMessage = "GPIO pin initialized correctly.";

        }

        private async void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            
            #region Code keeps Timing Out -- Using Amqp protocol
            try
            {
                string eventHubNamespace = "sensoreventhub";
                string eventHubName = "rasppihub";
                string policyName = "RootManageSharedAccessKey";
                string key = "9wHUrLBvdhHlzPhC86NO9lxY7DsYVj5AUEIp+i5v0uk=";
                //string partitionkey = "raspPiPartition";

                string currentDateTime = DateTime.Now.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
                string eventhubdata = "[\r\n{\r\n\"Sensor\" :\"Cube\",\r\n\"DateTime\" :\"" + currentDateTime + "\",\r\n\"Reading\" :10\r\n}\r\n]";

                Amqp.Address address = new Amqp.Address(//"Endpoint=sb://sensoreventhub.servicebus.windows.net/;SharedAccessKeyName=rasppihubaccess;SharedAccessKey=9hqpL/4hqJ2OE/d9NRRABBGH2BI82AgRqhZuDw5idhI=;EntityPath=rasppithub");
                string.Format("{0}.servicebus.windows.net", eventHubNamespace),
                5671, policyName, key);

                Amqp.Connection connection = await Amqp.Connection.Factory.CreateAsync(address); //new Amqp.Connection(address);
                Amqp.Session session = new Amqp.Session(connection);
                Amqp.SenderLink senderlink = new Amqp.SenderLink(session,
                    string.Format("send-link:{0}", eventHubName), eventHubName);

                Amqp.Message message = new Amqp.Message()
                {
                    BodySection = new Amqp.Framing.Data()
                    {
                        Binary = System.Text.Encoding.UTF8.GetBytes(eventhubdata)
                    }
                };

                message.MessageAnnotations = new Amqp.Framing.MessageAnnotations();
                //message.MessageAnnotations[new Amqp.Types.Symbol("x-opt-partition-key")] =
                //   string.Format("pk:", partitionkey);

                senderlink.Send(message, 120000);
            }
            catch(Exception ex)
            {
                string bad = "true";
            }
            #endregion
        }
    }
}
