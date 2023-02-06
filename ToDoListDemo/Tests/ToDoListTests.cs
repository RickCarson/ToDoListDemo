using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

    // User Story 1
    //1 - b.Two main tasks groups: pending and completed tasks.
    //i.Depending on the type (completed vs pending) the task will be displayed on a different group.
    [Test]
    public async Task All_Complete_ToDos_Are_Returned()
    {
        var toDoGroups = _toDoGroupService.GetAll().FirstOrDefault(g => g.Name.Equals("Complete"));

        var completeDoTos = _toDoService.GetByGroup(toDoGroups.Id);

        Assert.AreEqual(1, completeDoTos.Count(), "Correct number of pending ToDos returned by Group ID");

        completeDoTos = _toDoService.GetByGroup(toDoGroups.Name);

        Assert.AreEqual(1, completeDoTos.Count(), "Correct number of pending ToDos returned by Group Name");
    }

    [Test]
    public async Task When_New_ToDo_Group_Is_Added_It_Can_Be_Retrived()
    {
        var addedToDoGroup = await _toDoGroupService.AddToDoGroup(new ToDoGroup { Name = "Procrastinating" });


        Assert.IsNotNull(addedToDoGroup, "Value returned when adding new ToDo Group");
        Assert.AreEqual(3, _toDoGroupService.GetAll().Count(), "Number of ToDo groups correct");
        Assert.IsTrue(_toDoGroupService.GetAll()
            .FirstOrDefault(g => g.Id == addedToDoGroup.Id)
            .Name.Equals("Procrastinating"));
    }

    //User story 2
    //2. The user should be able to add this previous description to his or her to-do list
    [Test]
    public async Task When_A_New_ToDo_Is_Added_It_Is_Saved_To_The_List_Of_ToDo()
    {
        var numberOfToDos = _toDoService.GetAll().Count();
        await _toDoService.AddToDo(new ToDo { Details = "Complete user story 2" });

        Assert.AreEqual(numberOfToDos + 1, _toDoService.GetAll().Count(), "Number of ToDos has increased by 1");
    }

    //User story 2
    //3. The added to-do will be displayed as a pending task
    [Test]
    public async Task When_A_New_ToDo_Is_Added_It_Defaults_To_Pending()
    {
        var addedToDo = await _toDoService.AddToDo(new ToDo { Details = "Complete user story 2" });

        var toDoGroupPending = _toDoGroupService.GetGroup("Pending");

        Assert.AreEqual(toDoGroupPending, addedToDo.ToDoGroup, "Newly added ToDo has defaulted to pending");
    }

    //User story 2
    //1. The user should be able to pick a task and change its status:

    //If the task is pending it will become completed
    [Test]
    public async Task When_ToDo_Is_Pending_It_Is_Updated_To_Complete()
    {
        var completeGroup = _toDoGroupService.GetGroup("Complete");
        var toDoToUpdate = _toDoService.GetByGroup("Pending").FirstOrDefault();
        toDoToUpdate.ToDoGroup = completeGroup;
        _toDoService.UpdateToDo(toDoToUpdate);

        Assert.AreEqual(completeGroup, toDoToUpdate.ToDoGroup);
    }

    //If the task is completed it will become pending
    [Test]
    public async Task When_ToDo_Is_Complete_It_Is_Updated_To_Pending()
    {
        var pendingGroup = _toDoGroupService.GetGroup("Pending");
        var toDoToUpdate = _toDoService.GetByGroup("Complete").FirstOrDefault();
        toDoToUpdate.ToDoGroup = pendingGroup;
        _toDoService.UpdateToDo(toDoToUpdate);

        Assert.AreEqual(pendingGroup, toDoToUpdate.ToDoGroup);
    }

}
