object DayTwelve {
    const val UNREACHABLE = Int.MAX_VALUE - 10

    class Spot(private val elevation: Char, private val x: Int, private val y: Int, private val map: Map) {
        private var pathTo: MutableMap<Spot, List<Spot>> = mutableMapOf()
        private val unreachableDestinations: MutableSet<Spot> = mutableSetOf()

        fun elevation() = when (elevation) {
            'S' -> 'a'
            'E' -> 'z'
            else -> elevation
        }

        fun isStart() = elevation == 'S'

        fun isGoal() = elevation == 'E'

        fun stepsTo(destination: Spot): Int {
            val path = path(destination) ?: return UNREACHABLE
            return path.size
        }

        private fun path(destination: Spot): List<Spot>? {
            if (pathTo.containsKey(destination)) return pathTo[destination]
            val path = path(destination, emptySet())
            if (path == null) {
                unreachableDestinations.add(destination)
                return null
            }
            storePath(destination, path)
            return path
        }

        private fun path(destination: Spot, visitedSpots: Set<Spot>): List<Spot>? {
            if (this == destination) return emptyList()
            if (visitedSpots.contains(this)) return null
            if (unreachableDestinations.contains(destination)) return null
            if (pathTo.containsKey(destination)) return pathTo[destination]
            val neighbours = enterableNeighbours()
            if (neighbours.isEmpty()) return null
            val possiblePaths = neighbours.mapNotNull { n -> n.path(destination, copyWithThis(visitedSpots)) }
            if (possiblePaths.isEmpty()) return null
            val p = possiblePaths.minBy { p -> p.size }.toMutableList()
            p.add(0, this)
            return p
        }

        private fun storePath(destination: Spot, path: List<Spot>) {
            val p = path.toMutableList()
            while (p.size > 0) {
                val t = p.removeFirst()
                t.pathTo[destination] = t.copyWithThis(p.toList())
            }
        }

        private fun copyWithThis(visitedSpots: Set<Spot>): Set<Spot> {
            val v = visitedSpots.toMutableSet()
            v.add(this)
            return v
        }

        private fun copyWithThis(visitedSpots: List<Spot>): List<Spot> {
            val v = visitedSpots.toMutableList()
            v.add(0, this)
            return v
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

        override fun toString(): String = "Spot(elevation=$elevation, x=$x, y=$y)"
    }

    class Map(input: String) {
        private val map = input.trim().split('\n')
            .mapIndexed { x, l -> l.toCharArray().mapIndexed { y, c -> Spot(c, x, y, this) } }

        private fun getSpots() = map.flatten()

        fun getSpot(x: Int, y: Int): Spot? {
            return map.getOrNull(x)?.getOrNull(y)
        }

        private fun getStart() = map.firstNotNullOf { r -> r.firstOrNull { s -> s.isStart() } }
        private fun getGoal() = map.firstNotNullOf { r -> r.firstOrNull { s -> s.isGoal() } }

        fun getSteps() = getStart().stepsTo(getGoal())
        fun findBestLowLandSteps(): Int {
            val goal = getGoal()
            val spot = getSpots().filter { s -> s.elevation() == 'a' }.minBy { s -> s.stepsTo(goal) }
            println(spot)
            return spot.stepsTo(goal)
        }
    }
}

fun main() {
    val input = DayTwelve::class.java.getResourceAsStream("input-12.txt")?.bufferedReader()?.readText() ?: return
    val map = DayTwelve.Map(input)
    println("Steps to goal: " + map.getSteps())
    println("Steps from best 'a': " + map.findBestLowLandSteps())
}