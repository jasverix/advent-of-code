
object DayThree {
    class Item(private val key: Char) {
        private val isLowerCase = 'a'.code <= key.code && key.code <= 'z'.code
        private val isUpperCase = 'A'.code <= key.code && key.code <= 'Z'.code

        val priority = when (true) {
            (isLowerCase) -> key.code - 'a'.code + 1
            (isUpperCase) -> key.code - 'A'.code + 27
            else -> throw IllegalArgumentException("Char to Item must be A-Z or a-z, $key given")
        }

        override fun equals(other: Any?): Boolean {
            if (this === other) return true
            if (javaClass != other?.javaClass) return false
            return key == (other as Item).key
        }

        override fun hashCode(): Int {
            return key.hashCode()
        }
    }

    class Compartment(private val items: HashSet<Item>) {
        fun contains(item: Item): Boolean = items.contains(item)

        fun commonItems(other: Compartment): List<Item> =
                other.items.filter { i -> contains(i) }

        infix fun join(other: Compartment): Set<Item> = items.toMutableSet() + other.items.toMutableSet()
    }

    class RackSack(itemsInput: String) {
        private val firstCompartment: Compartment
        private val secondCompartment: Compartment

        init {
            val itemsInputTrimmed = itemsInput.trim()
            firstCompartment = Compartment(itemsInputTrimmed.substring(0, (itemsInputTrimmed.length / 2))
                .toCharArray()
                .map { c -> Item(c) }
                .toHashSet()
            )
            secondCompartment = Compartment(itemsInputTrimmed.substring((itemsInputTrimmed.length / 2))
                .toCharArray()
                .map { c -> Item(c) }
                .toHashSet()
            )
        }

        private val collidingItems = firstCompartment.commonItems(secondCompartment)
        val collidingItemsTotalPriority = collidingItems.sumOf { i -> i.priority }

        private fun getItems(): Set<Item> = firstCompartment join secondCompartment
        private fun contains(item: Item): Boolean = firstCompartment.contains(item) || secondCompartment.contains(item)

        fun commonItems(other: RackSack): List<Item> {
            return getItems().filter { i -> other.contains(i) }
        }

        fun commonItems(other: Collection<Item>): List<Item> {
            return getItems().filter { i -> other.contains(i) }
        }
    }

    class DayTree {
        private fun getRackSacks(input: String): List<RackSack> = input.split('\n').map { l -> RackSack(l) }

        fun getTotalPriority(input: String): Int =
                getRackSacks(input).sumOf { rackSack -> rackSack.collidingItemsTotalPriority }

        private fun getElfGroups(rackSacks: List<RackSack>): List<List<RackSack>> = rackSacks.chunked(3)
        private fun getCommonItemInGroup(group: List<RackSack>): List<Item> {
            var commonItems: List<Item>? = null
            var lastSack: RackSack? = null
            for (rackSack in group) {
                if (lastSack == null) {
                    lastSack = rackSack
                    continue
                }
                if (commonItems == null) {
                    commonItems = lastSack.commonItems(rackSack)
                    continue
                }
                commonItems = rackSack.commonItems(commonItems)
            }

            if (commonItems == null) return emptyList()
            return commonItems
        }

        fun getPrioritySumOfAllCommonItems(input: String): Int {
            return getElfGroups(getRackSacks(input))
                .map { g -> getCommonItemInGroup(g) }
                .sumOf { l -> l.sumOf { i -> i.priority } }
        }
    }
}

fun main() {
    val input = DayThree::class.java.getResourceAsStream("input-03.txt")?.bufferedReader()?.readText() ?: return
    println("Score: " + DayThree.DayTree().getTotalPriority(input))
    println("Sum of all common items: " + DayThree.DayTree().getPrioritySumOfAllCommonItems(input))
}