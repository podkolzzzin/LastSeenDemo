using System.Reflection;
using LastSeenDemo;

// Global Application Services
var dateTimeProvider = new DateTimeProvider();
var loader = new Loader();
var detector = new OnlineDetector(dateTimeProvider);
var predictor = new Predictor(detector);
var userLoader = new UserLoader(loader, "https://sef.podkolzin.consulting/api/users/lastSeen");
var application = new LastSeenApplication(userLoader);
var userTransformer = new UserTransformer(dateTimeProvider);
var allUsersTransformer = new AllUsersTransformer(userTransformer);
var worker = new Worker(userLoader, allUsersTransformer);
// End Global Application Services

Task.Run(worker.LoadDataPeriodically); // Launch collecting data in background

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// APIs
var app = builder.Build();

app.MapGet("/", () => "Hello!"); // Just Demo Endpoint
app.MapGet("/version", () => new
{
    Version = 2,
    Assembly = Assembly.GetAssembly(typeof(Program)).Location,
    Modified = File.GetLastWriteTime(Assembly.GetAssembly(typeof(Program)).Location)
});

Setup2ndAssignmentsEndpoints();
Setup3rdAssignmentsEndpoints();
Setup4thAssignmentsEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();


void Setup2ndAssignmentsEndpoints()
{
    app.MapGet("/formatted", () => application.Show(DateTimeOffset.Now)); // Assignment#2 in API form
}

void Setup3rdAssignmentsEndpoints()
{
    // Feature#1 - Implement endpoint that returns historical data for all users
    app.MapGet("/api/stats/users/", (DateTimeOffset date) =>
    {
        // int usersOnline = 0;
        // foreach (var (_, user) in users)
        // {
        //   if (detector.Detect(user, date))
        //   {
        //     usersOnline++;
        //   }
        // }
        // return new { usersOnline };
        return new { usersOnline = detector.CountOnline(worker.Users, date) };
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

    // Feature#4 - Implement a prediction mechanism based on a historical data for concrete user
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

void Setup4thAssignmentsEndpoints()
{
    // Feature#1 - Implement an endpoint that returns total time that user was online
    app.MapGet("/api/stats/user/total", (Guid userId) =>
    {
        if (!worker.Users.TryGetValue(userId, out var user))
            return Results.NotFound(new { userId });
        return Results.Json(new { totalTime = detector.CalculateTotalTimeForUser(user) });
    });

    // Feature#2 - Implement endpoints that returns average daily/weekly time for the specified user
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

    // Feature#3 - Implement endpoint to follow the EU regulator rules - GDPR - right to be forgotten
    app.MapPost("/api/user/forget", (Guid userId) =>
    {
        if (!worker.Users.ContainsKey(userId))
            return Results.NotFound(new { userId });
        worker.Forget(userId);
        return Results.Ok();
    });

    // Defines an HTTP GET endpoint at the specified route ("/api/stats/user/reports").
    // When this endpoint is hit, the enclosed lambda function is executed.
    app.MapGet("/api/stats/user/reports", () =>
    {
        // A dictionary to hold the daily average values for each user, 
        // with the user's Guid as the key and the average as the value.
        var dailyAverages = new Dictionary<Guid, double>();

        // Similar to the dailyAverages dictionary, this holds weekly average values for each user.
        var weeklyAverages = new Dictionary<Guid, double>();

        // Iterates over each userId found in the 'Users' dictionary of the 'worker' object.
        foreach (var userId in worker.Users.Keys)
        {
            // Tries to retrieve the user details from the 'Users' dictionary using the userId.
            // If the user is found, 'user' is set with the user's details, and the block executes.
            if (worker.Users.TryGetValue(userId, out var user))
            {
                // Calculates the daily average for the current user by invoking a method 
                // on the 'detector' object and stores the result in the dailyAverages dictionary.
                var dailyAverage = detector.CalculateDailyAverageForUser(user);
                dailyAverages.Add(userId, dailyAverage);

                // Similarly, calculates the weekly average for the user and stores it in the weeklyAverages dictionary.
                var weeklyAverage = detector.CalculateWeeklyAverageForUser(user);
                weeklyAverages.Add(userId, weeklyAverage);
            }
        }

        // Calculates the overall average weekly average across all users. 
        // This represents an aggregation over the entire dataset.
        var averageWeeklyAverage = detector.CalculateAverageWeeklyAverageForAllUsers(worker.Users);

        // Constructs a response in JSON format containing the daily and weekly averages per user,
        // and the overall average weekly average, then returns this JSON as the API response.
        return Results.Json(new { DailyAverages = dailyAverages, WeeklyAverages = weeklyAverages, AverageWeeklyAverage = averageWeeklyAverage });
    });
    
    // Define the endpoint for "/api/users/list"
    app.MapGet("/api/users/list", () =>
    {
        // Extracting and transforming user data
        var usersList = worker.Users.Select(userEntry => new
        {
            username = userEntry.Key.ToString(),
            userId = userEntry.Key,
            firstSeen = userEntry.Value.FirstOrDefault()?.Login.ToString("o") // ISO 8601 format
        }).ToList();

        return Results.Json(usersList);
    });
}

/*
// ssh -i deploy_key root@lastseendemo.top

/// <summary>
/// Sets up the endpoints for the 7th Assignment, including the endpoint for fetching report data.
/// </summary>
void Setup7thAssignmentEndpoints()
{
    app.MapGet("/api/report/{reportName}", (string reportName, DateTimeOffset from, DateTimeOffset to) =>
    {
        // Generate the report data based on the report name and the specified date range.
        var reportData = worker.GenerateReportData(reportName, from, to);

        // Calculate global metrics across all users.
        var globalMetrics = allUsersTransformer.CalculateGlobalMetrics();

        // Construct the response object.
        var response = new
        {
            Users = reportData,
            DailyAverage = globalMetrics.DailyAverage,
        };

        // Return the response in JSON format.
        return Results.Json(response);
    });
}

*/
