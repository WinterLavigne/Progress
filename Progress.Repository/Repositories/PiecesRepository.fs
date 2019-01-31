namespace Progress.Repository

open System
open FSharp.Data.Sql
open FSharp.Data.Sql.Common

type IPiecesRepository =
    abstract GetAll: GetPiece list
    abstract Get: Guid -> GetPiece option
    abstract Add: AddPiece -> GetPiece option


type PiecesRepository() =
    
    let ctx = sql.GetDataContext()
    let pieces = ctx.Dbo.Pieces
    
    let mapGetPiece (dbRecord:sql.dataContext.``dbo.PiecesEntity``) : GetPiece =
        let composer = dbRecord.``dbo.Composers by Id`` |> Seq.head
        { 
            Id = dbRecord.Id
            Name = dbRecord.Name
            Composer = {
                Id = composer.Id
                Name = composer.Name
                }
        }

    let getWithId id = 
        query {
            for p in pieces do
                where (p.Id = id)
                select p
            }
            |> Seq.map (fun x -> mapGetPiece x)
            |> Seq.toList

    interface IPiecesRepository with
        member __.GetAll = 
            pieces 
            |> Seq.toList 
            |> List.map (fun x -> mapGetPiece x)
        member __.Get id = 
            let result = getWithId id

            match result with
            | [] -> None
            | l -> Some(l |> Seq.head)
            
        member __.Add piece = 
            try
                let result = pieces.Create()
                result.Name <- piece.Name
                result.Composer <- piece.Composer.Id
                ctx.SubmitUpdates()
                Some(mapGetPiece result)
            with
            | Error(str) -> printfn "Error1 %s" str ; None
            
