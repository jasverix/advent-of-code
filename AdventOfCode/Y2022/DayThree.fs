module Y2022.DayThree

type Item = char
type Compartment = Set<Item>
type Racksack = Compartment * Compartment

let between a b c = c >= a && b <= b
let isLower item = item |> between 'a' 'z'
let isUpper item = item |> between 'A' 'Z'

let itemPriority item =
    let itemValue = int item

    if item |> isLower then
        let low = int 'a'
        itemValue - low + 1
    else
        let low = int 'A'
        itemValue - low + 27

let toCompartment (input: string) : Compartment = Set<Item>(input)

let splitStringInHalf (input: string) =
    let halfLength = (input.Length + 1) / 2
    (input[0 .. halfLength - 1], input[halfLength..])

let private _toRacksack (first, second) =
    (toCompartment first, toCompartment second)

let toRacksack input : Racksack =
    input |> Utils.trim |> splitStringInHalf |> _toRacksack

let getDuplicateItemsOfRacksack (first: Compartment, second: Compartment) = first |> Seq.filter (second.Contains)

let toRacksacks input =
    input |> Utils.trim |> Utils.split "\n" |> Seq.map toRacksack

let getDuplicateItemsOfRacksacks racksacks =
    racksacks |> Seq.map getDuplicateItemsOfRacksack |> Seq.concat

let totalPriority items =
    items |> Seq.map itemPriority |> Seq.sum

let getTotalPriorityByInput input =
    input |> toRacksacks |> getDuplicateItemsOfRacksacks |> totalPriority

let main() =
    let input = Utils.readInputFile "03"
    let racksacks = input |> toRacksacks
    
    racksacks |> getDuplicateItemsOfRacksacks |> totalPriority |> printfn "%d"