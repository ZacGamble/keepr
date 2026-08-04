using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using keepr.Repositories;
using keepr.Services;

namespace keepr
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
            ConfigureCors(services);
            ConfigureAuth(services);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "keepr", Version = "v1" });
            });
            services.AddScoped<IDbConnection>(x => CreateDbConnection());

            services.AddScoped<AccountsRepository>();
            services.AddScoped<AccountService>();

            services.AddTransient<KeepsRepository>();
            services.AddTransient<KeepsService>();

            services.AddTransient<VaultsRepository>();
            services.AddTransient<VaultsService>();

            services.AddTransient<VaultKeepsRepository>();
            services.AddTransient<VaultKeepsService>();
        }

        private void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsDevPolicy", builder =>
                {
                    builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins(new string[]{
                        "http://localhost:8080", "http://localhost:8081"
                    });
                });
            });
        }

        private void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{Configuration["AUTH0_DOMAIN"]}/";
                options.Audience = Configuration["AUTH0_AUDIENCE"];
            });

        }

        private IDbConnection CreateDbConnection()
        {
            string connectionString = Configuration["CONNECTION_STRING"];
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new System.InvalidOperationException("CONNECTION_STRING is missing or empty. Please set CONNECTION_STRING in appsettings.json or as an environment variable.");
            }

            connectionString = connectionString.Trim();

            if (connectionString.StartsWith("mysql://", System.StringComparison.OrdinalIgnoreCase) ||
                connectionString.StartsWith("mysqls://", System.StringComparison.OrdinalIgnoreCase))
            {
                connectionString = ConvertMysqlUriToConnectionString(connectionString);
            }

            return new MySqlConnection(connectionString);
        }

        private static string ConvertMysqlUriToConnectionString(string uriString)
        {
            var uri = new System.Uri(uriString);
            string host = uri.Host;
            int port = uri.Port > 0 ? uri.Port : 3306;
            string database = uri.AbsolutePath.TrimStart('/');
            
            string user = "";
            string password = "";
            if (!string.IsNullOrEmpty(uri.UserInfo))
            {
                string[] parts = uri.UserInfo.Split(new[] { ':' }, 2);
                user = System.Uri.UnescapeDataString(parts[0]);
                if (parts.Length > 1)
                {
                    password = System.Uri.UnescapeDataString(parts[1]);
                }
            }

            var sb = new System.Text.StringBuilder();
            sb.Append($"Server={host};Port={port};Database={database};User Id={user};Password={password};");
            
            if (host.Contains("tidbcloud.com") || uriString.Contains("ssl"))
            {
                sb.Append("SslMode=VerifyFull;");
            }

            return sb.ToString();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "keepr v1"));
                app.UseCors("CorsDevPolicy");
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
