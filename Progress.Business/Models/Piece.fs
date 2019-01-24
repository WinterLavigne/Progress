namespace Business.Models.Pieces

open System
open Business.Models.Composers

type public GetPiece = {
    Id : Guid
    Name: string
    Composer: GetComposer
    }

type public AddPiece = {
    Name: string
    }