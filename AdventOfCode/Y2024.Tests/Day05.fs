module Y2024.Tests.Day05

open NUnit.Framework
open Y2024.Day04
open Y2024.Day05

let testInput =
    "47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47"

[<Test>]
let ``Update mathces rule`` () =
    let update = [ 75; 47; 61; 53; 29 ]
    updateQualifiesToRule update (47, 53) |> Assert.That
    updateQualifiesToRule update (61, 75) |> not |> Assert.That
    updateQualifiesToRule update (62, 75) |> Assert.That
    updateQualifiesToRule update (61, 76) |> Assert.That

[<Test>]
let ``Find calculated number`` () =
    Assert.That(testInput |> parseInput |> getNumberByValidUpdates, Is.EqualTo 143)

[<Test>]
let ``Modify update`` () =
    let update = [ 75; 47; 61; 53; 29 ]
    Assert.That(modifyUpdateToMatchRule update (47, 53), Is.EqualTo [ 75; 47; 61; 53; 29 ])
    Assert.That(modifyUpdateToMatchRule update (61, 75), Is.EqualTo [ 47; 61; 75; 53; 29 ])
    
[<Test>]
let ``Find calculated number 2`` () =
    Assert.That(testInput |> parseInput |> getNumberByInvalidUpdates, Is.EqualTo 123)