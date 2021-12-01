using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using DAL;
using DAL.Services;
using Domain;
using Domain.validations;
using Microsoft.EntityFrameworkCore;
using todo_app_xamarin.Annotations;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace todo_app_xamarin.screens.priority
{
    public class PriorityPageVM : INotifyPropertyChanged
    {
        public Priority Priority { get; set; } = new Priority();
        public ObservableCollection<Priority> Priorities { get; set; }
        public PriorityValidation PriorityValidation { get; set; } = new PriorityValidation();
        public ICommand PriorityAddCom { get; set; }
        
        public PriorityPageVM()
        {
            PriorityAddCom = new Command(async () => await AddPriority());
            LoadPriorities();
        }
        
        private async void LoadPriorities()
        {
            using (var ctx = new AppDbContext())
            {
                Priorities = new ObservableCollection<Priority>(await ctx.Priorities.ToListAsync());
            }
        }
        private bool IsValid()
        {
            var valid = true;
            PriorityValidation = new PriorityValidation();

            if (string.IsNullOrWhiteSpace(Priority.PriorityName))
            {
                PriorityValidation.PriorityName = "Priority name field can not be empty!";
                valid = false;
            }

            OnPropertyChanged(nameof(PriorityValidation));
            return valid;
        }

        private async Task AddPriority()
        {
            if (!IsValid()) return;
            
            Priority.PriorityName = Priority.PriorityName.Trim();

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var response = await BaseService.Post("TodoPriorities", Priority);
                
                if (response.Ok)
                {
                    Priority = response.Data;
                }
            }
            else
            {
                Priority.Id = new Guid().ToString();
            }

            using (var ctx = new AppDbContext())
            {
                await ctx.Priorities.AddAsync(Priority);
                await ctx.SaveChangesAsync();
            }
            
            Priorities.Insert(0, Priority);
            Priority = new Priority();
            OnPropertyChanged(nameof(Priorities));
            OnPropertyChanged(nameof(Priority));
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}