
using Microsoft.IdentityModel.Tokens;
using StudentTrackingAPI.Core.Repository;
using StudentTrackingAPI.DataAccess.Context;
using StudentTrackingAPI.Services.ApiServices;
using StudentTrackingAPI.Services.Interfaces;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StudentTrackingAPI.Services.APIServices;
using StudentTrackingAPI.Core.Repositry;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add session state service
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    // options.Cookie.HttpOnly = true;
    options.Cookie.Name = "ephr";
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(o => o.AddPolicy("default", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:44351", "http://localhost:3000", "https://eohc.in")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    options.RequireHttpsMetadata = false;
//    options.SaveToken = true;
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<DatabaseContext>(opts => opts.UseSqlServer(builder.Configuration["ConnectionStrings:dev"]));
}
builder.Services.AddDbContext<DatabaseContext>(opts => opts.UseSqlServer(builder.Configuration["ConnectionStrings:prod"]));

builder.Services.AddScoped<IAuthService, AuthService>().AddScoped<AuthRepository>();
builder.Services.AddScoped<IStudentService, StudentService>().AddScoped<StudentRepository>();
builder.Services.AddScoped<IStateMasterService, StateMasterService>().AddScoped<ParameterMasterRepository>();
builder.Services.AddScoped<ICityValueMasterService, ParameterValueMasterService>().AddScoped<CityValueMasterRepository>();


builder.Services.AddHttpClient();
builder.Services.AddControllers().AddNewtonsoftJson();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles(); // Required to serve files from wwwroot


app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});
app.UseCors("AllowOrigin");


app.Run();
