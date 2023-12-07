using System.Reflection;
using LastSeenDemo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register services for dependency injection
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddTransient<ILoader, Loader>();
builder.Services.AddScoped<IOnlineDetector, OnlineDetector>();
builder.Services.AddScoped<IPredictor, Predictor>();
builder.Services.AddScoped<IUserLoader>(sp => new UserLoader(sp.GetRequiredService<ILoader>(), "https://sef.podkolzin.consulting/api/users/lastSeen"));
builder.Services.AddScoped<ILastSeenApplication, LastSeenApplication>();
builder.Services.AddScoped<IUserTransformer, UserTransformer>();
builder.Services.AddScoped<IAllUsersTransformer, AllUsersTransformer>();
builder.Services.AddScoped<IWorker, Worker>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use dependency injection in endpoints
app.MapGet("/", () => "Hello!"); // Just Demo Endpoint
app.MapGet("/version", () => new
{
    Version = 2,
    Assembly = Assembly.GetAssembly(typeof(Program)).Location,
    Modified = File.GetLastWriteTime(Assembly.GetAssembly(typeof(Program)).Location)
});

Setup2ndAssignmentsEndpoints(app);
Setup3rdAssignmentsEndpoints(app);
Setup4thAssignmentsEndpoints(app);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

void Setup2ndAssignmentsEndpoints(WebApplication app)
{
    app.MapGet("/formatted", (ILastSeenApplication application) => application.Show(DateTimeOffset.Now));
}

void Setup3rdAssignmentsEndpoints(WebApplication app)
{
    app.MapGet("/api/stats/users/", (IOnlineDetector detector, IWorker worker, DateTimeOffset date) =>
    {
        return new { usersOnline = detector.CountOnline(worker.Users, date) };
    });

    app.MapGet("/api/stats/user", (IOnlineDetector detector, IWorker worker, DateTimeOffset date, Guid userId) =>
    {
        if (!worker.Users.ContainsKey(userId))
            return Results.NotFound(new { userId });
        var user = worker.Users[userId];
        return Results.Json(new
        {
            wasUserOnline = detector.Detect(user, date),
            nearestOnlineTime = detector.GetClosestOnlineTime(user, date)
        });
    });

    app.MapGet("/api/predictions/users", (IPredictor predictor, IWorker worker, DateTimeOffset date) =>
    {
        return new { onlineUsers = predictor.PredictUsersOnline(worker.Users, date) };
    });

    app.MapGet("/api/predictions/user", (IPredictor predictor, IWorker worker, Guid userId, DateTimeOffset date, float tolerance) =>
    {
        if (!worker.Users.TryGetValue(userId, out var user))
            return Results.NotFound(new { userId });
        var onlineChance = predictor.PredictUserOnline(user, date);
        return Results.Json(new
        {
            onlineChance,
            willBeOnline = onlineChance > tolerance
        });
    });
}

void Setup4thAssignmentsEndpoints(WebApplication app)
{
    app.MapGet("/api/stats/user/total", (IOnlineDetector detector, IWorker worker, Guid userId) =>
    {
        if (!worker.Users.TryGetValue(userId, out var user))
            return Results.NotFound(new { userId });
        return Results.Json(new { totalTime = detector.CalculateTotalTimeForUser(user) });
    });

    app.MapGet("/api/stats/user/average", (IOnlineDetector detector, IWorker worker, Guid userId) =>
    {
        if (!worker.Users.TryGetValue(userId, out var user))
            return Results.NotFound(new { userId });
        return Results.Json(new
        {
            dailyAverage = detector.CalculateDailyAverageForUser(user),
            weeklyAverage = detector.CalculateWeeklyAverageForUser(user),
        });
    });

    app.MapPost("/api/user/forget", (IWorker worker, Guid userId) =>
    {
        if (!worker.Users.ContainsKey(userId))
            return Results.NotFound(new { userId });
        worker.Forget(userId);
        return Results.Ok();
    });
}

