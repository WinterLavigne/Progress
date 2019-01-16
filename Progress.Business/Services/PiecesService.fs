module Progress.Business

open Business.Models
open System
open Progress.Repository

type IPiecesService =
    abstract member GetAll: Piece list
    abstract member Get: Guid -> Piece option

type PiecesService(repository: IPiecesRepository) = 
    
    interface IPiecesService with

        member __.GetAll = 
             repository.GetAll |> List.map (fun x -> {
                    Id = x.Id
                    Name = x.Name
                    Composer = x.Composer
                    PercentCompleted = 0
                }) 
        member __.Get id = 
            let result = repository.Get id
            match result with
            | Some x -> Some({
                Id = x.Id
                Name = x.Name
                Composer = x.Composer
                PercentCompleted = 0
                })
            | None -> None


            //if (List.exists (fun x -> x.Id.Equals(id)) pieces)
            //then 
            //    let result = List.find (fun x -> x.Id.Equals(id)) pieces
            //    Some result
            //else None
            


