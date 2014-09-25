// Reference the Excel interop assembly
#r "Microsoft.Office.Interop.Excel.dll"
open Microsoft.Office.Interop.Excel
open System

// Run Excel as a visible application
let app = new ApplicationClass(Visible = true) 

// Create new file and get the first worksheet
let workbook = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet) 
// Note that worksheets are indexed from one instead of zero
let worksheet = (workbook.Worksheets.[1] :?> Worksheet)

// Store data in arrays of strings or floats
let rnd = new Random()
let titles = [| "No"; "Maybe"; "Yes" |]
let names = Array2D.init 10 1 (fun i _ -> string('A' + char(i)))
let data = Array2D.init 10 3 (fun _ _ -> rnd.NextDouble())

// Populate data into Excel worksheet
worksheet.Range("C2", "E2").Value2 <- titles
worksheet.Range("B3", "B12").Value2 <- names
worksheet.Range("C3", "E12").Value2 <- data

// Add new item to the charts collection
let chartobjects = (worksheet.ChartObjects() :?> ChartObjects) 
let chartobject = chartobjects.Add(400.0, 20.0, 550.0, 350.0) 

// Configure the chart using the wizard
chartobject.Chart.ChartWizard
  ( Title = "Stacked column chart", 
    Source = worksheet.Range("B2", "E12"),
    Gallery = XlChartType.xl3DColumnStacked100, 
    PlotBy = XlRowCol.xlColumns)

// Set graphical style of the chart
chartobject.Chart.ChartStyle <- 2


