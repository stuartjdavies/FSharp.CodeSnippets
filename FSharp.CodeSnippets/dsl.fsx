open System

type Propersition = | And | To
type Extra = | Egg | Bacon | Cheese | RegularChips | LargeChips | Coke
type BurgerType = | BigMac | CheeseBurger | QuarterPounder 
type Size = | Large | Medium | Small

type OrderItem() =
        member val Name = "" with get, set 

type Burger(burger) =
        inherit OrderItem()
        member val BurgerType  : BurgerType = burger 
        member val Extras : Extra list = [] with get, set

type IcecreamSunday() = 
        inherit OrderItem()

type Order() =
        member val Items : Extra list = [] with get, set

type X() =
    static member F([<ParamArray>] args: Object[]) =
        for arg in args do
            printfn "%A" arg

let myBurger = new Burger(BigMac)

//let ValueMeal(Large/Small/BurgerType = ()
//(X.F Coke myBurger)

//printfn
type Account =
    static member (<<-)(x: Account, name) = x.addName(name)

let acc1 = Account("acc-1", "David P.")
acc1 <<- "Mary R." <<- "Shawn P." <<- "John S."


let ValueMeal bt s d = new OrderItem()
 

// let Add e p (b : Burger) = b.Extras <- e::b.Extras
//let Add e p b = ()
let Add([<ParamArray>] args: Object[]) = () 


//Add("12",2)
//Add Coke To myBurger

/// from Chapter3/Account.Scala.fs


let myOrder = new Order()
Add To myOrder a Large BigMac With Coke