module Y2024.Day06

open System.Collections.Generic

type Direction =
    | Up
    | Down
    | Left
    | Right

type Position = int * int

type MapItem =
    | GuardUp
    | GuardLeft
    | GuardRight
    | GuardDown
    | Obstacle
    | Empty

type AreaMap = Map<int, Map<int, MapItem>>

let parseMapItem char =
    match char with
    | '^' -> GuardUp
    | '<' -> GuardLeft
    | '>' -> GuardRight
    | 'v' -> GuardDown
    | '#' -> Obstacle
    | '.' -> Empty
    | _ -> failwith "Invalid character"

let parseMapLine line =
    line |> Seq.mapi (fun x char -> (x, parseMapItem char)) |> Map.ofSeq

let parseMap mapInput : AreaMap =
    mapInput
    |> Str.split "\n"
    |> List.mapi (fun y line -> (y, parseMapLine line))
    |> Map.ofList

let findGuardPosition (areaMap: AreaMap) =
    areaMap
    |> Map.toSeq
    |> Seq.collect (fun (y, row) -> row |> Map.toSeq |> Seq.map (fun (x, item) -> (x, y, item)))
    |> Seq.find (fun (_, _, item) -> item = GuardUp || item = GuardDown || item = GuardLeft || item = GuardRight)
    |> fun (x, y, guard) ->
        ((x, y),
         match guard with
         | GuardUp -> Up
         | GuardDown -> Down
         | GuardLeft -> Left
         | GuardRight -> Right
         | _ -> failwith "Invalid guard")

let peekUp (x, y) (map: AreaMap) =
    if y = 0 then None else Some(map[y - 1][x])

let peekDown (x, y) (map: AreaMap) =
    try
        Some(map[y + 1][x])
    with :? KeyNotFoundException ->
        None

let peekLeft (x, y) (map: AreaMap) =
    if x = 0 then None else Some(map[y][x - 1])

let peekRight (x, y) (map: AreaMap) =
    try
        Some(map[y][x + 1])
    with :? KeyNotFoundException ->
        None

let peek position direction map =
    match direction with
    | Up -> peekUp position map
    | Down -> peekDown position map
    | Left -> peekLeft position map
    | Right -> peekRight position map

let right direction =
    match direction with
    | Up -> Right
    | Down -> Left
    | Left -> Up
    | Right -> Down

let getNextPosition direction (x, y) =
    match direction with
    | Up -> (x, y - 1)
    | Down -> (x, y + 1)
    | Left -> (x - 1, y)
    | Right -> (x + 1, y)

let rec move position direction map =
    let mapItem = map |> peek position direction

    match mapItem with
    | Some Obstacle -> map |> move position (right direction)
    | Some(Empty | GuardUp | GuardDown | GuardLeft | GuardRight) ->
        Some(position |> getNextPosition direction, direction)
    | None -> None

let rec progressGuardPath (position, direction) map path =
    let nextPosition = move position direction map

    match nextPosition with
    | Some newPosition -> progressGuardPath newPosition map (path @ [ newPosition ])
    | None -> path

let guardPath map =
    let guardPosition, guardDirection = findGuardPosition map
    progressGuardPath (guardPosition, guardDirection) map [ guardPosition, guardDirection ]
    
let drawItem item =
    match item with
    | GuardUp -> '^'
    | GuardDown -> 'v'
    | GuardLeft -> '<'
    | GuardRight -> '>'
    | Obstacle -> '#'
    | Empty -> '.'
    
let drawMapLine (line: Map<int, MapItem>) =
    line |> Map.toList |> List.map (fun (_, item) -> drawItem item) |> Str.ofSeq

let drawMap (map: AreaMap) =
    map |> Map.toList |> List.map (fun (_, line) -> drawMapLine line) |> String.concat "\n"

let main () =
    Utils.readInputFile "06" |> parseMap |> guardPath |> List.distinctBy fst |> List.length |> printfn "Part 1: %d"
