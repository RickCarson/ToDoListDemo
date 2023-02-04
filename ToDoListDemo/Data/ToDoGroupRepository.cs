namespace ToDoListDemo.Data;

public class ToDoGroupRepository : BaseRepository<ToDoGroup>
{
    public ToDoGroupRepository(ToDoContext toDoContext)
    {
        db = toDoContext;
    }
}
