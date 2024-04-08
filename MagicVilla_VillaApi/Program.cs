using Serilog;
using MagicVilla_VillaApi.logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//bothh third party logger package required 
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log/villaLogs/villalogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();
builder.Host.UseSerilog();


builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Ilogging Dependencies Injection(where it bind logging implentation of Ilogging to it)
builder.Services.AddSingleton<ILogging,LoggingV2>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
