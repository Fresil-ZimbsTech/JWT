using System.Text;
using JWT_Claim_Auth.Context;
using JWT_Claim_Auth.Interfaces;
using JWT_Claim_Auth.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<JWTDbcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//interface add in program.cs
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IAuthService, AuthService>();



builder.Services.AddControllers();

//JWT token service
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = "Role", //Ensuring role claims are recognized

    };
});

// Authorization Policies
builder.Services.AddAuthorization(options =>
{
    // Role-based policies
    options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("User", policy => policy.RequireClaim("Role", "User"));
    options.AddPolicy("Hr", policy => policy.RequireClaim("Role", "Hr"));
    options.AddPolicy("AdminOrHr", policy => policy.RequireClaim("Role", "Admin", "Hr")); // ? Combined

    // Permission-based policies
    options.AddPolicy("View", policy => policy.RequireClaim("Permission", "View"));
    options.AddPolicy("Edit", policy => policy.RequireClaim("Permission", "Edit"));
    options.AddPolicy("Delete", policy => policy.RequireClaim("Permission", "Delete"));
    options.AddPolicy("Update", policy => policy.RequireClaim("Permission", "Update"));
    options.AddPolicy("UpdateOrDeleteOrEdit", policy => policy.RequireClaim("Permission", "Update", "Delete", "Edit"));

    // Combined policies
    options.AddPolicy("ViewOrEditOrUser", policy =>
        policy.RequireClaim("Permission", "View", "Edit").RequireClaim("Role", "User"));
    options.AddPolicy("UpdateOrAdminOrDelete", policy =>
        policy.RequireClaim("Permission", "Update", "Delete").RequireClaim("Role", "Admin"));
    options.AddPolicy("HrOrView", policy =>
        policy.RequireClaim("Permission", "View").RequireClaim("Role", "Hr"));
});





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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
