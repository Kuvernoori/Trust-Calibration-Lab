using Microsoft.EntityFrameworkCore;
using TrustCalibrationLab.Api.Data;

var builder = WebApplication.CreateBuilder(args);

const string FrontendCorsPolicy = "Frontend";

builder.Services.AddControllers();
builder.Services.AddDbContext<LabDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default") ?? "Data Source=trust-calibration-lab.db"));
builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontendCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:4173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await SeedData.InitializeAsync(scope.ServiceProvider.GetRequiredService<LabDbContext>());
}

app.UseCors(FrontendCorsPolicy);
app.MapControllers();
app.MapGet("/", () => Results.Ok(new
{
    name = "Trust Calibration Lab API",
    status = "ok",
    endpoints = new[] { "/api/health", "/api/cases" }
}));

app.Run();

public partial class Program;
