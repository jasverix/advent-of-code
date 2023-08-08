module Y2022.Tests.Day13

open NUnit.Framework
open Y2022.Str
open Y2022.Day13

[<Test>]
let Substring() =
    Assert.AreEqual("Hello", "Hello World!" |> substring 0 5)
    Assert.AreEqual("ello ", "Hello World!" |> substring 1 5)
    Assert.AreEqual(" Worl", "Hello World!" |> substring 5 5)
    Assert.AreEqual("ello World", "Hello World!" |> substring 1 -1)
    Assert.AreEqual("ello Worl", "Hello World!" |> substring 1 -2)
    
let toString list = list |> List.map string |> String.concat " "
    
[<Test>]
let ``Parse packet``() =
    Assert.AreEqual("Sublist [Single 1; Single 2; Single 3]", "[1,2,3]" |> toPacket |> toString)
    Assert.AreEqual("Sublist [Single 1; Sublist [Single 2; Single 3]; Single 4]", "[1,[2,3],4]" |> toPacket |> toString)
    
[<Test>]
let ``Pair is correct``() =
    Assert.IsTrue("""
        [1,2,3]
        [1,2,3,4]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsTrue("""
        [1,2,3]
        [1,3,3]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsTrue("""
        [1,1,3,1,1]
        [1,1,5,1,1]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsTrue("""
        [[1],[2,3,4]]
        [[1],4]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsFalse("""
        [9]
        [[8,7,6]]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsTrue("""
        [[4,4],4,4]
        [[4,4],4,4,4]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsFalse("""
        [7,7,7,7]
        [7,7,7]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsTrue("""
        []
        [3]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsFalse("""
        [[[]]]
        [[]]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsFalse("""
        [1,[2,[3,[4,[5,6,7]]]],8,9]
        [1,[2,[3,[4,[5,6,0]]]],8,9]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsFalse("""
        [1,2,4]
        [1,2,3,4]
        """ |> toPacketPair |> packetPairIsCorrect)
    
    Assert.IsTrue("""
        [1,2,4]
        [1,[2,3],3,4]
        """ |> toPacketPair |> packetPairIsCorrect)
    
[<Test>]
let ``Compare empty packets``() =
    Assert.AreEqual(Equal, ("[[]]" |> toPacket |> comparePackets ("[[]]" |> toPacket)))
    Assert.AreEqual(Lesser, ("[[]]" |> toPacket |> comparePackets ("[0]" |> toPacket)))
    Assert.AreEqual(Lesser, ("[[]]" |> toPacket |> comparePackets ("[[[]]]" |> toPacket)))
    Assert.AreEqual(Greater, ("[[[]]]" |> toPacket |> comparePackets ("[[]]" |> toPacket)))
    Assert.AreEqual(Greater, ("[[],[]]" |> toPacket |> comparePackets ("[[]]" |> toPacket)))
    Assert.AreEqual(Lesser, ("[[]]" |> toPacket |> comparePackets ("[[],[]]" |> toPacket)))

let testInput = "[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]
"

[<Test>]
let ``Get correct index sum``() =
    Assert.AreEqual(13, testInput |> getPairs |> getIndexesOfRightPairs)