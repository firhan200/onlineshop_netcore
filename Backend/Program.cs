using System.Text;
using Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services;

var builder = WebApplication.CreateBuilder(args);

//add controller mapper
builder.Services.AddControllers();
//add swagger
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme{
        In = ParameterLocation.Header,
        Name = "Authorization",
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
//add auth
builder.Services.AddAuthorization();
builder.Services.AddAuthentication().AddJwtBearer(options => {
    var jwtSecretObj = builder.Configuration.GetSection("JWT:SecretKey");
    string jwtSecret = jwtSecretObj.Value ?? "";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret))
    };
});
//add db context
builder.Services.AddDbContext<DataContext>();
//add repository services
builder.Services.AddRepositories().AddServices();

//build builder configs
var app = builder.Build();

//only use swagger if on dev env only
if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.DisplayOperationId();
    });
}

app.UseStaticFiles();

//use controller instead
app.MapControllers();

//use auth
app.UseAuthorization();

//run web app
app.Run();