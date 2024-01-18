using Microsoft.EntityFrameworkCore;
using TodoApi;


var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ToDoDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Use CORS policy
app.UseCors("AllowAll");

// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }
app.Urls.Add("http://localhost:7216");

app.MapGet("/",() =>
   "ToDoServer is running");
app.MapGet("/todoitems", async (ToDoDbContext db) =>
    await db.Items.ToListAsync());

app.MapPost("/todoitems", async (Item todo, ToDoDbContext db) =>
{
    db.Items.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (int id, Item inputTodo, ToDoDbContext db) =>
{
    var todo = await db.Items.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (int id, ToDoDbContext db) =>
{
    if (await db.Items.FindAsync(id) is Item todo)
    {
        db.Items.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
