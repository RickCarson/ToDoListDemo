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

app.Run();

static void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddDbContext<ToDoContext>(opt =>
        opt.UseInMemoryDatabase("ToDo"));

    services.AddTransient<ToDoRepository>();
    services.AddTransient<ToDoGroupRepository>();

    services.AddTransient<ToDoService>();
    services.AddTransient<ToDoGroupService>();


}
