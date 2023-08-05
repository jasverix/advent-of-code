module Y2022.Tests.Day06

open NUnit.Framework
open Y2022.Day06

[<Test>]
let ``Chars are different``() =
    Assert.IsTrue("abcd" |> charsAreDifferent)
    Assert.IsFalse("abac" |> charsAreDifferent)
    Assert.IsFalse("abac" |> lastXCharsAreDifferent 4)
    Assert.IsTrue("abacdf" |> lastXCharsAreDifferent 4)
    
[<Test>]
let ``Find marker``() =
    Assert.AreEqual(7, "mjqjpqmgbljsphdztnvjfqwrcgsmlb" |> findMarker 4)
    Assert.AreEqual(5, "bvwbjplbgvbhsrlpgdmjqwftvncz" |> findMarker 4)
    Assert.AreEqual(6, "nppdvjthqldpwncqszvftbrmjlhg" |> findMarker 4)
    Assert.AreEqual(10, "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg" |> findMarker 4)
    Assert.AreEqual(11, "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw" |> findMarker 4)
    
[<Test>]
let ``Find message``() =
    Assert.AreEqual(19, "mjqjpqmgbljsphdztnvjfqwrcgsmlb" |> findMarker 14)
    Assert.AreEqual(23, "bvwbjplbgvbhsrlpgdmjqwftvncz" |> findMarker 14)
    Assert.AreEqual(23, "nppdvjthqldpwncqszvftbrmjlhg" |> findMarker 14)
    Assert.AreEqual(29, "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg" |> findMarker 14)
    Assert.AreEqual(26, "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw" |> findMarker 14)