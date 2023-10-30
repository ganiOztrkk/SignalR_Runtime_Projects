using SignalRServerExample.Business;
using SignalRServerExample.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddTransient<MyBusiness>();
builder.Services.AddSignalR();
builder.Services.AddControllers();

builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy =>
    policy.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .SetIsOriginAllowed(origin => true)
));

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

app.UseCors();
app.UseRouting();

app.MapHub<MyHub>("/myhub");
app.MapControllers();

app.UseAuthorization();

app.MapRazorPages();

app.Run();