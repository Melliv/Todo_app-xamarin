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
using todo_app_xamarin.screens.todo;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace todo_app_xamarin.screens.category
{
    public class CategoryPageVM : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public Category Category { get; set; } = new Category();
        public ICommand AddCategoryCom { get; set; }
        public ICommand CategoryClick { get; set; }
        public CategoryValidation CategoryValidation { get; set; } = new CategoryValidation();

        public CategoryPageVM()
        {
            CategoryClick = new Command<Category>(async (category) => await Navigation.PushAsync(new TodoPage(category)));
            AddCategoryCom = new Command(async () => await AddCategory());
            LoadCategories();
        }

        private async void LoadCategories()
        {
            using (var ctx = new AppDbContext())
            {
                Categories = new ObservableCollection<Category>(await ctx.Categories.ToListAsync());
            }
        }

        private bool IsValid()
        {
            var valid = true;
            CategoryValidation = new CategoryValidation();

            if (string.IsNullOrWhiteSpace(Category.CategoryName))
            {
                CategoryValidation.CategoryName = "Category field can not be empty!";
                valid = false;
            }

            OnPropertyChanged(nameof(CategoryValidation));
            return valid;
        }

        private async Task AddCategory()
        {
            if (!IsValid()) return;
            
            Category.CategoryName = Category.CategoryName.Trim();

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var response = await BaseService.Post("TodoCategories", Category);
                
                if (response.Ok)
                {
                    Category = response.Data;
                }
            }
            else
            {
                Category.Id = new Guid().ToString();
            }

            using (var ctx = new AppDbContext())
            {
                await ctx.Categories.AddAsync(Category);
                await ctx.SaveChangesAsync();
            }

            Categories.Insert(0, Category);
            Category = new Category();
            OnPropertyChanged(nameof(Categories));
            OnPropertyChanged(nameof(Category));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}