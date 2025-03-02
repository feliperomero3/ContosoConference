using System;
using System.Collections.Generic;
using System.Linq;
using ContosoConference.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace ContosoConference.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration);

        builder.Services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.Filters.Add(new AuthorizeFilter(policy));

            var outputFormatter = options.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();

            outputFormatter?.SupportedMediaTypes.Remove("text/json");
        });

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Contoso Conference API",
                Description = "A sample API with information related to a technical conference. " +
                    "The available resources include *Speakers*, *Sessions* and *Topics*. " +
                    " A single write operation is available to provide feedback on a Session.",
                Version = "v1"
            });
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(builder.Configuration["OpenApi:AuthorizationUrl"]!),
                        Scopes = new Dictionary<string, string>
                        {
                            { builder.Configuration["OpenApi:Scope"]!, builder.Configuration["OpenApi:ScopeDescription"] }
                        }
                    }
                }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                    },
                    new[] { builder.Configuration["OpenApi:Scope"] }
                }
            });
            c.OperationFilter<ContentTypeOperationFilter>();
            c.OperationFilter<AuthResponsesOperationFilter>();
        });

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(builder.Configuration["OpenApi:SwaggerEndpointUrl"], builder.Configuration["OpenApi:SwaggerEndpointName"]);
            c.OAuthClientId(builder.Configuration["OpenApi:ClientId"]);
            c.OAuthRealm(builder.Configuration["OpenApi:Realm"]);
            c.OAuthAppName(builder.Configuration["OpenApi:AppName"]);
        });

        app.MapControllers();

        app.Run();
    }
}