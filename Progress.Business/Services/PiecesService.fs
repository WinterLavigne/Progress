namespace Progress.Business

open System
open Progress.Repository

type IPiecesService =
    abstract member GetAll: Business.Models.GetPiece list
    abstract member Get: Guid -> Business.Models.GetPiece option
    abstract member Add: Business.Models.AddPiece -> Business.Models.GetPiece option

type PiecesService(repository: IPiecesRepository) = 
    
    interface IPiecesService with

        member __.GetAll = 
             repository.GetAll |> List.map (fun x -> {
                    Id = x.Id
                    Name = x.Name
                    //Composer = "To implement"
                    //PercentCompleted = 0
                }) 
        member __.Get id = 
            let result = repository.Get id
            match result with
            | Some x -> Some({
                Id = x.Id
                Name = x.Name
                //Composer = "To implement"
                //PercentCompleted = 0
                })
            | None -> None
        member __.Add newPiece = 
            let result = repository.Add {
                Name = newPiece.Name
                }
            match result with
            | Some x -> Some({
                Id = x.Id
                Name = x.Name
                //Composer = "To be implemented"
                //PercentCompleted = 0
                })
            | None -> None
           
            


