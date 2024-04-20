using Autofac;
using Autofac.Extensions.DependencyInjection;
using TestsService.API.Utility;
using System.Text.Json.Serialization;
using TestsService.Infrastructure.Configuration;
using TestsService.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using TestsService.AppCore;
using TestsService.External;
using TestsService.AppCore.Infrastructure.AutofacModules;
using Quartz;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => 
{
    options.CustomSchemaIds(type => type.ToString());
});

////configure autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//// Register services directly with Autofac here. 
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MediatorModule()));

//DB context configuration
// builder.Services.AddDataServices(builder.Configuration);

// MongoDB configuration
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection(nameof(MongoDbSettings)));
builder.Services.AddSingleton<IMongoDbSettings>(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
builder.Services.AddSingleton<IMongoClient>(s => new MongoClient(builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString")));

//AppCore configuration
builder.Services.AddAppCore(builder.Configuration);

//AppCore configuration
builder.Services.AddExternal(builder.Configuration);

//stickers configuration
//builder.Services.AddStickers(builder.Configuration);


builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy",
        builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
        );

});

//register resources languages
builder.Services.AddLocalizationServices();


//register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<IHttpClient, TestsService.External.HttpClient>();
builder.Services.AddMemoryCache();

var app = builder.Build();

//exception middleware
app.UseMiddleware<JsonExceptionMiddleware>();

////client language culture( set default or selected language by client )
//app.UseMiddleware<CultureProviderMiddleware>();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();
