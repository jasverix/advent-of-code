module Y2022.Utils

open System
open System.Text.RegularExpressions

let readInputFile input =
    __SOURCE_DIRECTORY__ + $"/Inputs/input-{input}.txt"
    |> System.IO.File.ReadAllText

let trim (input: string) = input.Trim()
let split (splitBy: string) (input: string) = input.Split(splitBy)

let splitAt (splitBy: string) (input: string) : string * string =
    let index = input.IndexOf(splitBy)

    if index < 0 then
        (input, "")
    else
        (input[0 .. (index - 1)], input[(index + splitBy.Length) ..])

let regexMatch (regex: string) (input: string) =
    let capture = Regex.Match(input, regex)

    if capture.Success then
        seq {
            for index in 0 .. capture.Groups.Count do
                (index, capture.Groups[index].Value)
        }
        |> Map.ofSeq
    else
        Map.empty

let (|Regex|_|) pattern input = 
    let m = Regex.Match(input, pattern)
    if m.Success then Some(List.tail [ for g in m.Groups -> g.Value])
    else None

let (|Int|_|) (str:string) = match Int32.TryParse str with true, value -> Some value | _ -> None

