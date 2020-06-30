using Android.OS;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APR_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        MqttFactory factory = new MqttFactory();
        IMqttClient mqttClient;

        public RegisterPage()
        {
            InitializeComponent();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("10.0.0.103", 1883) // Port is optional
                .Build();

            Connect(options);

            System.Threading.Thread.Sleep(1000);

            if (mqttClient.IsConnected)
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("APR/REGISTER")
                    .Build();
                mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
                Console.WriteLine("REGISTERED");
            }
            else
            {
                Console.WriteLine("NOT REGISTERED");
                return;
            }

            
        }


        async Task Connect(IMqttClientOptions options)
        {
            mqttClient = factory.CreateMqttClient();

            mqttClient.UseConnectedHandler(async e =>
            {

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("APR/REGISTER_RES").Build());

            });

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
               if(e.ApplicationMessage.Topic == "APR/REGISTER_RES")
                {
                    var reg_string = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    System.IO.File.WriteAllText(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "reg.txt"), reg_string);
                }
            });

            await mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None);

            

            /**/
        }


    }
}