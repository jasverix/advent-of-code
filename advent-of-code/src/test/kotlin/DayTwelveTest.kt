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
        // map.markGoal()
        assertSame(31, map.getSteps())
    }

    @Test
    fun testFindBestLowLand() {
        val map = DayTwelve.Map(input)
        // map.markGoal()
        assertSame(29, map.findBestLowLandSteps())
    }
}