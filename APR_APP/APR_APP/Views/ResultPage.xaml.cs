using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APR_APP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPage : ContentPage
    {
        public ResultPage(ImageSource image, string result)
        {
            InitializeComponent();
            Labels.Text = result;
            PhotoImage.Source = image;
            QuestionPass.Clicked += BackBack;
        }
        private async void BackBack(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new NavigationPage(new RiddlePage()));
        }

    }
}