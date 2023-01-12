namespace MinimalWeb.Models;

public class ToDoItemRepository : IToDoItemRepository
{
    private static List<ToDoItem>? _allToDoItems;

    public List<ToDoItem>? GetAllToDoItems()
    {
        if (_allToDoItems == null)
        {
            InitializeData();
        }

        return _allToDoItems;
    }

    public void AddToDoItem(ToDoItem toDoItem)
    {
        _allToDoItems?.Add(toDoItem);
    }

    private void InitializeData()
    {
        _allToDoItems = new List<ToDoItem>()
        {
            new ToDoItem() { Title = "Deneme1", Description = "Açıklama.", Completed = true },
            new ToDoItem() { Title = "Deneme2", Description = "Açıklama." },
            new ToDoItem() { Title = "Deneme3", Description = "Açıklama.", Completed = true },
            new ToDoItem() { Title = "Deneme4", Description = "Açıklama." },
            new ToDoItem() { Title = "Deneme5", Description = "Açıklama." }
        };
    }
}