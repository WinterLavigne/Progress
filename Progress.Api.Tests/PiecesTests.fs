module PiecesTests

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
    open Business.Models.Pieces
    open Giraffe
    open Newtonsoft.Json
    open System.Text
    open Progress.Domain

    type TestPiecesService() = 
    
        interface IPiecesService with
            member __.GetAll = [
                {
                    Id = Guid("00000000-0000-0000-0000-000000000001")
                    Name = "Test Name 1"
                    Composer = {
                        Id = Guid.Empty
                        Name = "TBD"
                        }
                    //Composer = "Test Composer 1";
                    //PercentCompleted = 0
                }
                {
                    Id = Guid("00000000-0000-0000-0000-000000000002")
                    Name = "Test Name 2";
                    Composer = {
                        Id = Guid.Empty
                        Name = "TBD"
                        }
                }
                ]
            member __.Get id = 
                if (id.Equals(Guid("00000000-0000-0000-0000-000000000003")))
                then Some  {
                    Id = Guid("00000000-0000-0000-0000-000000000003")
                    Name = "Test Name 1";
                    Composer = {
                        Id = Guid.Empty
                        Name = "TBD"
                        }
                    }
                else None
            member __.Add newPiece = Some ({ 
                Id = Guid("00000000-0000-0000-0000-000000000004")
                Name = newPiece.Name
                Composer = {
                        Id = Guid.Empty
                        Name = "TBD"
                        }
                })

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

    let post (client : HttpClient) (path : string) (piece: AddPiece) =
        let postData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(piece))
        client.PostAsync (path, new StreamContent(new MemoryStream(postData)))

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
        
            Assert.Equal("[{\"id\":\"00000000-0000-0000-0000-000000000001\",\"name\":\"Test Name 1\"},{\"id\":\"00000000-0000-0000-0000-000000000002\",\"name\":\"Test Name 2\"}]", content)
        }

    [<Fact>]
    let ``Get Piece returns NoContent`` () =
        task {
            use server = new TestServer(createHost())
            use client = server.CreateClient()
            let! response = get client "/api/pieces/54cc3236-1ff6-407a-bb47-34fe729958e0"
            let! content =
                response
                |> isStatus HttpStatusCode.NoContent
                |> readText
        
            Assert.Equal("", content)
        }

    [<Fact>]
    let ``Get Piece returns Piece`` () =
        task {
            use server = new TestServer(createHost())
            use client = server.CreateClient()
            let! response = get client "/api/pieces/00000000-0000-0000-0000-000000000003"
            let! content =
                response
                |> isStatus HttpStatusCode.OK
                |> readText
        
            Assert.Equal("{\"id\":\"00000000-0000-0000-0000-000000000003\",\"name\":\"Test Name 1\"}", content)
        }

    [<Fact>]
    let ``Add Piece returns Piece`` () =
        task {
            let newPiece = {
                    Name = "Some Name"
                }
            use server = new TestServer(createHost())
            use client = server.CreateClient()
            let! response = post client "/api/pieces" newPiece
            let! content =
                response
                |> isStatus HttpStatusCode.Created
                |> readText
        
            Assert.Equal("{\"id\":\"00000000-0000-0000-0000-000000000004\",\"name\":\"Some Name\"}", content)
        }
