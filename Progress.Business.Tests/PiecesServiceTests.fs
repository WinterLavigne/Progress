module Tests

open System
open Xunit
open Progress.Business
open FSharp.Control.Tasks.V2
open Progress.Repository

let repo = PiecesRepository()
let sut = PiecesService(repo) :> IPiecesService

[<Fact>]
let ``GetAll returns all pieces`` () =
    task {  
            let result = sut.GetAll

            Assert.Equal(2, result.Length)
        }

[<Fact>]
let ``Get returns none when id does not exist`` () =
    task {
            let id =  Guid.NewGuid()
            let result = sut.Get id

            Assert.True(result.IsNone)
        }

[<Fact>]
let ``Get returns some when id does exist`` () =
    task {
            let id =  Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
            let result = sut.Get id

            Assert.True(result.IsSome)
        }