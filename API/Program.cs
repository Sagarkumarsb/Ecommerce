using API.Errors;
using API.Extensions;
using API.Middleware;
using core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

 builder.Services.AddControllers();

 //Created our own services class inherited from IServiceCollection
 builder.Services.AddApplicationServices(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>(); // our own middleware added
app.UseStatusCodePagesWithReExecute("/errors/{0}");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); //To access static content from API/postman

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Code added to create migration/database while running app
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context); //To add the data whil running the app
}
catch(Exception ex){
    logger.LogError(ex,"An error occured during migration");
}

app.Run();
