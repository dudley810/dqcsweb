using dqcsweb.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dqcsweb
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
            services.AddControllersWithViews();
            services.Configure<WebSettings>(Configuration.GetSection("WebSettings"));

            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.SuppressXFrameOptionsHeader = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //For Extra Security
            //goto securityheaders.com to check it
            //https://docs.nwebsec.com/en/latest/nwebsec/Configuring-csp.html
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHsts(opt => opt.MaxAge(days: 365).IncludeSubdomains());
            app.UseXXssProtection(opt => opt.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
            app.UseXfo(opt => opt.SameOrigin());
            app.UseReferrerPolicy(opts => opts.NoReferrer());

            //Content-Security-Policy involved
            //app.UseCsp(options => options.DefaultSources(s => s.Self())
            //          .ScriptSources(s => s.Self().CustomSources("scripts.nwebsec.com"))
            //          .ReportUris(r => r.Uris("/report")));
            //app.UseCspReportOnly(options => options.DefaultSources(s => s.Self()).ImageSources(s => s.None()));

            //For Extra Security

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
