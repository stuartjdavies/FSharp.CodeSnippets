#r "..\packages\FSharpx.TypeProviders.Xaml.1.8.41\lib\40\FSharpx.TypeProviders.Xaml.dll"
#r "WindowsBase"
#r "PresentationCore"
#r "PresentationFramework"
#r "System.Xaml"

open System
open FSharpx
open System.Windows.Controls

type MainWindow = XAML<"DateDiffAppMain.xaml">
let mainwnd = new MainWindow()
let w = mainwnd.Root
w.Show()

let startDate = w.FindName("dtStartDate") :?> DatePicker
let endDate = w.FindName("dtEndDate") :?> DatePicker
let lbDiff = w.FindName("lbDiff") :?> Label
let btGetDiff = w.FindName("btGetDiff") :?> Button

do btGetDiff.Click.Add(fun _ -> if (startDate.SelectedDate.HasValue && endDate.SelectedDate.HasValue) then
                                  lbDiff.Content <- String.Format("The dates have a difference of {0} days", 
                                                        endDate.SelectedDate.Value.Subtract(startDate.SelectedDate.Value).Days)
                                else
                                  lbDiff.Content <- "Invalid date")