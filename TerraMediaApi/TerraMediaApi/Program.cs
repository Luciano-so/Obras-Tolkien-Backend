using TerraMedia.Api.Configuration;
using TerraMedia.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerConfiguration();

builder.Services.DependencyRegister();
builder.Services.ApplicationRegister();
builder.Services.AuthenticationRegister(builder.Configuration);
builder.Services.AutoMapperRegister();
builder.Services.AppSettingsRegister(builder.Configuration);
builder.Services.DataBaseRegister(builder.Configuration);

var app = builder.Build();

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
