//
// The following OO principles illustrated using a monopoly game. 
//
//  OO Design principles convered -
// - Encapsulation, Polymorhpism
// - Inheritance(Concrete, Abstract, Interface)
// - Anonymous 
// - Association, Agregation, Composition 
// - Sealed, 
// - Constructors
// - Static classes
// - Methods/Properties abstract, private, public, protected
// - Templated classes 
// - Design pattern - Factory Pattern
// - Design pattern - Proxy Pattern
// - Design pattern - Adaptor Pattern
// - Design pattern - Controller Pattern
open System

// Descrimating unions
type GenderType = Male | Female

type Piece =
        val Name : string 
        new (name) = { Name = name }
        new () = { Name = "" }
        
type Shoe =
        inherit Piece
        new (name) = { inherit Piece(name) } 
               
type Hat =
        inherit Piece
        new (name) = { inherit Piece(name) } 

type Cannon =  
        inherit Piece
        new (name) = { inherit Piece(name) } 
        
type House =
        inherit Piece
        new (name) = { inherit Piece(name) } 
        
type Hotel =
        inherit Piece
        new (name) = { inherit Piece(name) } 

type Player =
        val Name : string 
        val BoardPiece : Piece
        new (name, boardPiece) = { Name = name; BoardPiece = boardPiece}
         
type HumanPlayer =
        inherit Player
        val Gender : GenderType
        new (name, gender, piece) = { inherit Player(name, piece); Gender = gender } 
         
type ComputerPlayer =
        inherit Player
        val mutable Difficulty : Int32
        new (name, difficulty, piece) = { inherit Player(name, piece); Difficulty=difficulty }

type Board = 
        val mutable Pieces : Piece list
        
type Game = 
        val Players : Player list
        
// To be continued ...




