module Tests

open System
open Xunit
open Progress.Business
open FSharp.Control.Tasks.V2
open Progress.Repository
open Progress.Context
open Progress.Domain
open Progress.Business
open Microsoft.EntityFrameworkCore

let createServiceWithDatabaseObjects (name: string)=
    let context = new DbContextOptionsBuilder<ProgressContext>()
    let options = context.UseInMemoryDatabase(name).Options
    let context = new ProgressContext(options)
    context.Pieces.Add({
                Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e8")
                Name = "Test Name 1"
                //Composer = "Test Composer 1"
            }) |> ignore
    context.Pieces.Add({
                Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
                Name = "Test Name 2"
                //Composer = "Test Composer 2"
            })|> ignore
    context.SaveChanges() |> ignore

    let _sut = PiecesService(PiecesRepository(context)) :> IPiecesService
    _sut

let createService (name: string) = 
    let result = createServiceWithDatabaseObjects name
    result


[<Fact>]
let ``GetAll returns all pieces`` () =
    task {  
            let _sut = createService "GetAll returns all pieces"
            let result = _sut.GetAll

            Assert.Equal(2, result.Length)
        }

[<Fact>]
let ``Get returns none when id does not exist`` () =
    task {
            let _sut = createService "Get returns none when id does not exist"
            let id =  Guid.NewGuid()
            let result = _sut.Get id

            Assert.True(result.IsNone)
        }

[<Fact>]
let ``Get returns some when id does exist`` () =
    task {
            let _sut = createService "Get returns some when id does exist"
            let id =  Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
            let result = _sut.Get id

            Assert.True(result.IsSome)
        }

[<Fact>]
let ``Add returns new piece`` () =
    task {
            let mock = {
                new IPiecesRepository with
                    member __. GetAll = []
                    member __.Get id = Some({
                            Id = Guid.NewGuid()
                            Name = "Test" })   
                    member __.Add piece = Some({ 
                            Id = Guid.NewGuid()
                            Name = "From Repo"
                            })
            } 

            let _sut = PiecesService(mock) :> IPiecesService

            let result = _sut.Add({Name = "Add returns new piece"})
            
            Assert.True(result.IsSome)
            Assert.Equal("From Repo", result.Value.Name)
        }