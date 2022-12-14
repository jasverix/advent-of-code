
enum class Shape(val score: Int) {
    Rock(1) {
        override fun beatenBy(): Shape = Paper
        override fun winsOver(): Shape = Scissors
    },
    Paper(2) {
        override fun beatenBy(): Shape = Scissors
        override fun winsOver(): Shape = Rock
    },
    Scissors(3) {
        override fun beatenBy(): Shape = Rock
        override fun winsOver(): Shape = Paper
    };

    fun fight(shape: Shape): Int {
        return when (true) {
            (shape == this) -> 3
            (shape == this.beatenBy()) -> 0
            (shape == this.winsOver()) -> 6
            else -> throw IllegalArgumentException("Illegal shape")
        }
    }

    abstract fun beatenBy(): Shape
    abstract fun winsOver(): Shape
}

class Round(private val input: String) {
    private fun getOpponentShape(input: String): Shape {
        return when(input) {
            "A" -> Shape.Rock
            "B" -> Shape.Paper
            "C" -> Shape.Scissors
            else -> throw IllegalArgumentException("Bad input")
        }
    }

    private fun getUserShape(input: String): Shape {
        return when(input) {
            "X" -> Shape.Rock
            "Y" -> Shape.Paper
            "Z" -> Shape.Scissors
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