module Y2024.Day03

type Memory = Map<string, int>

let _handleMul args mem : Memory=
    match args with
    | Utils.Regex "(\d+),(\d+)" [Utils.Int a; Utils.Int b] -> mem |> Map.add "return" (a * b)
    | _ -> mem
    
let handleMul args (mem: Memory) =
    match mem["do"] with
    | 1 -> _handleMul args mem
    | _ -> mem
    
let handleDo mem = mem |> Map.add "do" 1
let handleDoNot mem = mem |> Map.add "do" 0
    
let executeFunction fn args mem =
    match fn with
    | Str.EndsWith "mul" _ -> handleMul args mem
    | Str.EndsWith "do" _ -> handleDo mem
    | Str.EndsWith "don't" _ -> handleDoNot mem
    | _ -> mem
    
let storeResult mem =
    mem
    |> Map.add "sum" (mem["sum"] + mem["return"])
    |> Map.add "return" 0
    
let next fn args (mem: Memory) =
    mem
    |> executeFunction fn args
    |> storeResult
    
let initMemory() = Map.ofList ["sum", 0; "do", 1; "return", 0]

let getMultiplications input =
    input
    |> Utils.regexMatches "([\w']*)\(([\d,\s]*?)\)"
    |> fun(m) -> Seq.zip m[1] m[2]
    |> Seq.fold (fun mem (fn, args) -> next fn args mem) (initMemory())
    |> fun mem -> mem["sum"]
    
let main() =
    Utils.readInputFile "03" |> getMultiplications |> printfn "Sum: %A\n"