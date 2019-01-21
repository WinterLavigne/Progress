namespace Progress.Business

open System
open Progress.Repository

type IComposersService =
    abstract member GetAll: Business.Models.Composers.GetComposer list
    //abstract member Get: Guid -> Business.Models.GetPiece option
    //abstract member Add: Business.Models.AddPiece -> Business.Models.GetPiece option

//type ComposersService(repository: IComposersRepository) = 
type ComposersService() = 
    
    interface IComposersService with

        member __.GetAll = []
             //repository.GetAll |> List.map (fun x -> {
             //       Id = x.Id
             //       Name = x.Name
             //       //Composer = "To implement"
             //       //PercentCompleted = 0
             //   }) 
        //member __.Get id = 
        //    let result = repository.Get id
        //    match result with
        //    | Some x -> Some({
        //        Id = x.Id
        //        Name = x.Name
        //        //Composer = "To implement"
        //        //PercentCompleted = 0
        //        })
        //    | None -> None
        //member __.Add newPiece = 
        //    let result = repository.Add {
        //        Name = newPiece.Name
        //        }
        //    match result with
        //    | Some x -> Some({
        //        Id = x.Id
        //        Name = x.Name
        //        //Composer = "To be implemented"
        //        //PercentCompleted = 0
        //        })
        //    | None -> None
           
            


