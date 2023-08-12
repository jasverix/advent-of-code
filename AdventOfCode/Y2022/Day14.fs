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

let getBottomCoordinate (crds: Set<Coordinate>) = crds |> Set.map snd |> Set.maxElement

let getLeftCoordinate (crds: Set<Coordinate>) =
    if crds.IsEmpty then
        490
    else
        crds |> Set.map fst |> Set.minElement

let getRightCoordinate (crds: Set<Coordinate>) =
    if crds.IsEmpty then
        510
    else
        crds |> Set.map fst |> Set.maxElement

type RockMap =
    { Rocks: Set<Coordinate>
      FloorPosition: int option }

type SandMap = Set<Coordinate> * Set<Coordinate>

let toRockMap withFloor input : RockMap =
    input
    |> Str.trim
    |> Str.split "\n"
    |> Array.map toPath
    |> Array.map toCoordinates
    |> Array.toList
    |> List.concat
    |> Set.ofList
    |> fun rocks ->
        { Rocks = rocks
          FloorPosition =
            match withFloor with
            | true -> Some((rocks |> getBottomCoordinate) + 2)
            | false -> None }

type Dir =
    | Down
    | DownLeft
    | DownRight

let step dir (x, y) =
    match dir with
    | Down -> (x, y + 1)
    | DownLeft -> (x - 1, y + 1)
    | DownRight -> (x + 1, y + 1)

let canGo (rockMap: RockMap) ((sandAtRest, _): SandMap) target =
    (rockMap.Rocks |> Set.contains target |> not)
    && (sandAtRest |> Set.contains target |> not)
    && match rockMap.FloorPosition with
        | Some floor -> (target |> snd) < floor
        | None -> true

let stepSand rockMap sandMap sandPosition : bool * Coordinate =
    [ Down; DownLeft; DownRight ]
    |> Seq.map (fun dir -> sandPosition |> step dir)
    |> Seq.tryFind (canGo rockMap sandMap)
    |> fun newPos ->
        match newPos with
        | Some pos -> (true, pos)
        | None -> (false, sandPosition)

let canAddNewSand rockMap sandMap =
    (canGo rockMap sandMap (500, 0))
    && (canGo rockMap sandMap (501, 1))
    && (canGo rockMap sandMap (503, 1))
    && (canGo rockMap sandMap (500, 1))
    && (canGo rockMap sandMap (500, 2))

let handleMovedSands sandAtRest (movedSands: (bool * Coordinate) seq) =
    movedSands
    |> Seq.fold
        (fun ((sandAtRest, movingSand): SandMap) (moving, pos) ->
            if moving then
                (sandAtRest, (movingSand |> Set.add pos))
            else
                (sandAtRest |> Set.add pos, movingSand))
        (sandAtRest, Set.empty)

let tick i rockMap sandMap : SandMap =
    let sandAtRest, movingSand = sandMap

    let newSandAtRest, newMovingSands =
        movingSand |> Seq.map (stepSand rockMap sandMap) |> handleMovedSands sandAtRest

    if newMovingSands |> Seq.isEmpty || ((i % 2) = 0) then
        (newSandAtRest, newMovingSands |> Set.add (500, 0))
    else
        newSandAtRest, newMovingSands

let getBottomRock (rockMap: RockMap) =
    match rockMap.FloorPosition with
    | Some floor -> floor
    | None -> rockMap.Rocks |> getBottomCoordinate

let getPositionChar rockMap ((sandAtRest, movingSands): SandMap) position =
    if rockMap.Rocks |> Set.contains position then
        '#'
    elif sandAtRest |> Set.contains position then
        'o'
    elif movingSands |> Seq.contains position then
        '~'
    elif rockMap.FloorPosition.IsSome && (position |> snd) = rockMap.FloorPosition.Value then
        '█'
    else
        '.'

let stringMap x1 x2 (rockMap: RockMap) (sandMap: SandMap) =
    let y1 = 0
    let y2 = (rockMap |> getBottomRock) + 1

    y1
    |> toRange y2
    |> List.map (fun y ->
        x1
        |> toRange x2
        |> List.map (fun x -> (x, y) |> getPositionChar rockMap sandMap))
    |> List.map (fun row -> row |> List.map string |> String.concat "")
    |> String.concat "\n"

let printMap x1 x2 rockMap sandMap =
    stringMap x1 x2 rockMap sandMap |> printf "%s\n\n"
    System.Console.SetCursorPosition(0, System.Console.CursorTop)
    System.Console.Clear()

let sandInAbyss rockMap (sandPosition: Coordinate) =
    if rockMap.FloorPosition.IsSome then
        false
    else
        sandPosition |> snd |> (fun y -> y > (rockMap |> getBottomRock))

let isBlocking (sandAtRest, _) = sandAtRest |> Set.contains (500, 0)

let anySandInAbyss rockMap (_, sandMap) =
    sandMap |> Seq.exists (sandInAbyss rockMap)

let rec run i print (rockMap: RockMap) (sandMap: SandMap) : SandMap =
    print rockMap sandMap

    if anySandInAbyss rockMap sandMap || isBlocking sandMap then
        let sandAtRest, _ = sandMap
        (sandAtRest, Set.empty)
    else
        run (i + 1) print rockMap (tick i rockMap sandMap)

let getResult print rockMap : SandMap =
    let sandMap = (Set.empty, Set.empty)
    run 0 print rockMap sandMap

let testInput =
    "498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9
"

let main () =
    let rockMap = readInputFile "14" |> toRockMap true
    // let rockMap = testInput |> toRockMap
    let sandAtRest, _ =
        getResult (printMap (rockMap.Rocks |> getLeftCoordinate) (rockMap.Rocks |> getRightCoordinate)) rockMap

    sandAtRest |> Set.count |> printfn "Part 1: %d"
