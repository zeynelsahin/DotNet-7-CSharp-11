using DotNet7BlazorWebAssembly.Model;
using Microsoft.AspNetCore.Components.QuickGrid;

namespace DotNet7BlazorWebAssembly.Pages;

public partial class ToDoHistory
{
    public IQueryable<ToDoItem> AllToDoItems { get; set; } = new List<ToDoItem>().AsQueryable();

    PaginationState pagination = new() { ItemsPerPage = 3 };
    public bool Loading { get; set; }

    public string titleFilter = string.Empty;
    
    private IQueryable<ToDoItem> FilteredItems => AllToDoItems.Where(x => x.Title.Contains(titleFilter, StringComparison.CurrentCultureIgnoreCase));

    protected override void OnInitialized()
    {
        Loading = true;

        AllToDoItems = new List<ToDoItem>()
        {
            new ToDoItem() { Title = "Deneme", Description = "Açıklama" },
            new ToDoItem() { Title = "Deneme1", Description = "Açıklama" },
            new ToDoItem() { Title = "Deneme2", Description = "Açıklama" },
            new ToDoItem() { Title = "Deneme3", Description = "Açıklama" },
            new ToDoItem() { Title = "Deneme4", Description = "Açıklama" }
        }.AsQueryable();
    }
}