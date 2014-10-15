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

open System.IO
open FSharp.ExcelProvider
open Microsoft.FSharp.Data.TypeProviders
open System.Linq
open System.Data
open FSharp.Charting

let filePath = @"C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.Data\SEEK_AU_EI_Data_Sep2014.xls"
type SeekJobsSchema = ExcelFile<"C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.Data\SEEK_AU_EI_Data_Sep2014.xls", "SEEK New Job Ads SA!A2:J161">
let seekJobs = new SeekJobsSchema(filePath)

let ToDate() = DateTime.Parse("")

[Chart.Line([ for row in seekJobs.Data -> DateTime.FromOADate(row.Month), row.`` ACT`` ],Name="ACT");
 Chart.Line([ for row in seekJobs.Data -> DateTime.FromOADate(row.Month), row.`` NT `` ],Name="NT");
 Chart.Line([ for row in seekJobs.Data -> DateTime.FromOADate(row.Month), row.``NSW `` ],Name="NSW");
 Chart.Line([ for row in seekJobs.Data -> DateTime.FromOADate(row.Month), row.`` QLD`` ],Name="QLD");
 Chart.Line([ for row in seekJobs.Data -> DateTime.FromOADate(row.Month), row.`` SA `` ],Name="SA");
 Chart.Line([ for row in seekJobs.Data -> DateTime.FromOADate(row.Month), row.`` TAS`` ],Name="TAS",Color=System.Drawing.Color.LightGreen);
 Chart.Line([ for row in seekJobs.Data -> DateTime.FromOADate(row.Month), row.`` VIC`` ],Name="VIC",Color=System.Drawing.Color.Yellow);
 Chart.Line([ for row in seekJobs.Data -> DateTime.FromOADate(row.Month), row.`` WA `` ],Name="WA",Color=System.Drawing.Color.Green)]
|> Chart.Combine
|> Chart.WithTitle("SEEK New Jobs Ads Posted, by State, Seasonally Adjusted Data Index",InsideArea=false)
|> Chart.WithYAxis(Title="Posted Ads")
|> Chart.WithXAxis(Title="Period")
|> Chart.WithLegend(Enabled=true)



