using GameStore.Extensions;
using LoggingService;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using Presentation.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

// add services to the container

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), 
    "/nlog.config")); 
builder.Services.ConfigureCors(); 
builder.Services.ConfigureIISIntegration(); 
builder.Services.ConfigureLoggerService(); 
builder.Services.AddAuthentication(); 
builder.Services.ConfigureIdentity(); 
builder.Services.ConfigureJWT(builder.Configuration); 
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager(); 
builder.Services.AddAutoMapper(typeof(Program)); 
builder.Services.AddScoped<ValidationFilterAttribute>();

builder.Services.ConfigureSqlContext(builder.Configuration); 

builder.Services.AddControllers()
    .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);
var app = builder.Build();

//Configure the Http  request pipeine

#region Global Error handler
var logger = app.Services.GetRequiredService<ILoggerManager>(); 
app.ConfigureExceptionHandler(logger); 
 
if (app.Environment.IsProduction()) 
    app.UseHsts(); 

#endregion

app.UseHttpsRedirection();

app.UseStaticFiles(); 
app.UseForwardedHeaders(new ForwardedHeadersOptions 
{ 
    ForwardedHeaders = ForwardedHeaders.All 
}); 
 
app.UseCors("CorsPolicy");
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();