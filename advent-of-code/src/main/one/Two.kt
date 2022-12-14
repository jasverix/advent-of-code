
abstract class Shape(val score: Int) {
    abstract fun fight(shape: Shape): Int
}

class Rock: Shape(1) {
    override fun fight(shape: Shape): Int {
        return when(true) {
            (shape is Rock) -> 3
            (shape is Paper) -> 0
            (shape is Scissors) -> 6
            else -> 0
        }
    }
}

class Paper: Shape(2) {
    override fun fight(shape: Shape): Int {
        return when(true) {
            (shape is Rock) -> 6
            (shape is Paper) -> 3
            (shape is Scissors) -> 0
            else -> 0
        }
    }
}

class Scissors: Shape(3) {
    override fun fight(shape: Shape): Int {
        return when(true) {
            (shape is Rock) -> 0
            (shape is Paper) -> 6
            (shape is Scissors) -> 3
            else -> 0
        }
    }

}

class Round(private val input: String) {
    private fun getOpponentShape(input: String): Shape {
        return when(input) {
            "A" -> Rock()
            "B" -> Paper()
            "C" -> Scissors()
            else -> throw IllegalArgumentException("Bad input")
        }
    }

    private fun getUserShape(input: String): Shape {
        return when(input) {
            "X" -> Rock()
            "Y" -> Paper()
            "Z" -> Scissors()
            else -> throw IllegalArgumentException("Bad input")
        }
    }

    fun getUserScore(): Int {
        val data = input.split(("\\s+".toRegex()))
        val userShape = getUserShape(data[1])
        return userShape.fight(getOpponentShape(data[0])) + userShape.score
    }
}

fun getTotalScore(input: String): Int {
    return input.trim().split("\n")
        .sumOf { l -> Round(l).getUserScore() }
}

fun main() {
    val input = Elf::class.java.getResourceAsStream("input-02.txt")?.bufferedReader()?.readText()
    if (input != null) {
        println("Score: " + getTotalScore(input))
    }
}