using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace todo_app_xamarin.screens.register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            ((RegisterPageVM) BindingContext).Navigation = Navigation;
        }
    }
}