namespace ToDoListDemo.Data;

public class ToDoRepository : BaseRepository<ToDo>
{
    public ToDoRepository(ToDoContext toDoContext)
    {
        db = toDoContext;
    }

    public override List<ToDo> GetAll()
    {
        return db.ToDos.Include(a => a.ToDoGroup)
            .ToList();
    }
}
