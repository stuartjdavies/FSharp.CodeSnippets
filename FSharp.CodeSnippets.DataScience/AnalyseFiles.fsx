#load "../packages/FsLab.0.0.19/FsLab.fsx"

open System.IO
open System.Drawing
open System.Windows.Forms
open System.Linq

let startPath="""C:\Users\stuart\Documents\GitHub\"""

let rec GetDirectories(path) =
         seq {                
                let ds = Directory.GetDirectories(path) 
                
                let subDs = ds |> Seq.map(fun d -> GetDirectories(d)) 
                               |> Seq.concat
                                                                   
                yield! Seq.append ds subDs
         }               
                          
let filterOut = [|"\\packages"; "\\lib"; "\\bin"; "\\obj"; "\.git"; "\.nuget";
                  "\\Properties";"\\data";"\\Microsoft.WindowsAzure.Caching"|]

let files = GetDirectories(startPath)  |> Seq.filter(fun d -> (filterOut |> Seq.exists(fun s -> d.Contains(s) ) = false))
                                       |> Seq.map(fun d -> Directory.GetFiles(d))
                                       |> Seq.concat
                                       |> Seq.filter(fun f -> f.Contains(".fsx"))                  
                                       |> Seq.map(fun f -> FileInfo(f))                                    
                                                                                          
let form = new Form(Visible = true, Text =  "Files",
                    TopMost = true, Size = Size(1200,800))

open FSharp
open FSharp.Charting

Chart.Bar([("A",1);("B",2);("C",3)]) 
 
let data = new DataGridView(Dock = DockStyle.Fill,Font = new Font("Lucida Console",12.0f),ForeColor = Color.DarkBlue)

form.Controls.Add(data)
data.DataSource <-  query {
                        for f in files do
                        sortByDescending f.LastAccessTime
                        select(f.Name, f.LastAccessTime, f.Directory)
                    } |> Seq.toArray

data.Columns.[0].HeaderText <- "Name"
data.Columns.[1].HeaderText <- "Last Accessed"
data.Columns.[2].HeaderText <- "Directory"

data.Columns.[0].Width <- 400
data.Columns.[1].Width <- 200
data.Columns.[2].Width <- 1000


                                            