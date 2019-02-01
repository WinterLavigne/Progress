module Api.Models

open System

type public ComposerOverview = {
    Id : Guid
    Name: string
    }

type public PieceOverview = {
    Id : Guid
    Name: string
    Composer: ComposerOverview
    PercentCompleted: int
    }

