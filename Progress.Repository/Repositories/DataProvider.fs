namespace Progress.Repository

open FSharp.Data.Sql

type sql  = SqlDataProvider<
                   Common.DatabaseProviderTypes.MSSQLSERVER, 
                   "Server=.,1433;Initial Catalog=Progress;Persist Security Info=False;User ID=SA;Password=W3nt2r!Docker);MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=10;"
                   >