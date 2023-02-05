using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using ToDoListDemo.Models;
using static System.Net.Mime.MediaTypeNames;

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

        var pendingGroup = _toDoGroupRepository.GetAll().FirstOrDefault(g => g.Name.Equals("Pending"));
        var completeGroup = _toDoGroupRepository.GetAll().FirstOrDefault(g => g.Name.Equals("Complete"));

        var toDos = new List<ToDo>
            {
                new ToDo { Title = "Start work", Details = "Sit at desk and get work started", ToDoGroup = completeGroup },
                new ToDo { Title = "Tech Test", Details = "Complete tech test demo'ing tech skill", ToDoGroup = pendingGroup },
                new ToDo { Title = "Publish to Git Hub", Details = "Complete tech test demo'ing tech skill", ToDoGroup = pendingGroup },
                new ToDo { Title = "Get Feed Back", Details = "Hopefully get good feed back from tech test", ToDoGroup = pendingGroup },
                new ToDo { Title = "Start New Job", Details = "Start exciting new journey with promising company", ToDoGroup = pendingGroup },
            };

        toDos.ForEach(t => _toDoRepository.Add(t));
        _toDoRepository.SaveChanges();
    }

    [TearDown]
    public async Task End()
    {
        _toDoRepository.DeleteAll();
        await _toDoRepository.SaveChanges();

        _toDoGroupRepository.DeleteAll();
        await _toDoGroupRepository.SaveChanges();
    }

    [Test]
    public async Task When_Data_Is_Seeded_It_Is_Stored_And_Can_Be_Retrieved()
    {
        var toDos = _toDoService.GetAll();
        var toDo = _toDoService.GetByTitle("Start work").FirstOrDefault();

        Assert.AreEqual(5, toDos.Count(), "Correct number of ToDos");
        Assert.IsTrue(toDo.ToDoGroup.Name.Equals("Complete"), "ToDo correctly linked to group");
    }

    // User Story 1
    //1. It should be possible to see all tasks:
    [Test]
    public async Task All_Tasks_Should_Be_Returned()
    {
        var toDos = _toDoService.GetAll();
        Assert.AreEqual(5, toDos.Count(), "Correct number of ToDos returned");
    }

    // User Story 1
    //1- a.Each task will be represented by a simple text description.
    [Test]
    public async Task ToDo_Has_Simple_Text_Description()
    {
        var toDo = _toDoService.GetByTitle("Start work").FirstOrDefault();

        Assert.IsTrue(toDo.Details.Equals("Sit at desk and get work started"), "Text description is correct");
    }

    // User Story 1
    //1 - b.Two main tasks groups: pending and completed tasks.
    //i.Depending on the type (completed vs pending) the task will be displayed on a different group.
    [Test]
    public async Task All_Pending_ToDos_Are_Returned()
    {
        var toDoGroups = _toDoGroupService.GetAll().FirstOrDefault(g => g.Name.Equals("Pending"));

        var pendingDoTos = _toDoService.GetByGroup(toDoGroups.Id);

        Assert.AreEqual(4, pendingDoTos.Count(), "Correct number of pending ToDos returned by Group ID");

        pendingDoTos = _toDoService.GetByGroup(toDoGroups.Name);

        Assert.AreEqual(4, pendingDoTos.Count(), "Correct number of pending ToDos returned by Group Name");
    }

    [Test]
    public async Task All_Complete_ToDos_Are_Returned()
    {
        var toDoGroups = _toDoGroupService.GetAll().FirstOrDefault(g => g.Name.Equals("Complete"));

        var completeDoTos = _toDoService.GetByGroup(toDoGroups.Id);

        Assert.AreEqual(1, completeDoTos.Count(), "Correct number of pending ToDos returned by Group ID");

        completeDoTos = _toDoService.GetByGroup(toDoGroups.Name);

        Assert.AreEqual(1, completeDoTos.Count(), "Correct number of pending ToDos returned by Group Name");
    }


    //2. Initially, this list will be empty.
}
