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
type UserController(config: IConfiguration, logger: ILogger<UserController>) =
    inherit ControllerBase()

    [<HttpGet>]
    member _.Get() : Task<IActionResult> =
        task {
            try
                use authenticationConnection =
                    new SqliteConnection(config.GetConnectionString("default"))

                do! authenticationConnection.OpenAsync() |> Async.AwaitTask
                logger.LogInformation("Connection successful.")

                let query = "select * from user"

                let! result = authenticationConnection.QueryAsync<User>(query) |> Async.AwaitTask
                let user = result |> Seq.tryHead

                return OkObjectResult(user) :> IActionResult

            with ex ->
                logger.LogError("{}", ex)
                return BadRequestObjectResult(ex) :> IActionResult
        }

    [<HttpPost>]
    member _.Post(user: User) : Task<IActionResult> =
        logger.LogInformation("Trying to insert user: {}", user)

        use hash = SHA256.Create()

        let hashedPassword =
            BitConverter.ToString(hash.ComputeHash(Encoding.Unicode.GetBytes(user.Password)))

        task {
            try
                use authenticationConnection =
                    new SqliteConnection(config.GetConnectionString("default"))

                do! authenticationConnection.OpenAsync() |> Async.AwaitTask
                logger.LogInformation("Connection successful.")

                let query =
                    """insert into user (UserName, FirstName, LastName, Password, Email, AuthenticationLevel)
                        values (@UserName, @FirstName, @LastName, @Password, @Email, @AuthenticationLevel)"""

                let parameters =
                    {| UserName = user.UserName
                       FirstName = user.FirstName
                       LastName = user.LastName
                       Password = hashedPassword
                       Email = user.Email
                       AuthenticationLevel = user.AuthenticationLevel |}

                let! rowCount = authenticationConnection.ExecuteAsync(query, parameters) |> Async.AwaitTask

                if rowCount = 1 then
                    logger.LogInformation("Added user {}", user.ToString())
                    return OkObjectResult("Added user ", user) :> IActionResult
                else
                    logger.LogError("Failed to add user.")
                    return BadRequestObjectResult("Failed to add user.") :> IActionResult

            with ex ->
                logger.LogError("{}", ex)
                return BadRequestObjectResult("Invalid user.") :> IActionResult
        }

    [<HttpPost("update")>]
    member _.PostUpdate(user: User) : Task<IActionResult> =
        logger.LogInformation("Trying to update user: {}", user)

        use hash = SHA256.Create()

        let hashedPassword =
            BitConverter.ToString(hash.ComputeHash(Encoding.Unicode.GetBytes(user.Password)))

        task {
            try
                use authenticationConnection =
                    new SqliteConnection(config.GetConnectionString("default"))

                do! authenticationConnection.OpenAsync() |> Async.AwaitTask
                logger.LogInformation("Connection successful.")

                let query =
                    """update user (UserName, FirstName, LastName, Password, Email, AuthenticationLevel)
                        values (@UserName, @FirstName, @LastName, @Password, @Email, @AuthenticationLevel)"""

                let parameters =
                    {| UserName = user.UserName
                       FirstName = user.FirstName
                       LastName = user.LastName
                       Password = hashedPassword
                       Email = user.Email
                       AuthenticationLevel = user.AuthenticationLevel |}

                let! rowCount = authenticationConnection.ExecuteAsync(query, parameters) |> Async.AwaitTask

                if rowCount = 1 then
                    logger.LogInformation("Added user {}", user.ToString())
                    return OkObjectResult("Added user ", user) :> IActionResult
                else
                    logger.LogError("Failed to add user.")
                    return BadRequestObjectResult("Failed to add user.") :> IActionResult

            with ex ->
                logger.LogError("{}", ex)
                return BadRequestObjectResult("Invalid user.") :> IActionResult
        }
