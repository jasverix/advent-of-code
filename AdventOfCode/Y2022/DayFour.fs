module Y2022.DayFour

type Elf = Set<int>
type AssignmentPair = Elf * Elf

let toRange sections : Elf =
    let capture = sections |> Utils.regexMatch "(\d+)-(\d+)"
    let fromValue = int capture[1]
    let toValue = int capture[2]
    Set<int>([ fromValue..toValue ])

let toAssignmentPair input : AssignmentPair =
    let substrings = input |> Utils.trim |> Utils.split ","

    match substrings with
    | [| first; second |] -> (first |> toRange, second |> toRange)
    | _ -> failwith "invalid input format"

let isTotallyOverlapping (elf1, elf2) =
    let intersect = Set.intersect elf1 elf2
    (elf1 |> Set.isSuperset intersect) || (elf2 |> Set.isSuperset intersect)

let isIntersecting (elf1, elf2) =
    not (Set.intersect elf1 elf2 |> Set.isEmpty)

let toAssignmentPairs input =
    input
    |> Utils.trim
    |> Utils.split("\n")
    |> Seq.map toAssignmentPair

let countTotallyOverlapping pairs =
    pairs
    |> Seq.filter(fun p -> p |> isTotallyOverlapping)
    |> Seq.length
    
let countIntersecting pairs =
    pairs
    |> Seq.filter(fun p -> p |> isIntersecting)
    |> Seq.length
    
let main() =
    let input = Utils.readInputFile "04"
    let assignmentPairs = input |> toAssignmentPairs
    
    assignmentPairs |> countTotallyOverlapping |> printfn "%d"
    assignmentPairs |> countIntersecting |> printfn "%d"