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

    MqttFactory factory = new MqttFactory();
    IMqttClient mqttClient;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RiddleLeader : ContentPage
    {
        public RiddleLeader()
        {
            InitializeComponent();
        }
    }
}