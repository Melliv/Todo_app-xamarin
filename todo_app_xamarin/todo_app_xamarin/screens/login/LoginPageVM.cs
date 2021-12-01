using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using DAL.Services;
using Domain;
using Domain.types;
using Domain.validations;
using todo_app_xamarin.Annotations;
using todo_app_xamarin.screens.category;
using todo_app_xamarin.screens.register;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace todo_app_xamarin.screens.login
{
    public class LoginPageVM : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public ILogin Login { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand ClickCommand { get; set; }        
        public ICommand OfflineMode { get; set; }
        public LoginValidation LoginValidation { get; set; }
        public bool HaveNoConnection { get; set; }
        public LoginPageVM()
        {
            Login = new Login();
            LoginValidation = new LoginValidation();
            LoginCommand = new Command(async () => await DoLogin());
            ClickCommand = new Command(async () => await Navigation.PushAsync(new RegisterPage()));
            OfflineMode = new Command(async () =>
            {
                HaveNoConnection = false;
                OnPropertyChanged(nameof(HaveNoConnection));
                await Navigation.PushAsync(new CategoryPage());
            });
        }

        private bool IsValid()
        {
            var valid = true;
            LoginValidation = new LoginValidation();
        
            if (string.IsNullOrWhiteSpace(Login.Email))
            {
                LoginValidation.Email = "Email field can not be empty!";
                valid = false;
                OnPropertyChanged(nameof(LoginValidation));
            }
            
            if (string.IsNullOrWhiteSpace(Login.Password))
            {
                LoginValidation.Password = "Password field can not be empty!";
                valid = false;
                OnPropertyChanged(nameof(LoginValidation));
            }
        
            return valid;
        }
        
        private async Task DoLogin()
        {
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                HaveNoConnection = true;
                OnPropertyChanged(nameof(HaveNoConnection));
                return;
            }
            
            if (!IsValid()) return;

            var response = await IdentityService.Login(Login);

            if (response.Ok)
            {
                Preferences.Set("token", response.Data.Token);
                Preferences.Set("name", response.Data.FirstName);
                await Initial.LoadInitialData();
                await Navigation.PushAsync(new CategoryPage());
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}