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
        assertFalse(DayThirteen.Packet("[1,2,3]").isLessThan(DayThirteen.Packet("[1,2,2]")))
        assertTrue(DayThirteen.Packet("[1,2,3]").isLessThan(DayThirteen.Packet("[1,2,10]")))
        assertTrue(DayThirteen.Packet("[1,2]").isLessThan(DayThirteen.Packet("[1,2,1]")))
        assertFalse(DayThirteen.Packet("[1,2,1]").isLessThan(DayThirteen.Packet("[1,2]")))
    }

    @Test
    fun testPacketsValueListInList() {
        assertTrue(DayThirteen.Packet("[1,[1,2],3]").isLessThan(DayThirteen.Packet("[1,[3,4],5]")))
        assertFalse(DayThirteen.Packet("[1,[1,5],3]").isLessThan(DayThirteen.Packet("[1,[1,4],5]")))
        assertTrue(DayThirteen.Packet("[1,[1,2,3],3]").isLessThan(DayThirteen.Packet("[1,[3,4],5]")))
        assertTrue(DayThirteen.Packet("[1,[1,2],7]").isLessThan(DayThirteen.Packet("[1,[3,4],5]")))
        assertTrue(DayThirteen.Packet("[1,[1,2],3]").isLessThan(DayThirteen.Packet("[1,[3,4],[1,2]]")))
        assertTrue(DayThirteen.Packet("[1,[1,2],[1,2]]").isLessThan(DayThirteen.Packet("[1,[3,4],5]")))
    }

    @Test
    fun testCompareEmptyListWithZero() {
        assertFalse(
            DayThirteen.Packet("[[2,9,7,[[5,10,7,9,5],[7,5,9,8,10],3]],[[9,1]],[[[1],[],[4],4,[7,0,10,8]],[[],2,[]]],[10,[5,10],5,5]]").isLessThan(
                DayThirteen.Packet("[[],[[],4,4,7],[0,1,[],9]]")
            )
        )
    }

    @Test
    fun testPacketsValueListInListInList() {
        assertTrue(DayThirteen.Packet("[1,[1,[2,3]],3]").isLessThan(DayThirteen.Packet("[1,[3,[4,5]],5]")))
    }

    @Test
    fun testPacketsSum() {
        assertEquals(listOf(1,2,4,6), DayThirteen.Signal(input).validPacketsIndexes())
        assertEquals(13, DayThirteen.Signal(input).validPacketsSum())
    }
}