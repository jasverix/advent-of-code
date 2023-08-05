module Y2022.Day05

open Utils

type Crate = char
type Stack = Crate array
type Storage = Map<int, Stack>

let private getIndexIndex (indexLine: string) =
    indexLine
    |> Seq.mapi (fun i c -> (i, c))
    |> Seq.filter (fun (_, c) -> not (c = ' '))
    |> Seq.map (fun (index, stackIndexChar) -> (index, int (stackIndexChar.ToString())))

let private storageAndIndexLine storageInput =
    let lines = storageInput |> split "\n"
    (lines |> Array.take (lines.Length - 1), lines |> Array.last)

let private getStackLine (index: seq<int * int>) (line: string) =
    index
    |> Seq.map (fun (index, stackIndex) -> (stackIndex, line[index]))
    |> Map.ofSeq

let private getStackLines (lines, indexLine) =
    let indexIndex = indexLine |> getIndexIndex

    lines
    |> Array.rev
    |> Seq.map (getStackLine indexIndex)
    |> Seq.collect Map.toList
    |> Seq.groupBy fst
    |> Seq.map (fun (index, indexAndCrate) ->
        index, indexAndCrate |> Seq.map snd |> Seq.filter (fun crate -> not (crate = ' ')))
    |> Seq.map (fun (index, stack) -> (index, stack |> Array.ofSeq))
    |> Map.ofSeq

let getStorage storageInput : Storage =
    storageInput |> storageAndIndexLine |> getStackLines

let private handleTakeFromIndex index (storage: Storage) (popped, remaining) = (popped, storage.Add(index, remaining))

let private takeFromIndex index items (storage: Storage) : Stack * Storage =
    storage[index] |> pop items |> handleTakeFromIndex index storage

let crateMover9000 (items: Stack) = items |> Array.rev
let crateMover9001 (items: Stack) = items

let private insertIntoIndex index crateMover (items: Stack, storage: Storage) =
    let existing = storage[index]
    storage.Add(index, Array.append existing (items |> crateMover))

let moveItems items fromStack toStack crateMover (storage: Storage) : Storage =
    storage |> takeFromIndex fromStack items |> insertIntoIndex toStack crateMover

let handleMoveCommand command crateMover storage =
    match command with
    | Regex "move (\d+) from (\d+) to (\d+)" [ Int items; Int fromStack; Int toStack ] ->
        moveItems items fromStack toStack crateMover storage
    | s -> failwith $"unknown command {s}"

let handleMoveCommands commands crateMover storage =
    commands
    |> trim
    |> split "\n"
    |> Seq.fold (fun storage command -> storage |> handleMoveCommand command crateMover) storage

let private _getFinalStorage crateMover (storageInput, commandInput) =
    storageInput |> getStorage |> handleMoveCommands commandInput crateMover

let getFinalStorage crateMover input =
    input |> splitAt "\n\n" |> _getFinalStorage crateMover

let private _getUsedIndexes (storage: Storage) = storage |> Map.keys

let getTopCrate index (storage: Storage) = storage[index] |> Array.last

let getTopCrates storage =
    storage
    |> _getUsedIndexes
    |> Seq.map (fun index -> storage |> getTopCrate index)

let main () =
    let input = readInputFile "05"

    let printTopCrates topCrates =
        topCrates |> Seq.map (fun c -> c.ToString()) |> String.concat ""

    input
    |> getFinalStorage crateMover9000
    |> getTopCrates
    |> printTopCrates
    |> printfn "%s"

    input
    |> getFinalStorage crateMover9001
    |> getTopCrates
    |> printTopCrates
    |> printfn "%s"
