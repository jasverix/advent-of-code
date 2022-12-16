object DayTwelve {
    const val UNREACHABLE = Int.MAX_VALUE - 100
    class Spot(private val elevation: Char, private val x: Int, private val y: Int, private val map: Map) {
        private var distanceTo: MutableMap<Spot, Int> = mutableMapOf()

        private fun initDistanceTo() {
            if (distanceTo.isNotEmpty()) return
            distanceTo = map.getSpots().associateWith { UNREACHABLE }.toMutableMap()
            distanceTo[this] = 0
            enterableNeighbours().forEach { s -> distanceTo[s] = 1 }
        }

        private fun elevation() = when (elevation) {
            'S' -> 'a'
            'E' -> 'z'
            else -> elevation
        }

        fun isStart() = elevation == 'S'

        fun isGoal() = elevation == 'E'

        fun stepsTo(spot: Spot): Int {
            initDistanceTo()
            while(distanceTo[spot] == UNREACHABLE) {
                explore()
            }
            return distanceTo[spot]!!
        }

        private fun explore() {
            val from = getEdges().minBy { e -> e.value }.key
            from.enterableNeighbours().forEach { s ->
                distanceTo[s] = distanceTo[from]!! + 1
            }
        }

        private fun getMappedSpots() = distanceTo.filter { t -> t.value < UNREACHABLE }.map { t -> t.key }
        private fun getEdges(): kotlin.collections.Map<Spot, Int> {
            val mapped = getMappedSpots()
            return mapped.filter { s -> s.enterableNeighbours().any { n -> !mapped.contains(n) } }.associateWith { s -> distanceTo[s]!! }
        }

        private fun enterableNeighbours() = neighbours().filter { s -> canEnter(s) }
        private fun canEnter(spot: Spot) = spot.elevation().code <= elevation().code + 1

        private fun down(): Spot? = map.getSpot(x + 1, y)
        private fun up(): Spot? = map.getSpot(x - 1, y)
        private fun right(): Spot? = map.getSpot(x, y + 1)
        private fun left(): Spot? = map.getSpot(x, y - 1)

        private fun neighbours(): List<Spot> = listOfNotNull(up(), right(), down(), left())

        override fun equals(other: Any?): Boolean {
            if (this === other) return true
            if (javaClass != other?.javaClass) return false
            other as Spot
            return (elevation == other.elevation && x == other.x && y == other.y)
        }

        override fun hashCode(): Int = 31 * (31 * elevation.hashCode() + x) + y
    }

    class Map(input: String) {
        private val map = input.trim().split('\n')
            .mapIndexed { x, l -> l.toCharArray().mapIndexed { y, c -> Spot(c, x, y, this) } }

        fun getSpots() = map.flatten()

        fun getSpot(x: Int, y: Int): Spot? {
            return map.getOrNull(x)?.getOrNull(y)
        }

        private fun getStart() = map.firstNotNullOf { r -> r.firstOrNull { s -> s.isStart() } }
        private fun getGoal() = map.firstNotNullOf { r -> r.firstOrNull { s -> s.isGoal() } }

        fun getSteps() = getStart().stepsTo(getGoal())
    }
}

fun main() {
    val input = DayTwelve::class.java.getResourceAsStream("input-12.txt")?.bufferedReader()?.readText() ?: return
    val map = DayTwelve.Map(input)
    println("Steps to goal: " + map.getSteps())
}