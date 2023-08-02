module Y2022.Tests.DayThree

open NUnit.Framework
open Y2022.DayThree

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