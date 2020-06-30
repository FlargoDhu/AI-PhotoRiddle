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
    public partial class FullApp : ContentPage
    {
        private readonly string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "reg.txt");

        public FullApp()
        {
            InitializeComponent();

            

        }

        protected override void OnAppearing()
        {
            if (!File.Exists(filename))
            {
                Navigation.PushModalAsync(new NavigationPage(new RegisterPage()));
            }
            else
            {
                Navigation.PushModalAsync(new NavigationPage(new RiddlePage()));
            }
        }
    }
}