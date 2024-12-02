module Y2024.Day01

type LocationId = int
type ListPair = LocationId list * LocationId list

let nextDistance (lists: ListPair): LocationId * ListPair =
   let list1, list2 = lists
   let num1, list1 = list1 |> List.popItem
   let num2, list2 = list2 |> List.popItem
   ((num1 - num2) |> abs, (list1, list2))

let rec runNextDistance (acc: LocationId list) lists: int list =
    match lists with
    | ([], []) -> acc
    | _ -> 
        let distance, lists = lists |> nextDistance
        lists |> runNextDistance (distance::acc)
   
let sortLists (lists: ListPair): ListPair =
    let list1, list2 = lists
    (list1 |> List.sort, list2 |> List.sort)
    
let totalDistance (lists: ListPair): int =
    lists |> sortLists |> runNextDistance [] |> List.sum
    
let parseInput (input: string): ListPair =
    input |> Str.trim |> Str.split "\n"
        |> List.map (fun line ->
            let parts = line |> Str.split " " |> List.filter (fun i -> i <> "") |> List.map Str.trim |> List.map int
            parts.[0], parts.[1])
        |> List.unzip

let main() =
    Utils.readInputFile "01" |> parseInput |> totalDistance |> printfn "%d\n"