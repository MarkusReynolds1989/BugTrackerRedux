namespace BugTrackerRedux

open BugTrackerRedux.Controllers
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe
open Microsoft.Extensions.Logging

module Program =
    let webApp =
        choose
            [ route "/api/ping" >=> TestController.testHandler
              route "/api/getallusers" >=> UserController.getAllUsers
              route "/" >=> text "All Clear." ]

    let configureApp (app: IApplicationBuilder) = app.UseGiraffe webApp

    let configureServices (services: IServiceCollection) = services.AddGiraffe() |> ignore

    let configureLogging (loggerBuilder: ILoggingBuilder) =
        loggerBuilder
            .AddFilter(fun level -> level.Equals LogLevel.Information)
            .AddConsole()
            .AddDebug()
        |> ignore

    [<EntryPoint>]
    let main _ =
        Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(fun webHostBuilder ->
                webHostBuilder
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                |> ignore)
            .Build()
            .Run()

        0
