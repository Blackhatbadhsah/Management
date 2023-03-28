using EmployeeManagement.BLL;
using EmployeeManagement.DAL;
using EmployeeManagement.DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;
using System.Data.Entity;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using EmployeeManagement.DAL.Repository;
using NUglify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
      builder =>
      {
          builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
      });
});
var Configuration = builder.Configuration;
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<DataBaseContest>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("Empmanagement")??""));
var jwt = Configuration.GetSection("Jwt");

//Services BLL
builder.Services.AddScoped<AccountServices>();

//Repository DAL
builder.Services.AddScoped<AccountRepository>();

builder.Services.AddIdentity<User, Role>()
     .AddEntityFrameworkStores<DataBaseContest>()
    .AddUserManager<UserManager<User>>()
    .AddRoleManager<RoleManager<Role>>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
});

builder.Services.AddAuthentication(options => { 
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"]??"",
            ValidAudience= "*",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? ""))
        };
    });

// ...
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option=>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Applied Information Employee Management Portal API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Applied Information Employee Management Portal V1");
});
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseRouting();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        var headers = context.Context.Response.GetTypedHeaders();

        if (context.File.Name.EndsWith(".js"))
        {
            // Minify the JavaScript file
            var fileContent = System.IO.File.ReadAllText(context.File.PhysicalPath);
            var minified = Uglify.Js(fileContent);
            headers.ContentType = new Microsoft.Net.Http.Headers.MediaTypeHeaderValue("application/javascript");
            headers.ContentLength = minified.Code.Length;
            context.Context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(minified.Code), 0, minified.Code.Length);
        }
    }
});


app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAll");


app.MapControllers();  
app.MapRazorPages();
app.MapFallbackToFile("index.html");

app.Run();
