module BugTrackerRedux.Models.UserModel

type AuthenticationLevel =
    | Admin = 0
    | User = 1

type User() =
    member val UserName: string = null with get, set
    member val FirstName: string = null with get, set
    member val LastName: string = null with get, set
    member val Email: string = null with get, set
    member val Password: string = null with get, set
    member val ActiveIndicator: int = 1 with get, set
    member val AuthenticationLevel: AuthenticationLevel = AuthenticationLevel.User with get, set
    member val UserId: int = 0 with get, set
