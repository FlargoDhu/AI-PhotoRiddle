using Android.Media;
using APR_APP.Models;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APR_APP.Views
{

   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RiddleLeader : ContentPage
    {
        MqttFactory factory = new MqttFactory();
        IMqttClient mqttClient;
        IMqttClientOptions options;
        string ID_RIDDLEUSER;


        public RiddleLeader()
        {
            InitializeComponent();
            options = new MqttClientOptionsBuilder()
                .WithTcpServer("10.0.0.103", 1883) // Port is optional
                .Build();
            ID_RIDDLEUSER = System.IO.File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "reg.txt"));

            Connect(options);

            Task.Delay(300).Wait();

            mqttClient.PublishAsync("APR/" + (ID_RIDDLEUSER) + "/" + "GET_L");

            QuestionPass.Clicked += QuestionPass_Clicked;
        }

        private async void QuestionPass_Clicked(object sender, EventArgs e)
        {

            await mqttClient.DisconnectAsync();
            await Navigation.PushModalAsync(new NavigationPage(new RiddlePage()));
        }

        async Task Connect(IMqttClientOptions options)
        {
            mqttClient = factory.CreateMqttClient();

            mqttClient.UseConnectedHandler(async e =>
            {

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("APR/" + (ID_RIDDLEUSER) + "/" + "RES_L").Build());


            });

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                if (e.ApplicationMessage.Topic == "APR/" + (ID_RIDDLEUSER) + "/" + "RES_L")
                {
                    var msg = System.Text.Encoding.UTF8.GetString(e.ApplicationMessage.Payload).Split(new string[] { ".", "/" }, StringSplitOptions.None);
                    Labels_0.Text = msg[0] + " Punkty: " + msg[1];
                    Labels_1.Text = msg[2] + " Punkty: " + msg[3];
                    Labels_2.Text = msg[4] + " Punkty: " + msg[5];
                    Labels_3.Text = msg[6] + " Punkty: " + msg[7];
                    Labels_4.Text = msg[8] + " Punkty: " + msg[9];
                    Labels_5.Text = msg[10] + " Punkty: " + msg[11];
                    Labels_6.Text = msg[12] + " Punkty: " + msg[13];
                    Labels_7.Text = msg[14] + " Punkty: " + msg[15];
                    Labels_8.Text = msg[16] + " Punkty: " + msg[17];
                    Labels_9.Text = msg[18] + " Punkty: " + msg[19];
                    if (msg.Length > 20)
                    {
                        Labels_10.Text = msg[20] + " Punkty: " + msg[21];
                    }
                }
            });

            await mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None);



            /**/
        }
    }
}