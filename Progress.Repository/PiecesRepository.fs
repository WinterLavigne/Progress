namespace Progress.Repository

open System
open Progress.Domain
open Progress.Context
open Microsoft.EntityFrameworkCore
open System.Linq

type IPiecesRepository =
    abstract GetAll: Piece list
    abstract Get: Guid -> Piece option
    abstract Add: Piece -> Piece option
   
exception Error of string   

type PiecesRepository(context: ProgressContext) =
    
    interface IPiecesRepository with
        member __. GetAll = context.Pieces |> Seq.toList
        member __.Get id = context.Pieces.Where(fun x -> x.Id.Equals(id))  |> Seq.tryHead        
        member __.Add piece = 
            try
                let result = context.Pieces.Add(piece) 
                context.SaveChanges() |> ignore
                Some(result.Entity)
            with
            | Error(str) -> printfn "Error1 %s" str ; None
            
