using ChatApplication.Code;
using ChatApplication.Code.Collections;
using ChatApplication.Models;
using ChatApplication.Models.Chat;
using ChatApplication.Services;
using MongoDB.Bson.Serialization.Conventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddRazorPages()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });


ConventionRegistry.Register("CamelCase", new ConventionPack()
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true)
            }, _ => true);


builder.Services.AddSignalR();
builder.Services.AddScoped<ProducerService>();
builder.Services.AddScoped<ConsumerManager>();

builder.Services.AddCollections(builder.Configuration);

builder.Services.AddMongoDbService();

builder.Services.AddScoped<CustomerCache>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapHub<ChatHub>("/chathub");

app.Run();
