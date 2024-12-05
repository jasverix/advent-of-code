module Y2024.Tests.Day04

open NUnit.Framework
open Y2024.Day04

let testInput =
    "MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX"

[<Test>]
let ``Make word map`` () =
    let wordMap = testInput |> parseInput
    Assert.That(wordMap[0][0], Is.EqualTo 'M')
    Assert.That(wordMap[0][1], Is.EqualTo 'M')
    Assert.That(wordMap[1][0], Is.EqualTo 'M')
    Assert.That(wordMap[1][1], Is.EqualTo 'S')
    Assert.That(wordMap[2][0], Is.EqualTo 'A')

[<Test>]
let ``Get words`` () =
    let wordMap = testInput |> parseInput
    Assert.That(wordMap |> getWordLTR 2 3 4 |> Option.get, Is.EqualTo "AMAS")
    Assert.That(wordMap |> getWordRTL 4 1 4 |> Option.get, Is.EqualTo "XMAS")
    Assert.That(wordMap |> getWordTTB 6 2 5 |> Option.get, Is.EqualTo "AMXXS")
    Assert.That(wordMap |> getWordBTT 8 8 4 |> Option.get, Is.EqualTo "MASM")
    Assert.That(wordMap |> getWordTRTBL 6 3 4 |> Option.get, Is.EqualTo "MMMM")
    Assert.That(wordMap |> getWordTLTBR 4 0 4 |> Option.get, Is.EqualTo "XMAS")
    Assert.That(wordMap |> getWordBRTTL 6 4 4 |> Option.get, Is.EqualTo "XSXM")
    Assert.That(wordMap |> getWordBLTTR 1 5 4 |> Option.get, Is.EqualTo "XAMX")

[<Test>]
let ``No word`` () =
    let wordMap = testInput |> parseInput
    Assert.That(wordMap |> getWordLTR 7 0 4, Is.Null)
    Assert.That(wordMap |> getWordRTL 2 1 4, Is.Null)

[<Test>]
let ``Count occurences of word`` () =
    Assert.That(testInput |> countOccurencesOfWord "XMAS", Is.EqualTo 18)
    
[<Test>]
let ``Count occurences of X-Mas`` () =
    Assert.That(testInput |> findCrossMases, Is.EqualTo 9)
