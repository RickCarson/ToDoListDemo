using ToDoListDemo.Models;

namespace ToDoListDemo.Services;

public class ToDoGroupService
{
    private readonly ILogger _logger;
    private readonly ToDoGroupRepository _toDoGroupRepository;

    public ToDoGroupService(ILogger<ToDoGroupService> logger, 
        ToDoGroupRepository toDoGroupRepository)
    {
        _logger = logger;
        _toDoGroupRepository = toDoGroupRepository;
    }

    public IEnumerable<ToDoGroup> GetAll()
    {
        _logger.LogInformation($"Getting all todo groups...");
        try
        {
            return _toDoGroupRepository.GetAll().OrderBy(g => g.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }

    public ToDoGroup? GetGroup(int id) 
    {
        _logger.LogInformation($"Getting group by ID {id}");
        try
        {
            return _toDoGroupRepository.GetAll()
                .FirstOrDefault(g => g.Id == id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }

    public ToDoGroup? GetGroup(string name)
    {
        _logger.LogInformation($"Getting group by name {name}");
        try
        {
            return _toDoGroupRepository.GetAll()
                .FirstOrDefault(g => g.Name.ToUpper() == name.ToUpper());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }
}
