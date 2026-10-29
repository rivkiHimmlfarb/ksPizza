using KsPizza.Interfaces;
using KsPizza.Middleware;
using KsPizza.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPizzaService, PizzaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// "Kitchen closed" switch: when KitchenClosed=true in appsettings, every request
// below this line is short-circuited with 503. Placed after Swagger on purpose,
// so you can still open /swagger while the kitchen is "closed".
app.UseKitchenClosed();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
