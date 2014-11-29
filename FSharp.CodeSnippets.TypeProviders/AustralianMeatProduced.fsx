﻿#r @".\bin\Debug\FSharp.CodeSnippets.TypeProviders.dll"
#r @"..\packages\ExcelProvider.0.1.2\lib\net40\Excel.dll"

open FSharp.CodeSnippets.TypeProviders
open System.IO
open Excel
open System

let path="""C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.DataScience\data\RedMeatProduced.xls"""

let csv = new ABSExcelSchemaProvider<"C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.DataScience\data\RedMeatProduced.xls">(path)
csv.Data |> Seq.iter (fun r -> printfn "%s %s"  r.Date r.``Meat Produced ;  Total Red Meat ;  New South Wales ; - (Original)``)         