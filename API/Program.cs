using core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Adding the connection string service
builder.Services.AddDbContext<StoreContext>(opt =>{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Adding the ProductRepository service to startup project to pick requests from API controller
builder.Services.AddScoped<IProductRepository, ProductRepository>();

//This is the service which we inject in controller and we add generic repository like this
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>)); // Since it is generic we add like this
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // o inject automapper service

var app = builder.Build();

// Configure the HTTP request pipeline.
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
