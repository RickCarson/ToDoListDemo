﻿
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

    // GET api/<ToDoController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
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

    // PUT api/<ToDoController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ToDoController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
