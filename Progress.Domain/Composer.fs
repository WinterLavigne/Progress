namespace Progress.Domain

open System
open System.ComponentModel.DataAnnotations.Schema

[<CLIMutable>]
type public Composer = {

    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] Id : Guid
    [<DatabaseGenerated(DatabaseGeneratedOption.Computed)>] Created : DateTime
    Name: string
    
   
 }