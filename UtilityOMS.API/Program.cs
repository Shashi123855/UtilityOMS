using Microsoft.EntityFrameworkCore;
using UtilityOMS.API.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ SQLite - Free, no installation needed!
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=utility_oms.db"));

// ✅ Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ SignalR - Real-time updates (free, built-in)
builder.Services.AddSignalR();

// ✅ CORS - Allow Blazor frontend to talk to API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ✅ Auto-create and migrate the SQLite database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ✅ Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();