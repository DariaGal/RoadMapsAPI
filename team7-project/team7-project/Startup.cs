using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.Trees.Services;
using Models.Users.Services;
using Swashbuckle.AspNetCore.Swagger;
using team7_project.Auth;
using team7_project.Auth.Tokens;

namespace team7_project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var signingKey = new SigningSymmetricKey(Environment.GetEnvironmentVariable("AUTH_KEY", EnvironmentVariableTarget.Process));
            services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITreeService, TreeService>();

            var signingDecodingKey = (IJwtSigningDecodingKey)signingKey;

            services
                .AddAuthentication()
                .AddJwtBearer(options =>
                {
                    //jwtBearerOptions.SaveToken = true;
                    options.RequireHttpsMetadata = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,

                        ValidateAudience = false,

                        ValidateLifetime = true,

                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = signingDecodingKey.GetKey()
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Skill Map API",
                    Description = "API",
                    Contact = new Contact
                    {
                        Name = "Team7",
                        Email = string.Empty
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            {
                if (env.IsDevelopment())
                    app.UseDeveloperExceptionPage();
                else
                    app.UseHsts();

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                 app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Skill Map API V1");
                });

                //app.UseAuthentication();
                app.UseMiddleware<AuthMiddleware>();
                app.UseHttpsRedirection();
                app.UseMvc();
            }
        }
    }
}
