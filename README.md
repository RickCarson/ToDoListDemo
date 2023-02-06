# ToDoListDemo
 Simple RESTful API to manages todo list. ASP.NET Core Web API .NET 6.0  
 I probably spent about 3 hours in total working on this project.
 To build and run the solution, copy code locally, open ToDoListDemo.sln, restore NuGet Packages and build and run via Visual Studio 2022 Preview.
 My technical design is a simple Entity Frame work data layer with generic repository. A service layer which consumes the repositories and handles any logic.  And a RESTful API controller.  
 I chose this as the best approach as it's simple to implement, easy to understand and navigate, separates my concerns, each controller/service/repository only have one job and it's easy to maintain and expand to add more functionality.

 ## 1 - Initial  set up
ASP.NET Core Web API projects created using .NET 6.0 for LTS  
NuGet Packages added  
> Microsoft.EntityFrameworkCore  
Microsoft.EntityFrameworkCore.InMemory  
NUnit  
NUnit3TestAdapter  
Microsoft.NET.Test.Sdk

To the default folder structure, I also added:  
Data - for my data layer  
Models - to hold my Entities  
Services - to hold business logic  
Tests - for my tests.

Added a GlobalUsings.cs to keep files looking cleaner
Using file scoped name spaces to keep files looking cleaner

## 2 - Entities and service added
Created abstract class BaseTest to do a one time set up of a service provider to then be inherited by any other test classes  

Added ToDoListTest to hold unit tests  
Added [SetUp] and [TearDown] to seed test data  
Added [Test] to assert data is seeded correctly  

To satify seed data test:  
Created Entities  ToDo and ToDoGroups  
Added BaseRepository.cs generic repository and derived ToDoRepository and ToDoGroupRepository  
Added ToDoService   
  
Separating entites ToDo and ToDoGroups this way means I can demonstrate entity relationships, and also means in future more groups can be added by configuration without a code change required.  

Tidied Program.cs a little by adding ConfigureServices() while setting up DI for the above.

# User story 1
Created tests for user story 1

To satisfy User story 1 test:
Added a ToDoGroupService which will probably only be used by the tests for this task, but I didn't like going direct to the repository in the tests.  Also if the project is expanded to allow the user to add custom groups this service will be required.  

Created overloaded ToDoGroupService\GetGroup() so group can be retrieved by ID or Name  

Created overloaded ToDoService\GetByGroup() so a group of ToDos can be retried by ToDoGroup.ID or ToDoGroup.Name  

Added ToDo Controller with simple CRUD end points   
Disabled CORS  

Added query parameter /api/ToDo?toDoGroupId=  
This I think would be expected, having done a GET and see the field "toDoGroupId" I would expect to be able to   filter on that field.  

Also added query parameter /api/ToDo?toDoGroupName=  
This may not be as obvious that it would be available, but it would be included in the documentation.  

Using just a small amount of data as we will be doing during testing, I would just download all data and filter in the front end.  
However, assuming this could grow over normal use to hold a lot of data I have included the filter.  

Also, I wanted to demonstrate adding this functionality.  

Added ToDoGroups contoller so I can see data when first running the application.    
If we were to assume that only 'pending' and 'complete' would ever exist, this may not be needed as these values could be hard coded.    
But adding this means future groups can be added via config.  

## Tagged - Version 1.0.0.1 - User story 1

# User Story 2
Created tests for user story 2

To satisfy User story 2 test:
Made out of scope ToDo field non mandatory   
Created add ToDo functionality to ToDo service  
Added POST action to controller  

## Tagged - Version 1.0.0.2 - User story 2

# User Story 3  
Created tests for user story 3

To satisfy User story 3 test:
Added UpdateTodo to TodoService  

Added POST to ToDoController  

## Tagged - Version 1.0.0.3 - User story 3
