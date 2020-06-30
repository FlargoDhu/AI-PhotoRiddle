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
    public partial class RiddlePage : ContentPage
    {
        string ID_RIDDLEUSER;
        public RiddlePage()
        {
            InitializeComponent();

            ID_RIDDLEUSER = System.IO.File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "reg.txt"));
        }
    }
}