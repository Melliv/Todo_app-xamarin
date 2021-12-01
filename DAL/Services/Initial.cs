using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DAL.Services
{
    public static class Initial
    {
        public static async Task LoadInitialData()
        {
            var task1 = LoadCategories();
            var task2 = LoadTodos();
            var task3 = LoadPriorities();
            await Task.WhenAll(task1, task2, task3);
        }

        private static async Task LoadCategories()
        {
            var resultCategories = await BaseService.Get<List<Category>>("TodoCategories");

            if (resultCategories.Ok)
            {
                using (var ctx = new AppDbContext())
                {
                    try
                    {
                        ctx.Categories.AddRange(resultCategories.Data);
                        await ctx.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }
        
        private static async Task LoadTodos()
        {
            var resultTodos = await BaseService.Get<List<Todo>>("TodoTasks");

            if (resultTodos.Ok)
            {
                using (var ctx = new AppDbContext())
                {
                    try
                    {
                        ctx.Todos.AddRange(resultTodos.Data);
                        await ctx.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }
        
        private static async Task LoadPriorities()
        {
            var resultPriorities = await BaseService.Get<List<Priority>>("TodoPriorities");

            if (resultPriorities.Ok)
            {
                using (var ctx = new AppDbContext())
                {
                    try
                    {
                        ctx.Priorities.AddRange(resultPriorities.Data);
                        await ctx.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }
    }
}