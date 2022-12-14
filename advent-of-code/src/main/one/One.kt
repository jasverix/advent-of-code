
class Elf(input: String) {
    val carryCapacity = input.trim().split('\n').sumOf { l -> l.toInt() }
}

fun getElves(input: String): List<Elf> {
    return input.split("\n\n")
        .map { i -> Elf(i) }
}

fun getMaxCarryCapacity(input: String): Int {
    return getElves(input).maxOf { e -> e.carryCapacity }
}

fun main() {
    val input = Elf::class.java.getResourceAsStream("input-01.txt")?.bufferedReader()?.readText()
    // val input = File("src/main/resources/input-01.txt").readText()
    if (input != null) {
        println(getMaxCarryCapacity(input))
    }
}