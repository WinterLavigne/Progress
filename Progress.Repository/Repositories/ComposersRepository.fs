namespace Progress.Repository

open System
open System.Linq
open FSharp.Data.Sql



type IComposersRepository =
    abstract GetAll: GetComposer list
    //abstract Get: Guid -> GetPiece option
    //abstract Add: AddPiece -> GetPiece option


type ComposersRepository() =
    
    let ctx = sql.GetDataContext()
    let composers = ctx.Dbo.Composers

    interface IComposersRepository with
        member __.GetAll = composers |> Seq.toList |> List.map (fun x -> 
                {
                    Id = x.Id
                    Name = x.Name
                })
        //member __.Get id = 
        //    context.Pieces.AsNoTracking().Where(fun x -> x.Id.Equals(id))  |> Seq.toList |> List.map (fun x -> {
        //        Id = x.Id
        //        Name = x.Name
        //        })  |> Seq.tryHead        
        //member __.Add piece = 
        //    try
        //        let result = context.Pieces.Add({
        //            Id = Guid.Empty
        //            Name = piece.Name
        //            Created = DateTime.MinValue
        //            }) 
        //        context.SaveChanges() |> ignore
        //        Some({
        //            Id = result.Entity.Id
        //            Name = result.Entity.Name
        //        })
        //    with
        //    | Error(str) -> printfn "Error1 %s" str ; None
            
