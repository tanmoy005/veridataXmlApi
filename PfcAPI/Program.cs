using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using VERIDATA.BLL.apiContext.Common;
using VERIDATA.BLL.apiContext.karza;
using VERIDATA.BLL.apiContext.surepass;
using VERIDATA.BLL.Authentication;
using VERIDATA.BLL.Context;
using VERIDATA.BLL.Extensions;
using VERIDATA.BLL.Interfaces;
using VERIDATA.BLL.Notification.Provider;
using VERIDATA.BLL.Notification.Sender;
using VERIDATA.BLL.Services;
using VERIDATA.BLL.utility;
using VERIDATA.DAL.DataAccess.Context;
using VERIDATA.DAL.DataAccess.Interfaces;
using VERIDATA.DAL.DBContext;
using VERIDATA.Model.Configuration;
//using Hangfire.PostgreSql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
EmailConfiguration? emailConfig = builder.Configuration.GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();

TokenConfiguration? tokenConfig = builder.Configuration.GetSection("JwtToken")
    .Get<TokenConfiguration>();

ApiConfiguration? aadhaarConfig = builder.Configuration.GetSection("ApiConfiguration")
    .Get<ApiConfiguration>();
ConfigurationSetup? configSetup = builder.Configuration.GetSection("ConfigurationSetup")
    .Get<ConfigurationSetup>();
ConfigurationManager config = builder.Configuration;
CommonUtility.Initialize(aadhaarConfig);  //program.cs file

builder.Services.AddSingleton(emailConfig);
builder.Services.AddSingleton(tokenConfig);
builder.Services.AddSingleton(aadhaarConfig);
builder.Services.AddSingleton(configSetup);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IAuthorizationHandler, CustomAuthorizeAttribute>();
builder.Services.AddScoped<ICandidateContext, CandidateContext>();
builder.Services.AddScoped<IFileContext, FileContext>();
builder.Services.AddScoped<IReportingContext, ReportingContext>();
builder.Services.AddScoped<ISetupConfigarationContext, SetupConfigarationContext>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IWorkFlowContext, WorkFlowContext>();
builder.Services.AddScoped<IVerifyDataContext, VerifyDataContext>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<ITokenAuth, TokenAuth>();
builder.Services.AddScoped<IWorkerService, WorkerService>();

builder.Services.AddScoped<IActivityDalContext, ActivityDalContext>();
builder.Services.AddScoped<IAppointeeDalContext, AppointeeDalContext>();
builder.Services.AddScoped<IMasterDalContext, MasterDalContext>();
builder.Services.AddScoped<IReportingDalContext, ReportingDalContext>();
builder.Services.AddScoped<IUserDalContext, UserDalContext>();
builder.Services.AddScoped<IWorkFlowDalContext, WorkFlowDalContext>();

builder.Services.AddScoped<IkarzaApiContext, KarzaApiContext>();
builder.Services.AddScoped<IsurepassApiContext, SurepassApiContext>();
builder.Services.AddScoped<IUitityContext, UitityContext>();

//builder.Services.AddDbContext<DbContextDB>(Options => Options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));
builder.Services.AddDbContext<DbContextDalDB>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase"), builder => builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));
//builder.Services.AddDbContext<DbContextDB>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase")));
builder.Services.AddCors();
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory =  // the interjection
        ModelStateValidator.ValidateModelState;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//builder.Services.AddControllers();

builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.MemoryBufferThreshold = Int32.MaxValue;
});



builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Veridata API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddScheme<CustomAuthenticationOptions, CustomAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, options => { });

builder.Services.AddAuthorization();

//builder.Services.AddHangfire(configuration => configuration.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("WebApiDatabase")));
builder.Services.AddHangfire(configuration => configuration
      .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
      .UseSimpleAssemblyNameTypeSerializer()
      .UseRecommendedSerializerSettings()
      .UseSqlServerStorage(builder.Configuration.GetConnectionString("WebApiDatabase")));
////builder.Services.AddHangfire(configuration => configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("WebApiDatabase")));
builder.Services.AddHangfireServer();

builder.Services.AddApplicationInsightsTelemetry();

Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
//configuration.ReadFrom.Configuration(context.Configuration));
builder.Logging.ClearProviders();

builder.Services.AddScoped<IApiConfigService, ApiConfigService>();
builder.Services.AddMemoryCache();

WebApplication app = builder.Build();

app.UseSerilogRequestLogging();

CultureInfo tempCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
tempCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
tempCulture.DateTimeFormat.LongDatePattern = "dd/MM/yyyy";
List<CultureInfo> supportedCultures = new() { tempCulture };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-GB"),
    SupportedCultures = supportedCultures,
    FallBackToParentCultures = false
});

//builder.Services.AddHostedService<WorkerService>();
app.ConfigureExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}
// Shows UseCors with CorsPolicyBuilder.
app.UseCors(builder =>
{
    _ = builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.UseAuthentication();
app.UseAuthorization();
//backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new DashboardAuthorizationFilter() },
    DashboardTitle = "My Background Jobs"
    // You can customize the dashboard options here.
});

//app.UseHttpsRedirection();
app.MapHangfireDashboard();


app.MapControllers();

app.Run();
public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Check if user is authenticated and has necessary permissions
        //return context.GetHttpContext().User.Identity.IsAuthenticated;
        return true;
    }
}