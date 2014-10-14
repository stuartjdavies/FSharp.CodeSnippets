#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r @"..\packages\ExcelProvider.0.1.2\lib\net40\ExcelProvider.dll"
#r @"FSharp.Data.TypeProviders.dll" 

open System
open RDotNet
open RProvider
open FSharp.Data
open RProvider.``base``
open RProvider.graphics
open System.Collections.Generic
open FSharp.Charting

open System.IO
open FSharp.ExcelProvider
open Microsoft.FSharp.Data.TypeProviders
open System.Linq
open System.Data

//
// Statistics of Highly skilled visas approved in 2014
//
let filePath = @"C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.Data\FA140301378-3.xls"
type ApplicationsSchema = ExcelFile<"C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.Data\FA140301378-3.xls", "2012-13!A8:F65536">

let applications = new ApplicationsSchema(filePath)

let applicationsApprovedByIndustry = applications.Data |> Seq.filter(fun r -> r.``Nomination Approved``.CompareTo("Y") = 0)
                                                       |> Seq.groupBy(fun r -> r.``Sponsor Industry (self identified)``)
                                                       |> Seq.map(fun (g, rows) -> (g, (rows |> Seq.length)))
                                                       |> Seq.sortBy(fun (g, l) -> -l)
