//using Microsoft.AspNetCore.Mvc;
//using Mock;
//using Repository.Interface;
//using Service.Services;
//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<IContext, FoodDbContext>();
//builder.Services.AddServices();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();



// דוגמה לכיצד צריך להיראות Program.cs (אם אתה משתמש ב-.NET 6 ומעלה)

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Interface;
using Service.Services;
using Repository.Interface;
using Repository;
using Mock;

var builder = WebApplication.CreateBuilder(args);

// הוספת שירותים לקונטיינר
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// הזרקת תלויות
builder.Services.AddScoped<ISelectCategoryService, SelectCategoryService>();
builder.Services.AddScoped<IConstraintsServices, ConstraintsServices>();
builder.Services.AddScoped<IContext, FoodDbContext>();

var app = builder.Build();

// הגדרת צינור הבקשות
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();  // וודא שהניתוב מוגדר כראוי

app.Run();
