using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

const string fixedWindowRateLimitedPolicy = "fixed";
const string concurrencyRateLimitedPolicy = "concurrency";
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Fixed
builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter(policyName: fixedWindowRateLimitedPolicy, options =>
{
    options.PermitLimit = 4;
    options.Window = TimeSpan.FromSeconds(20);
    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // sıra doluysa eskilerin çalıştırılmasını söyülüyoruz
    options.QueueLimit = 2; // eğereki kuyrukta 2den fazla istek oluşturulmaya çalışılırsa direkt 503 alacaktır.
}));
//Concurrency
// builder.Services.AddRateLimiter(_ => _.AddFixedWindowLimiter(policyName: concurrencyRateLimitedPolicy, options =>
// {
//     options.PermitLimit = 4;
//     options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
//     options.QueueLimit = 2;
// }));
//Global
// builder.Services.AddRateLimiter(options =>
// {
//     options.RejectionStatusCode = 429;
//     options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context => RateLimitPartition.GetFixedWindowLimiter(partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(), factory: partition => new FixedWindowRateLimiterOptions()
//     {
//         AutoReplenishment = true,
//         PermitLimit = 4,
//         QueueLimit = 0,
//         Window = TimeSpan.FromSeconds(20)
//     }));
// });
//Chained
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.CreateChained<HttpContext>(
        PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                factory: partition => new FixedWindowRateLimiterOptions()
                {
                    AutoReplenishment = true,
                    PermitLimit = 100,
                    QueueLimit = 0,
                    Window = TimeSpan.FromMinutes(1)
                })),
        PartitionedRateLimiter.Create<HttpContext, string>(context =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                factory: partition => new FixedWindowRateLimiterOptions()
                {
                    AutoReplenishment = true,
                    PermitLimit = 1000,
                    QueueLimit = 0,
                    Window = TimeSpan.FromHours(1)
                })));
});

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

builder.Services.AddAuthorization(options => { options.FallbackPolicy = options.DefaultPolicy; });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseAuthorization();
// app.MapControllers().RequireRateLimiting(fixedWindowRateLimitedPolicy);
// app.MapControllers().RequireRateLimiting(concurrencyRateLimitedPolicy);
app.MapControllers();
app.Run();