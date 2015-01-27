#r "Microsoft.Office.Interop.Excel.dll"
open Microsoft.Office.Interop.Excel
open System

open System
open System.ComponentModel

type TrainingPlanItem = {  
    Training : string; ShortDescription : string; TrainingProvider : string; Status : string; 
    Duration : int; StartDate : DateTime; FinishDate : DateTime; PaidFor : Boolean; 
    Cost : Double; Comments : String; }

let trainingPlan = 
    [{ TrainingPlanItem.Training="Visualise the City (Tableau workshop + government collaboration)"; ShortDescription="Tableau";
       Status="Completed"; TrainingProvider="Tableau"; Duration=1; StartDate=DateTime.Parse("Thursday, 16 October 2014"); 
       FinishDate=DateTime.Parse("Thursday, 16 October 2014"); PaidFor=true; Cost=0.00; Comments="" }]
//                    ("Azure Machine Learning Workshop","Azure Machine Learning","Completed","Readify",1,"Thursday, 9 October 2014","Thursday, 9 October 2014","Free","Free",None);	
//                    ("AWS Essentials","AWS Essentials","Completed", "Bespoke", 1, "Friday, 31 October 2014",	"Friday, 31 October 2014",	"Yes",Some(825.00));	
//                    ("70-487 Developing Windows Azure and Web Services", "70-487 Azure Web Services", "Completed","New Horizons", 5, "Monday, 17 November 2014", "Friday, 21 November 2014","Yes",3950.00)]


module TrainingPlan =
        let createExcel (items : TrainingPlanItem seq)  = ()
        let createWord (items : TrainingPlanItem seq) = ()
        let createHtml (items : TrainingPlanItem seq) = ()


