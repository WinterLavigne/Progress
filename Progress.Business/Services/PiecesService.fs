namespace Progress.Business

open System
open Progress.Repository
open Progress.Domain

type IPiecesService =
    abstract member GetAll: Business.Models.Pieces.GetPiece list
    abstract member Get: Guid -> Business.Models.Pieces.GetPiece option
    abstract member Add: Business.Models.Pieces.AddPiece -> Business.Models.Pieces.GetPiece option

type PiecesService(repository: IPiecesRepository) = 
    
    interface IPiecesService with

        member __.GetAll = 
             repository.GetAll |> List.map (fun x -> {
                    Id = x.Id
                    Name = x.Name
                    Composer = {
                        Id = Guid.Empty
                        Name = "TBD"
                        }
                })

        member __.Get id = 
            let result = repository.Get id
            match result with
            | Some x -> Some({
                Id = x.Id
                Name = x.Name
                Composer = {
                        Id = Guid.Empty
                        Name = "TBD"
                        }
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
                Composer = {
                        Id = Guid.Empty
                        Name = "TBD"
                        }
                })
            | None -> None
           
            


