namespace Progress.Business

open System
open Progress.Repository

type IPiecesService =
    abstract member GetAll: Business.Models.Pieces.GetPiece list
    abstract member Get: Guid -> Business.Models.Pieces.GetPiece option
    abstract member Add: Business.Models.Pieces.AddPiece -> Business.Models.Pieces.GetPiece option

type PiecesService(repository: IPiecesRepository, composersService: IComposersService) = 
    
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
            let composer = composersService.Add({
                Name = newPiece.Composer.Name
                })
            match composer with
            | Some c -> 
                let result = repository.Add {
                    Name = newPiece.Name
                    Composer = {
                        Id = c.Id
                        Name = c.Name
                        }
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
            | _ -> None
            
           
            


