using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APR_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RiddlePage : ContentPage
    {
        string[] reg_string = null;
        MqttFactory factory = new MqttFactory();
        IMqttClient mqttClient;
        string ID_RIDDLEUSER;
        bool from_photo;

        static string subscriptionKey = "ec2d27ff7e28441381e9c180f6dc2806";
        static string endpoint = "https://flargoaprhavefun.cognitiveservices.azure.com/";
        private string value = null;
        private HttpClient client;
        IMqttClientOptions options;
        public RiddlePage()
        {
            InitializeComponent();

            ID_RIDDLEUSER = System.IO.File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "reg.txt"));

            QuestionPass.Clicked += QuestionPass_Clicked;
            LeaderboardPass.Clicked += LeaderboardPass_Clicked;

            options = new MqttClientOptionsBuilder()
                .WithTcpServer("10.0.0.103", 1883) // Port is optional
                .Build();

            Connect(options);


            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Host", "flargoaprhavefun.cognitiveservices.azure.com");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            CameraButton.Clicked += CameraButton_Clicked;
            GalleryButton.Clicked += GalleryButton_Clicked;

        }

        /*protected async override void OnAppearing()
        {
            if (from_photo = false)
            {
                if (mqttClient.IsConnected)
                {
                    mqttClient.DisconnectAsync();
                }
                Connect(options);

                reg_string = null;
            }

        }*/
        /*protected override void OnAppearing()
        {
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("10.0.0.103", 1883) // Port is optional
                .Build();

            Connect(options);

            reg_string = null;
        }*/

        async Task Connect(IMqttClientOptions options)
        {
            mqttClient = factory.CreateMqttClient();

            mqttClient.UseConnectedHandler(async e =>
            {

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("APR/" + (ID_RIDDLEUSER) + "/" + "RIDDLE_RES").Build());
               

            });

            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                if (e.ApplicationMessage.Topic == "APR/" + (ID_RIDDLEUSER) + "/" + "RIDDLE_RES")
                {
                   reg_string = Encoding.UTF8.GetString(e.ApplicationMessage.Payload).Split('/');
                   
                }
            });

            await mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None);



            /**/
        }

        private async void QuestionPass_Clicked(object sender, EventArgs e)
        {
            
            if (mqttClient.IsConnected && reg_string == null)
            {
                var message = new MqttApplicationMessageBuilder()
                    .WithTopic("APR/" + (ID_RIDDLEUSER) + "/" + "GEN_RIDDLE")
                    .Build();
                await mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);

                //mqttClient.DisconnectAsync();
            }
            else
            {
                RiddleText.Text = reg_string[0] + " Odpowiedz: " + reg_string[1];

            }
        }

        private async void LeaderboardPass_Clicked(object sender, EventArgs e)
        {
            if (mqttClient.IsConnected)
            {
                await mqttClient.DisconnectAsync();
            }
            await Navigation.PushModalAsync(new NavigationPage(new RiddleLeader()));
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            if (reg_string != null)
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions());
                if (photo != null)
                {
                    PhotoImage.Source = Xamarin.Forms.ImageSource.FromStream(() => { return photo.GetStream(); });
                    string requesturi = "https://flargoaprhavefun.cognitiveservices.azure.com/vision/v3.0/analyze?visualFeatures=Objects&language=en";
                    var response = await SendRequestAsync<Stream, string>(new HttpMethod("POST"), requesturi, photo.GetStream());
                    if (value != null)
                    {
                        string random = "\"";
                        var value_array = value.Split(new string[] { ":", random, "," }, StringSplitOptions.None);
                        var i = 0;
                        bool found = false;
                        foreach (string item in value_array)
                        {
                            Console.WriteLine(item);
                            if(item == reg_string[1])
                            {
                                found = true;
                                RiddleResult.Text = value_array[i+2].ToString();
                                Console.WriteLine("Odpowiedz: " + value_array[i + 2].ToString());
                            }
                            i++;
                        }
                        if(found == false)
                        {
                            RiddleResult.Text = "You lost";
                            var photo_2 = Xamarin.Forms.ImageSource.FromStream(() => { return photo.GetStream(); });
                            await Navigation.PushModalAsync(new NavigationPage(new ResultPage(photo_2, "You Lost")));
                        }
                        else
                        {
                            //Connect(options);
                            RiddleResult.Text = "You Won";
                            var message = new MqttApplicationMessageBuilder()
                                .WithTopic("APR/" + (ID_RIDDLEUSER) + "/" + "UPDATE_SCORES")
                                .WithPayload(reg_string[2]+"/"+reg_string[3]+"/"+"0")
                                .Build();
                            mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
                            //mqttClient.DisconnectAsync();
                            var photo_2 = Xamarin.Forms.ImageSource.FromStream(() => { return photo.GetStream(); });
                            await Navigation.PushModalAsync(new NavigationPage(new ResultPage(photo_2, "You Won")));
                        }
                        reg_string = null;
                        RiddleText.Text = "Press Question Again";

                    }
                    //Console.WriteLine(response);
                }
            }
            from_photo = false;
        }

        private async void GalleryButton_Clicked(object sender, EventArgs e)
        {
            if (reg_string != null)
            {
                from_photo = true;
                var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions() { });
                
                if (photo != null)
                {
                    PhotoImage.Source = Xamarin.Forms.ImageSource.FromStream(() => { return photo.GetStream(); });
                    string requesturi = "https://flargoaprhavefun.cognitiveservices.azure.com/vision/v3.0/analyze?visualFeatures=Objects&language=en";
                    var response = await SendRequestAsync<Stream, string>(new HttpMethod("POST"), requesturi, photo.GetStream());
                    if (value != null)
                    {
                        string random = "\"";
                        var value_array = value.Split(new string[] { ":", random, "," }, StringSplitOptions.None);
                        var i = 0;
                        bool found = false;
                        foreach (string item in value_array)
                        {
                            Console.WriteLine(item);
                            if(item == reg_string[1])
                            {
                                found = true;
                                RiddleResult.Text = value_array[i+2].ToString();
                                Console.WriteLine("Odpowiedz: " + value_array[i + 2].ToString());
                            }
                            i++;
                        }
                        if(found == false)
                        {
                            RiddleResult.Text = "You lost";
                            var photo_2 = Xamarin.Forms.ImageSource.FromStream(() => { return photo.GetStream(); });
                            await Navigation.PushModalAsync(new NavigationPage(new ResultPage(photo_2, "You Lost")));
                        }
                        else
                        {
                            //Connect(options);
                            RiddleResult.Text = "You Won";
                            var message = new MqttApplicationMessageBuilder()
                                .WithTopic("APR/" + (ID_RIDDLEUSER) + "/" + "UPDATE_SCORES")
                                .WithPayload(reg_string[2]+"/"+reg_string[3]+"/"+"0")
                                .Build();
                            mqttClient.PublishAsync(message, System.Threading.CancellationToken.None);
                            //mqttClient.DisconnectAsync();
                            var photo_2 = Xamarin.Forms.ImageSource.FromStream(() => { return photo.GetStream(); });
                            await Navigation.PushModalAsync(new NavigationPage(new ResultPage(photo_2, "You Won")));
                        }
                        reg_string = null;
                        RiddleText.Text = "Press Question Again";

                    }
                    //Console.WriteLine(response);
                }
            }
            from_photo = false;
        }

        async Task<TResponse> SendRequestAsync<TRequest, TResponse>(HttpMethod httpMethod, string requestUrl, TRequest requestBody)
        {
            var request = new HttpRequestMessage(httpMethod, endpoint);
            request.RequestUri = new Uri(requestUrl);
            if (requestBody != null)
            {
                if (requestBody is Stream)
                {
                    request.Content = new StreamContent(requestBody as Stream);

                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                }
            }

            HttpResponseMessage responseMessage = await client.SendAsync(request);
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseContent = null;
                if (responseMessage.Content != null)
                {
                    responseContent = await responseMessage.Content.ReadAsStringAsync();

                }
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    value = responseContent;
                }
                return default(TResponse);
            }
            else
            {
                Console.WriteLine("code:" + responseMessage.StatusCode.ToString() + " cONTENT: ");
            }
            return default(TResponse);
        }

    }
}