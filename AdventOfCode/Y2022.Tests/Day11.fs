module Y2022.Tests.Day11

open NUnit.Framework
open Y2022.Day11

[<Test>]
let ``Read monkey`` () =
    let monkey0Input =
        "Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3"

    let monkey0 = monkey0Input |> getMonkey
    Assert.AreEqual("old * 19", monkey0.Operation)
    Assert.AreEqual([ 79L; 98L ], monkey0.Items)
    Assert.AreEqual("divisible by 23", monkey0.Test.Input)
    Assert.AreEqual("throw to monkey 2", monkey0.Test.IfTrue)
    Assert.AreEqual("throw to monkey 3", monkey0.Test.IfFalse)


let testInput =
    "Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1
"

[<Test>]
let ``One monkey`` () =
    let monkeys = testInput |> getMonkeys |> handleMonkey 0 reliefBy3
    Assert.AreEqual(2, monkeys[0].InspectedItems)

    Assert.AreEqual(
        [ 74; 500; 620 ] |> List.map string |> String.concat " ",
        monkeys[3].Items |> List.map string |> String.concat " "
    )

[<Test>]
let ``One round`` () =
    let monkeys = testInput |> getMonkeys |> rounds 1 reliefBy3
    Assert.AreEqual(2, monkeys[0].InspectedItems)

    Assert.AreEqual(
        [ 20; 23; 27; 26 ] |> List.map string |> String.concat " ",
        monkeys[0].Items |> List.map string |> String.concat " "
    )

[<Test>]
let ``Most active by 20 rounds`` () =
    let monkeys = testInput |> getMonkeys |> rounds 20 reliefBy3
    Assert.AreEqual(
        [ 101; 95; 7; 105 ] |> List.map string |> String.concat " ",
        monkeys
        |> List.map (fun m -> m.InspectedItems)
        |> List.map string
        |> String.concat " "
    )
    Assert.AreEqual(10605, monkeys |> getMonkeyBusinessLevel)

[<Test>]
let ``Most active by one round with no relief`` () =
    let monkeys = testInput |> getMonkeys |> rounds 1 noRelief

    Assert.AreEqual(
        [ 2; 4; 3; 6 ] |> List.map string |> String.concat " ",
        monkeys
        |> List.map (fun m -> m.InspectedItems)
        |> List.map string
        |> String.concat " "
    )

    Assert.AreEqual(4 * 6, monkeys |> getMonkeyBusinessLevel)

[<Test>]
let ``Most active by 20 rounds with no relief`` () =
    let monkeys = testInput |> getMonkeys
    let monkeys2 = monkeys |> rounds 20 (reliefByModulo monkeys)

    Assert.AreEqual(
        [ 99; 97; 8; 103 ] |> List.map string |> String.concat " ",
        monkeys2
        |> List.map (fun m -> m.InspectedItems)
        |> List.map string
        |> String.concat " "
    )

    Assert.AreEqual(99 * 103, monkeys2 |> getMonkeyBusinessLevel)

[<Test>]
let ``Most active by 10000 rounds with no relief`` () =
    let monkeys = testInput |> getMonkeys
    let monkeys = monkeys |> rounds 10000 (reliefByModulo monkeys)
    Assert.AreEqual(52166, monkeys[0].InspectedItems)
    Assert.AreEqual(52013, monkeys[3].InspectedItems)
    Assert.AreEqual(2713310158L, monkeys |> getMonkeyBusinessLevel)
