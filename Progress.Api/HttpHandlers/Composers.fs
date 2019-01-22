namespace Progress.Api

module HttpHandlersComposers =

    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Giraffe
    open Progress.Business
    open System
    open Business.Models.Composers

    let handleGetComposers =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let service = ctx.GetService<IComposersService>()
                let result = List.toArray service.GetAll
                
                return! Successful.OK result next ctx
            }

    let handleGetComposer (id: Guid) =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let service = ctx.GetService<IComposersService>()
                let result = service.Get id
                
                return!
                    (match result with
                    | Some r -> Successful.OK r
                    | None -> Successful.NO_CONTENT) next ctx
            }

    let handleAddComposer =
        fun (next : HttpFunc) (ctx : HttpContext) ->
            task {
                let! _model = ctx.BindJsonAsync<AddComposer>()
                let service = ctx.GetService<IComposersService>()
                let result = service.Add _model
                
                return!
                    (match result with
                    | Some r -> Successful.CREATED r
                    | None -> ServerErrors.INTERNAL_ERROR "Oops.") next ctx
            }
    