module Tests

open System
open Xunit
open Progress.Business
open FSharp.Control.Tasks.V2
open Progress.Repository
open Progress.Context
open Progress.Domain
open Microsoft.EntityFrameworkCore

type TestPiecesRepository() =
    let pieces = [
            {
                Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e8")
                Name = "Test Name 1"
                //Composer = "Test Composer 1"
            }
            {
                Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
                Name = "Test Name 2"
                //Composer = "Test Composer 2"
            }
            ]

    interface IPiecesRepository with
        member __. GetAll = pieces
        member __.Get id = List.filter (fun x -> x.Id.Equals(id)) pieces |> Seq.tryHead

//let context = new DbContextOptionsBuilder<ProgressContext>()
//let options = context.UseInMemoryDatabase("InMemoryDatabase").Options


//let sut = PiecesService(PiecesRepository(new ProgressContext(options))) :> IPiecesService

[<Fact>]
let ``GetAll returns all pieces`` () =
    task {  
            let context = new DbContextOptionsBuilder<ProgressContext>()
            let options = context.UseInMemoryDatabase("GetAll returns all pieces").Options


            let _sut = PiecesService(PiecesRepository(new ProgressContext(options))) :> IPiecesService
            let result = _sut.GetAll

            Assert.Equal(2, result.Length)
        }

//[<Fact>]
//let ``Get returns none when id does not exist`` () =
//    task {
//            let id =  Guid.NewGuid()
//            let result = sut.Get id

//            Assert.True(result.IsNone)
//        }

//[<Fact>]
//let ``Get returns some when id does exist`` () =
//    task {
//            let id =  Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
//            let result = sut.Get id

//            Assert.True(result.IsSome)
//        }

//[<Fact>]
//let ``Add returns new piece`` () =
//    task {
//            //let id =  Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
//            //let result = sut.Add { Name = "Add piece" }

//            Assert.True(false)
//        }