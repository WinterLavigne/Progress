module Progress.Business

open Business.Models
open System

type IPiecesService =
    abstract member GetAll: Piece list
    abstract member Get: Guid -> Piece option

type PiecesService() = 
    
    let pieces = [
            {
                Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e8")
                Name = "Test Name 1";
                Composer = "Test Composer 1";
                PercentCompleted = 0
            }
            {
                Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
                Name = "Test Name 2";
                Composer = "Test Composer 2";
                PercentCompleted = 0
            }
            ]

    interface IPiecesService with

        member __.GetAll = pieces
        member __.Get id = 
            if (List.exists (fun x -> x.Id.Equals(id)) pieces)
            then 
                let result = List.find (fun x -> x.Id.Equals(id)) pieces
                Some result
            else None
            


