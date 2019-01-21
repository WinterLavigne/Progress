module ComposersTests

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
    open Business.Models.Composers
    open Giraffe
    open Newtonsoft.Json
    open System.Text

    type TestComposersService() = 
    
        interface IComposersService with
            member __.GetAll = [
                {
                    Id = Guid("00000000-0000-0000-0000-000000000001")
                    Name = "Test Name 1";
                    //Composer = "Test Composer 1";
                    //PercentCompleted = 0
                }
                {
                    Id = Guid("00000000-0000-0000-0000-000000000002")
                    Name = "Test Name 2";
                    //Composer = "Test Composer 2";
                    //PercentCompleted = 0
                }
                ]
            //member __.Get id = 
            //    if (id.Equals(Guid("00000000-0000-0000-0000-000000000003")))
            //    then Some  {
            //        Id = Guid("00000000-0000-0000-0000-000000000003")
            //        Name = "Test Name 1";
            //        //Composer = "Test Composer 1";
            //        //PercentCompleted = 0
            //        }
            //    else None
            //member __.Add newPiece = Some ({ 
            //    Id = Guid("00000000-0000-0000-0000-000000000004")
            //    Name = newPiece.Name
            //    //Composer = "To be implemented"
            //    //PercentCompleted = 0
            //    })

    let configureTestServices (services : IServiceCollection) =
        services.AddTransient<IComposersService, TestComposersService>() |> ignore
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

    let post (client : HttpClient) (path : string) (piece: AddComposer) =
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
    let ``Get Composer returns all composers`` () =
        task {
            use server = new TestServer(createHost())
            use client = server.CreateClient()
            let! response = get client "/api/composers"
            let! content =
                response
                |> isStatus HttpStatusCode.OK
                |> readText
        
            Assert.Equal("[{\"id\":\"00000000-0000-0000-0000-000000000001\",\"name\":\"Composer Name 1\"},{\"id\":\"00000000-0000-0000-0000-000000000002\",\"name\":\"Composer Name 2\"}]", content)
        }

    //[<Fact>]
    //let ``Get Piece returns NoContent`` () =
    //    task {
    //        use server = new TestServer(createHost())
    //        use client = server.CreateClient()
    //        let! response = get client "/api/pieces/54cc3236-1ff6-407a-bb47-34fe729958e0"
    //        let! content =
    //            response
    //            |> isStatus HttpStatusCode.NoContent
    //            |> readText
        
    //        Assert.Equal("", content)
    //    }

    //[<Fact>]
    //let ``Get Piece returns Piece`` () =
    //    task {
    //        use server = new TestServer(createHost())
    //        use client = server.CreateClient()
    //        let! response = get client "/api/pieces/00000000-0000-0000-0000-000000000003"
    //        let! content =
    //            response
    //            |> isStatus HttpStatusCode.OK
    //            |> readText
        
    //        Assert.Equal("{\"id\":\"00000000-0000-0000-0000-000000000003\",\"name\":\"Test Name 1\"}", content)
    //    }

    //[<Fact>]
    //let ``Add Piece returns Piece`` () =
    //    task {
    //        let newPiece = {
    //                Name = "Some Name"
    //            }
    //        use server = new TestServer(createHost())
    //        use client = server.CreateClient()
    //        let! response = post client "/api/pieces" newPiece
    //        let! content =
    //            response
    //            |> isStatus HttpStatusCode.Created
    //            |> readText
        
    //        Assert.Equal("{\"id\":\"00000000-0000-0000-0000-000000000004\",\"name\":\"Some Name\"}", content)
    //    }
