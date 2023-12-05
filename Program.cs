using Microsoft.EntityFrameworkCore;
using WebApi.Helpers;
using Repositories.Interfaces;
using Util = Utils.Utils;
using EfExtensions = Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions;

using Repositories;
using Services.Interfaces;
using Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseSqlite("Data Source=database.db")
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICollaborationRepository, CollaborationRepository>();
builder.Services.AddScoped<ICollaborationPermissionRepository, CollaborationPermissionRepository>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICollaborationService, CollaborationService>();
builder.Services.AddScoped<ICollaborationPermissionService, CollaborationPermissionService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IUserPermissionService, UserPermissionService>();
builder.Services.AddScoped<IPostService, PostService>();



// builder.Services.AddScoped<>

// Add services to the container.

builder.Services.AddControllers();


// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
}
// Console.WriteLine(app.Configuration.GetConnectionString("rhavy"));
// app.Configuration.GetConnectionString("rhavy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
