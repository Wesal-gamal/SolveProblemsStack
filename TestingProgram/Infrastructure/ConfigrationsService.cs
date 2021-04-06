using Attendleave.Erp.Core.APIUtilities;
using Attendleave.Erp.Core.Repository;
using Attendleave.Erp.Core.UnitOfWork;
using DataLayer.Identity;
using DataLayer.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingProgram.Business;

namespace TestingProgram.Infrastructure
{
    public static class ConfigrationsService
    {
        public static IServiceCollection AddRegisterServices(this IServiceCollection services)
        {
            services.AddScoped<DbContext, MyDBContext>();
            services.AddScoped<ISolutionsBusiness, solutionsBusiness>();
            services.AddTransient<IActionResultResponseHandler, ActionResultResponseHandler>();
            services.AddTransient<IRepositoryActionResult, RepositoryActionResult>();
            services.AddTransient<IRepositoryResult, RepositoryResult>();
            services.AddScoped<IRepository<ApplicationUser>, Repository<ApplicationUser>>();
            services.AddScoped<IUnitOfWork<ApplicationUser>, UnitOfWork<ApplicationUser>>();
            services.AddScoped<IRepository<ApplicationRole>, Repository<ApplicationRole>>();
            services.AddScoped<IUnitOfWork<ApplicationRole>, UnitOfWork<ApplicationRole>>();

            services.AddScoped<IRepository<Problems>, Repository<Problems>>();
            services.AddScoped<IUnitOfWork<Problems>, UnitOfWork<Problems>>();
            services.AddScoped<IRepository<Comments>, Repository<Comments>>();
            services.AddScoped<IUnitOfWork<Comments>, UnitOfWork<Comments>>();
            services.AddScoped<IRepository<Solutions>, Repository<Solutions>>();
            services.AddScoped<IUnitOfWork<Solutions>, UnitOfWork<Solutions>>();
            services.AddScoped<IRepository<CommentLikes>, Repository<CommentLikes>>();
            services.AddScoped<IUnitOfWork<CommentLikes>, UnitOfWork<CommentLikes>>();
            services.AddScoped<IRepository<Categories>, Repository<Categories>>();
            services.AddScoped<IUnitOfWork<Categories>, UnitOfWork<Categories>>();
            services.AddScoped<IRepository<SolutionLikes>, Repository<SolutionLikes>>();
            services.AddScoped<IUnitOfWork<SolutionLikes>, UnitOfWork<SolutionLikes>>();

            return services;
        }
    }
}
