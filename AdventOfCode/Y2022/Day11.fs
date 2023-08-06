module Y2022.Day11

open Utils

type Operation = string

type Test =
    { Input: string
      IfTrue: string
      IfFalse: string }

let runMath one operator two : int64 =
    match operator with
    | "+" -> one + two
    | "*" -> one * two
    | _ -> 0

let handleOperation (old: int64) (operation: Operation) =
    match operation.Replace("old", old.ToString()) with
    | Regex "(\d+)\s+([*+])\s+(\d+)" [ Int64 one; operator; Int64 two ] -> runMath one operator two
    | _ -> 0

let getDivisibleBy test =
    match test with
    | Regex "divisible by (\d+)" [ Int64 value ] -> Some value
    | _ -> None

let handleTestInput (input: int64) test =
    match getDivisibleBy test with
    | Some value -> (input % value) = 0
    | None -> false

let getReceiverMonkey throwStatement =
    match throwStatement with
    | Regex "throw to monkey (\d+)" [ Int monkey ] -> monkey
    | s -> failwith $"invalid throw statement: {s}"

let handleTest test value =
    if (test.Input |> handleTestInput value) then
        test.IfTrue
    else
        test.IfFalse
    |> getReceiverMonkey

let parseItemList itemList =
    itemList |> trim |> split "," |> Seq.map trim |> Seq.map int64 |> List.ofSeq

type Monkey =
    { Items: int64 list
      Operation: Operation
      Test: Test
      InspectedItems: int64 }

let monkeyRegex =
    "Monkey 0:
  Starting items: ([\d,\s]+)
  Operation: new = ([\w\s+*]+)
  Test: ([\w\d]+)
    If true: ([\w\s]+)
    If false: ([\w\s]+)"

let _getMonkey (lines: string array) =
    let items = lines[1] |> split ":" |> Array.last |> parseItemList
    let operation = lines[2] |> split "new = " |> Array.last |> trim
    let test = lines[3] |> split ":" |> Array.last |> trim
    let ifTrue = lines[4] |> split ":" |> Array.last |> trim
    let ifFalse = lines[5] |> split ":" |> Array.last |> trim

    { Items = items
      Operation = operation
      Test =
        { Input = test
          IfTrue = ifTrue
          IfFalse = ifFalse }
      InspectedItems = 0 }

let getMonkey input =
    input |> trim |> split "\n" |> _getMonkey

let getMonkeys input =
    input |> split "\n\n" |> Array.map getMonkey |> List.ofArray

let getNewItemValue item monkey =
    monkey.Operation |> handleOperation item

let private _getModulo (monkeys: Monkey list) =
    monkeys
    |> Seq.map (fun m -> (getDivisibleBy m.Test.Input) |> Option.get)
    |> Seq.reduce (*)

let reliefBy3 item : int64 = item / 3L
let noRelief item : int64 = item

// the worry level gets too high for an int64 - we need to find a common divisor and use that as modulo
let reliefByModulo (monkeys: Monkey list) item = item % (monkeys |> _getModulo)


let inspectItem item (relief: int64 -> int64) monkey =
    let newItemValue = monkey |> getNewItemValue item |> relief
    let receiverMonkey = newItemValue |> handleTest monkey.Test
    (receiverMonkey, newItemValue)

let inspectItems relief monkey =
    monkey.Items
    |> Seq.map (fun item -> monkey |> inspectItem item relief)
    |> Seq.groupBy fst
    |> Seq.map (fun (key, seq) -> key, seq |> Seq.map snd)
    |> Map.ofSeq

let withItemsInspected monkey =
    { monkey with
        Items = List.empty
        InspectedItems = monkey.InspectedItems + int64 monkey.Items.Length }

let receiveItems index (throws: Map<int, int64 seq>) monkey =
    if throws |> Map.containsKey index then
        { monkey with
            Items = Seq.append monkey.Items throws[index] |> List.ofSeq }
    else
        monkey

let handleThrows (throws: Map<int, int64 seq>) (monkeys: Monkey list) =
    monkeys |> List.mapi (fun index monkey -> monkey |> receiveItems index throws)

let handleMonkey index relief (monkeys: Monkey list) =
    let throws = inspectItems relief monkeys[index]

    monkeys
    |> List.updateAt index (monkeys[index] |> withItemsInspected)
    |> handleThrows throws

let private round relief (monkeys: Monkey list) =
    [ 0 .. (monkeys.Length - 1) ]
    |> Seq.fold (fun monkeys index -> monkeys |> handleMonkey index relief) monkeys

let rounds rounds relief monkeys =
    [ 1..rounds ] |> Seq.fold (fun monkeys _ -> monkeys |> round relief) monkeys

let getMonkeyBusinessLevel monkeys =
    monkeys
    |> List.sortByDescending (fun m -> m.InspectedItems)
    |> Seq.take 2
    |> Seq.map (fun m -> m.InspectedItems)
    |> Seq.reduce (*)

let main () =
    let monkeys = readInputFile "11" |> getMonkeys
    monkeys |> rounds 20 reliefBy3 |> getMonkeyBusinessLevel |> printfn "%d"

    monkeys
    |> rounds 10000 (reliefByModulo monkeys)
    |> getMonkeyBusinessLevel
    |> printfn "%d"
