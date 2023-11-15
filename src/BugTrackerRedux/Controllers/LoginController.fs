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
[<Route("[controller]")>]
type LoginController(config: IConfiguration, logger: ILogger<LoginController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get(username: string, password: string) : Task<IActionResult> =
        use hash = SHA256.Create()

        let hashedPassword =
            BitConverter.ToString(hash.ComputeHash(Encoding.Unicode.GetBytes(password)))

        let context = base.HttpContext

        task {
            try
                use authenticationConnection =
                    new SqliteConnection(config.GetConnectionString("default"))

                do! authenticationConnection.OpenAsync() |> Async.AwaitTask
                logger.LogInformation("Connection successful.")

                let query = "select * from user where UserName = @UserName and Password = @Password"

                let parameters =
                    {| UserName = DbString(Value = username, IsFixedLength = true, Length = 45, IsAnsi = false)
                       Password = DbString(Value = hashedPassword, IsFixedLength = true, Length = 100, IsAnsi = false) |}

                let! result = authenticationConnection.QueryAsync<User>(query, parameters) |> Async.AwaitTask

                return
                    result
                    |> Seq.tryHead
                    |> function
                        | Some user -> OkObjectResult(user) :> IActionResult
                        | None ->
                            logger.LogWarning("Invalid login from: {}.", context.Connection.RemoteIpAddress)
                            OkObjectResult("User not found or password invalid.") :> IActionResult

            with ex ->
                logger.LogError("{}", ex)
                return BadRequestResult() :> IActionResult
        }
