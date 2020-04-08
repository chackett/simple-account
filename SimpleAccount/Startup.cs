using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleAccount.DTO.Response;
using SimpleAccount.Repositories;
using SimpleAccount.Services;

namespace SimpleAccount
{
    public class Startup
    {
        private IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IRepository<Consent, string>, ConsentRepositoryMemory>();
            services.AddSingleton<IRepository<List<Account>, string>, AccountRepositoryMemory>();
            services.AddSingleton<ITransactionRepository<Transaction, string>, TransactionRepositoryMemory>();
            services.AddSingleton<ITrueLayerDataApi, ConcreteTrueLayerDataApi>();
            services.AddSingleton<IConsentService, ConsentService>();
            services.AddSingleton<IAnalysisService, AnalysisService>();
            services.AddSingleton<IAccountService, AccountService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}