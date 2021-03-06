module ComposersTests

open System
open Xunit
open Progress.Business
open FSharp.Control.Tasks.V2
open Progress.Repository

type MockComposersRepository() =
    
    let mutable adds = 0
    let mutable getAlls = 0
    let mutable gets = 0
    let mutable getByNames = 0
    let db: (Progress.Repository.GetComposer list) = [
            {
                Id = Guid("00000000-0000-0000-0001-000000000000")
                Name = "Test Composer 1"
            }
            {
                Id = Guid("00000000-0000-0000-0002-000000000000")
                Name = "Test Composer 2"
            }]

    member __.Adds = adds    
    member __.GetAlls = getAlls
    member __.Gets = gets
    member __.GetByNames = getByNames

    

    interface IComposersRepository with
        member __.GetAll = db
        member __.Get id = 
            let list = db |> List.filter (fun x -> x.Id.Equals(id)) 
            match list with
            | [] -> None
            | l -> Some(List.head l)   
        member __.Add composer = 
            adds <- adds + 1
            Some({ 
                Id = Guid.NewGuid()
                Name = composer.Name
                }) 
        member __.GetByName name = 
            getByNames <- getByNames + 1
            let list = db |> List.filter (fun x -> x.Name.Equals(name)) 
            match list with
            | [] -> None
            | l -> Some(List.head l)  


[<Fact>]
let ``GetAll returns all pieces`` () =
    task {  
            let _sut = ComposersService(MockComposersRepository()) :> IComposersService
           // let _sut = createService "GetAll returns all pieces"
            let result = _sut.GetAll

            Assert.Equal(2, result.Length)
        }

[<Fact>]
let ``Get returns none when id does not exist`` () =
    task {
            let _sut = ComposersService(MockComposersRepository()) :> IComposersService
            let id =  Guid.NewGuid()
            let result = _sut.Get id

            Assert.True(result.IsNone)
        }

[<Fact>]
let ``Get returns some when id does exist`` () =
    task {
            let _sut = ComposersService(MockComposersRepository()) :> IComposersService
            let id =  Guid("00000000-0000-0000-0002-000000000000")
            let result = _sut.Get id

            Assert.True(result.IsSome)
        }

[<Fact>]
let ``Add returns new composer`` () =
    task {
            let repo = MockComposersRepository()
            let _sut = ComposersService(repo) :> IComposersService

            let result = _sut.Add({Name = "Add returns new piece"})
            
            Assert.True(result.IsSome)
            Assert.Equal("Add returns new piece", result.Value.Name)
            Assert.Equal(1, repo.Adds)
        }

[<Fact>]
let ``Add returns existing composer when composer exist`` () =
    task {
            let repo = MockComposersRepository()
            let _sut = ComposersService(repo) :> IComposersService
            
            let result = _sut.Add({Name = "Test Composer 1"})
            
            Assert.True(result.IsSome)
            Assert.Equal("Test Composer 1", result.Value.Name)
            Assert.Equal(Guid("00000000-0000-0000-0001-000000000000"), result.Value.Id)
            Assert.Equal(1, repo.GetByNames)
        }  