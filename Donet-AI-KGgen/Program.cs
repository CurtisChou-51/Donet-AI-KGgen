using Donet_AI_KGgen.Services;
using Microsoft.Extensions.AI;
using OpenAI.Chat;

namespace Donet_AI_KGgen
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped(sp =>
            {
                return new ChatClient
                (
                    model: "gpt-4o",
                    credential: new(builder.Configuration["AI:Key"]!),
                    options: new() { Endpoint = new Uri(builder.Configuration["AI:Endpoint"]!) }
                ).AsIChatClient();
            });

            builder.Services.AddScoped<IKnowledgeGraphService, KnowledgeGraphService>();
            builder.Services.AddSingleton<ISystemMessageService, SystemMessageService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
