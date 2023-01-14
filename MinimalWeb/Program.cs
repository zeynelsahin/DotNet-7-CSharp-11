using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using MinimalWeb;
using MinimalWeb.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IToDoItemRepository, ToDoItemRepository>();

builder.Logging.ClearProviders();//provider delete
builder.Logging.AddConsole();//Add console provide

builder.Logging.AddDebug();//Add debug provider

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/person", () => new PersonModel.Person("Zeynel","Şahin"));

app.MapGet("/todos", (IToDoItemRepository repository) => repository.GetAllToDoItems()).AddEndpointFilter(async (context, next) =>
{
    app.Logger.LogInformation("Request for {RequestPath} received at {LongTimeString}.", context.HttpContext.Request.Path, DateTime.Now.ToLongTimeString());

    var result = await next(context);
    
    app.Logger.LogInformation("Request for {RequestPath} handled at {LongTimeString}.", context.HttpContext.Request.Path, DateTime.Now.ToLongTimeString());
    return result;
});


app.MapGet("/todos1", (ToDoItem toDoItem,IToDoItemRepository repository) =>
{
    if (toDoItem==null)
        return Results.BadRequest();

    if (string.IsNullOrWhiteSpace(toDoItem.Title))
        return Results.BadRequest();
    
    repository.AddToDoItem(toDoItem);
    return Results.NoContent();
}).AddEndpointFilter(async (context, next) =>
{
    app.Logger.LogInformation("Request for {RequestPath} received at {LongTimeString}.", context.HttpContext.Request.Path, DateTime.Now.ToLongTimeString());

    var result = await next(context);
    
    app.Logger.LogInformation("Request for {RequestPath} handled at {LongTimeString}.", context.HttpContext.Request.Path, DateTime.Now.ToLongTimeString());
    return result;
});

app.UseHttpsRedirection();
app.MapGet("/todos3", (IToDoItemRepository repository) => repository.GetAllToDoItems()).AddEndpointFilter<MyEndpointFilter>();

var logGroup = app.MapGroup("/api").AddEndpointFilter(async (context, next) =>
{
    app.Logger.LogInformation($"Request for {context.HttpContext.Request.Path} received at {DateTime.Now.ToLongTimeString()}.");

    var result = await next(context);

    app.Logger.LogInformation($"Request for {context.HttpContext.Request.Path} handled at {DateTime.Now.ToLongTimeString()}.");

    return result;
});

logGroup.MapGet("/todos4", (IToDoItemRepository repository) => repository.GetAllToDoItems());
logGroup.MapPost("/todos5", (ToDoItem toDoItem, ToDoItemRepository repository) =>
{
    if (toDoItem==null)
    {
        return Results.BadRequest();
    }

    if (String.IsNullOrWhiteSpace(toDoItem.Title))
    {
        return Results.BadRequest();
    }
    repository.AddToDoItem(toDoItem);
    return Results.NoContent();
});

app.MapPost("/file", async (IFormFile file) =>
{
    using var stream = File.OpenWrite("upload.jpg");
    await file.CopyToAsync(stream);
});

app.Run();