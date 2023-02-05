

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoListDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDoGroupsController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ToDoGroupService _toDoGroupService;

    public ToDoGroupsController(ILogger<ToDoGroupsController> logger,
        ToDoGroupService toDoGroupService)
    {
        _logger = logger;
        _toDoGroupService = toDoGroupService;
    }

    // GET: api/<ToDoGroupsController>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_toDoGroupService.GetAll());
    }

    // POST api/<ToDoGroupsController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ToDoGroup toDoGroup)
    {
        var result = await _toDoGroupService.AddToDoGroup(toDoGroup);

        if (result is null)
            return StatusCode(500);

        return Ok(result);
    }
}
