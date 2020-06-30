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
       // private string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "reg.txt");

        public FullApp()
        {
            InitializeComponent();

            /*if (File.Exists(filename))
            {
                //Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
            }
            else
            {
                //Navigation.PushModalAsync(new NavigationPage(new Camera()));
            }*/

        }
    }
}