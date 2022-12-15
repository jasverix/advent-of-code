
object DayTwo {
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

        fun fight(shape: Shape): Int = when (true) {
            (shape == this) -> 3
            (shape == this.beatenBy()) -> 0
            (shape == this.winsOver()) -> 6
            else -> throw IllegalArgumentException("Illegal shape")
        }

        abstract fun beatenBy(): Shape
        abstract fun winsOver(): Shape
    }

    class Round(private val input: String) {
        private fun getOpponentShape(input: String): Shape = when (input) {
            "A" -> Shape.Rock
            "B" -> Shape.Paper
            "C" -> Shape.Scissors
            else -> throw IllegalArgumentException("Bad input")
        }

        fun getUserScore(): Int {
            val data = input.split(("\\s+".toRegex()))
            val opponentShape = getOpponentShape(data[0])
            val userShape = when (data[1]) {
                "X" -> opponentShape.winsOver()
                "Y" -> opponentShape
                "Z" -> opponentShape.beatenBy()
                else -> throw IllegalArgumentException("Bad input")
            }
            return userShape.fight(opponentShape) + userShape.score
        }
    }

    fun getTotalScore(input: String): Int = input
        .trim().split("\n")
        .sumOf { l -> Round(l).getUserScore() }
}

fun main() {
    val input = DayTwo::class.java.getResourceAsStream("input-02.txt")?.bufferedReader()?.readText() ?: return
    println("Score: " + DayTwo.getTotalScore(input))
}