import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.Test

class DaySixTest {
    @Test
    fun testValidSequence() {
        assertEquals(7, DaySix.findFirstStartOfPacket("mjqjpqmgbljsphdztnvjfqwrcgsmlb"))
        assertEquals(5, DaySix.findFirstStartOfPacket("bvwbjplbgvbhsrlpgdmjqwftvncz"))
        assertEquals(6, DaySix.findFirstStartOfPacket("nppdvjthqldpwncqszvftbrmjlhg"))
        assertEquals(10, DaySix.findFirstStartOfPacket("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"))
        assertEquals(11, DaySix.findFirstStartOfPacket("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"))

        assertEquals(19, DaySix.findFirstStartOfMessage("mjqjpqmgbljsphdztnvjfqwrcgsmlb"))
        assertEquals(23, DaySix.findFirstStartOfMessage("bvwbjplbgvbhsrlpgdmjqwftvncz"))
        assertEquals(23, DaySix.findFirstStartOfMessage("nppdvjthqldpwncqszvftbrmjlhg"))
        assertEquals(29, DaySix.findFirstStartOfMessage("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg"))
        assertEquals(26, DaySix.findFirstStartOfMessage("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"))
    }
}