object DayNine {
    class Position(val x: Int, val y: Int) {
        fun north(): Position = Position(x, y - 1)
        fun south(): Position = Position(x, y + 1)
        fun west(): Position = Position(x - 1, y)
        fun east(): Position = Position(x + 1, y)

        fun edgeCloser(pos: Position): Position {
            val closerPosition = getCloserPosition(pos)
            return if (closerPosition == pos) this else closerPosition
        }

        private fun getCloserPosition(pos: Position): Position {
            if (x == pos.x && y == pos.y) return pos
            if (x == pos.x) return if (y < pos.y) Position(x, y + 1) else Position(x, y - 1)
            if (y == pos.y) return if (x < pos.x) Position(x + 1, y) else Position(x - 1, y)
            return Position(if (x < pos.x) x + 1 else x - 1, if (y < pos.y) y + 1 else y - 1)
        }

        override fun equals(other: Any?): Boolean {
            if (this === other) return true
            if (javaClass != other?.javaClass) return false
            other as Position
            return (x == other.x) && (y == other.y)
        }

        override fun hashCode(): Int = 31 * x + y
    }

    class Rope(size: Int) {
        private var headPosition = Position(0, 0)
        private var inBetweens = List(size - 2) { Position(0, 0) }
        private var tailPosition = Position(0, 0)
        private val visitedPositionsByTail: HashSet<Position> = hashSetOf(tailPosition)

        fun moveMany(input: String) {
            input.trim().split('\n').forEach { l -> move(l) }
        }
        private fun move(input: String) {
            val parts = input.split(' ')
            return move(parts[0].first(), parts[1].toInt())
        }
        private fun move(dir: Char, length: Int) {
            (1..length).forEach { _ -> move(dir) }
        }
        private fun move(dir: Char) {
            headPosition = when (dir) {
                'R' -> headPosition.east()
                'L' -> headPosition.west()
                'U' -> headPosition.north()
                'D' -> headPosition.south()
                else -> throw IllegalArgumentException("Unrecognized command $dir")
            }
            var lastPosition = headPosition
            inBetweens = inBetweens.map { position ->
                val newPosition = position.edgeCloser(lastPosition)
                lastPosition = newPosition
                newPosition
            }
            tailPosition = tailPosition.edgeCloser(lastPosition)
            visitedPositionsByTail.add(tailPosition)
        }

        fun countVisitedPositions(): Int = visitedPositionsByTail.size
    }
}

fun main() {
    val input = DayNine::class.java.getResourceAsStream("input-09.txt")?.bufferedReader()?.readText() ?: return
    val twoSizedRope = DayNine.Rope(2)
    twoSizedRope.moveMany(input)
    println("Count of movements with rope 2 length: " + twoSizedRope.countVisitedPositions())

    val tenSizedRope = DayNine.Rope(10)
    tenSizedRope.moveMany(input)
    println("Count of movements with rope 10 length: " + tenSizedRope.countVisitedPositions())
}