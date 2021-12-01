using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace todo_app_xamarin.screens.category
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        public CategoryPage()
        {
            InitializeComponent();
            
            ((CategoryPageVM) BindingContext).Navigation = Navigation;
        }
    }
}