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

type SandMap = Set<Coordinate>

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

let canGo (rockMap: RockMap) (sandMap: SandMap) target =
    (rockMap.Rocks |> Set.contains target |> not)
    && (sandMap |> Set.contains target |> not)
    && match rockMap.FloorPosition with
       | Some floor -> (target |> snd) < floor
       | None -> true

let nextSandPosition rockMap sandMap sandPosition =
    [ Down; DownLeft; DownRight ]
    |> Seq.map (fun dir -> sandPosition |> step dir)
    |> Seq.tryFind (canGo rockMap sandMap)

let getBottomRock (rockMap: RockMap) =
    match rockMap.FloorPosition with
    | Some floor -> floor
    | None -> rockMap.Rocks |> getBottomCoordinate

let getPositionChar rockMap (sandMap: SandMap) position =
    if rockMap.Rocks |> Set.contains position then
        '#'
    elif sandMap |> Set.contains position then
        'o'
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

let rec nextSand rockMap sandMap newSand =
    if sandInAbyss rockMap newSand then
        newSand
    else
        let nextPosition = nextSandPosition rockMap sandMap newSand

        match nextPosition with
        | Some pos -> nextSand rockMap sandMap pos
        | None -> newSand

let rec run print (rockMap: RockMap) (sandMap: SandMap) : SandMap =
    print rockMap sandMap

    let nextSand = nextSand rockMap sandMap (500, 0)

    if nextSand = (500, 0) then sandMap |> Set.add (500, 0)
    elif sandInAbyss rockMap nextSand then sandMap
    else run print rockMap (sandMap |> Set.add nextSand)

let getResult print rockMap : SandMap = run print rockMap Set.empty

let testInput =
    "498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9
"

let main () =
    let rockMap = readInputFile "14" |> toRockMap true
    // let rockMap = testInput |> toRockMap true

    let sandMap =
        getResult (fun _ _ -> ()) rockMap

    sandMap |> Set.count |> printfn "Part 1: %d"
