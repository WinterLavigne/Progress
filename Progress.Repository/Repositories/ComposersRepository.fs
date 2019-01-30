namespace Progress.Repository

open System
open System.Linq
open FSharp.Data.Sql

exception Error of string  

type IComposersRepository =
    abstract GetAll: GetComposer list
    abstract Get: Guid -> GetComposer option
    abstract Add: AddComposer -> GetComposer option


type ComposersRepository() =
    
    let ctx = sql.GetDataContext()
    let composers = ctx.Dbo.Composers

    let getWithId id : (GetComposer list)= 
        query {
            for p in composers do
                where (p.Id = id)
                select {
                    GetComposer.Id = p.Id
                    GetComposer.Name = p.Name
                } 
            }
            |> Seq.toList

    interface IComposersRepository with
        member __.GetAll = composers |> Seq.toList |> List.map (fun x -> 
                {
                    Id = x.Id
                    Name = x.Name
                })
        member __.Get id = 
            let result = getWithId id

            match result with
            | [] -> None
            | l -> Some(l |> Seq.head)
        member __.Add composer = 
            try
                let result = composers.Create()
                result.Name <- composer.Name
                ctx.SubmitUpdates()
                Some({
                    Id = result.Id
                    Name = result.Name
                })
            with
            | Error(str) -> printfn "Error1 %s" str ; None
            
