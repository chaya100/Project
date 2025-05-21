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



// ����� ����� ���� ������� Program.cs (�� ��� ����� �-.NET 6 �����)

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Interface;
using Service.Services;
using Repository.Interface;
using Repository;
using Mock;

var builder = WebApplication.CreateBuilder(args);

// ����� ������� ���������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ����� ������
builder.Services.AddScoped<ISelectCategoryService, SelectCategoryService>();
builder.Services.AddScoped<IConstraintsServices, ConstraintsServices>();
builder.Services.AddScoped<IContext, FoodDbContext>();

var app = builder.Build();

// ����� ����� ������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();  // ���� ������� ����� �����

app.Run();
