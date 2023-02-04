namespace ToDoListDemo.Data;

public class ToDoRepository : BaseRepository<ToDo>
{
    public ToDoRepository(ToDoContext toDoContext)
    {
        db = toDoContext;
    }
}
