object DayThirteen {
    internal class PacketValue(input: String) {
        private val valueAsInt: Int
        private val valueAsList: List<PacketValue>
        private val isInt: Boolean

        init {
            if (input.isEmpty()) {
                throw IllegalArgumentException("Input cannot be empty in Packet")
            }
            if (input.first() == '[' && input.last() == ']') {
                valueAsList = parseList(input)
                valueAsInt = 0
                isInt = false
            } else {
                valueAsInt = input.toInt()
                valueAsList = listOf(this)
                isInt = true
            }
        }

        private fun parseList(input: String): List<PacketValue> {
            if (input == "[]") return emptyList()
            var i = input.substring(1, input.lastIndex) // remove the []
            val lists: MutableList<String> = mutableListOf()
            val listFindRegex = "\\[([\\d,\\s$]*)]".toRegex()
            while (listFindRegex.containsMatchIn(i)) {
                i = i.replace(listFindRegex) { m ->
                    val (listString) = m.destructured
                    lists.add(listString)
                    "$" + lists.lastIndex
                }
            }
            try {
                return i.split(',').map { v -> PacketValue(injectList(v, lists)) }
            } catch (e: IllegalArgumentException) {
                throw IllegalArgumentException("Could not parse input $input", e)
            }
        }

        private fun injectList(input: String, lists: List<String>): String {
            val rx = "\\$(\\d+)".toRegex()
            return input.replace(rx) { matches ->
                val (listIndex) = matches.destructured
                "[" + injectList(lists[listIndex.toInt()], lists) + "]"
            }
        }

        operator fun compareTo(other: PacketValue): Int {
            if (isInt && other.isInt) return valueAsInt.compareTo(other.valueAsInt)
            return compareListTo(valueAsList, other.valueAsList)
        }

        private companion object {
            private fun compareListTo(first: List<PacketValue>, second: List<PacketValue>): Int {
                for ((index, value) in first.withIndex()) {
                    if (index > second.lastIndex) return 1
                    val isLessThan = value.compareTo(second[index])
                    if (isLessThan != 0) return isLessThan
                }
                return first.size.compareTo(second.size)
            }
        }
    }

    class Packet(input: String) {
        private val value: PacketValue = PacketValue(input)

        operator fun compareTo(other: Packet) = value.compareTo(other.value)
    }

    class PacketPair(input: String) {
        private val pair = input.trim().split('\n').map { l -> Packet(l) }.zipWithNext().single()

        val isValid = pair.first <= pair.second
    }

    class Signal(input: String) {
        private val packets = input.trim().split("\n\n").map { i -> PacketPair(i) }

        fun validPacketsIndexes() = packets.mapIndexed { index, packetPair -> if (packetPair.isValid) index + 1 else 0 }
            .filter { i -> i > 0 }

        fun validPacketsSum() = validPacketsIndexes().sum()
    }
}

fun main() {
    val input = DayThirteen::class.java.getResourceAsStream("input-13.txt")?.bufferedReader()?.readText() ?: return
    val signal = DayThirteen.Signal(input)
    println("Sum of valid packets: " + signal.validPacketsSum())
}

// 6208 - too high