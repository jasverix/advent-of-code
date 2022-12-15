import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DayNineTest {
    @Test
    fun testRopeMovementsWithTwoSizedRope() {
        val r = DayNine.Rope(2)
        r.moveMany("R 4\n" +
                "U 4\n" +
                "L 3\n" +
                "D 1\n" +
                "R 4\n" +
                "D 1\n" +
                "L 5\n" +
                "R 2")
        assertSame(13, r.countVisitedPositions())
    }

    @Test
    fun testRopeMovementsWithTenSizedRope() {
        val r = DayNine.Rope(10)
        r.moveMany("R 4\n" +
                "U 4\n" +
                "L 3\n" +
                "D 1\n" +
                "R 4\n" +
                "D 1\n" +
                "L 5\n" +
                "R 2")
        assertSame(1, r.countVisitedPositions())
    }

    @Test
    fun testRopeMovementsWithTenSizedRopeBiggerMap() {
        val r = DayNine.Rope(10)
        r.moveMany("R 5\n" +
                "U 8\n" +
                "L 8\n" +
                "D 3\n" +
                "R 17\n" +
                "D 10\n" +
                "L 25\n" +
                "U 20")
        assertSame(36, r.countVisitedPositions())
    }
}