#r @".\bin\Debug\FSharp.CodeSnippets.TypeProviders.dll"
#r @"..\packages\ExcelProvider.0.1.2\lib\net40\Excel.dll"
#r "PresentationCore.dll"
#r "PresentationFramework.dll"
#r "System.Xaml.dll"
#r "WindowsBase.dll"
 
#load "../packages/XPlot.GoogleCharts.1.0.1/XPlot.GoogleCharts.fsx"

open FSharp.CodeSnippets.TypeProviders
open System.IO
open Excel
open System
open XPlot.GoogleCharts

let filePath = __SOURCE_DIRECTORY__ + @"\data\Labour underutilisation by Age and Sex - Trend.xls"

let trends = new ABSExcelSchemaProvider<"""..\FSharp.CodeSnippets.DataScience\data\Labour underutilisation by Age and Sex - Trend.xls""">(filePath)

let totalUnderUtiliRate = trends.Data |> Seq.map(fun r -> DateTime.Parse(r.Date), (Single.Parse(r.``Labour force underutilisation rate ;  Total (Age) ;  Persons ; - (Trend)``)))
 
let options =
    Options(
        title = "Labour force underutilisation rate",
        curveType = "function"
    )
            
let chart =
    [totalUnderUtiliRate]
    |> Chart.Line
    |> Chart.WithOptions options
    |> Chart.Show

let rate15_24 = trends.Data |> Seq.map(fun r -> DateTime.Parse(r.Date), (Single.Parse(r.``Labour force underutilisation rate ;  15 - 24 ;  Persons ; - (Original)``)))
let rate25_34 = trends.Data |> Seq.map(fun r -> DateTime.Parse(r.Date), (Single.Parse(r.``Labour force underutilisation rate ;  25 - 34 ;  Persons ; - (Original)``)))
let rate35_44 = trends.Data |> Seq.map(fun r -> DateTime.Parse(r.Date), (Single.Parse(r.``Labour force underutilisation rate ;  35 - 44 ;  Persons ; - (Original)``)))
let rate45_54 = trends.Data |> Seq.map(fun r -> DateTime.Parse(r.Date), (Single.Parse(r.``Labour force underutilisation rate ;  45 - 54 ;  Persons ; - (Original)``)))
let rate55_Over = trends.Data |> Seq.map(fun r -> DateTime.Parse(r.Date), (Single.Parse(r.``Labour force underutilisation rate ;  55 and over ;  Persons ; - (Original)``)))

let options2 =
    Options(
        title = "Labour force underutilisation rate",
        curveType = "function",
        legend = Legend(position = "bottom")
    )
            
let chart2 =
    [rate15_24;rate25_34;rate35_44;rate45_54;rate55_Over]
    |> Chart.Line
    |> Chart.WithOptions options2
    |> Chart.WithLabels ["15 - 24"; "25 - 34"; "35 - 44"; "45 - 54"; "Over 55"]
    |> Chart.Show

let chart3 =
    [rate15_24;rate25_34;rate35_44;rate45_54;rate55_Over]
    |> Chart.Line
    |> Chart.WithOptions options2
    |> Chart.WithLabels ["15 - 24"; "25 - 34"; "35 - 44"; "45 - 54"; "Over 55"]