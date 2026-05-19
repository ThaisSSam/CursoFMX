using Microsoft.AspNetCore.Mvc;

namespace Treinamento.API.Configurations;

public static class ApiConfig
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        CrossCutting.Configuration.WriteConnectionString =
            builder.Configuration.GetConnectionString("WriteConnection")
            ?? builder.Configuration.GetConnectionString("DefaultConnection")
            ?? string.Empty;

        CrossCutting.Configuration.ReadConnectionString =
            builder.Configuration.GetConnectionString("ReadConnection")
            ?? CrossCutting.Configuration.WriteConnectionString;
    }

    public static IServiceCollection WebApiConfig(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddCors(options =>
        {
            options.AddPolicy("Desenvolvimento", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseWebApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseCors("Desenvolvimento");

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
