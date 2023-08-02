module Y2022.Utils

let readInputFile input = __SOURCE_DIRECTORY__ + $"/Inputs/input-{input}.txt" |> System.IO.File.ReadAllText

let trim (input: string) = input.Trim()
let split (splitBy: string) (input: string) = input.Split(splitBy) 

