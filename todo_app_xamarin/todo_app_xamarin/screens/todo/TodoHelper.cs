using System;
using System.Threading.Tasks;
using DAL;
using DAL.Services;
using Domain;
using Xamarin.Essentials;

namespace todo_app_xamarin.screens.todo
{
    public class TodoHelper
    {
        public async Task<Todo> PostTodo(Todo todo)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                var response = await BaseService.Post<Todo>("TodoTasks", todo);

                if (response.Ok)
                {
                    todo = response.Data;
                }
            }
            else
            {
                todo.Id = new Guid().ToString();
            }
            
            using (var ctx = new AppDbContext())
            {
                await ctx.Todos.AddAsync(todo);
                await ctx.SaveChangesAsync();
            }

            return todo;
        }
        public async Task UpdateTodo(Todo todo)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await BaseService.Put<Todo>($"TodoTasks/{todo.Id}", todo);
            }
            
            using (var ctx = new AppDbContext())
            {
                ctx.Todos.Update(todo);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task RemoveTodo(Todo todo)
        {
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await BaseService.Delete<Todo>($"TodoTasks/{todo.Id}");
            }

            using (var ctx = new AppDbContext())
            {
                ctx.Todos.Remove(todo);
                await ctx.SaveChangesAsync();
            }
        }
    }
}