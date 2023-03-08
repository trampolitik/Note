using Microsoft.EntityFrameworkCore;
using ToDo.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Connecting the database
builder.Services.AddDbContext<ToDoItemDbContext>(options =>
options.UseSqlServer(
builder.Configuration.GetConnectionString("ToDoConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ToDoItem}/{action=Index}/{id?}");

app.Run();
