
class Elf(input: String) {
    val carryCapacity = input.trim().split('\n').sumOf { l -> l.toInt() }
}

fun getElves(input: String): List<Elf> = input.split("\n\n").map { i -> Elf(i) }

fun getMaxCarryCapacity(input: String): Int = getElves(input).maxOf { e -> e.carryCapacity }

fun getTopTreeCapacity(input: String): Int = getElves(input)
    .sortedByDescending { e -> e.carryCapacity }
    .take(3)
    .sumOf { e -> e.carryCapacity }

fun main() {
    val input = Elf::class.java.getResourceAsStream("input-01.txt")?.bufferedReader()?.readText()
    if (input != null) {
        println("Elf with most capacity: " + getMaxCarryCapacity(input))
        println("Compined top tree capacities: " + getTopTreeCapacity(input))
    }
}