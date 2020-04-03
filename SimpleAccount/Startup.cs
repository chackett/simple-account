using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleAccount.Domains;
using SimpleAccount.Repositories;
using SimpleAccount.Services;

namespace SimpleAccount
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IRepository<Consent, string>, ConsentRepositoryMemory>();
            services.AddSingleton<IRepository<Account, string>, AccountRepositoryMemory>();
            services.AddSingleton<IRepository<Transaction, string>, TransactionRepositoryMemory>();
            services.AddSingleton<IConsentService, TrueLayerConsent>();
            services.AddSingleton<ITrueLayerDataApi, ConcreteTrueLayerDataApi>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<ICache<Account, string>, SimpleAccountCache<Account, String>>();
            services.AddSingleton<ICache<Transaction, string>, SimpleAccountCache<Transaction, String>>();
            // services.AddSingleton<>();
            // services.AddSingleton<>();
            // services.AddSingleton<>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}