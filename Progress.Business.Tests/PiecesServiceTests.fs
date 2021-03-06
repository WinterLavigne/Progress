module PiecesTests

open System
open Xunit
open Progress.Business
open FSharp.Control.Tasks.V2
open Progress.Repository

type MockPiecesRepository() =
    
    let mutable adds = 0
    let mutable getAlls = 0
    let mutable gets = 0
    let db: (Progress.Repository.GetPiece list) = [
            {
                Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e8")
                Name = "Test Name 1"
                Composer = {
                    Id = Guid.Empty
                    Name = "TBD"
                    }
            }
            {
                Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
                Name = "Test Name 2"
                Composer = {
                    Id = Guid.Empty
                    Name = "TBD"
                    }
            }]

    member __.Adds = adds    
    member __.GetAlls = getAlls
    member __.Gets = gets

    

    interface IPiecesRepository with
        member __.GetAll = db
        member __.Get id = 
            let list = db |> List.filter (fun x -> x.Id.Equals(id)) 
            match list with
            | [] -> None
            | l -> Some(List.head l)
            
        member __.Add piece = 
            adds <- adds + 1
            Some({ 
                Id = Guid.NewGuid()
                Name = piece.Name
                Composer = {
                    Id = Guid.Empty
                    Name = "TBD"
                    }
                }) 


type MockComposersService() =
   
    interface IComposersService with
        member __.GetAll = []
        member __.Get id = None
        member __.Add composer =    
            if composer.Name = "Existing" 
            then
                Some({
                    Id = Guid.NewGuid()
                    Name = composer.Name
                    })
            else if composer.Name = "New Composer" 
            then 
                Some({
                    Id = Guid.NewGuid()
                    Name = composer.Name
                    })
            else
                None


[<Fact>]
let ``GetAll returns all pieces`` () =
    task {  
            let _sut = PiecesService(MockPiecesRepository(), MockComposersService()) :> IPiecesService
           // let _sut = createService "GetAll returns all pieces"
            let result = _sut.GetAll

            Assert.Equal(2, result.Length)
        }

[<Fact>]
let ``Get returns none when id does not exist`` () =
    task {
            let _sut = PiecesService(MockPiecesRepository(), MockComposersService()) :> IPiecesService
            let id =  Guid.NewGuid()
            let result = _sut.Get id

            Assert.True(result.IsNone)
        }

[<Fact>]
let ``Get returns some when id does exist`` () =
    task {
            let _sut = PiecesService(MockPiecesRepository(), MockComposersService()) :> IPiecesService
            let id =  Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
            let result = _sut.Get id

            Assert.True(result.IsSome)
        }

[<Fact>]
let ``Add returns new piece`` () =
    task {
            let repo = MockPiecesRepository()
            let _sut = PiecesService(repo, MockComposersService()) :> IPiecesService

            let result = _sut.Add({
                Name = "Add returns new piece"
                Composer = {
                    Id = Guid.Empty
                    Name = "Existing"
                    }
                })
            
            Assert.True(result.IsSome)
            Assert.Equal("Add returns new piece", result.Value.Name)
            Assert.Equal(1, repo.Adds)
        }

[<Fact>]
let ``Add returns None when Composer could not be found or added`` () =
    task {
            let repo = MockPiecesRepository()
            let _sut = PiecesService(repo, MockComposersService()) :> IPiecesService

            let result = _sut.Add({
                Name = "Add returns new piece"
                Composer = {
                    Id = Guid.Empty
                    Name = "Non Existing"
                    }
                })
            
            Assert.True(result.IsNone)
           
        } 

[<Fact>]
let ``Add returns Piece when Composer could be added`` () =
    task {
            let repo = MockPiecesRepository()
            let _sut = PiecesService(repo, MockComposersService()) :> IPiecesService

            let result = _sut.Add({
                Name = "Add returns new piece"
                Composer = {
                    Id = Guid.Empty
                    Name = "New Composer"
                    }
                })
            
            Assert.True(result.IsSome)
            Assert.Equal("Add returns new piece", result.Value.Name)
            Assert.Equal(1, repo.Adds)
           
        }    