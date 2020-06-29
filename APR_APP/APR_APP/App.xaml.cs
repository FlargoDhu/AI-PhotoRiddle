using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APR_APP.Services;
using APR_APP.Views;

namespace APR_APP
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
