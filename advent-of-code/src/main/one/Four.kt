object DayFour {
    class Elf(input: String) {
        private val range: IntRange
        init {
            val regex = "(\\d+)-(\\d+)".toRegex()
            val matches = regex.find(input)
            val (fromStr, toStr) = matches!!.destructured
            val from = fromStr.toInt()
            val to = toStr.toInt()
            range = from..to
        }

        infix fun intersect(other: Elf): Set<Int> = range intersect other.range

        fun isContainedBy(sections: Collection<Int>): Boolean = sections.containsAll(range.toList())
    }

    class Pair(input: String) {
        private val first: Elf
        private val second: Elf
        init {
            val elves = input.split(',').map { l -> Elf(l) }
            first = elves[0]
            second = elves[1]
        }

        fun isIntersecting(): Boolean = (first intersect second).isNotEmpty()

        fun oneElfIsFullyContained(): Boolean {
            val intersect = first intersect second
            return first.isContainedBy(intersect) || second.isContainedBy(intersect)
        }
    }

    private fun getPairs(input: String): List<Pair> = input.split('\n').map { l -> Pair(l) }

    fun getOverlappingPairCount(input: String): Int {
        return getPairs(input).count { p -> p.oneElfIsFullyContained() }
    }

    fun getIntersectingPairCount(input: String): Int {
        return getPairs(input).count { p -> p.isIntersecting() }
    }
}

fun main() {
    val input = DayFour::class.java.getResourceAsStream("input-04.txt")?.bufferedReader()?.readText() ?: return
    println("Pairs with overlapping sections: " + DayFour.getOverlappingPairCount(input.trim()))
    println("Pairs with intersecting sections: " + DayFour.getIntersectingPairCount(input.trim()))
}
