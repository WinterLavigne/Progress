namespace Progress.Repository

open System
open FSharp.Data.Sql.Common

//type public GetPiece = {
//    Id : Guid
//    Name: string
//    }

type GetPiece = {
    [<MappedColumn("Id")>] Id: Guid
    [<MappedColumn("Name")>] Name: string
    Composer: GetComposer
    }
    
type public AddPiece = {
    Name: string
    Composer: GetComposer
    }