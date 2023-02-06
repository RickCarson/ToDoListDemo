using ToDoListDemo.Data;

namespace ToDoListDemo.Services;

public class ToDoService
{
    private readonly ILogger _logger;
    private readonly ToDoRepository _toDoRepository;
    private readonly ToDoGroupService _toDoGroupService;

    public ToDoService(ILogger<ToDoService> logger,
        ToDoRepository toDoRepository,
        ToDoGroupService toDoGroupService)
    {
        _logger = logger;
        _toDoRepository = toDoRepository;
        _toDoGroupService = toDoGroupService;
    }

    public IEnumerable<ToDo> GetAll()
    {
        _logger.LogInformation($"Getting all todos...");
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

    public IEnumerable<ToDo> GetByGroup(int groupId)
    {
        _logger.LogInformation($"Getting todos by group id {groupId}");
        try
        {
            var toDoGroup = _toDoGroupService.GetGroup(groupId);

            if (toDoGroup is null)
                return new List<ToDo>();

            return GetByGroup(toDoGroup);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }

    public IEnumerable<ToDo> GetByGroup(string groupName)
    {
        _logger.LogInformation($"Getting todos by group name {groupName}");
        try
        {
            var toDoGroup = _toDoGroupService.GetGroup(groupName);

            if (toDoGroup is null)
                return new List<ToDo>();

            return GetByGroup(toDoGroup);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }

    private IEnumerable<ToDo> GetByGroup(ToDoGroup toDoGroup)
    {
        _logger.LogInformation("Getting todos by group {@toDoGroup}", toDoGroup);
        try
        {
            var toDosByGroup = _toDoRepository.GetAll().Where(t => t.ToDoGroup == toDoGroup);

            if (toDosByGroup is null)
                return new List<ToDo>();

            return toDosByGroup;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }

    public async Task<ToDo> AddToDo(ToDo newToDo)
    {
        _logger.LogInformation("Adding new ToDo {@newToDo}", newToDo);
        try
        {
            if (newToDo.ToDoGroup is null)
            {
                _logger.LogInformation("Defaulting to pending");
                var toDoGroupPending = _toDoGroupService.GetGroup("Pending");
                newToDo.ToDoGroup = toDoGroupPending;
            }

            await _toDoRepository.Add(newToDo);
            await _toDoRepository.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null;
        }

        return newToDo;
    }

    public async Task<ToDo> UpdateToDo(ToDo toDoToUpdate)
    {
        _logger.LogInformation("Updating ToDo {@toDoToUpdate}", toDoToUpdate);
        try
        {
            _toDoRepository.Edit(toDoToUpdate);
            await _toDoRepository.SaveChanges();

            // Check updataed
            var updatedToDo = GetAll().FirstOrDefault(t => t.Id == toDoToUpdate.Id);

            if (updatedToDo is null)
                return new ToDo();

            return updatedToDo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null!;
        }
    }
}
