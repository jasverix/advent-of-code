object DaySix {
    private fun isValidSequence(sequence: CharSequence): Boolean {
        val list = sequence.toList()
        return list.distinct().size == sequence.length
    }

    fun findFirstStartOfPacket(input: String): Int? {
        val range = 4 until input.length
        for(i in range) {
            val sequence = input.subSequence(i-4, i)
            if (isValidSequence(sequence)) return i
        }
        return null
    }

    fun findFirstStartOfMessage(input: String): Int? {
        val range = 14 until input.length
        for(i in range) {
            val sequence = input.subSequence(i-14, i)
            if (isValidSequence(sequence)) return i
        }
        return null
    }
}

fun main() {
    val input = DayFive::class.java.getResourceAsStream("input-06.txt")?.bufferedReader()?.readText() ?: return
    println("Chars until valid start: " + DaySix.findFirstStartOfPacket(input))
    println("Chars until valid start: " + DaySix.findFirstStartOfMessage(input))
}