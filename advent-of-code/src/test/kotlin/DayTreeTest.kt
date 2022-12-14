import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DayTreeTest {
    @Test
    fun testPriority() {
        val input = "vJrwpWtwJgWrhcsFMMfFFhFp\n" +
                "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL\n" +
                "PmmdzqPrVvPwwTWBwg\n" +
                "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn\n" +
                "ttgJtRGJQctTZtZT\n" +
                "CrZsJsPPZsGzwwsLwLmpwMDw"
        assertEquals(157, DayTree().getTotalPriority(input))
        assertEquals(70, DayTree().getPrioritySumOfAllCommonItems(input))
    }
}