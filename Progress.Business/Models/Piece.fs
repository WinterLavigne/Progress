namespace Business.Models.Pieces

open System

type public GetPiece = {
    Id : Guid
    Name: string
    }

type public AddPiece = {
    Name: string
    }