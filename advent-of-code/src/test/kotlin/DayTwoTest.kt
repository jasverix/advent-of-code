import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DayTwoTest {
    @Test
    fun testTotalScore() {
        val input = "A Y\n" +
                "B X\n" +
                "C Z"
        assertEquals(12, DayTwo.getTotalScore(input))
    }
}