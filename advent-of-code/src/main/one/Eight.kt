object DayEight {
    class Tree(private val height: Int, private val grid: Grid, private val x: Int, private val y: Int) {
        private enum class Direction {
            North {
                override fun go(tree: Tree): Tree? = tree.grid.getTree(tree.x, tree.y - 1)
            },
            South {
                override fun go(tree: Tree): Tree? = tree.grid.getTree(tree.x, tree.y + 1)
            },
            West {
                override fun go(tree: Tree): Tree? = tree.grid.getTree(tree.x - 1, tree.y)
            },
            East {
                override fun go(tree: Tree): Tree? = tree.grid.getTree(tree.x + 1, tree.y)
            };

            abstract fun go(tree: Tree): Tree?
        }

        private fun visibleInDirection(direction: Direction): Boolean =
                treeVisibleInDirection(direction.go(this), direction)
        private fun treeVisibleInDirection(nextTree: Tree?, direction: Direction): Boolean {
            if (nextTree == null) return true
            if (nextTree.height >= height) return false
            return treeVisibleInDirection(direction.go(nextTree), direction)
        }
        fun isVisible(): Boolean =
                visibleInDirection(Direction.North) || visibleInDirection(Direction.East)
                        || visibleInDirection(Direction.South) || visibleInDirection(Direction.West)

        private fun countVisibleTreesInDirection(direction: Direction): Int {
            var countVisibleTrees = 0
            var tree: Tree? = this
            while (tree != null) {
                tree = direction.go(tree) ?: return countVisibleTrees
                countVisibleTrees++
                if (tree.height >= height) return countVisibleTrees
            }
            return countVisibleTrees
        }

        fun scenicScore(): Int =
                countVisibleTreesInDirection(Direction.North) * countVisibleTreesInDirection(Direction.East) *
                        countVisibleTreesInDirection(Direction.South) * countVisibleTreesInDirection(Direction.West)
    }

    class Grid(input: String) {
        private val grid: List<List<Tree>> = input.trim().split('\n')
            .mapIndexed { y, l -> l.toCharArray().mapIndexed { x, c -> Tree(c.digitToInt(), this, x, y) } }

        fun getTree(x: Int, y: Int): Tree? = grid.getOrNull(y)?.getOrNull(x)

        fun countVisibleTrees(): Int = grid.sumOf { r -> r.count { t -> t.isVisible() } }

        fun highestScenicScore(): Int = grid.maxOf { r -> r.maxOf { t -> t.scenicScore() } }
    }
}

fun main() {
    val input = DayEight::class.java.getResourceAsStream("input-08.txt")?.bufferedReader()?.readText() ?: return
    val grid = DayEight.Grid(input)
    println("Visible trees: " + grid.countVisibleTrees())
    println("Highest score: " + grid.highestScenicScore())
}
