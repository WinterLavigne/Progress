namespace Progress.Business

open System
open Progress.Repository

type IComposersService =
    abstract member GetAll: Business.Models.Composers.GetComposer list
    abstract member Get: Guid -> Business.Models.Composers.GetComposer option
    abstract member Add: Business.Models.Composers.AddComposer -> Business.Models.Composers.GetComposer option

type ComposersService(repository: IComposersRepository) = 
    
    interface IComposersService with

        member __.GetAll = 
             repository.GetAll |> List.map (fun x -> {
                    Id = x.Id
                    Name = x.Name
                }) 
        member __.Get id =
            let result = repository.Get id
            
            match result with
            | Some x -> Some({
                Id = x.Id
                Name = x.Name
                })
            | None -> None
        member __.Add composer =
            let result = repository.Add {
                Name = composer.Name
                }
            match result with
            | Some x -> Some({
                Id = x.Id
                Name = x.Name
                })
            | None -> None
           
            


