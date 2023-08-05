module Y2022.Day02

open System.Text.RegularExpressions

type Shape = Rock | Scissors | Paper
type Outcome = Win | Lose | Draw

let scoreByShape shape =
    match shape with
    | Rock -> 1
    | Paper -> 2
    | Scissors -> 3
    
let scoreByOutcome outcome =
    match outcome with
    | Lose -> 0
    | Draw -> 3
    | Win -> 6
    
let getLoser shape =
    match shape with
    | Rock -> Scissors
    | Paper -> Rock
    | Scissors -> Paper

let getWinner shape =
    match shape with
    | Rock -> Paper
    | Paper -> Scissors
    | Scissors -> Rock

let getOutcome (opp, me): Outcome =
    let winner = opp |> getWinner
    if me = opp then Draw
    elif me = winner then Win
    else Lose
        
let getRoundScore (opp, me) =
    let outcome = getOutcome (opp, me)
    (me |> scoreByShape) + (outcome |> scoreByOutcome)

let opponentPlay char =
    match char with
    | 'A' -> Rock
    | 'B' -> Paper
    | 'C' -> Scissors
    | c -> failwith $"unknown option {c}"
    
let myPlay1 char =
    match char with
    | 'X' -> Rock
    | 'Y' -> Paper
    | 'Z' -> Scissors
    | c -> failwith $"unknown option {c}"

let lineToRound1 line =
    let capture = Regex.Match(line, "(\w)\s+(\w)")
    let opp = opponentPlay capture.Groups[1].ValueSpan[0]
    let me = myPlay1 capture.Groups[2].ValueSpan[0]
    
    (opp, me)
    
let getRounds1 (input: string) =
    input.Trim().Split("\n")
    |> Seq.map lineToRound1

let getTotalScore1 input =
    input
    |> getRounds1
    |> Seq.map getRoundScore
    |> Seq.reduce (+)

let myPlay2 opp char =
    match char with
    | 'X' -> opp |> getLoser
    | 'Y' -> opp
    | 'Z' -> opp |> getWinner
    | c -> failwith $"unknown option {c}"

let lineToRound2 line =
    let capture = Regex.Match(line, "(\w)\s+(\w)")
    let opp = opponentPlay capture.Groups[1].ValueSpan[0]
    let me = myPlay2 opp capture.Groups[2].ValueSpan[0]
    
    (opp, me)
    
let getRounds2 (input: string) =
    input
    |> Utils.trim
    |> Utils.split "\n"
    |> Seq.map lineToRound2

let getTotalScore2 input =
    input
    |> getRounds2
    |> Seq.map getRoundScore
    |> Seq.reduce (+)    

let main() =
    let input = Utils.readInputFile "02"
    
    input |> getTotalScore1 |> printfn "%d"
    input |> getTotalScore2 |> printfn "%d"