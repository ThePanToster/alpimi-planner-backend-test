﻿using System.Data;
using AlpimiAPI.Utilities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;

namespace AlpimiTest.TestUtilities
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            DotNetEnv.Env.Load(
                Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".env")
            );
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(
                (context, services) =>
                {
                    services.AddScoped<IDbConnection>(sp => new SqlConnection(
                        Configuration.GetTestConnectionString()
                    ));
                }
            );
        }
    }
}
