using Dependences.Facture.Common.IdentityAccess.Contracts.Repositories;
using Facture.IdentityAccess.Application.Components.Services;
using Facture.IdentityAccess.Application.Services;
using Facture.IdentityAccess.Contracts.Repositories;
using Facture.IdentityAccess.Domain.Services;
using Facture.IdentityAccess.Infrastructure.AuthEnrichers;
using Facture.IdentityAccess.Infrastructure.Repositories;
using MicroLogin.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IJwtServices, JwtServices>();
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
builder.Services.AddSingleton<IAuthorizedSoftwareRepository, AuthorizedSoftwareRepository>();
builder.Services.AddSingleton<ITokenEnrichService, SupplierEnrichToken>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ICreateJwtServices, CreateJwtServices>();
builder.Services.AddSingleton<IJwtServices, JwtServices>();
builder.Services.AddSingleton<ITenantRepository, TenantRepository>();
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
