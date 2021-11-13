using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SolanaWebWallet.Core.Configuration;
using SolanaWebWallet.Core.Exchanges;
using SolanaWebWallet.Core.Exchanges.Interfaces;
using SolanaWebWallet.Core.Interfaces;
using SolanaWebWallet.Core.Managers;

namespace Solana.WebWallet.Api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICryptoProviderFactory, CryptoProviderFactory>();

            services.AddScoped<IWalletManager, WalletManager>(fn =>
            {
                SolanaCliConfig solanaCli = new SolanaCliConfig();
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    Configuration.GetSection("WinSolanaCli").Bind(solanaCli);
                }
                else
                {
                    //2021-11-13 only windows / osx supported at the moment
                    Configuration.GetSection("OSXSolanaCli").Bind(solanaCli);

                }
                return new WalletManager(fn.GetRequiredService<ILogger<WalletManager>>(), Configuration, solanaCli);
            });
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(
                            "http://localhost:8080",
                            "https://localhost:8080/"
                            /*Configuration.GetSection("CORS").Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)*/
                            )
                        .WithHeaders("X-Requested-With", "X-SignalR-User-Agent")
                        .WithMethods("GET", "POST")
                        .AllowCredentials();
                    });
            });

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    //options.RoutePrefix = string.Empty;
                });
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            
        }
    }
}
