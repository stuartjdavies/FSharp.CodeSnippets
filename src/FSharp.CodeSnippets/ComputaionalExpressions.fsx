type Result<'A> = | Success of 'A 
                  | ErrorMessage of string
                  | Exception of System.Exception
                  | ErrorCode of int

type AttemptBuilder() =
        member this.Bind(x, f) = 
                    match(x) with
                    | Success x -> f(x) 
                    | ErrorMessage x -> ErrorMessage x 
                    | Exception x -> Exception x 
                    | ErrorCode x -> ErrorCode x
        member this.Delay(f) = f()
        member this.Return(x) = Success x

let attempt = new AttemptBuilder()

let Operation1() = Success("Operation 1 Success")
let Operation2() = Success("Operation 2 Success")
let Operation3() = ErrorMessage("Operation 3 Failure")
let Operation4() = Exception(new System.Exception("TestException"))

// Success Attempt
let result1 = attempt {
                let! r1 = Operation1()
                let! r2 = Operation2() 

                return r2   
              }

// Failure Attempt
let result2 = attempt {
                let! r1 = Operation1()
                let! r2 = Operation2() 
                let! r3 = Operation3() 

                return r1   
              }

let PrintResult result = match(result) with
                         | Success r -> printfn "Success value - %s" r
                         | ErrorMessage r -> printfn "Failure value - %s" r
                         | Exception r -> printfn "Exception failure - %s" (r.Message)
                         | ErrorCode r -> printfn "Exception failure - %d" r

PrintResult result1
PrintResult result2