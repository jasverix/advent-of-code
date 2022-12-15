import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DayEightTest {
    @Test
    fun testVisibleTrees() {
        val input = "30373\n" +
                "25512\n" +
                "65332\n" +
                "33549\n" +
                "35390\n"
        assertEquals(21, DayEight.Grid(input).countVisibleTrees())
        assertEquals(8, DayEight.Grid(input).highestScenicScore())
    }
}