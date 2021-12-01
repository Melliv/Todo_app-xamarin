using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace todo_app_xamarin.screens.todo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TodoPage : ContentPage
    {
        public TodoPage(Category category)
        {
            InitializeComponent();

            ((TodoPageVM) BindingContext).Navigation = Navigation;
            ((TodoPageVM) BindingContext).Category = category;
            ((TodoPageVM) BindingContext).InitTodos();
        }
    }
}