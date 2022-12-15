
object DayOne {
    class Elf(input: String) {
        val carryCapacity = input.trim().split('\n').sumOf { l -> l.toInt() }
    }

    private fun getElves(input: String): List<Elf> = input.split("\n\n").map { i -> Elf(i) }

    fun getMaxCarryCapacity(input: String): Int = getElves(input).maxOf { e -> e.carryCapacity }

    fun getTopTreeCapacity(input: String): Int = getElves(input)
        .sortedByDescending { e -> e.carryCapacity }
        .take(3)
        .sumOf { e -> e.carryCapacity }
}

fun main() {
    val input = DayOne::class.java.getResourceAsStream("input-01.txt")?.bufferedReader()?.readText() ?: return
    println("Elf with most capacity: " + DayOne.getMaxCarryCapacity(input))
    println("Combined top tree capacities: " + DayOne.getTopTreeCapacity(input))
}