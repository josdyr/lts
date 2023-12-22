using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.SqlServer;
using Azure.Messaging.WebPubSub;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using lts.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://app-lts.azurewebsites.net")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Learn more at https://aka.ms/aspnetcore/swashbuckle

var keyVaultUrl = builder.Configuration["AzureKeyVault:VaultUrl"];
var credential = new DefaultAzureCredential();
var client = new SecretClient(new Uri(keyVaultUrl), credential);
var secret = client.GetSecret("ConnectionStringSecretName");
var connectionString = secret.Value.Value;

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.MapTeslaCarEndpoints();
app.MapCityCodeEndpoints();
app.MapCommentEndpoints();

app.Run();
