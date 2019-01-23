module Api.Models

open System


type public PieceOverview = {
    Id : Guid
    Name: string
    Composer: string
    PercentCompleted: int
    }