using DotNet7BlazorWebAssembly.Model;
using DotNet7BlazorWebAssembly.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace DotNet7BlazorWebAssembly.Components;

public partial class ToDoItemComponent
{
    [Parameter]
    public ToDoItem ToDoItem { get; set; }
    
    [Parameter]
    public EventCallback<MouseEventArgs> OnDeleteToDoItem { get; set; } 

    protected override void OnInitialized()
    {
           
    }
}