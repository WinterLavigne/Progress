namespace Progress.Repository

open System

type public GetComposer = {
    Id : Guid
    Name: string
    }
    
type public AddComposer = {
    Name: string
    }