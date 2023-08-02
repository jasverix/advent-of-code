module Y2022.DayOne

let toElf (input: string) =
    input.Trim().Split("\n") |> Seq.map int |> Seq.sum

let getMostCarry limit (input: string) =
    input.Trim().Split("\n\n")
    |> Seq.map toElf
    |> Seq.sortDescending
    |> Seq.take limit
    |> Seq.sum

let main () =
    let input = Utils.readInputFile "01"

    input |> getMostCarry 1 |> printfn "%d\n"
    input |> getMostCarry 3 |> printfn "%d\n"
