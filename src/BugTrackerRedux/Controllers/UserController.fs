module BugTrackerRedux.Controllers.UserController

open System.Threading
open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open BugTrackerRedux.Models.UserModel

let users =
    [ User(UserName = "Dan")
      User(UserName = "Fred")
      User(UserName = "Tim")
      User(UserName = "Bryan")
      User(UserName = "Frank")
      User(UserName = "Ted")
      User(UserName = "Ben") ]
    |> List.ofSeq

let getAllUsers =

    fun (next: HttpFunc) (ctx: HttpContext) ->
        let logger = ctx.GetService<ILogger<User>>()
        logger.LogInformation("Getting all users")
        Thread.Sleep(2000)
        json users next ctx
