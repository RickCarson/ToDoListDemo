var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Add Pending and Complete into InMemory database
// as these will always be required.
// Assume in production we would be using a real database 
// with values already configured

if (app.Environment.IsDevelopment())
{
    ConfigureDevelopmentData(builder.Services);
}

app.Run();

static void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddDbContext<ToDoContext>(opt =>
        opt.UseInMemoryDatabase("ToDo"));

    services.AddScoped<ToDoRepository>();
    services.AddScoped<ToDoGroupRepository>();

    services.AddScoped<ToDoService>();
    services.AddScoped<ToDoGroupService>();


}

static async Task ConfigureDevelopmentData(IServiceCollection services)
{
    var toDoGroupsService = services.BuildServiceProvider().GetService<ToDoGroupService>();
    if (toDoGroupsService is null)
        return;

    await toDoGroupsService.AddToDoGroup(new ToDoGroup { Name = "Pending" });
    await toDoGroupsService.AddToDoGroup(new ToDoGroup { Name = "Complete" });
}
