using System;
using System.Collections.Generic;
using System.Linq;
using ContosoConference.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace ContosoConference.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMicrosoftIdentityWebApiAuthentication(Configuration);

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));

                var outputFormatter = options.OutputFormatters.OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();

                outputFormatter?.SupportedMediaTypes.Remove("text/json");
            });

            services.AddSwaggerGen(c =>
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
                            AuthorizationUrl = new Uri(Configuration["OpenApi:AuthorizationUrl"]),
                            Scopes = new Dictionary<string, string>
                            {
                                { Configuration["OpenApi:Scope"], Configuration["OpenApi:ScopeDescription"] }
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
                        new[] { Configuration["OpenApi:Scope"] }
                    }
                });
                c.OperationFilter<ContentTypeOperationFilter>();
                c.OperationFilter<AuthResponsesOperationFilter>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Configuration["OpenApi:SwaggerEndpointUrl"], Configuration["OpenApi:SwaggerEndpointName"]);
                c.OAuthClientId(Configuration["OpenApi:ClientId"]);
                c.OAuthRealm(Configuration["OpenApi:Realm"]);
                c.OAuthAppName(Configuration["OpenApi:AppName"]);
            });
        }
    }
}
