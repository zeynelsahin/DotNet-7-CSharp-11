using System.Diagnostics;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Primitives;
using Microsoft.OpenApi.Models;
using WiredBrainCoffee.MinApi.Services;
using WiredBrainCoffee.MinApi.Services.Interfaces;
using WiredBrainCoffee.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddCors();
builder.Services.AddOutputCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseOutputCache();
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseRateLimiter(new RateLimiterOptions() { RejectionStatusCode = 429 }.AddConcurrencyLimiter("Concurrency", options => { options.PermitLimit = 1; }).AddFixedWindowLimiter("FixedWindow", options =>
{
    options.Window = TimeSpan.FromSeconds(5);
    options.PermitLimit = 10; //Her beş saniyede 10 tane istek cevaplanır.2saniye içinde 10 istek gelirse 3 saniye daha istekler 429 HTTP kodu ile reddedilir.
    options.QueueLimit = 3; //PermitLimiti aşan 3 istek bekletilir.
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; //Bekletile requestlerin çalışması en eski olandan başlar
}));
app.MapGet("/unlimited", () => "Unlimited.");
app.MapGet("/unlimited", () => "Rate Limited.").RequireRateLimiting("Concurrency");
var mobileApiGroup = app.MapGroup("/api").AddEndpointFilter(async (context, next) =>
{
    StringValues deviceType;
    context.HttpContext.Request.Headers.TryGetValue("x-device-type", out deviceType);
    if (deviceType != "mobile")
    {
        return Results.BadRequest();
    }

    var result = await next(context);
    Debug.WriteLine("after");
    return result;
});
;

app.MapGet("/orders", (IOrderService orderService) => { return Results.Ok(orderService.GetOrders()); }).WithOpenApi(operation =>
{
    operation.OperationId = "GetOrders";
    operation.Description = "Gets all of the orders. Use with cation due to performance.";
    operation.Summary = "Gets all the orders.";
    operation.Tags = new List<OpenApiTag>()
    {
        new OpenApiTag()
        {
            Name = "Orders"
        }
    };
    return operation;
});

app.MapGet("/ordersByIds", (IOrderService orderService, int[] orderIds) => { return Results.Ok(orderService.GetOrders().Where(p => orderIds.Contains(p.Id))); });

app.MapGet("/orderById", (IOrderService orderService, int id) => { return Results.Ok(orderService.GetOrderById(id)); });

app.MapPost("/contact", (Contact contact) =>
{
    // save contact to database
});

app.MapGet("/menu", (IMenuService menuService) => { return menuService.GetMenuItems(); }).CacheOutput(p=>
{
    p.Expire(TimeSpan.FromSeconds(20));
});  


mobileApiGroup.MapGet("/rewards", () => { return "Headers x-device-type : mobile"; });

mobileApiGroup.MapGet("/survey", ([AsParameters] SurveyResults Results) => "Deneme");

app.MapPost("/file", async (IFormFile file) =>
{
    using var stream = File.OpenWrite("upload.jpg");
    await file.CopyToAsync(stream);
});

app.Run();