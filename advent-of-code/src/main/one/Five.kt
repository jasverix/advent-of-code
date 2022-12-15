import java.util.Hashtable

object DayFive {
    class Crate(val key: Char) {
    }

    class Procedure(input: String) {
        private val steps = input.split('\n')
            .map { l -> ProcedureStep(l) }

        fun run(storage: Storage) {
            steps.forEach { s -> s.run(storage) }
        }
    }

    class ProcedureStep(input: String) {
        private val operation: String
        private val amount: Int
        private val fromIndex: Int
        private val toIndex: Int
        init {
            val regex = "(\\w+) (\\d+) from (\\d+) to (\\d+)".toRegex()
            val matchResult = regex.find(input)
            val (operation, amount, fromIndex, toIndex) = matchResult!!.destructured
            this.operation = operation
            this.amount = amount.toInt()
            this.fromIndex = fromIndex.toInt()
            this.toIndex = toIndex.toInt()
        }

        fun run(storage: Storage) {
            val from = storage.getStack(fromIndex)
            val to = storage.getStack(toIndex)
            from.move(amount, to)
        }
    }

    class Stack {
        private val crates: MutableList<Crate> = mutableListOf()

        fun add(crate: Crate) {
            crates.add(crate)
        }

        fun top(): Crate {
            return crates.last()
        }

        fun move(amount: Int, to: Stack) {
            if(amount == 0) return
            for(i in 1..amount) {
                to.add(crates.removeLast())
            }
        }
    }

    class Storage(input: String) {
        private val stacks: List<Stack>
        init {
            val lines = input.split('\n').toMutableList()
            val indexLine = lines.removeLast().toCharArray()
            val indexes = Hashtable<Int, Int>()
            for((i, char) in indexLine.withIndex()) {
                if(char == ' ') continue
                val index = char.digitToIntOrNull(10) ?: continue
                indexes[i] = index
            }
            val stacks = MutableList(indexes.size) { Stack() }
            for(line in lines.reversed()) {
                val lineChars = line.toCharArray()
                for((lineIndex, stackIndex) in indexes) {
                    val crateChar = lineChars[lineIndex]
                    if(crateChar == ' ') continue
                    stacks[stackIndex - 1].add(Crate(crateChar))
                }
            }
            this.stacks = stacks
        }

        fun getStack(index: Int): Stack {
            return stacks[index - 1]
        }

        fun getTopCrates(): List<Crate> {
            return stacks.map { s -> s.top() }
        }
    }

    private fun getStorageAndOperations(input: String): Pair<Storage, Procedure> {
        val parts = input.split("\n\n")
        return Pair(Storage(parts[0]), Procedure(parts[1]))
    }

    fun getTopCrates(input: String): String {
        val (storage, procedure) = getStorageAndOperations(input.trimEnd())
        procedure.run(storage)
        return storage.getTopCrates().map { c -> c.key }.joinToString("")
    }
}

fun main() {
    val input = DayFive::class.java.getResourceAsStream("input-05.txt")?.bufferedReader()?.readText() ?: return
    println("Top crates after movement is: " + DayFive.getTopCrates(input))
}