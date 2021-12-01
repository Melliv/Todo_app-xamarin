using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using DAL;
using DAL.Services;
using Domain;
using Domain.validations;
using todo_app_xamarin.Annotations;
using todo_app_xamarin.screens.category;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace todo_app_xamarin.screens.register
{
    public class RegisterPageVM: INotifyPropertyChanged
    {
        private const string EmailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        private const string PasswordPattern = @"^(?=.*?[A-Z])(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,}$";
        public INavigation Navigation { get; set; }
        public Register Register { get; set; } = new Register();
        public ICommand RegisterCommand { get; set; }
        public ICommand ClickCommand { get; set; }
        public RegisterValidation RegisterValidation { get; set; }
        public bool HaveNoConnection { get; set; }

        public RegisterPageVM()
        {
            RegisterValidation = new RegisterValidation();
            RegisterCommand = new Command( async () => await DoRegister());
            ClickCommand = new Command(async () => await Navigation.PopAsync());
        }

        private bool IsValid()
        {
            bool valid = true;
            RegisterValidation = new RegisterValidation();
        
            if (string.IsNullOrWhiteSpace(Register.FirstName))
            {
                RegisterValidation.FirstName = "First name field can not be empty!";
                valid = false;
            }
            
            if (string.IsNullOrWhiteSpace(Register.LastName))
            {
                RegisterValidation.LastName = "Last name field can not be empty!";
                valid = false;
            }

            if (!Regex.IsMatch(Register.Email, EmailPattern))
            {
                RegisterValidation.Email = "Email is not valid!";
                valid = false;
            }

            if (!Regex.IsMatch(Register.Password, PasswordPattern))
            {
                RegisterValidation.Password = "Password requirements: Minimum 8 characters, at least 1 uppercase letter, 1 lowercase letter, 1 number and 1 special character!";
                valid = false;
            }

        
            return valid;
        }
        
        private async Task DoRegister()
        {
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                HaveNoConnection = true;
                OnPropertyChanged(nameof(HaveNoConnection));
                return;
            }
            
            if (!IsValid())
            {
                OnPropertyChanged(nameof(RegisterValidation));
                return;
            }

            var response = await IdentityService.Register(Register);
            
            if (response.Ok)
            {
                using (var ctx = new AppDbContext())
                {
                    ctx.DeleteDb();
                }
                
                Preferences.Set("token", response.Data.Token);
                Preferences.Set("name", response.Data.FirstName);
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