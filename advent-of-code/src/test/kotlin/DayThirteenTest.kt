import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DayThirteenTest {
    private val input = "[1,1,3,1,1]\n" +
            "[1,1,5,1,1]\n" +
            "\n" +
            "[[1],[2,3,4]]\n" +
            "[[1],4]\n" +
            "\n" +
            "[9]\n" +
            "[[8,7,6]]\n" +
            "\n" +
            "[[4,4],4,4]\n" +
            "[[4,4],4,4,4]\n" +
            "\n" +
            "[7,7,7,7]\n" +
            "[7,7,7]\n" +
            "\n" +
            "[]\n" +
            "[3]\n" +
            "\n" +
            "[[[]]]\n" +
            "[[]]\n" +
            "\n" +
            "[1,[2,[3,[4,[5,6,7]]]],8,9]\n" +
            "[1,[2,[3,[4,[5,6,0]]]],8,9]\n"

    @Test
    fun testPacketsValueSimpleList() {
        assertFalse(DayThirteen.Packet("[1,2,3]") <= (DayThirteen.Packet("[1,2,2]")))
        assertTrue(DayThirteen.Packet("[1,2,3]") <= (DayThirteen.Packet("[1,2,10]")))
        assertTrue(DayThirteen.Packet("[1,2]") <= (DayThirteen.Packet("[1,2,1]")))
        assertFalse(DayThirteen.Packet("[1,2,1]") <= (DayThirteen.Packet("[1,2]")))
    }

    @Test
    fun testPacketsValueListInList() {
        assertTrue(DayThirteen.Packet("[1,[1,2],3]") <= (DayThirteen.Packet("[1,[3,4],5]")))
        assertFalse(DayThirteen.Packet("[1,[1,5],3]") <= (DayThirteen.Packet("[1,[1,4],5]")))
        assertTrue(DayThirteen.Packet("[1,[1,2,3],3]") <= (DayThirteen.Packet("[1,[3,4],5]")))
        assertTrue(DayThirteen.Packet("[1,[1,2],7]") <= (DayThirteen.Packet("[1,[3,4],5]")))
        assertTrue(DayThirteen.Packet("[1,[1,2],3]") <= (DayThirteen.Packet("[1,[3,4],[1,2]]")))
        assertTrue(DayThirteen.Packet("[1,[1,2],[1,2]]") <= (DayThirteen.Packet("[1,[3,4],5]")))
    }

    @Test
    fun testCompareEmptyListWithZero() {
        assertFalse(
            DayThirteen.Packet("[[2,9,7,[[5,10,7,9,5],[7,5,9,8,10],3]],[[9,1]],[[[1],[],[4],4,[7,0,10,8]],[[],2,[]]],[10,[5,10],5,5]]") <=
                    DayThirteen.Packet("[[],[[],4,4,7],[0,1,[],9]]")
        )

        assertTrue(
            DayThirteen.Packet("[[[[2,4],[10,4,7,2],[4,8,7,9],[2,0,2,3],7],[2],8,[[1,0,9],5,8,3,[4,1,3]]],[[[8,6],4,6]],[0,[[4,6,0,2,7],[2,0,4,5]],9],[],[8,[[5],[3]],0]]") <=
                    DayThirteen.Packet("[[5,[[],[0],8,[4,6],[5,10,0,3,7]]],[[],[3,3,[7],[10]]],[[[3,4,0,0,9],[7,7,1,4,0],[6,1,6,2],0],[],4,9,[[2,1],[10,1],[],[0,2],[0,8,1,4]]],[[[7,4,4,9,0],[9,10,3,5],8,[9,2,5,9,2],[]],6],[5,[7,8,[5]],5]]")
        )
    }

    @Test
    fun testCompareSomeTrickyList() {
        assertFalse(
            DayThirteen.Packet("[[5,[],4],[[[8],2,5,9],[[0,6],9,8],[[0,7,9],7,[10],5,[5,6,10]],[0]],[1]]") <=
                    DayThirteen.Packet("[[[[2,9,10,0,4],2,10],4]]")
        )
    }

    @Test
    fun testPacketsValueListInListInList() {
        assertTrue(DayThirteen.Packet("[1,[1,[2,3]],3]") <= DayThirteen.Packet("[1,[3,[4,5]],5]"))
    }

    @Test
    fun testPacketsSum() {
        assertEquals(listOf(1, 2, 4, 6), DayThirteen.Signal(input).validPacketsIndexes())
        assertEquals(13, DayThirteen.Signal(input).validPacketsSum())
    }

    @Test
    fun testCompareEqualLists() {
        assertFalse(DayThirteen.Packet("[[1],[2,3],5]") <= DayThirteen.Packet("[[1],[2,3],4]"))
    }

    @Test
    fun testEmptyLists() {
        assertTrue(DayThirteen.Packet("[[]]") <= DayThirteen.Packet("[[]]"))
        assertTrue(DayThirteen.Packet("[[]]") <= DayThirteen.Packet("[0]"))
        assertTrue(DayThirteen.Packet("[[]]") <= DayThirteen.Packet("[[[]]]"))
        assertFalse(DayThirteen.Packet("[[[]]]") <= DayThirteen.Packet("[[]]"))
        assertFalse(DayThirteen.Packet("[[],[]]") <= DayThirteen.Packet("[[]]"))
        assertTrue(DayThirteen.Packet("[[]]") <= DayThirteen.Packet("[[],[]]"))
    }

    @Test
    fun testFinalResult() {
        val input = DayThirteen::class.java.getResourceAsStream("input-13.txt")?.bufferedReader()?.readText() ?: return
        val signal = DayThirteen.Signal(input)
        val validPacketsSum = signal.validPacketsSum()
        val expectedMax = 5800
        assertTrue(
            validPacketsSum < expectedMax,
            "Failed asserting that $validPacketsSum is less than $expectedMax\n" + signal.validPacketsIndexes()
                .joinToString(",")
        )

        val expectedLess = 5766
        assertNotEquals(
            validPacketsSum, expectedLess,
            "Failed asserting that $validPacketsSum is greater than $expectedLess\n" + signal.validPacketsIndexes()
                .joinToString(",")
        )

        assertEquals(5013, validPacketsSum)
    }

    @Test
    fun testOrdered() {
        val signal = DayThirteen.Signal(input)
        val packets = signal.getOrderedPacketsWithDividers()
        assertEquals("[]\n" +
                "[[]]\n" +
                "[[[]]]\n" +
                "[1,1,3,1,1]\n" +
                "[1,1,5,1,1]\n" +
                "[[1],[2,3,4]]\n" +
                "[1,[2,[3,[4,[5,6,0]]]],8,9]\n" +
                "[1,[2,[3,[4,[5,6,7]]]],8,9]\n" +
                "[[1],4]\n" +
                "[[2]]\n" +
                "[3]\n" +
                "[[4,4],4,4]\n" +
                "[[4,4],4,4,4]\n" +
                "[[6]]\n" +
                "[7,7,7]\n" +
                "[7,7,7,7]\n" +
                "[[8,7,6]]\n" +
                "[9]", packets.joinToString("\n"))

        assertEquals(140, signal.getDecoderKey())
    }
}