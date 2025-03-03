using System.Text;
using JWT_Identity_Policy.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
         );

// ADDED IDENTITY SERVICES  AND OPTIONS FOR WEAK PASSWORD FOR TESTING
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1; // Set to a very low value if needed
    options.Password.RequiredUniqueChars = 0; // No unique characters required
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//ADDED AUTHENTICATION SERVICES (JWT AUTH)

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.
        GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanViewStudentsPolicy", policy =>
        policy.RequireClaim("CanViewStudents", "true"));

    options.AddPolicy("CanManageStudentsPolicy", policy =>
        policy.RequireClaim("CanManageStudents", "true"));

    options.AddPolicy("TeacherAccess", policy =>
        policy.RequireRole("Teacher")
              .RequireClaim("Age")
              .RequireClaim("CanViewStudents", "true")
             .RequireAssertion(context =>
             {
                 var ageClaim = context.User.FindFirst(c => c.Type == "Age");
                 var final = ageClaim != null && int.TryParse(ageClaim.Value, out int age) && age < 60 && age >= 18;
                 return final;
             }));

    options.AddPolicy("TeacherAgeBasedAccess", policy =>
    policy.RequireRole("Teacher") // Only applies to Teachers
          .RequireClaim("Age") // Ensures Age claim is present
          .RequireAssertion(context =>
          {
              var ageClaim = context.User.FindFirst(c => c.Type == "Age");
              if (ageClaim == null || !int.TryParse(ageClaim.Value, out int age))
                  return false;

              if (age >= 18 && age < 35)
                  return context.User.HasClaim("CanViewStudents", "true"); // Can only view students

              if (age >= 35 && age < 60)
                  return context.User.HasClaim("CanManageStudents", "true"); // Can only manage students

              return false; // Deny access if age is out of range
          }));


    options.AddPolicy("AdminAccess", policy =>
        policy.RequireRole("Admin")
              .RequireClaim("Age")
              .RequireClaim("CanViewStudents", "true")
              .RequireClaim("CanManageStudents", "true")
              .RequireAssertion(context =>
              {
                  var ageClaim = context.User.FindFirst(c => c.Type == "Age");
                  var final = ageClaim != null && int.TryParse(ageClaim.Value, out int age) && age >= 18;
                  return final;
              }));

    options.AddPolicy("AdminOrTeacherAccess", policy =>
         policy.RequireRole("Admin", "Teacher")
              // Ensures user has either Admin or Teacher role
          .RequireAssertion(context =>
          {
              var ageClaim = context.User.FindFirst(c => c.Type == "Age");
              if (ageClaim == null || !int.TryParse(ageClaim.Value, out int age))
                  return false;

              // Admins must be 18+, Teachers must be 18-59
              var isAdmin = context.User.IsInRole("Admin") && age >= 18;
              var isTeacher = context.User.IsInRole("Teacher") && age >= 18 && age < 60;

              return isAdmin || isTeacher;
          }));




    //options.AddPolicy("SeniorCitizenAccess", policy =>
    //    policy.RequireClaim("Age")
    //          .RequireAssertion(context =>
    //          {
    //              var ageClaim = context.User.FindFirst(c => c.Type == "Age");
    //              return ageClaim != null && int.TryParse(ageClaim.Value, out int age) && age < 60;
    //          }));
});

// Add services to the container.

builder.Services.AddControllers();
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
