#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r @"..\packages\ExcelProvider.0.1.2\lib\net40\ExcelProvider.dll"
#r @"FSharp.Data.TypeProviders.dll" 
 
open RProvider
open RProvider.``base``
open RProvider.Internal.Converters
open RProvider.plan
open System
open FSharp.ExcelProvider
open Microsoft.FSharp.Data.TypeProviders

//let filePath = __SOURCE_DIRECTORY__ + @[TrainingFile].xls"
let filePath="""C:\Users\stuart\OneDrive\Documents\JobSearch\Training.xlsx"""

type TrainingSchema = ExcelFile<"""Data\Training.xlsx""">
let training = (new TrainingSchema(filePath)).Data 
               |> Seq.filter(fun x -> x.Status <> "To be confirmed")
               |> Seq.filter(fun x -> x.``Start Date``.Year = 2015)  

let keys = training |> Seq.mapi(fun i x -> i.ToString()) 
let descriptions = training |> Seq.mapi(fun i x -> x.``Short Description``)                                                   
let startDates = training |> Seq.map(fun x -> String.Format("{0}-{1}-{2} 12:00:00 EST", x.``Start Date``.Year, x.``Start Date``.Month, x.``Start Date``.Day))
let endDates = training |> Seq.map(fun x -> String.Format("{0}-{1}-{2} 12:00:00 EST", x.``Finish Date``.Year, x.``Finish Date``.Month, x.``Finish Date``.Day)) 
let statuses = training |> Seq.map(fun x -> 0)

let g = R.as_gantt(namedParams [                
                       "key", box keys
                       "description", box descriptions
                       "start", box startDates
                       "end", box endDates
                       "done", box statuses
                   ])

R.plot_gantt(x=g)
                
                

