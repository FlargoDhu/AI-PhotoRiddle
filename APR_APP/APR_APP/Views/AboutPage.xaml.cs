using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APR_APP.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "reg.txt")))
            {
                // If file found, delete it    
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "reg.txt"));
            }
        }
    }
}