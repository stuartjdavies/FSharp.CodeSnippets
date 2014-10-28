#load "../packages/FsLab.0.0.19/FsLab.fsx"
#r "..\packages\iTextSharp.5.5.3\lib\itextsharp.dll"

open System
open System.IO; 
open iTextSharp.text;
open iTextSharp.text.pdf;
open iTextSharp.text.pdf.parser
open System.Text
open System.Globalization
open FSharp.Charting

type MikiTransaction = { Date : DateTime; TransactionType : string; Service : string; Zone : String; 
                         Description : string; Credit : Single; Debit : Single; MikiBalance : Single; }

let ParseAmountField(rawString : string) = let passed, amt = Single.TryParse(s=(rawString.Replace("$", "").Trim()))
                                           if (passed) then
                                             amt
                                           else
                                             Single.NaN

let reader = new PdfReader("""C:\Users\stuart\Downloads\myki.pdf""")

let trans = seq { 1 .. reader.NumberOfPages } 
            |> (Seq.fold (fun acc pg -> let s = PdfTextExtractor.GetTextFromPage(reader, pg, new SimpleTextExtractionStrategy())                                                                                                           
                                        acc + Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)))) "")                                                                       
            |> (fun d -> d.Split([| "\r\n"; "\n" |], StringSplitOptions.None))   
            |> Seq.filter(fun ln -> ln.Length > 10 && ln.Contains("""/"""))         
            |> Seq.map(fun (ln : string) -> let tokens = ln.Split(' ')
                                            let description = seq { for i in 6 .. (tokens.Length - 4) do yield i } 
                                                              |> Seq.toList 
                                                              |> List.rev 
                                                              |> (Seq.fold(fun acc t -> String.Format("{0} {1}", (tokens.[t]), acc)) "")                                                                                              
                                            { 
                                                MikiTransaction.Date = DateTime.ParseExact(tokens.[0], """dd/MM/yyyy""", System.Globalization.CultureInfo.InvariantCulture)                                             
                                                TransactionType = String.Format("{0} {1}", tokens.[2], tokens.[3]); 
                                                Zone = tokens.[5]; Service = tokens.[4]; Description = description.Trim(); 
                                                Credit = ParseAmountField(tokens.[tokens.Length - 3]);                                        
                                                Debit = ParseAmountField(tokens.[tokens.Length - 2]); 
                                                MikiBalance = ParseAmountField(tokens.[tokens.Length - 1])
                                            })
            |> Seq.sortBy(fun t -> t.Date)

reader.Close()

// Display Transactions 
query { for t in trans do 
        sortBy(t.Date)
        select(t) 
} |> Seq.iter(fun t -> printfn "Date - %s, Description - %s Debit - %0.2f" 
                                               (t.Date.ToString("""dd/MM/yyyy""")) t.Description t.Debit)

let debits = trans |> Seq.filter(fun t -> not (Single.IsNaN t.Debit))

// Money Spent per month
query { for d in debits do         
        groupBy(String.Format("{0}/{1}", d.Date.Month, d.Date.Year)) into g
        let total = query { for debit in g do
                            sumBy debit.Debit }
        select(g.Key, total) 
} |> fun ds -> Chart.Bar(ds, Title="Amount spent each month on myki", XTitle="Month", YTitle="($) Dollars")
          
// Debits line graph
debits |> Seq.map(fun t -> (t.Date, t.Debit)) |> Chart.Line

// Top 10 Debits
debits |> Seq.sortBy(fun t -> -t.Debit)       
       |> Seq.take 10       
       |> Seq.iteri(fun i t -> printfn "%d. Date - %s, Description - %s Debit - %0.2f" (i + 1) (t.Date.ToString()) t.Description t.Debit)



// Top 10 Daily amount 
debits |> Seq.groupBy(fun t -> t.Date)
       |> Seq.map(fun (dt,ts) -> let total = ts |> Seq.sumBy(fun t -> t.Debit)
                                 let desc = ts |> Seq.fold (fun acc t -> if (acc = "") then 
                                                                           t.Description
                                                                         else
                                                                           t.Description  + ", " + acc  ) ""
                                 (dt, total, ts, desc.Trim()))
       |> Seq.sortBy(fun (dt,total,ts, desc) -> -total)       
       |> Seq.take 10       
       |> Seq.iteri(fun i (dt,total,ts, desc) -> printfn "%d. - Date - %s, Total - %0.2f, Places Debited - %s" (i + 1) (dt.ToString()) total desc)

// Daily amount graph
debits |> Seq.groupBy(fun t -> t.Date)
       |> Seq.map(fun (dt,ts) -> let total = ts |> Seq.sumBy(fun t -> t.Debit)
                                 let desc = ts |> Seq.fold (fun acc t -> if (acc = "") then 
                                                                           t.Description
                                                                         else
                                                                           t.Description  + ", " + acc  ) ""
                                 (dt, total, ts, desc.Trim()))
       |> Seq.map(fun (dt, total, ts, desc) -> (dt, total))
       |> Chart.FastLine

// Select all zones twos
query {
    for t in trans do              
    where(t.Zone.Contains("2") || t.Zone.Contains("3"))    
    select(t) 
} |> Seq.iter(fun t -> printfn "Date %s, Trans Type %s, Zone %s, %s, Debit - %.2f" t.Date t.Zone t.TransactionType t.Description t.Debit)