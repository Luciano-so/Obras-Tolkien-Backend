using TerraMedia.Api.Configuration;
using TerraMedia.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerConfiguration();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.CreateDatabasePostgresIfNotExists(builder.Configuration);
builder.Services.DependencyRegister();
builder.Services.ApplicationRegister();
builder.Services.AuthenticationRegister(builder.Configuration);
builder.Services.AutoMapperRegister();
builder.Services.AppSettingsRegister(builder.Configuration);
builder.Services.DataBaseRegister(builder.Configuration);

var app = builder.Build();
app.UseCors("AllowFrontend");
app.DataBaseRegister();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<DomainExceptionMiddleware>();
app.UseMiddleware<UnauthorizedExceptionMiddleware>();

app.UseHttpsRedirection();

app.AuthenticationRegister();

app.MapControllers();

app.Run();
