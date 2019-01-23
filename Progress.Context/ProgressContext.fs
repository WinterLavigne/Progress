namespace Progress.Context

open Microsoft.EntityFrameworkCore
open Microsoft.EntityFrameworkCore.Storage.ValueConversion
open Progress.Domain

type ProgressContext =
    inherit DbContext

    new() = { inherit DbContext() }
    new(options: DbContextOptions<ProgressContext>) = { inherit DbContext(options) }
    
    //override __.OnConfiguring optionsBuilder = 
    //    optionsBuilder.UseSqlServer("Server=.,1433;Initial Catalog=Progress;Persist Security Info=False;User ID={userid};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;") |> ignore


    [<DefaultValue>]
    val mutable pieces:DbSet<Piece>
    member x.Pieces 
        with get() = x.pieces 
        and set v = x.pieces <- v


    [<DefaultValue>]
    val mutable composers:DbSet<Composer>
    member x.Composers 
        with get() = x.composers 
        and set v = x.composers <- v
