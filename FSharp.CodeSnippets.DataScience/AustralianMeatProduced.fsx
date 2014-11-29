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

let filePath = __SOURCE_DIRECTORY__ + @"\Data\RedMeatProduced.xls"

let redMeatProd = new ABSExcelSchemaProvider<"C:\Users\stuart\Documents\GitHub\FSharp.CodeSnippets\FSharp.CodeSnippets.DataScience\data\RedMeatProduced.xls">(filePath)

let totals = redMeatProd.Data |> Seq.map(fun r -> DateTime.Parse(r.Date), Int32.Parse(r.``Meat Produced ;  Total Red Meat ;  Total (State) ; - (Original)``))
 
let options =
    Options(
        title = "Total red meat produced in tonnes",
        curveType = "function")
            
let chart =
    [totals]
    |> Chart.Line
    |> Chart.WithOptions options
    |> Chart.WithLabels ["Produced"; "Year"]
    |> Chart.Show

let lastYear = redMeatProd.Data |> Seq.last

let lastYearPieChart =
    ["New South Wales", Int32.Parse(lastYear.``Meat Produced ;  Total Red Meat ;  New South Wales ; - (Original)``);
     "Queensland", Int32.Parse(lastYear.``Meat Produced ;  Total Red Meat ;  Queensland ; - (Original)``);
     "South Australia", Int32.Parse(lastYear.``Meat Produced ;  Total Red Meat ;  South Australia ; - (Original)``);
     "Western Australia", Int32.Parse(lastYear.``Meat Produced ;  Total Red Meat ;  Western Australia ; - (Original)``);
     "Australian Capital Territory", Int32.Parse(lastYear.``Meat Produced ;  Total Red Meat ;  Australian Capital Territory ; - (Original)``);
     "Tasmania", Int32.Parse(lastYear.``Meat Produced ;  Total Red Meat ;  Tasmania ; - (Original)``);
     "Victoria", Int32.Parse(lastYear.``Meat Produced ;  Total Red Meat ;  Victoria ; - (Original)``)] 
    |> Chart.Pie
    |> Chart.WithTitle (String.Format("Meat Production in {0} in tonnes ", lastYear.Date))
    |> Chart.WithLegend true
    |> Chart.Show

