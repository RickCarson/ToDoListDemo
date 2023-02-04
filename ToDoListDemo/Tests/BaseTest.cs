
namespace ToDoListDemo.Tests;

[TestFixture]
public abstract class BaseTest
{
    protected readonly ILogger _logger;
    protected readonly ToDoContext _toDoContext;
    protected readonly ToDoRepository _toDoRepository;
    protected readonly ToDoGroupRepository _toDoGroupRepository;
    protected readonly ToDoService _toDoService;
    protected readonly ToDoGroupService _toDoGroupService;




    public BaseTest()
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddDbContext<ToDoContext>(opt =>
                opt.UseInMemoryDatabase("ToDo"))
            .AddTransient<ToDoRepository>()
            .AddTransient<ToDoGroupRepository>()
            .AddTransient<ToDoService>()
            .AddTransient<ToDoGroupService>()
            .BuildServiceProvider();

        _toDoContext = serviceProvider.GetService<ToDoContext>();
        _toDoRepository = serviceProvider.GetService<ToDoRepository>();
        _toDoGroupRepository = serviceProvider.GetService<ToDoGroupRepository>();
        _toDoService = serviceProvider.GetService<ToDoService>();
        _toDoGroupService = serviceProvider.GetService<ToDoGroupService>();
    }

}
