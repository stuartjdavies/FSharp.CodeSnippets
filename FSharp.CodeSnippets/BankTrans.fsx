#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r "Microsoft.Office.Interop.Excel.dll"

open FSharp.Data
open System
open Deedle
open FSharp.Charting
open Microsoft.Office.Interop.Excel

let transFileName = "testtrans.csv"

type BankTrans = { Date : DateTime; PaymentType : String; 
                   Description : String; Category : String; 
                   Debit : Decimal; Credit : Decimal; 
                   Balance : Decimal; }

module ImportBankTrans = 
         type Transactions = CsvProvider<".\\testtrans.csv">

         let GetPaymentType(s : string) = 
                if (s.Length > 30) then
                  s.Substring(0, 30).Trim()
                else
                  String.Empty  

         let GetDesc(s : string) =
                if (s.Length > 30) then         
                  s.Substring(s.IndexOf(" ", 30)).Trim()
                else
                  s         
         
         let MapCategory(t : Transactions.Row) =
                if (t.Description.Contains("Woolworths")) then
                    "Supermarket"
                else if (t.Description.Contains("Carshare")) then
                    "Carshare"
                else if (t.Description.Contains("Big W")) then
                    "Big W"
                else if (t.Description.Contains("Myki")) then
                    "Myki"                         
                else if (t.Description.Contains("Primus")) then
                    "Primus"
                else if (t.Description.Contains("Hcf")) then
                    "Hcf"
                else if (t.Description.Contains("Aldi")) then
                    "Aldi"
                else if (t.Description.Contains("Rent")) then                        
                    "Rent"
                else
                    GetDesc(t.Description)
       
         let ImportCsv(fileName : string) = 
                let rawTs = Transactions.Load(fileName)

                rawTs.Rows 
                |> Seq.filter(fun t -> t.Date <> "")                       
                |> Seq.map(fun t ->                                
                    { Date = DateTime.Parse(t.Date);
                      PaymentType = GetPaymentType(t.Description);  
                      Description = GetDesc(t.Description);                                 
                      Category = MapCategory(t);
                      Debit = t.Debit;
                      Credit = t.Credit;
                      Balance = t.Balance }) 
                |> Seq.sortBy(fun t -> t.Date)                      
              
module TransStats = 
        let private GetWeek(d : DateTime) =
             let day = (int d.DayOfWeek)
             String.Format("{0} - {1}", 
                           d.AddDays((float (1 - day))).ToShortDateString(), 
                           d.AddDays((float (7 - day))).ToShortDateString())

        let debits(ts : seq<BankTrans>) =
            ts |> Seq.filter(fun t -> t.Debit <> 0.0m)                               
        
        let credits(ts : seq<BankTrans>) =
            ts |> Seq.filter(fun t -> t.Credit <> 0.0m)        
        
        let maxDebit(ts : seq<BankTrans>) = 
            ts |> Seq.maxBy(fun t -> t.Debit)
         
        let minDebit(ts : seq<BankTrans>) = 
            ts |> debits
               |> Seq.minBy(fun t -> t.Debit)
         
        let avgDebit(ts : seq<BankTrans>) =            
            ts |> debits
               |> Seq.averageBy(fun t -> t.Debit)        
        
        let maxCredit(ts : seq<BankTrans>) = 
            ts |> credits
               |> Seq.maxBy(fun t -> t.Credit)
         
        let minCredit(ts : seq<BankTrans>) = 
            ts |> Seq.minBy(fun t -> t.Credit)

        let avgCredit(ts : seq<BankTrans>) = 
            ts |> credits
               |> Seq.averageBy(fun t -> t.Credit)        
                        
        let count(ts : seq<BankTrans>) = 
            ts |> Seq.length
            
        let minBalance(ts : seq<BankTrans>) =
            ts |> Seq.minBy(fun t -> t.Balance)

        let maxBalance(ts : seq<BankTrans>) =
            ts |> Seq.maxBy(fun t -> t.Balance)

        let totalCredits(ts : seq<BankTrans>) = 
            ts |> Seq.sumBy(fun t -> t.Credit)

        let totalDebits(ts : seq<BankTrans>) = 
            ts |> Seq.sumBy(fun t -> t.Debit)

        let totalDiff(ts : seq<BankTrans>) = 
            totalCredits(ts) - totalCredits(ts)
        
        let debitTotalsByCategory(ts : seq<BankTrans>, 
                                  category : string) =                         
            ts |> Seq.groupBy(fun t -> t.Category)
               |> Seq.map(fun (d, ts) -> 
                   (d,(ts |> Seq.sumBy(fun t -> t.Debit))))

        let creditTotalsByCategory(ts : seq<BankTrans>) =
            ts |> Seq.groupBy(fun t -> t.Category)
               |> Seq.map(fun (d, ts) ->
                              (d,(ts |> Seq.sumBy(fun t -> t.Credit))))

        let dailyDebits(ts : seq<BankTrans>) =
            ts |> Seq.groupBy(
                    fun t -> t.Date.ToShortDateString())
               |> Seq.map(fun (d, ts) ->
                    (d,(ts |> Seq.sumBy(fun t -> t.Debit))))

        let dailyCredits(ts : seq<BankTrans>) =
                ts |> Seq.groupBy(
                        fun t -> (t.Date.Day, t.Date.Month, t.Date.Year))
                   |> Seq.map(
                        fun (d, ts) ->
                            (d,(ts |> Seq.sumBy(fun t -> t.Credit))))

        let monthlyDebits(ts : seq<BankTrans>) = 
                ts |> Seq.groupBy(
                        fun t -> t.Date.ToShortDateString())                                                       
                   |> Seq.map(
                        fun (d, ts) ->
                            (d,(ts |> Seq.sumBy(fun t -> t.Debit))))       

        let monthlyCredits(ts : seq<BankTrans>) = 
                ts |> Seq.groupBy(
                        fun t -> (t.Date.Month, t.Date.Year))                                                       
                   |> Seq.map(
                        fun (d, ts) ->
                            (d,(ts |> Seq.sumBy(fun t -> t.Credit))))       

        let weeklyDebitTotals (ts : seq<BankTrans>) =
                ts |> Seq.groupBy(
                         fun t -> GetWeek(t.Date))
                   |> Seq.map(fun (d, ts) ->
                         (d,(ts |> Seq.sumBy(fun t -> t.Debit))))

        let weeklyCreditTotals(ts : seq<BankTrans>) = 
                ts |> Seq.groupBy(
                         fun t -> GetWeek(t.Date))
                   |> Seq.map(fun (d, ts) ->
                         (d,(ts |> Seq.sumBy(fun t -> t.Credit))))

        let debitsByCategory(ts : seq<BankTrans>) = 
                ts |> Seq.filter(
                         fun t -> t.Debit <> 0.0m)
                   |> Seq.groupBy(
                         fun t -> t.Category)
                   |> Seq.map(
                         fun (d, ts) ->
                         (d,(ts |> Seq.sumBy(fun t -> t.Debit))))                           

        let creditsByCategory(ts : seq<BankTrans>) = 
                ts |> Seq.filter(
                         fun t -> t.Credit <> 0.0m)
                   |> Seq.groupBy(
                         fun t -> t.Category)
                   |> Seq.map(
                         fun (d, ts) ->
                            (d,(ts |> Seq.sumBy(fun t -> t.Credit))))

        let first(ts : seq<BankTrans>) =
                (ts |> Seq.sort |> Seq.head)
        
        let last(ts : seq<BankTrans>) =
                (ts |> Seq.sort |> Seq.last)

        let summary(ts : seq<BankTrans>) =                       
                [["Number of transactions" :> obj; (count(ts) :> obj)];                          
                 ["Start date"; (first(ts).Date.ToShortDateString() :> obj)];
                 ["Finish date"; (last(ts).Date.ToShortDateString())];
                 ["Starting balance"; (first(ts).Balance)];
                 ["Ending balance"; (last(ts).Balance :> obj)];                           
                 ["Number of Debits"; (ts |> debits |> count)];           
                 ["Max Debit"; (ts |> maxDebit).Debit];
                 ["Min Debit"; (ts |> minDebit).Debit];
                 ["Avg Debit"; (ts |> avgDebit)];
                 ["Total Debits"; (totalDebits(ts))]; 
                 ["Number of Credits"; (ts |> credits |> count)];
                 ["Max Credit"; (ts |> maxCredit).Credit]; 
                 ["Min Credit"; (ts |> minCredit).Credit];
                 ["Avg Credit"; (ts |> avgCredit)];
                 ["Total Credits"; (totalCredits(ts))];
                 ["Net(Credits - Debits)"; (ts |> totalDiff)]];        
                          
        let printSummary(ts : seq<BankTrans>) =
                let c = count(ts)
                printfn "Number of transactions %i" (count(ts))                          
                printfn "Start date - %s" (first(ts).Date.ToShortDateString())
                printfn "Finish date - %s" (last(ts).Date.ToShortDateString())
                printfn "Starting balance - %f" (first(ts).Balance)
                printfn "Ending balance - %f" (last(ts).Balance)            
               
                printfn "Number of Debits %d" (ts |> debits |> count)           
                printfn "Max Debit - $%0.0f" (ts |> maxDebit).Debit
                printfn "Min Debit - $%0.0f" (ts |> minDebit).Debit
                printfn "Avg Debit - $%0.0f" (ts |> avgDebit)
                printfn "Total Debits - %f" (totalDebits(ts)) 

                printfn "Number of Credits %d" (ts |> credits |> count)
                printfn "Max Credit - $%0.0f" (ts |> maxCredit).Credit 
                printfn "Min Credit - $%0.0f" (ts |> minCredit).Credit
                printfn "Avg Credit - $%0.0f" (ts |> avgCredit)
                printfn "Total Credits - %f" (totalCredits(ts)) 

                printfn "Net(Credits - Debits) - $%0.0f" (ts |> totalDiff)
                printfn "Trans count - %i" (count(ts))

        let PrintTopDebitsByCat(n, ts : seq<BankTrans>) =                
                ts |> debitsByCategory                                                                   
                   |> Seq.sortBy(fun (c,t) -> -t)
                   |> Seq.take n                     
                   |> Seq.iter(fun (c,t) -> 
                              printfn "%s - $%0.0f" c t
                              ())

        let PrintTopCreditsByCat(n, ts : seq<BankTrans>) =                
                ts |> creditsByCategory                                                                   
                   |> Seq.sortBy(fun (c,t) -> -t)
                   |> Seq.take n                     
                   |> Seq.iter(fun (c,t) -> 
                                   printfn "%s - $%0.0f" c t
                                   ())                        

module ExpensesReport =
    let CreateReport(ts : seq<BankTrans>) =
           let app = new ApplicationClass(Visible = true) 
           let workbook = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet)                       
           workbook.Worksheets.Add() |> ignore
           workbook.Worksheets.Add() |> ignore
           workbook.Worksheets.Add() |> ignore
           workbook.Worksheets.Add() |> ignore
           
           let summaryWorksheet = (workbook.Worksheets.[1] :?> Worksheet)            
           summaryWorksheet.Name <- "Summary"         
           let weeklySummaryWorksheet = (workbook.Worksheets.[2] :?> Worksheet)
           weeklySummaryWorksheet.Name <- "Weekly Summary"
           let monthlySummaryWorksheet = (workbook.Worksheets.[3] :?> Worksheet)
           monthlySummaryWorksheet.Name <- "Monthly Summary"           
           let transWorksheet = (workbook.Worksheets.[4] :?> Worksheet)
           transWorksheet.Name <- "Transactions"         

           let titles = [| "Date";"PaymentType"; 
                           "Description";"Category"; 
                           "Debit"; "Credit"; 
                           "Balance" |]                                   
           transWorksheet.Range("A1", "F1").Value2 <- titles                                        
           
           let data = ts |> Seq.map(fun t -> 
                                [(t.Date.ToString() :> obj);
                                 (t.PaymentType.ToString() :> obj);
                                 (t.Category :> obj);
                                 (t.Credit :> obj);
                                 (t.Debit :> obj);
                                 (t.Balance :> obj);])
                         |> array2D           
           
           let numberOfTrans = Seq.length(ts)       
                                                                                                                       
           transWorksheet.Range("A2", String.Format("F{0}", 
                                 Seq.length(ts)))
                         .Value2 <- data 
         
           let summaryData = ts |> TransStats.summary
                                |> array2D
                
           let kpvs = ts |> TransStats.summary |> Seq.length

           summaryWorksheet.Range("A1","A1").Value2 <- "Summary"           
           summaryWorksheet.Range("A3", 
                                  String.Format("B{0}", 
                                           kpvs))
                           .Value2 <- summaryData
           
           // Weekly           
           //let ts |> TransStats.weeklyDebitTotals
           //       |> array2D
           //ts |> TransStats.weeklyCreditTotals
           //   |> array2D

           //weeklySummaryWorksheet.Range("A2", )
           
           // Monthly
           //monthlySummaryWorksheet




           //["Total"; ""; "", "=SUM(A1:"; "=SI< ] 

           //worksheet.Range()

           // 2. Write Transactions By Date 
           // 3. How much was spent each week
           // 4. How much was spent per month
           // 5. Write Transactions By Debit.
           // 6. Order Transactions By Credits           
           // 9. Write Bar Graph of top 10 Debits
           // 10. Write Bar Graph of top 10 Credits
           // 11. 
                                                              
let trans = ImportBankTrans.ImportCsv(transFileName)

ExpensesReport.CreateReport(trans)

trans |> TransStats.debitsByCategory      
      |> Seq.sortBy (fun (d,t) -> -t)
      |> Seq.take 6
      |> Chart.Pie

trans |> TransStats.monthlyDebits
      |> Chart.Bar

trans |> TransStats.summary

           

// Load "Asde"
// Display debit transactions greater_than 10
// Display debit transactions greater_than 20
// Create piechart where transactions greater_than 30
// Display_top 5 debits
// Display_last 3 debits
// Display_summary"
// 


//trans |> PrintTransBy fun t ->

//1. Debits
//2. Credits
//3. PrintBy
//4. PrintGreaterThan trans
//5. PrintLessThan 
//6. PrintPieGraph 
//7. PrintLineGraph

// 2. Pie char of debits
// 4. Pie chart of credits
// 6. Line graph
// 8. Line graph
// 9. Payment type count
// 10. Payment type pie chart


//for (desc,d,c) in catTotals do
   
  // printfn "%s %f %f" desc d c
                        

