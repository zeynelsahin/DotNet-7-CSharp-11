using DotNet7BlazorWebAssembly.Model;
using Microsoft.AspNetCore.Components;

namespace DotNet7BlazorWebAssembly.Pages;

public partial class SearchToDos
{
    private bool Loading { get; set; }
    private string? searchText { get; set; }
    private List<ToDoItem> AllToDoItems { get; set; } = new List<ToDoItem>();
    private List<ToDoItem> FilteredItems { get; set; } = new List<ToDoItem>();

    public async Task PerformSearch()
    {
        FilteredItems = AllToDoItems.Where(t => t.Title.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)).ToList();
    }

    protected override void OnInitialized()
    {
        Loading = true;
        AllToDoItems = new List<ToDoItem>()
        {
            new ToDoItem() {Title = "Aaaa",Description = "Açıklama"},
            new ToDoItem(){Title = "bbbb",Description = "Açıklama"},
            new ToDoItem(){Title="aaa",Description = "Aaaa"}
        };
        Loading = false;
    }
}