using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.SqlServer;
using Azure.Messaging.WebPubSub;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using lts.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Learn more at https://aka.ms/aspnetcore/swashbuckle

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", builder =>
    {
        builder.WithOrigins("https://app-lts-app.azurewebsites.net", "https://maps.googleapis.com", "https://data.ssb.no/api/v0/no/table/07459/")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

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

app.MapTeslaCarEndpoints();
app.MapCityCodeEndpoints();
app.MapCommentEndpoints();

app.UseCors("MyCorsPolicy");

app.Run();
