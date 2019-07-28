using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using Sample.App.Filters;
using Sample.App.Session;
using Sample.App.SwaggerScripts;
using Sample.Shared.Options;

namespace Sample.App
{
  public class Startup
  {
    private readonly ILogger<Startup> _logger;
    private readonly IHostingEnvironment _hostingEnvironment;

    public IConfiguration Configuration { get; }

    public Startup(
      IConfiguration configuration,
      ILogger<Startup> logger,
      IHostingEnvironment hostingEnvironment)
    {
      Configuration = configuration;
      _logger = logger;
      _hostingEnvironment = hostingEnvironment;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      AddAppConfiguration(services);
      AddLoggingConfiguration(services);
      AddMongoDb(services);
      AddDependencies(services);
      AddCoreConfiguration(services);
      AddSwaggerConfiguration(services);
      AddAuthentication(services);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
      IApplicationBuilder app,
      IHostingEnvironment env,
      IApplicationLifetime applicationLifetime)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseExceptionHandler(errorApp =>
        {
          new ErrorApp(errorApp, _logger).Run();
        });
      }
      else
      {
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseExceptionHandler(errorApp =>
        {
          new ErrorApp(errorApp, _logger).Run();
        });
      }

      LogApplicationLifetimeEvents(applicationLifetime);
      app.UseCors(builder => builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins(
          $"*"));

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample API V1");
      });

      app.UseAuthentication();
      app.UseMvc();
    }


    protected virtual void AddSwaggerConfiguration(IServiceCollection services)
    {
      // Register the Swagger generator, defining 1 or more Swagger documents
      services.AddSwaggerGen(c =>
      {
        c.OperationFilter<UserIdHeaderFilter>();
        c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "Sample API", Version = "v1" });
      });
    }

    protected virtual void AddCoreConfiguration(IServiceCollection services)
    {
      services
        .AddMvcCore()
        .AddApiExplorer()
        .AddAuthorization()
        .AddFormatterMappings()
        .AddJsonFormatters()
        .AddCors()
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
        .AddJsonOptions(options =>
        {
          options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
          options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
    }

    protected virtual void AddAuthentication(IServiceCollection services)
    { }

    protected virtual void AddMongoDb(IServiceCollection services)
    {
      services.AddSingleton<IMongoClient, MongoClient>(context =>
      {
        var connectionString = Configuration.GetConnectionString("MongoDb");
        return new MongoClient(connectionString);
      });
      services.AddSingleton(context =>
      {
        var mongoDbClient = context.GetRequiredService<IMongoClient>();
        var databaseName = Configuration.GetValue<string>(StartupConfigurationKey.MongoDbDatabaseName);
        return mongoDbClient.GetDatabase(databaseName);
      });
    }

    protected virtual void AddAppConfiguration(IServiceCollection services)
    {
      services.Configure<AppOptions>(Configuration.GetSection(StartupConfigurationKey.App));
      services.Configure<MongoDbOptions>(mongoDbOptions =>
      {
        mongoDbOptions.DatabaseName = Configuration.GetSection(StartupConfigurationKey.MongoDbDatabaseName).Value;
        mongoDbOptions.ConnectionString = Configuration.GetConnectionString(StartupConfigurationKey.ConnectionStringsMongoDb);
      });
    }

    protected virtual void AddLoggingConfiguration(IServiceCollection services)
    { }

    protected virtual void AddDependencies(IServiceCollection services)
    {
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddScoped<IIdentityProvider, IdentityProvider>();
    }

    protected virtual void UseAuthentication(IApplicationBuilder app)
    {
      app.UseMiddleware<SessionMiddleware>();
      app.UseAuthentication();
    }

    private void LogApplicationLifetimeEvents(IApplicationLifetime applicationLifetime)
    {
      applicationLifetime.ApplicationStarted.Register(() =>
      {
        _logger.LogInformation("Application started");
      });

      applicationLifetime.ApplicationStopping.Register(() =>
      {
        _logger.LogInformation("Application stopping");
      });

      applicationLifetime.ApplicationStopped.Register(() =>
      {
        _logger.LogInformation("Application stopped");
      });
    }
  }
}
