module Y2022.Day14

open Utils

type Coordinate = int * int
type Vectors = Coordinate list

let toCoordinate input : Coordinate =
    match input |> trim with
    | Regex "(\d+),(\d+)" [ Int x; Int y ] -> (x, y)
    | _ -> (0, 0)

let toPath input : Vectors =
    input |> trim |> split "->" |> Array.map toCoordinate |> Array.toList

let toRange x2 x1 =
    if x1 > x2 then [ x2..x1 ] |> List.rev
    elif x1 = x2 then [ x1 ]
    else [ x1..x2 ]

let coordinatesBetween (x2, y2) (x1, y1) : Coordinate list =
    let xpath = x1 |> toRange x2
    let ypath = y1 |> toRange y2

    let yPosiions = ypath |> List.map (fun y -> (x1, y))
    let lastY = yPosiions |> List.last |> snd

    let xPositions =
        if xpath |> List.isEmpty then
            []
        else
            xpath |> List.tail |> List.map (fun x -> (x, lastY))

    yPosiions @ xPositions

let toCoordinates (vectors: Vectors) =
    vectors
    |> List.fold
        (fun positions vector ->
            if positions |> List.isEmpty then
                [ vector ]
            else
                let lastPosition, newPositions = positions |> List.popItem
                newPositions @ (lastPosition |> coordinatesBetween vector))
        []

let toRockMap input : Set<Coordinate> =
    input
    |> Str.trim
    |> Str.split "\n"
    |> Array.map toPath
    |> Array.map toCoordinates
    |> Array.toList
    |> List.concat
    |> Set.ofList

type Dir =
    | Down
    | DownLeft
    | DownRight

let step dir (x, y) =
    match dir with
    | Down -> (x, y + 1)
    | DownLeft -> (x - 1, y + 1)
    | DownRight -> (x + 1, y + 1)

let canGo (rockMap: Set<Coordinate>) (sandMap: Coordinate list) target =
    (target |> rockMap.Contains |> not) && (sandMap |> List.contains target |> not)

let stepSand rockMap sandMap sandPosition : bool * Coordinate =
    [ Down; DownLeft; DownRight ]
    |> Seq.map (fun dir -> sandPosition |> step dir)
    |> Seq.tryFind (canGo rockMap sandMap)
    |> fun newPos ->
        match newPos with
        | Some pos -> (true, pos)
        | None -> (false, sandPosition)

let canAddNewSand rockMap sandMap =
    (canGo rockMap sandMap (500, 0)) &&
    (canGo rockMap sandMap (501, 1)) &&
    (canGo rockMap sandMap (503, 1)) &&
    (canGo rockMap sandMap (500, 1)) &&
    (canGo rockMap sandMap (500, 2))

let tick rockMap (sandMap: Coordinate list) : Coordinate list =
    let anyMoving, newSandMap = sandMap |> List.map (stepSand rockMap sandMap) |> List.unzip
    if anyMoving |> List.contains true |> not || (newSandMap |> canAddNewSand rockMap) then
        (500, 0) :: newSandMap
    else
        newSandMap

let getBottomRock (rockMap: Set<Coordinate>) =
    rockMap |> Set.map snd |> Set.maxElement
    
let getLeftRock (rockMap: Set<Coordinate>) =
    rockMap |> Set.map fst |> Set.minElement

let getRightRock (rockMap: Set<Coordinate>) =
    rockMap |> Set.map fst |> Set.maxElement
    
let getPositionChar rockMap sandMap position =
    if rockMap |> Set.contains position then
        '#'
    elif sandMap |> List.contains position then
        'o'
    else
        '.'
    
let stringMap rockMap sandMap =
    let x1 = (rockMap |> getLeftRock) - 1
    let x2 = (rockMap |> getRightRock) + 1
    let y1 = 0
    let y2 = (rockMap |> getBottomRock) + 1
    y1 |> toRange y2
    |> List.map (fun y -> x1 |> toRange x2 |> List.map (fun x -> (x, y) |> getPositionChar rockMap sandMap))
    |> List.map (fun row -> row |> List.map string |> String.concat "")
    |> String.concat "\n"
    
let printMap rockMap sandMap =
    stringMap rockMap sandMap |> printf "%s\n"
    System.Console.SetCursorPosition(0, System.Console.CursorTop)
    System.Console.Clear()

let sandInAbyss rockMap (sandPosition: Coordinate) =
    sandPosition |> snd |> (fun y -> y > (rockMap |> getBottomRock))

let anySandInAbyss rockMap sandMap =
    sandMap |> List.exists (sandInAbyss rockMap)
    
let tickToEnd rockMap (sandMap: Coordinate list) : bool * Coordinate list =
    let anyMoving, newSandMap = sandMap |> List.map (stepSand rockMap sandMap) |> List.unzip
    (anyMoving |> List.contains true, newSandMap |> List.filter (fun s -> s |> sandInAbyss rockMap |> not))
        
let rec runEnd print rockMap sandMap =
    print rockMap sandMap
    let _, newSandMap = (tickToEnd rockMap sandMap)
    if sandMap <> newSandMap then
        runEnd print rockMap newSandMap
    else
        newSandMap

let rec run print rockMap sandMap =
    print rockMap sandMap
    if anySandInAbyss rockMap sandMap then
        runEnd print rockMap (sandMap |> List.filter (fun s -> s |> sandInAbyss rockMap |> not))
    else
        run print rockMap (tick rockMap sandMap)

let testInput = "498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9
"

let main() =
    // let rockMap = readInputFile "14" |> toRockMap
    let rockMap = testInput |> toRockMap
    let sandMap = run printMap rockMap [ (500, 0) ]
    
    sandMap |> List.length |> printfn "Part 1: %d"