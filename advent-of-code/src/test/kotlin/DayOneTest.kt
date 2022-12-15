import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DayOneTest {
    @Test
    fun testCarryCapacity() {
        val input = "1000\n2000\n3000\n\n4000\n\n5000\n6000\n\n7000\n8000\n9000\n\n10000"
        assertEquals(24000, DayOne.getMaxCarryCapacity(input))
    }
}