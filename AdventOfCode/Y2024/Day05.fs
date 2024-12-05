module Y2024.Day05

type PageNumber = int
type PageOrderingRule = PageNumber * PageNumber
type Update = PageNumber list

let updateQualifiesToRule (update: Update) (rule: PageOrderingRule) =
    let (page1, page2) = rule

    try
        let page1Index, page2Index =
            (List.findIndex ((=) page1) update, List.findIndex ((=) page2) update)

        page1Index < page2Index
    with _ ->
        true

let updateQualifiesToRules (rules: PageOrderingRule list) (update: Update) =
    rules |> List.forall (updateQualifiesToRule update)

let modifyUpdateToMatchRule (update: Update) (rule: PageOrderingRule) =
    let (page1, page2) = rule

    try
        let page1Index, page2Index =
            (List.findIndex ((=) page1) update, List.findIndex ((=) page2) update)

        if page1Index < page2Index then
            update
        else
            update |> List.removeAt page2Index |> List.insertAt page1Index page2
    with _ ->
        update

let rec modifyUpdateToMatchRules (rules: PageOrderingRule list) (update: Update) =
    let update =rules |> List.fold modifyUpdateToMatchRule update
    if updateQualifiesToRules rules update then
        update
    else
        modifyUpdateToMatchRules rules update

let findMatchingUpdates updates rules =
    updates |> List.filter (updateQualifiesToRules rules)

let findNotMatchingUpdates updates rules =
    updates |> List.filter (fun update -> not (updateQualifiesToRules rules update))

let getMiddlePageNumber (update: Update) =
    let middleIndex = (List.length update) / 2
    List.item middleIndex update

let getNumberByValidUpdates (rules, updates) : int =
    findMatchingUpdates updates rules |> List.map getMiddlePageNumber |> List.sum

let getNumberByInvalidUpdates (rules, updates) : int =
    findNotMatchingUpdates updates rules
    |> List.map (fun u -> u |> modifyUpdateToMatchRules rules)
    |> List.map getMiddlePageNumber
    |> List.sum

let parsePageOrderingRule (line: string) : PageOrderingRule =
    let parts = line.Split('|')
    (int parts[0], int parts[1])

let parseUpdate (line: string) : Update = line |> Str.split "," |> List.map int

let splitInput (input: string) =
    match input.Split "\n\n" with
    | [| rules; updates |] -> rules, updates
    | _ -> failwith "Invalid input"

let parseInput (input: string) : (PageOrderingRule list * Update list) =
    let rules, updates = input |> Str.trim |> splitInput
    rules |> Str.split "\n" |> List.map parsePageOrderingRule, updates |> Str.split "\n" |> List.map parseUpdate

let main () =
    let input = Utils.readInputFile "05" |> parseInput
    
    input |> getNumberByValidUpdates |> printfn "%A"
    input |> getNumberByInvalidUpdates |> printfn "%A"