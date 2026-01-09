using Server.Hubs;
using Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddHostedService<CounterWorker>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowClient", policy => {
        policy.WithOrigins("http://localhost:5184", "http://127.0.0.1:5184") 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowClient"); 

app.MapControllers();
app.MapHub<CounterHub>("/counterhub");

app.Run();