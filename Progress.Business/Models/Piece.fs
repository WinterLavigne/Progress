module Business.Models

open System

[<CLIMutable>]
type public Piece = {
    Id : Guid
    Name: string
    Composer: string
    PercentCompleted: int
    }

[<CLIMutable>]
type public NewPiece = {
    Name: string
    }