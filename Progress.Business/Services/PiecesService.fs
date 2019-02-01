namespace Progress.Business

open System
open Progress.Repository

type IPiecesService =
    abstract member GetAll: Business.Models.Pieces.GetPiece list
    abstract member Get: Guid -> Business.Models.Pieces.GetPiece option
    abstract member Add: Business.Models.Pieces.AddPiece -> Business.Models.Pieces.GetPiece option

type PiecesService(repository: IPiecesRepository, composersService: IComposersService) = 
    

    let GetOrAddComposer (composer: Business.Models.Composers.GetComposer) =
        let test = composersService.Get composer.Id
        match test with
        | Some _ -> test
        | _ -> 
            composersService.Add({
                Name = composer.Name
                })
            

    interface IPiecesService with

        member __.GetAll = 
             repository.GetAll |> List.map (fun x -> {
                    Id = x.Id
                    Name = x.Name
                    Composer = {
                        Id = x.Composer.Id
                        Name = x.Composer.Name
                        }
                })

        member __.Get id = 
            let result = repository.Get id
            match result with
            | Some x -> Some({
                Id = x.Id
                Name = x.Name
                Composer = {
                        Id = x.Composer.Id
                        Name = x.Composer.Name
                        }
                })
            | None -> None
        member __.Add newPiece = 
            let composer = GetOrAddComposer newPiece.Composer
            
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
                            Id = x.Composer.Id
                            Name = x.Composer.Name
                            }
                    })
                | None -> None
            | _ -> None
            
           
            


