﻿using Business.Features.Account;
using Business.Features.Authors;
using Business.Features.Catalogs;
using Business.Features.Comments;
using Business.Features.Favorites;
using Business.Features.Products;
using Business.Features.Publishers;
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

        services.AddScoped<IRepositoryBase<Publisher>, RepositoryBase<Publisher>>();
        services.AddScoped<IPublishersService, PublishersService>();

        services.AddScoped<IRepositoryBase<Author>, RepositoryBase<Author>>();
        services.AddScoped<IAuthorsService, AuthorsService>();

        services.AddScoped<IRepositoryBase<Product>, RepositoryBase<Product>>();
        services.AddScoped<IProductsService, ProductsService>();

        services.AddScoped<IRepositoryBase<Favorite>, RepositoryBase<Favorite>>();
        services.AddScoped<IFavoritesService, FavoritesService>();

        services.AddScoped<IRepositoryBase<Comment>, RepositoryBase<Comment>>();
        services.AddScoped<ICommentsService, CommentsService>();

        services.AddScoped<IRepositoryBase<Customer>, RepositoryBase<Customer>>();

        services.AddScoped<IAccountService, AccountService>();

        return services;
    }
}
