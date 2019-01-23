namespace Progress.Api

module HttpHandlersPieces =

    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Giraffe
    open Progress.Business
    open System
    open Business.Models.Pieces
    open Api.Models

    let handleGetPieces =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let service = ctx.GetService<IPiecesService>()
                let result = service.GetAll |> List.map (fun x -> {
                    Id = x.Id
                    Name = x.Name
                    Composer = "To implement"
                    PercentCompleted = 100
                }) 
                
                return! Successful.OK result next ctx
            }

    let handleGetPiece (id: Guid) =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let service = ctx.GetService<IPiecesService>()
                let result = service.Get id
                
                return!
                    (match result with
                    | Some r -> Successful.OK r
                    | None -> Successful.NO_CONTENT) next ctx
            }

    let handleAddPiece =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! _model = ctx.BindJsonAsync<AddPiece>()
                let service = ctx.GetService<IPiecesService>()
                let result = service.Add _model
                
                return!
                    (match result with
                    | Some r -> Successful.CREATED r
                    | None -> ServerErrors.INTERNAL_ERROR "Oops.") next ctx
            }
    