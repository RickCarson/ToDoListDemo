using ToDoListDemo.Data;

namespace ToDoListDemo.Services;

public class ToDoService
{
    private readonly ILogger _logger;
    private readonly ToDoRepository _toDoRepository;

    public ToDoService(ILogger<ToDoService> logger
    , ToDoRepository toDoRepository)
    {
        _logger = logger;
        _toDoRepository = toDoRepository;
    }

    public IEnumerable<ToDo> GetAll()
    {
        _logger.LogInformation($"Getting all...");
        try
        {
            return _toDoRepository.GetAll().OrderBy(t => t.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }

    public IEnumerable<ToDo> GetByTitle(string title)
    {
        _logger.LogInformation($"Searching for ToDo with title {title}");
        try
        {
            return _toDoRepository.GetAll().Where(t => t.Title.Contains(title));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }

}
