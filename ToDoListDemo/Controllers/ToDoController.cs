
namespace ToDoListDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ToDoController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly ToDoService _toDoService;

    public ToDoController(ILogger<ToDoController> logger, ToDoService toDoService)
    {
        _logger = logger;
        _toDoService = toDoService;
    }


    // GET: api/<ToDoController>
    [HttpGet]
    public IActionResult Get()
    {
        if (!string.IsNullOrEmpty(HttpContext?.Request.Query["toDoGroupName"]))
        {
            var toDoGroup = HttpContext.Request.Query["toDoGroupName"];
            return Ok(_toDoService.GetByGroup(toDoGroup));
        }

        if (!string.IsNullOrEmpty(HttpContext?.Request.Query["toDoGroupId"]))
        {
            if (int.TryParse(HttpContext.Request.Query["toDoGroupId"], out var toDoGroupId))
                return Ok(_toDoService.GetByGroup(toDoGroupId));
        }

        return Ok(_toDoService.GetAll());
    }

    // POST api/<ToDoController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ToDo newToDo)
    {
        var result = await _toDoService.AddToDo(newToDo);

        if (result is null)
            return StatusCode(500);

        return Ok(result);
    }

    // PUT api/<ToDoController>
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] ToDo toDoToUpdate)
    {
        var result = await _toDoService.UpdateToDo(toDoToUpdate);

        if (result is null)
            return StatusCode(500);

        return Ok(result);
    }
}
