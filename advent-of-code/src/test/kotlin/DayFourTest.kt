import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DayFourTest {
    @Test
    fun testOverlapping(){
        val input = "2-4,6-8\n" +
                "2-3,4-5\n" +
                "5-7,7-9\n" +
                "2-8,3-7\n" +
                "6-6,4-6\n" +
                "2-6,4-8"
        assertEquals(2, DayFour.getOverlappingPairCount(input))
        assertEquals(4, DayFour.getIntersectingPairCount(input))
    }
}