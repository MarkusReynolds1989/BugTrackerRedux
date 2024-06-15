module BugTrackerRedux.Controllers.TestController

open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

type TestItem = { Data: int; OtherItem: string }

let item = { Data = 1; OtherItem = "Banana" }

let testHandler =

    fun (next: HttpFunc) (ctx: HttpContext) ->
        let logger = ctx.GetService<ILogger<TestItem>>()
        logger.LogInformation("Test")
        json item next ctx
