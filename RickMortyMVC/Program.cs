using RickMorty.Data;

namespace RickMortyMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Marco addition 1 to program
            builder.Services.AddDbContext<RickMortyDbContext>();
            // Marco addition output caching to program
            builder.Services.AddOutputCache(options =>
            {
                options.AddBasePolicy(builder =>
                {
                    builder.Expire(TimeSpan.FromSeconds(2));
                });
                options.AddPolicy("Expire30", builder =>
                {
                    builder.Expire(TimeSpan.FromSeconds(30));
                    builder.Tag("TagHandleForExpire30Policy");
                });
                options.AddPolicy("Expire300", builder =>
                {
                    builder.Expire(TimeSpan.FromSeconds(300));
                    builder.Tag("TagHandleForExpire300Policy");
                });
            });
            //builder.Services.AddResponseCaching();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Marco add the use statement of caching
            app.UseOutputCache();
            //app.UseResponseCaching();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Characters}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
