namespace Progress.Repository

open System
open Progress.Domain
open Progress.Context
open Microsoft.EntityFrameworkCore
open System.Linq

type IPiecesRepository =
    abstract GetAll: GetPiece list
    abstract Get: Guid -> GetPiece option
    abstract Add: AddPiece -> GetPiece option
   
exception Error of string   

type PiecesRepository(context: ProgressContext) =
    
    interface IPiecesRepository with
        member __.GetAll = context.Pieces.Include(fun x -> x.Composer).AsNoTracking() |> Seq.toList |> List.map (fun x -> 
                {
                    Id = x.Id
                    Name = x.Name
                })
        member __.Get id = 
            context.Pieces.AsNoTracking().Where(fun x -> x.Id.Equals(id))  |> Seq.toList |> List.map (fun x -> {
                Id = x.Id
                Name = x.Name
                })  |> Seq.tryHead        
        member __.Add piece = 
            try
                let result = context.Pieces.Add({
                    Id = Guid.Empty
                    Name = piece.Name
                    Created = DateTime.MinValue
                    Composer = None
                    }) 
                context.SaveChanges() |> ignore
                Some({
                    Id = result.Entity.Id
                    Name = result.Entity.Name
                })
            with
            | Error(str) -> printfn "Error1 %s" str ; None
            
