namespace ToDoListDemo.Models;

public class ToDo
{
    public int Id { get; set; }
    public int? ToDoGroupId { get; set; }
    public string? Title { get; set; }
    public string Details { get; set; }

    public ToDoGroup? ToDoGroup { get; set; }
}
