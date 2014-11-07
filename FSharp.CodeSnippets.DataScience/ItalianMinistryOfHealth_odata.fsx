#r "System.Data.Linq.dll"
#r "System.Data.Entity.dll"
#r "FSharp.Data.TypeProviders.dll"
#r "System.Data.Services.Client.dll"

open System.Data.Linq
open System.Data.Entity
open Microsoft.FSharp.Data.TypeProviders
open System
open System.Data
open System.Data.SqlClient
open System.Data.EntityClient
open System.Data.Metadata.Edm

// Italy Ministry of Health Open Data Portal
type ItalianMinstryOfHealthService = ODataService<"http://opendatasalutedata.cloudapp.net/v1/datacatalog/">
let db = ItalianMinstryOfHealthService.GetDataContext()

query {
    for dispositivo in db.DispositiviMedici do
    select(dispositivo)
} 
|> Seq.take 5 
|> Seq.iter (fun d -> printfn "Description - %s, Type - %s" d.descrizionecnd d.tipo) 





