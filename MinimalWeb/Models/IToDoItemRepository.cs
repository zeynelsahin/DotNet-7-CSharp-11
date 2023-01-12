namespace MinimalWeb.Models;

public interface IToDoItemRepository
{
    List<ToDoItem>? GetAllToDoItems();
    void AddToDoItem(ToDoItem toDoItem);
}