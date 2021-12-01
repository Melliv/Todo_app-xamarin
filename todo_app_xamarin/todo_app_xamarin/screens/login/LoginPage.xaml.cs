using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace todo_app_xamarin.screens.login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            ((LoginPageVM) BindingContext).Navigation = Navigation;
        }
    }
}