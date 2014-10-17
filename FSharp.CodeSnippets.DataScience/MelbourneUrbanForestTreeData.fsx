#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r @"FSharp.Data.TypeProviders.dll"

open Deedle

// let csvFile = """https://data.melbourne.vic.gov.au/api/views/fp38-wiyy/rows.csv?accessType=DOWNLOAD"""
let csvFile = """C:\Users\stuart\Downloads\Melbourne_s_Urban_Forest_Tree_data.csv"""
let mf = Frame.ReadCsv(csvFile)

printfn "Number of columns - %d" mf.ColumnCount
printfn "Number of rows - %d" mf.RowCount

// let mfByCommonName = mf.GroupRowsBy<string>("Common Name")

//mfByCommonName.AggregateRowsBy(function s =)



// Get 'Common Name' column and count per type
//let byClass =
//  melbourneUrbanForest.GetColumn<string>("Common Name")
//  |> Series.applyLevel fst (fun s ->
      // Get counts for 'True' and 'False' values of 'Survived'
//      series (Seq.countBy id s.Values))
  // Create frame with 'Pclass' as row and 'Died' & 'Survived' columns
//  |> Frame.ofRows 
//  |> Frame.sortRowsByKey
//  |> Frame.indexColsWith ["Died"; "Survived"]

// Add column with Total number of males/females on Titanic
//byClass?Total <- byClass?Died + byClass?Survived/

// Build a data frame with nice summary of rates in percents
//frame [ "Died (%)" => round (byClass?Died / byClass?Total * 100.0)
//        "Survived (%)" => round (byClass?Survived / byClass?Total * 100.0) ]


