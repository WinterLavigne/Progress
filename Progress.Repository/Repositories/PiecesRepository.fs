namespace Progress.Repository

open System
open Progress.Domain
open FSharp.Data.Sql
open FSharp.Data.Sql.Common
open FSharp.Data.Sql.Runtime

type IPiecesRepository =
    abstract GetAll: GetPiece list
    abstract Get: Guid -> GetPiece option
    abstract Add: AddPiece -> GetPiece option
   
exception Error of string   

type PiecesRepository() =
    
    let ctx = sql.GetDataContext()
    let pieces = ctx.Dbo.Pieces
    
    let getWithId id = 
        query {
            for p in pieces do
                where (p.Id = id)
                select {
                    GetPiece.Id = p.Id
                    GetPiece.Name = p.Name
                }
            }
            |> Seq.toList

    interface IPiecesRepository with
        member __.GetAll = pieces |> Seq.toList |> List.map (fun x -> 
                {
                    Id = x.Id
                    Name = x.Name
                })
        member __.Get id = 
            let result = getWithId id

            match result with
            | [] -> None
            | l -> Some(l |> Seq.head)
            
        member __.Add piece = 
            try
                let result = pieces.Create()
                result.Name <- piece.Name
                ctx.SubmitUpdates()
                Some({
                    Id = result.Id
                    Name = result.Name
                })
            with
            | Error(str) -> printfn "Error1 %s" str ; None
            
