
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.Apache.Http.Client.Methods;
using Newtonsoft.Json.Serialization;

namespace APR_APP.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Camera : ContentPage
    {
        static string subscriptionKey = "ec2d27ff7e28441381e9c180f6dc2806";
        static string endpoint = "https://flargoaprhavefun.cognitiveservices.azure.com/";
        private string value = null;
        private HttpClient client;
        static Label label;
        private static JsonSerializerSettings s_settings = new JsonSerializerSettings()
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        public Camera()
        {
            InitializeComponent();

            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Host", "flargoaprhavefun.cognitiveservices.azure.com");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
            //client.Endpoint = "https://randomvision.cognitiveservices.azure.com/";
            if (client != null)
            {
                Labels.Text = client.ToString();
            }
            Console.WriteLine("Test Console");
            label = Labels;
            CameraButton.Clicked += CameraButton_Clicked;
            GalleryButton.Clicked += GalleryButton_Clicked;
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            {
                SaveToAlbum = true
            });

            if (photo != null)
                PhotoImage.Source = Xamarin.Forms.ImageSource.FromStream(() => { return photo.GetStream(); });
        }

        private async void GalleryButton_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions() { });

            if (photo != null)
            {
                PhotoImage.Source = Xamarin.Forms.ImageSource.FromStream(() => { return photo.GetStream(); });
                label.Text = "going home 2";
                Console.WriteLine("WAAA");
                string requesturi = "https://flargoaprhavefun.cognitiveservices.azure.com/vision/v3.0/analyze?visualFeatures=Objects&language=en";
                var response = await SendRequestAsync<Stream, string>(new HttpMethod("POST"), requesturi, photo.GetStream());
                if (value != null)
                {
                    string random = "\"";
                    var value_array = value.Split(new string[] {  ":", random,"," }, StringSplitOptions.None);
                    foreach (string item in value_array){
                        Console.WriteLine(item);
                    }
                }
                Console.WriteLine(response);
                //AnalyzeImageUrl(client, photo.GetStream()).Wait();
                //foreach (EntityAnnotation label in labels)
                // {
                // Labels.Text += $"Score: {(int)(label.Score * 100)}%; Description: {label.Description}";
                //Console.WriteLine($"Score: {(int)(label.Score * 100)}%; Description: {label.Description}");
                // }
            }
        }

        /*public static async Task AnalyzeImageUrl(ComputerVisionClient client, Stream image)
        {
            label.Text = "going home 1";
            Console.WriteLine("Test Console REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            // Creating a list that defines the features to be extracted from the image. 
            List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
{
                VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                VisualFeatureTypes.Color, VisualFeatureTypes.Brands,
                VisualFeatureTypes.Objects
            };
            Console.WriteLine("REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE REEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            // Analyze the URL image 
            using (Stream analyzeImageStream = image)
            {
                Console.WriteLine("Started Analysis");
                // Analyze the local image.
                ImageAnalysis results = await client.AnalyzeImageInStreamAsync(analyzeImageStream, features);
                foreach (var category in results.Categories)
                {
                    label.Text += $"{category.Name} with confidence {category.Score}";
                }
            }*/

        async Task<TResponse> SendRequestAsync<TRequest, TResponse>(HttpMethod httpMethod, string requestUrl, TRequest requestBody)
        {
            Console.WriteLine("1111111111111111111111");
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
            Console.WriteLine("222222222222222222");
            HttpResponseMessage responseMessage = await client.SendAsync(request);
            if (responseMessage.IsSuccessStatusCode)
            {
                string responseContent = null;
                if (responseMessage.Content != null)
                {
                    responseContent = await responseMessage.Content.ReadAsStringAsync();
                    label.Text = responseContent;
                    Console.WriteLine(responseContent);
                    
                }
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    value = responseContent;
                }
                return default(TResponse);
            }
            else
            {
                Console.WriteLine("code:" + responseMessage.StatusCode.ToString() +" cONTENT: ");
            }
            return default(TResponse);
        }

    }
}
