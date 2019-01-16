module Tests

    open System
    open Xunit

    open System
    open System.Net
    open System.Net.Http
    open System.IO
    open Microsoft.AspNetCore.Builder
    open Microsoft.AspNetCore.Hosting
    open Microsoft.AspNetCore.TestHost
    open Microsoft.Extensions.DependencyInjection
    open FSharp.Control.Tasks.V2.ContextInsensitive
    open Xunit
    open Progress.Business
    open Giraffe

    type TestPiecesService() = 
    
        interface IPiecesService with
            member __.GetAll = [
                {
                    Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e8")
                    Name = "Test Name 1";
                    Composer = "Test Composer 1";
                    PercentCompleted = 0
                }
                {
                    Id = Guid("68f519a6-8c1e-4288-a744-9103e1c50b1b")
                    Name = "Test Name 2";
                    Composer = "Test Composer 2";
                    PercentCompleted = 0
                }
                ]
            member __.Get id = 
                if (id.Equals(Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")))
                then Some  {
                    Id = Guid("54cc3236-1ff6-407a-bb47-34fe729958e1")
                    Name = "Test Name 1";
                    Composer = "Test Composer 1";
                    PercentCompleted = 0
                    }
                else None

    let configureTestServices (services : IServiceCollection) =
        services.AddTransient<IPiecesService, TestPiecesService>() |> ignore
        services.AddCors()    |> ignore
        services.AddGiraffe() |> ignore

    let createHost() =
        WebHostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .Configure(Action<IApplicationBuilder> Progress.Api.App.configureApp)
            .ConfigureServices(Action<IServiceCollection>  configureTestServices)

    let get (client : HttpClient) (path : string) =
        path
        |> client.GetAsync

    let isStatus (code : HttpStatusCode) (response : HttpResponseMessage) =
        Assert.Equal(code, response.StatusCode)
        response

    let readText (response : HttpResponseMessage) =
        response.Content.ReadAsStringAsync()

    let shouldEqual expected actual =
        Assert.Equal(expected, actual)

    [<Fact>]
    let ``Get base end point`` () =
        task {
            use server = new TestServer(createHost())
            use client = server.CreateClient()
            let! response = get client "/api/"
            let! content =
                response
                |> isStatus HttpStatusCode.OK
                |> readText
        
            Assert.Equal("index", content)
        }

    [<Fact>]
    let ``Get Pieces return all pieces`` () =
        task {
            use server = new TestServer(createHost())
            use client = server.CreateClient()
            let! response = get client "/api/pieces"
            let! content =
                response
                |> isStatus HttpStatusCode.OK
                |> readText
        
            Assert.Equal("[{\"id\":\"54cc3236-1ff6-407a-bb47-34fe729958e8\",\"name\":\"Test Name 1\",\"composer\":\"Test Composer 1\",\"percentCompleted\":0},{\"id\":\"68f519a6-8c1e-4288-a744-9103e1c50b1b\",\"name\":\"Test Name 2\",\"composer\":\"Test Composer 2\",\"percentCompleted\":0}]", content)
        }

    [<Fact>]
    let ``Get Piece returns NotFound`` () =
        task {
            use server = new TestServer(createHost())
            use client = server.CreateClient()
            let! response = get client "/api/pieces/54cc3236-1ff6-407a-bb47-34fe729958e0"
            let! content =
                response
                |> isStatus HttpStatusCode.NotFound
                |> readText
        
            Assert.Equal("\"Id not found.\"", content)
        }

    [<Fact>]
    let ``Get Piece returns Piece`` () =
        task {
            use server = new TestServer(createHost())
            use client = server.CreateClient()
            let! response = get client "/api/pieces/54cc3236-1ff6-407a-bb47-34fe729958e1"
            let! content =
                response
                |> isStatus HttpStatusCode.OK
                |> readText
        
            Assert.Equal("{\"id\":\"54cc3236-1ff6-407a-bb47-34fe729958e1\",\"name\":\"Test Name 1\",\"composer\":\"Test Composer 1\",\"percentCompleted\":0}", content)
        }
