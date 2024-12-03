module Y2024.Day03

let getMultiplications input =
    input
    |> Utils.regexMatches "mul\((\d+),(\d+)\)"
    |> fun(m) -> Seq.zip m[1] m[2]
    |> Seq.map (fun(a, b) -> (int a, int b))
    |> Seq.map (fun(a, b) -> a * b)
    |> Seq.sum
    
let main() =
    Utils.readInputFile "03" |> getMultiplications |> printfn "Sum: %A\n"