﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            services.Configure<SolanaCliConfig>(Configuration.GetSection("SolanaCli"));

            services.AddScoped<IWalletManager, WalletManager>();
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
