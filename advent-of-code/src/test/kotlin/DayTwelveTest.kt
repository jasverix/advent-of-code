import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DayTwelveTest {
    private val input = "Sabqponm\n" +
            "abcryxxl\n" +
            "accszExk\n" +
            "acctuvwj\n" +
            "abdefghi"
    @Test
    fun testSteps() {
        val map = DayTwelve.Map(input)
        assertSame(31, map.getSteps())
    }
}