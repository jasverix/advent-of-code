module Y2024.Tests.Day02

open NUnit.Framework
open Y2024.Day02

let testInput = "7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
"

[<Test>]
let ``Reports are safe / unsafe`` () =
    "7 6 4 2 1" |> parseInputLine |> reportIsSafe |> Assert.That
    "1 2 7 8 9" |> parseInputLine |> reportIsSafe |> not |> Assert.That
    "9 7 6 2 1" |> parseInputLine |> reportIsSafe |> not |> Assert.That
    "1 3 2 4 5" |> parseInputLine |> reportIsSafe |> not |> Assert.That
    "8 6 4 4 1" |> parseInputLine |> reportIsSafe |> not |> Assert.That
    "1 3 6 7 9" |> parseInputLine |> reportIsSafe |> Assert.That
    
[<Test>]
let ``Count safe reports`` () =
    Assert.That(testInput |> parseInput |> countSafeReports, Is.EqualTo(2))
    
[<Test>]
let ``Reports are safe / unsafe with dampener`` () =
    "7 6 4 2 1" |> parseInputLine |> reportIsSafeWithDampener |> Assert.That
    "1 2 7 8 9" |> parseInputLine |> reportIsSafeWithDampener |> not |> Assert.That
    "9 7 6 2 1" |> parseInputLine |> reportIsSafeWithDampener |> not |> Assert.That
    "1 3 2 4 5" |> parseInputLine |> reportIsSafeWithDampener |> Assert.That
    "8 6 4 4 1" |> parseInputLine |> reportIsSafeWithDampener |> Assert.That
    "1 3 6 7 9" |> parseInputLine |> reportIsSafeWithDampener |> Assert.That