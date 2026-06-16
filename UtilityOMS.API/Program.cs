using Microsoft.EntityFrameworkCore;
using UtilityOMS.API.Data;
using UtilityOMS.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

// ✅ SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=utility_oms.db"));

// ✅ Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ SignalR
builder.Services.AddSignalR();

// ✅ CORS - Allow Blazor + SignalR
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5001",
                           "https://localhost:5001",
                           "http://localhost:7000",
                           "https://localhost:7000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // ← Required for SignalR!
    });
});

var app = builder.Build();

// ✅ Auto-create DB on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

// ✅ Map SignalR Hub
app.MapHub<OutageHub>("/outageHub");

app.Run();