module Y2024.Day04

type Direction =
    | LTR
    | RTL
    | TTB
    | BTT
    | BLTTR
    | BRTTL
    | TLTBR
    | TRTBL

type CharacterMap = Map<int, Map<int, char>>
type WordWithDirection = string * Direction
type WordWithCoordinates = int * int * WordWithDirection

let toMapLine (line: string) : Map<int, char> =
    line.ToCharArray() |> Array.mapi (fun x c -> (x, c)) |> Map.ofArray

let parseInput (input: string) : CharacterMap =
    input.Split('\n')
    |> Array.map toMapLine
    |> Array.mapi (fun y line -> (y, line))
    |> Map.ofArray

let validateWord length (word: string) =
    if word.Length = length then Some word else None

let getWord x y length predicate =
    try
        [ 0 .. length - 1 ] |> List.map predicate |> Str.ofSeq |> validateWord length
    with _ ->
        None

let getWordLTR x y length (map: CharacterMap) =
    getWord x y length (fun i -> map[y][x + i])

let getWordRTL x y length (map: CharacterMap) =
    getWord x y length (fun i -> map[y][x - i])

let getWordTTB x y length (map: CharacterMap) =
    getWord x y length (fun i -> map[y + i][x])

let getWordBTT x y length (map: CharacterMap) =
    getWord x y length (fun i -> map[y - i][x])

let getWordBLTTR x y length (map: CharacterMap) =
    getWord x y length (fun i -> map[y - i][x + i])

let getWordBRTTL x y length (map: CharacterMap) =
    getWord x y length (fun i -> map[y - i][x - i])

let getWordTLTBR x y length (map: CharacterMap) =
    getWord x y length (fun i -> map[y + i][x + i])

let getWordTRTBL x y length (map: CharacterMap) =
    getWord x y length (fun i -> map[y + i][x - i])

let findAllCoordsOfChar (char: char) (map: CharacterMap) =
    map
    |> Map.toSeq
    |> Seq.collect (fun (y, line) -> line |> Map.toSeq |> Seq.map (fun (x, c) -> (x, y, c)))
    |> Seq.filter (fun (_, _, c) -> c = char)
    |> Seq.map (fun (x, y, _) -> (x, y))

let getWordsOfPosition x y length (map: CharacterMap) : WordWithDirection list =
    [ getWordLTR x y length map, LTR
      getWordRTL x y length map, RTL
      getWordTTB x y length map, TTB
      getWordBTT x y length map, BTT
      getWordBLTTR x y length map, BLTTR
      getWordBRTTL x y length map, BRTTL
      getWordTLTBR x y length map, TLTBR
      getWordTRTBL x y length map, TRTBL ]
    |> List.filter (fun (word, _) -> word.IsSome)
    |> List.map (fun (word, dir) -> word.Value, dir)

let getWordsWithCoordinates (word: string) map : WordWithCoordinates seq =
    map
    |> findAllCoordsOfChar (word[0])
    |> Seq.map (fun (x, y) -> getWordsOfPosition x y word.Length map |> List.map (fun w -> x, y, w))
    |> Seq.collect id
    |> Seq.filter (fun (_, _, w) -> fst w = word)

let isInCross x y (map: CharacterMap) (word: string) =
    let char = map[y][x]

    if char <> word[1] then
        false
    else
        try
            let TL = map[y - 1][x - 1]
            let TR = map[y - 1][x + 1]
            let BL = map[y + 1][x - 1]
            let BR = map[y + 1][x + 1]

            let TLTBR = String.concat "" [ TL.ToString(); char.ToString(); BR.ToString() ]
            let TRTBL = String.concat "" [ TR.ToString(); char.ToString(); BL.ToString() ]
            let BLTTR = String.concat "" [ BL.ToString(); char.ToString(); TR.ToString() ]
            let BRTTL = String.concat "" [ BR.ToString(); char.ToString(); TL.ToString() ]

            // if two are equal to word, we are good
            [ TLTBR; TRTBL; BLTTR; BRTTL ] |> List.filter ((=) word) |> List.length = 2
        with _ ->
            false

let getCrossesWithCoordinates (word: string) map : (int * int) seq =
    map
    |> findAllCoordsOfChar (word[1])
    |> Seq.filter (fun (x, y) -> isInCross x y map word)

let countOccurencesOfWord (word: string) (input: string) =
    input |> parseInput |> getWordsWithCoordinates word |> Seq.length

let findCrossMases input =
    input |> parseInput |> getCrossesWithCoordinates "MAS" |> Seq.length

let main () =
    Utils.readInputFile "04"
    |> countOccurencesOfWord "XMAS"
    |> printfn "Count XMAS: %A"
    
    Utils.readInputFile "04"
    |> findCrossMases
    |> printfn "Count XMAS: %A"
