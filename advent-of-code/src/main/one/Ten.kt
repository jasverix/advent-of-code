object DayTen {
    class CPU {
        private val cycles: MutableList<Int> = mutableListOf(1)
        private val cycleCommands: MutableList<String> = mutableListOf("initial value")
        private var x = 1
        private val tickListeners: MutableList<()->Unit> = mutableListOf()

        fun addOnTickListener(handler: ()->Unit) {
            tickListeners.add(handler)
        }

        private fun tick() {
            cycles.add(x)
            tickListeners.forEach { h -> h.invoke() }
        }

        private fun addx(v: Int) {
            tick()
            cycleCommands.add("addx $v (processing)")
            tick()
            cycleCommands.add("addx $v")
            x += v
        }
        private fun noop() {
            tick()
            cycleCommands.add("noop")
        }
        private fun readCommandLine(input: String) {
            if (input == "noop") {
                noop()
                return
            }
            if (!input.contains(' ')) throw IllegalArgumentException("Illegal command line $input")
            val (cmd, v) = input.split(' ')
            if (cmd == "addx") {
                addx(v.toInt())
                return
            }
            throw IllegalArgumentException("Illegal command line $input")
        }

        fun readCommandSet(input: String) {
            input.trim().split('\n').forEach { l -> readCommandLine(l.trim()) }
        }

        fun getValueAtCycle(cycle: Int) = cycles[cycle]

        fun x(): Int = x

        fun getSignalStrength(cycle: Int) = cycles[cycle] * cycle

        fun getCommand(cycle: Int) = cycleCommands[cycle]

        fun getSignalStrengthOfRoundingCycles(): Int {
            var cycle = 20
            var sum = 0
            while(cycle < cycles.lastIndex) {
                sum += getSignalStrength(cycle)
                cycle += 40
            }
            return sum
        }
    }

    class CRT(private val cpu: CPU) {
        private val screen = (1..6).map { (1..40).map { ' ' }.toMutableList() }.toMutableList()
        private var row = 0
        private var pos = 0

        init {
            cpu.addOnTickListener { tick() }
        }

        private fun tick() {
            screen[row][pos] = draw()
            pos++
            if(pos > 39) {
                pos = 0
                row++
            }
        }

        private fun draw() = if(withinRange(cpu.x())) '#' else '.'

        private fun withinRange(x: Int): Boolean = x <= pos + 1 && x >= pos - 1

        fun drawScreen(): String = screen.joinToString("\n") { r -> r.joinToString("") }
    }
}

fun main() {
    val input = DayTen::class.java.getResourceAsStream("input-10.txt")?.bufferedReader()?.readText() ?: return
    val processor = DayTen.CPU()
    val gpu = DayTen.CRT(processor)
    processor.readCommandSet(input)
    println("Result: " + processor.getSignalStrengthOfRoundingCycles())
    println("")
    println(gpu.drawScreen())
}