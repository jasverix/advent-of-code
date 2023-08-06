module Y2022.Day10

open Utils

type Instruction = List<int -> int>
type CPU = { X: int }
type Cycle = CPU * CPU

let noop () : Instruction = [ fun X -> X ]

let addx V : Instruction =
    [ fun X -> X
      fun X -> (X + V) ]

let readCommand command =
    match command with
    | "noop" -> noop ()
    | Regex "addx (-?\d+)" [ Int V ] -> addx V
    | _ -> []

let run instruction (cpu: CPU) : Cycle list =
    instruction
    |> Seq.scan (fun (_, cpu) cycle -> (cpu, { X = cycle cpu.X })) (cpu, cpu)
    |> List.ofSeq

let readProgram program : Instruction =
    program |> trim |> split "\n" |> Seq.map readCommand |> List.concat

let createCPU () = { X = 1 }

let getCycle cycle (cycles: Cycle list) = cycles[cycle]
let getXDuring (cpu, _) = cpu.X
let getXAfter (_, cpu) = cpu.X
let getFinalX (cycles: Cycle list) = cycles |> List.last |> getXAfter

let getCycleStrength cycle cycles =
    (cycles |> getCycle cycle |> getXDuring) * cycle

let getSignificantSignalSum (cycles: Cycle list) =
    seq {
        let mutable cycle = 20

        while cycle < (cycles |> List.length) do
            yield cycles |> getCycleStrength cycle
            cycle <- (cycle + 40)
    }
    |> Seq.sum

let spriteIsAtPosition position sprite = position |> Day09.isCloseTo sprite

let spriteIsAtPositionForCycle cycle cycles colIndex =
    cycles |> getCycle cycle |> getXDuring |> spriteIsAtPosition colIndex

let getCycleChar cycle cycles colIndex =
    if spriteIsAtPositionForCycle cycle cycles colIndex then '█' else ' '
    
let getCRTRow cycles rowIndex =
    [1..40] |> Seq.map(fun colIndex -> getCycleChar ((rowIndex * 40) + colIndex) cycles (colIndex - 1)) |> Str.ofSeq

let getCRTString (cycles: Cycle list) =
    [0..5] |> List.map(getCRTRow cycles) |> String.concat "\n"

let main () =
    let program = readInputFile "10" |> readProgram
    let cpu = createCPU ()
    let cycles = cpu |> run program

    cycles |> getSignificantSignalSum |> printfn "%d"
    
    cycles |> getCRTString |> printfn "\n%s"
