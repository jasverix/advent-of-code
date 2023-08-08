module Y2022.Str

let ofSeq (chars: char seq) = chars |> System.String.Concat

let substring (start: int) (length: int) (input: string) =
    if length < 0 then
        input.Substring(start, (input.Length - 1) + length)
    else
        input.Substring(start, length)
