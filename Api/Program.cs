using Api.Helper;
using Asp.Versioning;
using Errlake.Crosscutting;
using IOC.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Shared.Common;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
.AddEnvironmentVariables();

builder.Services.Configure<EmailConfigurationKeys>(builder.Configuration.GetSection("EmailConfigurationKeys"));//for Email configuration keys
builder.Services.Configure<SiteKeys>(builder.Configuration.GetSection("SiteKeys"));
SiteKeys.SitePhysicalPath = builder.Configuration["SiteKeys:SitePhysicalPath"];
SiteKeys.SiteUrl = builder.Configuration["SiteKeys:SiteUrl"];
SiteKeys.EncryptDecryptKey = builder.Configuration["SiteKeys:EncryptDecryptKey"];


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.ConfigureModelSettings(builder.Configuration);

builder.Services.RegisterWebApi(builder.Configuration);

builder.Services.ConfigureJWTAuthentication(builder.Configuration);

builder.Services.ConfigureAuthorization();

builder.Services.AddRazorPages();
builder.Services.AddError();

builder.Services.ConfigureApiVersioning();

builder.Services.ConfigureSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.ConfigureApiExceptionMiddlewareHandler();

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                );




app.UseHttpsRedirection();

app.UseAuthentication();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var descriptions = app.DescribeApiVersions();

    // Build a swagger endpoint for each discovered API version
    foreach (var description in descriptions)
    {
        var url = $"/swagger/{description.GroupName}/swagger.json";
        if (!app.Environment.IsDevelopment())
        {
            url = $"/api/swagger/{description.GroupName}/swagger.json";
        }
        var name = description.GroupName.ToUpperInvariant();
        options.SwaggerEndpoint(url, name);
    }
});

app.MapControllers();
app.MapRazorPages();
app.Run();
