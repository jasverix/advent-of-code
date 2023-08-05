module Y2022.Tests.Day03

open NUnit.Framework
open Y2022.Day03

let testInput = "vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw
"

[<Test>]
let ``Duplicate items``() =
    let racksack = "vJrwpWtwJgWrhcsFMMfFFhFp" |> toRacksack
    let dupes = racksack |> getDuplicateItemsOfRacksack
    Assert.AreEqual("p", dupes |> Seq.map(fun c -> c.ToString()) |> String.concat "")
    
[<Test>]
let ``Total priority``() =
    Assert.AreEqual(157, testInput |> getTotalPriorityByInput)
    
[<Test>]
let ``Common items of one group``() =
    let racksacks = [
        "vJrwpWtwJgWrhcsFMMfFFhFp" |> toRacksack
        "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL" |> toRacksack
        "PmmdzqPrVvPwwTWBwg" |> toRacksack
    ]
    let commonItems = racksacks |> commonItems
    Assert.AreEqual("r", commonItems |> Seq.map(fun i -> i.ToString()) |> String.concat " ")

[<Test>]
let ``Common items of all``() =
    let commonItems = testInput |> toRacksacks |> getCommonItemsOfGroups
    Assert.AreEqual("r Z", commonItems |> Seq.map(fun i -> i.ToString()) |> String.concat " ")