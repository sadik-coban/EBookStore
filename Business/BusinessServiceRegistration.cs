using Business.Features.Account;
using Business.Features.Catalogs;
using Core.Entities;
using Core.Infrastructure.Base.RepositoriesBase;
using Microsoft.Extensions.DependencyInjection;

namespace Business;
public static class BusinessServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryBase<Catalog>,RepositoryBase<Catalog>>();
        services.AddScoped<ICatalogsService,CatalogsService>();
        services.AddScoped<IAccountService, AccountService>();
        return services;
    }
}
