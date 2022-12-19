object DayTwelve {
    const val UNEXPORED = Int.MAX_VALUE - 100
    const val UNREACHABLE = UNEXPORED - 10
    class Spot(private val elevation: Char, private val x: Int, private val y: Int, private val map: Map) {
        private var distanceTo: MutableMap<Spot, Int> = mutableMapOf()
        private var distanceFrom: MutableMap<Spot, Int> = mutableMapOf()

        private fun initDistanceTo() {
            if (distanceTo.isNotEmpty()) return
            distanceTo = map.getSpots().associateWith { UNEXPORED }.toMutableMap()
            distanceFrom = map.getSpots().associateWith { UNEXPORED }.toMutableMap()
            distanceTo[this] = 0
            distanceFrom[this] = 0
            enterableNeighbours().forEach { s -> distanceTo[s] = 1 }
        }

        fun elevation() = when (elevation) {
            'S' -> 'a'
            'E' -> 'z'
            else -> elevation
        }

        fun isStart() = elevation == 'S'

        fun isGoal() = elevation == 'E'

        fun stepsTo(spot: Spot): Int {
            initDistanceTo()
            while(distanceTo[spot] == UNEXPORED) {
                if(!explore()) distanceTo[spot] = UNREACHABLE
            }
            return distanceTo[spot]!!
        }

        private fun explore(): Boolean {
            val edges = getEdges()
            if(edges.isEmpty()) return false
            val from = edges.minBy { e -> e.value }.key
            from.enterableNeighbours().forEach { s ->
                val d = distanceTo[from]!! + 1
                distanceTo[s] = d
                s.distanceFrom[this] = d
            }
            return true
        }

        private fun getMappedSpots() = distanceTo.filter { t -> t.value < UNEXPORED }.map { t -> t.key }
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
        fun findBestLowLandSteps(): Int {
            val goal = getGoal()
            return getSpots().filter { s -> s.elevation() == 'a' }.minOf { s -> s.stepsTo(goal) }
        }
    }
}

fun main() {
    val input = DayTwelve::class.java.getResourceAsStream("input-12.txt")?.bufferedReader()?.readText() ?: return
    val map = DayTwelve.Map(input)
    // map.markGoal()
    println("Steps to goal: " + map.getSteps())
    println("Steps from best 'a': " + map.findBestLowLandSteps())
}