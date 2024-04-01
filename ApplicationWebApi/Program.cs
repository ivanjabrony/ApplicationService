using ConfApplicationService.Applications;
using ConfApplicationService.Contracts.Users;
using ConfApplicationService.Extensions;
using ConfApplicationService.Implementation.Extensions;
using ConfApplicationService.Models.ResultTypes;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructureDataAccess();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/DraftApplication/new", async ([FromBody]DraftApplicationJsonModel model) =>
{
    var userService = app.Services.GetService<IUserService>();
    var applicationService = app.Services.GetService<ApplicationService>();
    var currentUserManager = app.Services.GetService<ICurrentUserService>();
    if (userService == null || applicationService == null || currentUserManager == null) 
        return TypedResults.NotFound();
    
    var result = await userService.LoginUserAsync(model.AuthorId);
    if (result is RepoActResult.FailRepoAct badResult)
    {
        return Results.NotFound(badResult.Message);
    }

    
    var appResult = await applicationService.CreateNewDraftAsync(currentUserManager);
    if (appResult is RepoActResult.FailRepoAct failRepoAct)
    {
        return Results.NotFound(failRepoAct.Message);
    }
    
    applicationService.CurrentDraftEditor.ChangeTitle(model.Title);
    applicationService.CurrentDraftEditor.ChangeActivity(model.Activity);
    applicationService.CurrentDraftEditor.ChangeDescription(model.Description);
    applicationService.CurrentDraftEditor.ChangePlan(model.Plan);

    var saveRes = await applicationService.SaveDraftChanges(currentUserManager);
    return saveRes switch
    {
        RepoActResult.FailRepoAct failRepoAct1 => Results.Problem(failRepoAct1.Message),
        RepoActResult.SuccessRepoAct => Results.Ok(),
        _ => Results.NotFound()
    };
}).WithName("PostNewDraftApplication");

app.MapPut("/DraftApplication/update", async ([FromBody] DraftApplicationJsonModel model) =>
{
    var userService = app.Services.GetService<IUserService>();
    var applicationService = app.Services.GetService<ApplicationService>();
    var currentUserManager = app.Services.GetService<ICurrentUserService>();
    if (userService == null || applicationService == null || currentUserManager == null) 
        return TypedResults.NotFound();
    
    var result = await userService.LoginUserAsync(model.AuthorId);
    if (result is RepoActResult.FailRepoAct badResult)
    {
        return Results.NotFound(badResult.Message);
    }
    
    var appResult = await applicationService.GetDraftAsync(currentUserManager);
    if (appResult is RepoActResult.FailRepoAct failRepoAct)
    {
        return Results.NotFound(failRepoAct.Message);
    }
    
    applicationService.CurrentDraftEditor.ChangeTitle(model.Title);
    applicationService.CurrentDraftEditor.ChangeActivity(model.Activity);
    applicationService.CurrentDraftEditor.ChangeDescription(model.Description);
    applicationService.CurrentDraftEditor.ChangePlan(model.Plan);

    var saveRes = await applicationService.SaveDraftChanges(currentUserManager);
    return saveRes switch
    {
        RepoActResult.FailRepoAct failRepoAct1 => Results.Problem(failRepoAct1.Message),
        RepoActResult.SuccessRepoAct => Results.Ok(),
        _ => Results.NotFound()
    };
}).WithName("PutDraftApplicationUpdate");

app.MapPut("/DraftApplication/finish/{id}", async (Guid id) =>
{
    var userService = app.Services.GetService<IUserService>();
    var applicationService = app.Services.GetService<ApplicationService>();
    var currentUserManager = app.Services.GetService<ICurrentUserService>();
    if (userService == null || applicationService == null || currentUserManager == null) 
        return TypedResults.NotFound();
    
    var result = await userService.LoginUserAsync(id);
    if (result is RepoActResult.FailRepoAct badResult)
    {
        return Results.NotFound(badResult.Message);
    }
    
    var finishResult = await applicationService.FinishDraftApplicationAsync(currentUserManager);
    return finishResult switch
    {
        RepoActResult.FailRepoAct failRepoAct1 => Results.Problem(failRepoAct1.Message),
        RepoActResult.SuccessRepoAct => Results.Ok(),
        _ => Results.NotFound()
    };
}).WithName("PutFinishedApplication");

app.MapDelete("/DraftApplication/delete/userId={userId}", async (Guid userId) =>
{
    var userService = app.Services.GetService<IUserService>();
    var applicationService = app.Services.GetService<ApplicationService>();
    var currentUserManager = app.Services.GetService<ICurrentUserService>();
    if (userService == null || applicationService == null || currentUserManager == null) 
        return TypedResults.NotFound();
    
    var result = await userService.LoginUserAsync(userId);
    if (result is RepoActResult.FailRepoAct badResult)
    {
        return Results.NotFound(badResult.Message);
    }

    var deleteRes = await applicationService.DeleteDraftAsync(currentUserManager);
    return deleteRes switch
    {
        RepoActResult.FailRepoAct failRepoAct1 => Results.Problem(failRepoAct1.Message),
        RepoActResult.SuccessRepoAct => Results.Ok(),
        _ => Results.NotFound()
    };
}).WithName("DeleteDraftApplication");

app.MapGet("/DraftApplication/fromData/", async Task<Results<Ok<List<DraftApplicationJsonModel>>,NotFound>> ([FromBody] DateTime date) =>
{
    var applicationService = app.Services.GetService<ApplicationService>();
    if (applicationService == null) return TypedResults.NotFound();
 
    var finishResult = await applicationService.GetAllDraftsAsync();
    return TypedResults.Ok(
        finishResult
            .Where(a => a.CreatingDate > date)
            .Select(a => new DraftApplicationJsonModel(a))
            .ToList());
}).WithName("GetDraftApplicationsSinceDate");

app.MapGet("/DraftApplication/userId={id}", async Task<Results<Ok<DraftApplicationJsonModel>, NotFound>> (Guid id) =>
{
    var userService = app.Services.GetService<IUserService>();
    var applicationService = app.Services.GetService<ApplicationService>();
    var currentUserManager = app.Services.GetService<ICurrentUserService>();
    if (userService == null || applicationService == null || currentUserManager == null) 
        return TypedResults.NotFound();
    
    var result = await userService.LoginUserAsync(id);
    if (result is RepoActResult.FailRepoAct badResult)
    {
        return TypedResults.NotFound();
    }
    
    var getResult = await applicationService.GetDraftAsync(currentUserManager);
    if (applicationService.CurrentDraftEditor.EditingDraft == null) return TypedResults.NotFound();
    return getResult switch
    {
        RepoActResult.FailRepoAct failRepoAct1 => TypedResults.NotFound(),
        RepoActResult.SuccessRepoAct => TypedResults.Ok(new DraftApplicationJsonModel(applicationService.CurrentDraftEditor.EditingDraft)),
        _ => TypedResults.NotFound()
    }; 
}).WithName("GetDraftOfUser");



app.MapGet("/FinishedApplication/{id}", async Task<Results<Ok<FinishedApplicationJsonModel>, NotFound>>(Guid id) =>
{
    var applicationService = app.Services.GetService<ApplicationService>();
    if (applicationService == null) return TypedResults.NotFound();
 
    var finishResult = await applicationService.GetAllApplicationsAsync();
    return TypedResults.Ok(
        finishResult
            .Where(a => a.FinishedApplicationId == id)
            .Select(a => 
                new FinishedApplicationJsonModel(a))
            .SingleOrDefault());    
}).WithName("GetFinishedApplication");

app.MapGet("/FinishedApplication/getFromData/", async Task<Results<Ok<List<FinishedApplicationJsonModel>>,NotFound>> ([FromBody] DateTime date) =>
{
    var applicationService = app.Services.GetService<ApplicationService>();
    if (applicationService == null) return TypedResults.NotFound();
 
    var finishResult = await applicationService.GetAllApplicationsAsync();
    return TypedResults.Ok(
        finishResult
        .Where(a => a.CreationTime > date)
        .Select(a => 
            new FinishedApplicationJsonModel(a))
        .ToList());
}).WithName("GetFinishedApplicationsSinceDate");


app.MapPost("User/RegisterUser/{id};{name}", async (string name) =>
{
    var userService = app.Services.GetService<IUserService>();
    if (userService == null) return Results.NotFound();
    
    var result = await userService.RegisterUserAsync(name);
    if (result is RepoActResult.FailRepoAct failRepoAct)
    {
        return Results.Problem(failRepoAct.Message);
    }
    return Results.Ok(); 
}).WithName("PostNewRegisteredUser");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}