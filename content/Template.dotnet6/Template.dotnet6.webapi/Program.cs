using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Template.dotnet6.webapi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioningConfigured();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        // add a custom operation filter which sets default values
        options.OperationFilter<SwaggerDefaultValues>();

        // integrate xml comments
        options.IncludeXmlComments(XmlCommentsFilePath());
    });
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

var app = builder.Build();
//
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

string XmlCommentsFilePath()
{
    var basePath = AppDomain.CurrentDomain.BaseDirectory;
    var fileName = "ProjDoc.xml";
    return Path.Combine(basePath, fileName);
}