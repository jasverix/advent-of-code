module Y2024.Str

let ofSeq (chars: char seq) = chars |> System.String.Concat

let substring (start: int) (length: int) (input: string) =
    if length < 0 then
        input.Substring(start, (input.Length - 1) + length)
    else
        input.Substring(start, length)

let trim (input: string) = input.Trim()
let split (splitBy: string) (input: string) = input.Split(splitBy) |> Array.toList

let (|EndsWith|_|) (endsWith: string) (str: string) =
    match str.EndsWith(endsWith) with
    | true -> Some str
    | _ -> None