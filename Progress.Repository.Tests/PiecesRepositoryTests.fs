module Tests

open System
open Xunit


open Progress.Repository
open Progress.Context
open Progress.Domain
open Microsoft.EntityFrameworkCore

let createInMemoryContext (name: string)=
    let contextBuilder = new DbContextOptionsBuilder<ProgressContext>()
    let options = contextBuilder.UseInMemoryDatabase(name).Options
    let context = new ProgressContext(options)
    context.Pieces.Add({
                Id = Guid("00000000-0000-0000-0000-000000000001")
                Name = "Test Name 1"
                Created = DateTime.Now
                Composer = None
                //Composer = "Test Composer 1"
            }) |> ignore
    context.Pieces.Add({
                Id = Guid("00000000-0000-0000-0000-000000000002")
                Name = "Test Name 2"
                Created = DateTime.Now
                Composer = None
            })|> ignore
    context.SaveChanges() |> ignore

    context

[<Fact>]
let ``GetAll Returns all Pieces`` () =
    
    let sut = PiecesRepository(createInMemoryContext("GetAll Returns all Pieces")) :> IPiecesRepository
    
    let result = sut.GetAll

    Assert.Equal(2, result.Length)

[<Fact>]
let ``Get Returns piece when exists`` () =
    
    let sut = PiecesRepository(createInMemoryContext("Get Returns piece when exists")) :> IPiecesRepository
    
    let result = sut.Get (Guid("00000000-0000-0000-0000-000000000002"))

    Assert.True(result.IsSome)
    Assert.Equal(Guid("00000000-0000-0000-0000-000000000002"), result.Value.Id)
    Assert.Equal("Test Name 2", result.Value.Name)

[<Fact>]
let ``Get Returns None when not exists`` () =
    
    let sut = PiecesRepository(createInMemoryContext("Get Returns None when not exists")) :> IPiecesRepository
    
    let result = sut.Get (Guid("54cc3236-1ff6-407a-bb47-34fe729958e1"))

    Assert.True(result.IsNone)

[<Fact>]
let ``Add Saves to database`` () =
    let context = createInMemoryContext("Add Saves to database")
    let sut = PiecesRepository(context) :> IPiecesRepository
    
    sut.Add {
        Name = "Add Name"
        } |> ignore
    
    let pieces = context.Pieces |> Seq.toList
    Assert.Equal(3, pieces.Length)

[<Fact>]
let ``Add Returns piece`` () =
    
    let sut = PiecesRepository(createInMemoryContext("Add Returns piece")) :> IPiecesRepository
    
    let result = sut.Add {
        Name = "Add Name"
        }

    Assert.True(result.IsSome)
    Assert.NotEqual(Guid("00000000-0000-0000-0000-000000000003"), result.Value.Id)
    Assert.Equal("Add Name", result.Value.Name)
