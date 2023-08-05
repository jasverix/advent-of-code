module Y2022.Day09

open Utils

type Position = int * int
type Snake = List<Position>

let private _isCloseTo i1 i2 =
    i1 = i2 || i1 = (i2 - 1) || i1 = (i2 + 1)

let isCloseTo i1 i2 = _isCloseTo i1 i2 || _isCloseTo i2 i1

let isTouching (x1, y1) (x2, y2) =
    (x1 |> isCloseTo x2) && (y1 |> isCloseTo y2)

let edgeCloser (x1, y1) (x2, y2) : Position =
    if (x1, y1) |> isTouching (x2, y2) then
        (x2, y2)
    elif x1 = x2 then
        if y2 < y1 then (x2, y2 + 1) else (x2, y2 - 1)
    elif y1 = y2 then
        if x2 < x1 then (x2 + 1, y2) else (x2 - 1, y2)
    else
        ((if x2 < x1 then x2 + 1 else x2 - 1), (if y2 < y1 then y2 + 1 else y2 - 1))

let getHead (snake: Snake) = snake[0]
let getTail (snake: Snake) = snake |> List.last

let _moveSnake (snake: Snake) =
    seq {
        let mutable last = snake[0]
        yield last
        for index in 1 .. (snake.Length - 1) do
            let newPos = snake[index] |> edgeCloser last
            yield newPos
            last <- newPos
    }

type Dir =
    | R
    | U
    | L
    | D

let stepPosition dir (x, y) =
    match dir with
    | R -> (x + 1, y)
    | U -> (x, y + 1)
    | L -> (x - 1, y)
    | D -> (x, y - 1)

let move dir (snake: Snake) : Snake =
    snake
    |> List.updateAt 0 (snake[0] |> stepPosition dir)
    |> _moveSnake
    |> List.ofSeq

let toDir input =
    match input with
    | "R" -> R
    | "U" -> U
    | "L" -> L
    | "D" -> D
    | _ -> failwith "bad input"

let readCommand command =
    match command with
    | Regex "(\w) (\d+)" [ dirInput; Int steps ] -> (dirInput |> toDir, steps)
    | c -> failwith $"bad command {c}"

let unpackCommand (dir: Dir, steps: int) =
    seq {
        for _ in 1..steps do
            yield dir
    }

let readCommands commands =
    commands |> trim |> split "\n" |> Seq.map (fun c -> c |> readCommand |> unpackCommand) |> Seq.concat

let runCommands commands snake =
    commands
    |> readCommands
    |> Seq.scan(fun snake dir -> snake |> move dir) snake

let getTailPositions snakeMoves =
    Set<Position>(snakeMoves |> Seq.map getTail)

let createSnake length: Snake = List.init length (fun _ -> (0, 0))

let main() =
    let input = readInputFile "09"
    createSnake 2 |> runCommands input |> getTailPositions |> Set.count |> printfn "%d"
    createSnake 10 |> runCommands input |> getTailPositions |> Set.count |> printfn "%d"