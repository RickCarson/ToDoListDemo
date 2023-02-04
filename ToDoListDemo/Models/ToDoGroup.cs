namespace ToDoListDemo.Models;

public class ToDoGroup
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<ToDo> ToDos { get; }
}
