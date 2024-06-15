namespace BugTrackerRedux.Controllers

open System
open System.Text
open System.Security.Cryptography
open System.Threading.Tasks
open BugTrackerRedux.Models.User
open Microsoft.AspNetCore.Mvc
open Microsoft.Data.Sqlite
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Logging
open Dapper

[<ApiController>]
[<Route("api/[controller]")>]
type TestController(config: IConfiguration, logger: ILogger<LoginController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get(): IActionResult =
        logger.LogInformation("Hit test endpoint.")
        let response = {| message = "Test successful"; data = "Dorritos" |}
        base.Ok(response) :> IActionResult