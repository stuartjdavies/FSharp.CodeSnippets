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

let filePath = __SOURCE_DIRECTORY__ + @"\data\Labour force status by Sex - Trend.xls"

let trends = new ABSExcelSchemaProvider<"""..\FSharp.CodeSnippets.DataScience\data\Labour force status by Sex - Trend.xls""">(filePath)

let femaleFullTimeEmp = trends.Data |> Seq.map(fun r -> DateTime.Parse(r.Date), (Single.Parse(r.``Employed - full-time ;  Females ; - (Trend)``) * 1000.0F))
let maleFullTimeEmp = trends.Data |> Seq.map(fun r -> DateTime.Parse(r.Date), (Single.Parse(r.``Employed - full-time ;  Males ; - (Trend)``) * 1000.0F))
 
let options =
    Options(
        title = "Male vs Female Full Time Employed per month",
        curveType = "function",
        legend = Legend(position = "bottom")
    )
            
let chart =
    [maleFullTimeEmp; femaleFullTimeEmp]
    |> Chart.Line
    |> Chart.WithOptions options
    |> Chart.WithLabels ["Male"; "Female"]
    |> Chart.Show