using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services,IConfiguration config){

                            // Add services to the container.

              
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                Services.AddEndpointsApiExplorer();
                Services.AddSwaggerGen();

                //Adding the connection string service
                Services.AddDbContext<StoreContext>(opt =>{
                    opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
                });

                // Adding the ProductRepository service to startup project to pick requests from API controller
                Services.AddScoped<IProductRepository, ProductRepository>();

                //This is the service which we inject in controller and we add generic repository like this
                Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>)); // Since it is generic we add like this
               Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // o inject automapper service
                Services.Configure<ApiBehaviorOptions>(options =>{
                    options.InvalidModelStateResponseFactory = actionContext =>
                    {
                        var errors = actionContext.ModelState
                        .Where(e=>e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                        var errorResponse = new ApiValidationErrorResponse{
                            Errors = errors
                        };
                        return new BadRequestObjectResult(errorResponse);
                    };
                });

                return Services;
        }
    }
}