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
                valueAsList = emptyList()
                valueAsInt = input.toInt()
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
                return i.split(',')
                    .map { v -> PacketValue(injectList(v, lists)) }
            } catch (e: IllegalArgumentException) {
                throw IllegalArgumentException("Could not parse input $input", e)
            }
        }

        private fun injectList(input: String, lists: List<String>): String {
            val matches = "\\$(\\d+)".toRegex().find(input) ?: return input
            val (listIndex) = matches.destructured
            return "[" + injectList(lists[listIndex.toInt()], lists) + "]"
        }

        fun isLessThan(value: PacketValue): Boolean? {
            if (isInt && value.isInt) {
                if (valueAsInt < value.valueAsInt) return true
                if (valueAsInt > value.valueAsInt) return false
                return null
            }
            if (!isInt && !value.isInt) return listIsLessThan(valueAsList, value.valueAsList)
            if (isInt) return listIsLessThan(listOf(this), value.valueAsList)
            return listIsLessThan(this.valueAsList, listOf(value))
        }

        private fun listIsLessThan(first: List<PacketValue>, second: List<PacketValue>): Boolean {
            for ((index, value) in first.withIndex()) {
                if (index > second.lastIndex) return false
                val isLessThan = value.isLessThan(second[index])
                if (isLessThan != null) return isLessThan
            }
            return second.size >= first.size
        }
    }

    class Packet(input: String) {
        private val value: PacketValue = PacketValue(input)

        fun isLessThan(other: Packet) = value.isLessThan(other.value) != false
    }

    class PacketPair(input: String) {
        private val pair = input.trim().split('\n')
            .map { l -> Packet(l) }
            .zipWithNext()
            .single()

        val isValid = pair.first.isLessThan(pair.second)
    }

    class Signal(input: String) {
        private val packets = input.trim().split("\n\n")
            .map { i -> PacketPair(i) }

        fun validPacketsIndexes() = packets.mapIndexed { index, packetPair -> if(packetPair.isValid) index + 1 else 0 }.filter { i -> i > 0 }

        fun validPacketsSum() = validPacketsIndexes().sum()
    }
}

fun main() {
    val input = DayThirteen::class.java.getResourceAsStream("input-13.txt")?.bufferedReader()?.readText() ?: return
    val signal = DayThirteen.Signal(input)
    println("Sum of valid packets: " + signal.validPacketsSum())
}