import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DayFiveTest {
    @Test
    fun testTopCrates(){
        val input = "    [D]    \n" +
                "[N] [C]    \n" +
                "[Z] [M] [P]\n" +
                " 1   2   3 \n" +
                "\n" +
                "move 1 from 2 to 1\n" +
                "move 3 from 1 to 3\n" +
                "move 2 from 2 to 1\n" +
                "move 1 from 1 to 2"
        assertEquals("CMZ", DayFive.getTopCrates(input,1))
        assertEquals("MCD", DayFive.getTopCrates(input,2))
    }
}