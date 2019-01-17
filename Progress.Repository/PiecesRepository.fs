namespace Progress.Repository

open System
open Progress.Domain
open Progress.Context
open Microsoft.EntityFrameworkCore
open System.Linq


type IPiecesRepository =
    abstract member GetAll: Piece list
    abstract member Get: Guid -> Piece option

type PiecesRepository(context: ProgressContext) =

    interface IPiecesRepository with
        member __. GetAll = context.Pieces |> Seq.toList
        member __.Get id = context.Pieces.Where(fun x -> x.Id.Equals(id))  |> Seq.tryHead