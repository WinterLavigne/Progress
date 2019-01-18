module Progress.Api.App

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Progress.Api.HttpHandlers
open Progress.Business
open Giraffe
open Progress.Repository
open Progress.Context
open Microsoft.EntityFrameworkCore
open System.Configuration
open Microsoft.Extensions.Configuration
open System.Linq
open FsConfig
open System.IO
open FSharp.Data

// ---------------------------------
// Web app
// ---------------------------------


let webApp =
    choose [
        subRoute "/api"
            (choose [
                GET >=> choose [
                    route  "/" >=> text "index"
                    route "/pieces" >=> handleGetPieces
                    routef "/pieces/%O" handleGetPiece
                ]
                POST >=> choose [
                    route "/pieces" >=> handleAddPiece
                ]
            ])
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder : CorsPolicyBuilder) =
    builder.WithOrigins("http://localhost:8080")
           .AllowAnyMethod()
           .AllowAnyHeader()
           |> ignore

let configureApp (app : IApplicationBuilder) =
    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
    (match env.IsDevelopment() with
    | true  -> app.UseDeveloperExceptionPage()
    | false -> app.UseGiraffeErrorHandler errorHandler)
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseGiraffe(webApp)

let configureAppConfiguration  (context: WebHostBuilderContext) (config: IConfigurationBuilder) =  
    config
        .AddJsonFile("appsettings.json",false,true)
        .AddJsonFile(sprintf "appsettings.%s.json" context.HostingEnvironment.EnvironmentName ,true)
        .AddEnvironmentVariables() |> ignore

[<Literal>]
let appSettingsFile = "./appsettings.json"
type Sample = JsonProvider<appSettingsFile>

let configureServices (services : IServiceCollection) =
   
    let content = Sample.Parse(File.ReadAllText(appSettingsFile))
    let conn = content.ConnectionString

    services.AddDbContext<ProgressContext>(fun options -> options.UseSqlServer(conn) |> ignore) |> ignore
    services.AddScoped<IPiecesRepository, PiecesRepository>() |> ignore
    services.AddScoped<IPiecesService, PiecesService>() |> ignore
    services.AddCors()    |> ignore
    services.AddGiraffe() |> ignore

let configureLogging (builder : ILoggingBuilder) =
    builder.AddFilter(fun l -> l.Equals LogLevel.Error)
           .AddConsole()
           .AddDebug() |> ignore

[<EntryPoint>]
let main _ =
    WebHostBuilder()
        .UseKestrel()
        .UseIISIntegration()
        .ConfigureAppConfiguration(configureAppConfiguration)
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0