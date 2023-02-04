using System.Security.Cryptography.X509Certificates;
using ToDoListDemo.Models;

namespace ToDoListDemo.Tests;

public class ToDoListTests : BaseTest
{

    public ToDoListTests() : base()
    {

    }

    [SetUp]
    public async Task Init()
    {
        var toDoGroups = new List<ToDoGroup>
            {
                new ToDoGroup { Name = "Pending" },
                new ToDoGroup { Name = "Complete" }
            };

        toDoGroups.ForEach(g => _toDoGroupRepository.Add(g));
        _toDoGroupRepository.SaveChanges();

        var toDos = new List<ToDo>
            {
                new ToDo { Title = "Start work", Details = "Sit at test and get work started", ToDoGroupId = 2 },
                new ToDo { Title = "Tech Test", Details = "Complete tech test demo'ing tech skill", ToDoGroupId = 1 },
                new ToDo { Title = "Publish to Git Hub", Details = "Complete tech test demo'ing tech skill", ToDoGroupId = 1 },
                new ToDo { Title = "Get Feed Back", Details = "Hopefully get good feed back from tech test", ToDoGroupId = 1 },
                new ToDo { Title = "Start New Job", Details = "Start exciting new journey with promising company", ToDoGroupId = 1 },
            };

        toDos.ForEach(t => _toDoRepository.Add(t));
        _toDoRepository.SaveChanges();
    }

    [TearDown]
    public async Task End()
    {
        _toDoRepository.DeleteAll();
        await _toDoRepository.SaveChanges();
    }

    [Test]
    public async Task When_Data_Is_Seeded_It_Is_Stored_And_Can_Be_Retrieved()
    {
        var toDos = _toDoService.GetAll();
        var toDo = _toDoService.GetByTitle("Start work").FirstOrDefault();

        Assert.AreEqual(5, toDos.Count(), "Correct number of ToDos");
        Assert.IsTrue(toDo.ToDoGroup.Name.Equals("Complete"), "ToDo correctly linked to group");
    }
}
