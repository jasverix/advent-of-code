import java.lang.RuntimeException

object DayElleven {
    class Item(private var worryLevel: Long) {
        fun runOperation(operation: Operation) {
            val old = worryLevel
            worryLevel = operation.getNewValue(worryLevel)
            if (worryLevel < 0) throw RuntimeException("Too low worry level after operation $operation where old value was $old")
        }

        fun relax() {
            worryLevel /= 3
        }

        fun relaxByModulo(mod: Long) {
            worryLevel %= mod
        }

        fun getWorryLevel() = worryLevel
    }

    class Operation(input: String) {
        private val mathLine = input.split('=').last().trim()
        fun getNewValue(value: Long): Long {
            val math = mathLine.replace("old", value.toString())
            val matchResult = "(-?\\d+)\\s([*+])\\s(-?\\d+)".toRegex().find(math)
                ?: throw RuntimeException("Could not parse math «$math»")
            val (firstNumberStr, operator, lastNumberStr) = matchResult.destructured
            return doMath(firstNumberStr.toLong(), operator.first(), lastNumberStr.toLong())
        }

        private fun doMath(firstNumber: Long, operator: Char, lastNumber: Long): Long {
            return when(operator) {
                '*' -> firstNumber * lastNumber
                '+' -> firstNumber + lastNumber
                else -> throw IllegalArgumentException("Unknown operator $operator")
            }
        }

        override fun toString(): String = "Operation(mathLine='$mathLine')"
    }

    class Test(input: List<String>, monkeys: Monkeys) {
        private val divisibleBy: Long
        private val trueAction: Action
        private val falseAction: Action
        init {
            val matchResult = "divisible by (\\d+)".toRegex().find(input[0])
            val (divisibleBy) = matchResult!!.destructured
            this.divisibleBy = divisibleBy.toLong()
            this.trueAction = Action(input[1].split(':').last().trim(), monkeys)
            this.falseAction = Action(input[2].split(':').last().trim(), monkeys)
        }

        private fun matches(value: Long): Boolean = value % divisibleBy == 0.toLong()

        fun handleItem(item: Item) {
            if(matches(item.getWorryLevel())) {
                trueAction.execute(item)
            } else {
                falseAction.execute(item)
            }
        }

        fun getDivisibleBy() = divisibleBy
    }

    class Action(input: String, private val monkeys: Monkeys) {
        private val monkeyIndex: Int
        init {
            val matchResult = "throw to monkey (\\d+)".toRegex().find(input)
            val (monkeyIndex) = matchResult!!.destructured
            this.monkeyIndex = monkeyIndex.toInt()
        }

        fun execute(item: Item) {
            monkeys.throwItemToMonkey(item, monkeyIndex)
        }
    }

    class Monkey(input: String, private val monkeys: Monkeys) {
        private val items: MutableList<Item>
        private val operation: Operation
        private val test: Test
        private var itemsInspected: Long = 0
        private var modulo: Long = 0
        init {
            val lines = input.split('\n').toMutableList()
            lines.removeFirst()
            items = parseItems(lines.removeFirst()).toMutableList()
            operation = parseOperation(lines.removeFirst())
            test = Test(lines, monkeys)
        }

        private fun parseItems(line: String): List<Item> =
                line.split(':').last().split(',').map { n -> Item(n.trim().toLong()) }

        private fun parseOperation(line: String): Operation =
                Operation(line.split(':').last().trim())

        fun turn() {
            while (items.size > 0) {
                val item = items.removeFirst()
                handleItem(item)
            }
        }

        private fun handleItem(item: Item) {
            ++itemsInspected
            item.runOperation(operation)
            if(monkeys.canRelax()) item.relax()
            else item.relaxByModulo(modulo)
            test.handleItem(item)
        }

        fun receiveItem(item: Item) {
            items.add(item)
        }

        fun itemsInspected(): Long = itemsInspected

        fun getDivisibleBy() = test.getDivisibleBy()

        fun setModulo(modulo: Long) {
            this.modulo = modulo
        }
    }

    inline fun <T> Iterable<T>.multiplyBy(selector: (T) -> Long): Long {
        this.firstOrNull() ?: return 0
        var result: Long = 1
        for (element in this) result *= selector(element)
        return result
    }

    class Monkeys(input: String) {
        private val monkeys = input.split("\n\n").map { t -> Monkey(t, this) }
        private var canRelax: Boolean = true

        private fun round() {
            monkeys.forEach { m -> m.turn() }
        }

        fun stressOut() {
            canRelax = false
            val modulo = monkeys.multiplyBy { m -> m.getDivisibleBy() }
            monkeys.forEach { m -> m.setModulo(modulo) }
        }

        fun canRelax() = canRelax

        fun throwItemToMonkey(item: Item, monkeyIndex: Int) {
            monkeys[monkeyIndex].receiveItem(item)
        }

        fun runRounds(rounds: Int) {
            (1..rounds).forEach { _ -> round() }
        }

        fun getMonkeyBusinessLevel(): Long =
                monkeys.sortedByDescending { m -> m.itemsInspected() }
                    .take(2)
                    .multiplyBy { m -> m.itemsInspected() }
    }
}

fun main() {
    val input = DayElleven::class.java.getResourceAsStream("input-11.txt")?.bufferedReader()?.readText() ?: return
    val monkeys = DayElleven.Monkeys(input)
    monkeys.runRounds(20)
    println("Monkey business level: " + monkeys.getMonkeyBusinessLevel())

    val monkeys2 = DayElleven.Monkeys(input)
    monkeys2.stressOut()
    monkeys2.runRounds(10000)
    println("Monkey business level stressed: " + monkeys2.getMonkeyBusinessLevel())
}
