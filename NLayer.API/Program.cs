using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Caching.Redis;
using NLayer.Repository.Entityframework.Contexts.AppDbContexts;
using NLayer.Service.Mapping;
using NLayer.Service.Validations;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.
    AddControllers(options => 
    { 
        options.Filters.Add(new ValidateFilterAttribute()); 
    }).
    AddFluentValidation(x => 
    { 
        x.RegisterValidatorsFromAssemblyContaining<ProductCreateDtoValidator>(); 
        x.DisableDataAnnotationsValidation = true;
        x.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
    });

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddScoped(typeof(NotFoundFilter<>));

builder.Services.AddAutoMapper(typeof(MapProfile));

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

builder.Host.UseServiceProviderFactory
    (new AutofacServiceProviderFactory ());
builder.Host.ConfigureContainer<ContainerBuilder>(containetBuilder => containetBuilder.RegisterModule(new RepoServiceModule()));

builder.Services.AddSingleton<RedisService>(sp =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseCustomException();

app.MapControllers();

app.Run();
