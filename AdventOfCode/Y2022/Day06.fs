module Y2022.Day06

let charsAreDifferent (input: string) =
    input.Length = (Set<char>(input) |> Seq.length)

let getLastXChars amount (input: string) : string =
    if amount <= input.Length then
        input[(input.Length - amount) ..]
    else
        input

let lastXCharsAreDifferent amount input =
    input |> getLastXChars amount |> charsAreDifferent

let iterateString (input: string) =
    seq {
        let mutable acc = ""
        for c in input do
            acc <- acc + c.ToString()
            yield acc
    }

let findMarker requiredDiff input =
    input |> iterateString
    |> Seq.filter(fun s -> s.Length > requiredDiff)
    |> Seq.find(lastXCharsAreDifferent requiredDiff)
    |> String.length
    
let main() =
    let input = Utils.readInputFile "06"
    
    input |> findMarker 4 |> printfn "%d"
    input |> findMarker 14 |> printfn "%d"