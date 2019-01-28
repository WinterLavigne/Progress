module Progress.Api.App

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Cors
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Progress.Business
open Giraffe
open Microsoft.EntityFrameworkCore
open System.Configuration
open Microsoft.Extensions.Configuration
open System.Linq
open FsConfig
open System.IO
open FSharp.Data

open Progress.Api.HttpHandlersPieces
open Progress.Api.HttpHandlersComposers
open Progress.Repository


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
                    route "/composers" >=> handleGetComposers
                    routef "/composers/%O" handleGetComposer
                ]
                POST >=> choose [
                    route "/pieces" >=> handleAddPiece
                    route "/composers" >=> handleAddComposer
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
    //app.UseGiraffeErrorHandler errorHandler
    app.UseStaticFiles() |> ignore
    //app.UseAuthentication() |> ignore
    app.UseCors(new Action<_>(fun (b: Infrastructure.CorsPolicyBuilder) -> b.AllowAnyHeader() |> ignore; b.AllowAnyMethod() |> ignore; b.AllowAnyOrigin() |> ignore)) |> ignore
    app.UseGiraffe webApp

//let configureApp (app : IApplicationBuilder) =
//    let env = app.ApplicationServices.GetService<IHostingEnvironment>()
//    (match env.IsDevelopment() with
//    | true  -> app.UseDeveloperExceptionPage()
//    | false -> app.UseGiraffeErrorHandler errorHandler)
//        .UseHttpsRedirection()
//        .UseCors(configureCors)
//        .UseGiraffe(webApp)

[<Literal>]
let appSettingsFile = "./appsettings.winter.json" // this should be your own appsettings file!!!
type Sample = JsonProvider<appSettingsFile>

let configureServices (services : IServiceCollection) =
   
    let content = Sample.Parse(File.ReadAllText(appSettingsFile))
    let conn = content.ConnectionString

    services.AddScoped<IComposersRepository, ComposersRepository>() |> ignore
    services.AddScoped<IComposersService, ComposersService>() |> ignore
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
        .Configure(Action<IApplicationBuilder> configureApp)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0