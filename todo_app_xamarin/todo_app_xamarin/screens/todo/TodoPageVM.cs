using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using DAL;
using Domain;
using Domain.validations;
using todo_app_xamarin.Annotations;
using todo_app_xamarin.screens.priority;
using Xamarin.Forms;

namespace todo_app_xamarin.screens.todo
{
    public class TodoPageVM : INotifyPropertyChanged
    {
        public INavigation Navigation { get; set; }
        public Category Category { get; set; }
        public Priority Priority { get; set; }
        public ObservableCollection<Todo> Todos { get; set; }
        public ObservableCollection<Priority> Priorities { get; set; }
        public Todo Todo { get; set; } = new Todo();
        public TodoValidation TodoValidation { get; set; } = new TodoValidation();
        private TodoHelper TodoHelper { get; set; } = new TodoHelper();
        public ICommand TodoAddCom { get; set; }
        public ICommand TodoEditCom { get; set; }
        public ICommand TodoDeleteCom { get; set; }
        public ICommand TodoClickCom { get; set; }
        public ICommand PrioritiesCom { get; set; }
        public ICommand SortCompletedCom { get; set; }
        private bool Edit { get; set; }
        private bool TodoSortOrderAsc { get; set; } = true;

        public TodoPageVM()
        {
            TodoEditCom = new Command<Todo>( PreEditTodo);
            TodoDeleteCom = new Command<Todo>(async (todo) => await DeleteTodo(todo));
            TodoAddCom = new Command<Todo>(async (todo) => await AddTodo());
            TodoClickCom = new Command<Todo>(async (todo) => await TodoClick(todo));
            PrioritiesCom = new Command(async () => await Navigation.PushAsync(new PriorityPage()));
            SortCompletedCom = new Command(SortCompleted);
            LoadPriorities();
        }


        private void PreEditTodo(Todo todo)
        {
            Todo = todo;
            Edit = true;
            Priority = Priorities.First(p => p.Id == todo.TodoPriorityId);
            OnPropertyChanged(nameof(Priority));
            OnPropertyChanged(nameof(Todo));
        }
        private void SortCompleted()
        {
            var todoList = Todos.OrderBy(t => t.IsCompleted).ToList();

            if (!TodoSortOrderAsc) todoList.Reverse();

            TodoSortOrderAsc = !TodoSortOrderAsc;
            Todos = new ObservableCollection<Todo>(todoList);
            OnPropertyChanged(nameof(Todos));
        }
        
        private async Task EditTodo()
        {
            await TodoHelper.UpdateTodo(Todo);

            Todos.Remove(Todo);
            Todos.Insert(0, Todo);
            Todo = new Todo();
            Edit = false;
            Todo.TodoCategoryId = Category.Id;
            OnPropertyChanged(nameof(Todo));
            OnPropertyChanged(nameof(Todos));
        }

        private async Task DeleteTodo(Todo todo)
        {
            await TodoHelper.RemoveTodo(todo);

            Todos.Remove(todo);
            OnPropertyChanged(nameof(Todos));
        }

        private async Task TodoClick(Todo todo)
        {
            todo.IsCompleted = !todo.IsCompleted;

            await TodoHelper.UpdateTodo(todo);

            Todos.Remove(todo);
            Todos.Insert(0, todo);
            OnPropertyChanged(nameof(Todos));
        }

        private void LoadPriorities()
        {
            using (var ctx = new AppDbContext())
            {
                var prioritiesList = ctx.Priorities.ToList();
                Priorities = new ObservableCollection<Priority>(prioritiesList);
            }
        }

        public void InitTodos()
        {
            Todo.TodoCategoryId = Category.Id;
            using (var ctx = new AppDbContext())
            {
                var todosList = ctx.Todos.Where(t => t.TodoCategoryId == Category.Id).ToList();
                Todos = new ObservableCollection<Todo>(todosList);
            }
            OnPropertyChanged(nameof(Category));
            OnPropertyChanged(nameof(Todos));
        }

        private bool IsValid()
        {
            var valid = true;
            TodoValidation = new TodoValidation();

            if (string.IsNullOrWhiteSpace(Todo.TaskName))
            {
                TodoValidation.TaskName = "Description field can not be empty!";
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(Todo.TodoPriorityId))
            {
                TodoValidation.Priority = "Priority field can not be empty!";
                valid = false;
            }

            OnPropertyChanged(nameof(TodoValidation));
            return valid;
        }

        private async Task AddTodo()
        {
            if (Priority != null)
            {
                Todo.TodoPriorityId = Priority.Id;
            }

            if (!IsValid()) return;

            Todo.TaskName = Todo.TaskName.Trim();
            
            if (Edit)
            {
                await EditTodo();
                return;
            }

            Todo = await TodoHelper.PostTodo(Todo);
            
            Todos.Insert(0, Todo);
            Todo = new Todo
            {
                TodoCategoryId = Category.Id
            };
            OnPropertyChanged(nameof(Todo));
            OnPropertyChanged(nameof(Todos));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}