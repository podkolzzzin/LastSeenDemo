using System.Reflection;
using LastSeenDemo;

var builder = WebApplication.CreateBuilder(args);

// Global Application Services



// Dependency Injection Setup
builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<ILoader, Loader>();
builder.Services.AddScoped<IOnlineDetector, OnlineDetector>();
builder.Services.AddTransient<IPredictor, Predictor>();

// Register UserLoader as IUserLoader implementation
builder.Services.AddTransient<UserLoader>(); // Add this line

builder.Services.AddTransient<IAllUsersTransformer, AllUsersTransformer>();
builder.Services.AddSingleton<IWorker, Worker>();



// End Dependency Injection Setup

var app = builder.Build();

// Application Initialization
app.Services.GetRequiredService<IWorker>().LoadDataPeriodically();

// Setup APIs
app.MapGet("/", () => "Hello!"); // Just Demo Endpoint
app.MapGet("/version", () => new
{
    Version = 2,
    Assembly = Assembly.GetAssembly(typeof(Program)).Location,
    Modified = File.GetLastWriteTime(Assembly.GetAssembly(typeof(Program)).Location)
});

Setup2ndAssignmentsEndpoints(app);
app.UseSwagger();
app.UseSwaggerUI();

app.Run();

void Setup2ndAssignmentsEndpoints(WebApplication app)
{
    app.MapGet("/formatted", (IUserLoader userLoader, ILastSeenApplication application) => 
        application.Show(DateTimeOffset.Now)); // Assignment#2 in API form
}

void Setup3rdAssignmentsEndpoints(WebApplication app)
{
    var detector = app.Services.GetRequiredService<IOnlineDetector>(); // Add this line
    var worker = app.Services.GetRequiredService<IWorker>(); // Add this line
    var predictor = app.Services.GetRequiredService<IPredictor>(); // Add this line

    // Feature#1 - Implement an endpoint that returns historical data for all users
    app.MapGet("/api/stats/users/", (DateTimeOffset date) =>
    {
        int usersOnline = detector.CountOnline(worker.Users, date);
        return Results.Json(new { usersOnline });
    });

    // Feature#2 - Implement endpoint that returns historical data for a concrete user
    app.MapGet("/api/stats/user", (DateTimeOffset date, Guid userId) =>
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

    // Feature#3 - Implement endpoint that returns historical data for a concrete user
    app.MapGet("/api/predictions/users", (DateTimeOffset date) =>
    {
        return new { onlineUsers = predictor.PredictUsersOnline(worker.Users, date) };
    });

    // Feature#4 - Implement a prediction mechanism based on historical data for a concrete user
    app.MapGet("/api/predictions/user", (Guid userId, DateTimeOffset date, float tolerance) =>
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
    var detector = app.Services.GetRequiredService<IOnlineDetector>(); // Add this line
    var worker = app.Services.GetRequiredService<IWorker>(); // Add this line

    // Feature#1 - Implement an endpoint that returns total time that user was online
    app.MapGet("/api/stats/user/total", (Guid userId) =>
    {
        if (!worker.Users.TryGetValue(userId, out var user))
            return Results.NotFound(new { userId });
        return Results.Json(new { totalTime = detector.CalculateTotalTimeForUser(user) });
    });

    // Feature#2 - Implement endpoints that return average daily/weekly time for the specified user
    app.MapGet("/api/stats/user/average", (Guid userId) =>
    {
        if (!worker.Users.TryGetValue(userId, out var user))
            return Results.NotFound(new { userId });
        return Results.Json(new
        {
            dailyAverage = detector.CalculateDailyAverageForUser(user),
            weeklyAverage = detector.CalculateWeeklyAverageForUser(user),
        });
    });

    // Feature#3 - Implement an endpoint to follow the EU regulator rules - GDPR - right to be forgotten
    app.MapPost("/api/user/forget", (Guid userId) =>
    {
        if (!worker.Users.ContainsKey(userId))
            return Results.NotFound(new { userId });
        worker.Forget(userId);
        return Results.Ok();
    });
}

