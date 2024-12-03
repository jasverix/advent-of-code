module Y2024.Day02

type Level = int
type Report = Level list

let parseInputLine line : Report =
    line |> Str.split " " |> List.map int

let parseInput input =
    input |> Str.trim |> Str.split "\n" |> List.map parseInputLine

let allIncreasing report  =
    report |> List.pairwise |> List.forall (fun (a, b) -> a <= b)
    
let allDecreasing report =
    report |> List.pairwise |> List.forall (fun (a, b) -> a >= b)
    
let noTooBigDifference report =
    report |> List.pairwise |> List.forall (fun (a, b) -> abs (b - a) <= 3 && abs (b - a) >= 1)
    
let reportIsSafe report =
    (allIncreasing report || allDecreasing report) && noTooBigDifference report
    
let withoutItem index list =
    list |> List.mapi (fun i item -> if i = index then None else Some item) |> List.choose id
    
let reportIsSafeWithDampener report =
    (report |> reportIsSafe) ||
    (report |> List.mapi (fun i _item -> withoutItem i report |> reportIsSafe) |> List.exists id)
    
let countSafeReports reports =
    reports |> List.countIf reportIsSafe
    
let countSafeReportsWithDampener reports =
    reports |> List.countIf reportIsSafeWithDampener

let main() =
    let reports = Utils.readInputFile "02" |> parseInput
    
    reports |> countSafeReports |> printfn "Safe reports: %d\n"
    reports |> countSafeReportsWithDampener |> printfn "Safe reports with dampener: %d\n"