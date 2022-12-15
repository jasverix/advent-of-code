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
        assertEquals(157, DayThree.DayTree().getTotalPriority(input))
        assertEquals(70, DayThree.DayTree().getPrioritySumOfAllCommonItems(input))
    }
}