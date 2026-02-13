using Microsoft.OpenApi.Models;

namespace Store.Web.Extensions;

public static class SwaggerServiceExtension
{

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "Store API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "zeyad",
                        Email = "zeyadahmed20042020@gmail.com",
                        Url = new Uri("https://www.facebook.com/profile.php?id=100083071811939")
                    }
                });
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition("Bearer", securitySchema);
            var securityRequirement = new OpenApiSecurityRequirement
            {
                { securitySchema, new[] { "Bearer" } }
            };
            options.AddSecurityRequirement(securityRequirement);
        });
        return services;
    }
}