object DaySeven {
    interface IoObject {
        fun size(): Int
    }

    class File(private val size: Int): IoObject {
        override fun size(): Int = size
    }

    class Directory(private val name: String): IoObject {
        private var parentDirectory: Directory? = null
        private val children: MutableList<Directory> = mutableListOf()
        private val files: MutableList<File> = mutableListOf()

        private fun getChild(name: String): Directory? = children.firstOrNull { c -> c.name == name }

        private fun createChild(name: String): Directory {
            val child = Directory(name)
            child.parentDirectory = this
            children.add(child)
            return child
        }

        fun getOrCreateChild(name: String): Directory = getChild(name) ?: createChild(name)

        fun createFile(size: Int): File {
            val file = File(size)
            files.add(file)
            return file
        }

        override fun size(): Int {
            return children.sumOf { c -> c.size() } + files.sumOf { f -> f.size() }
        }

        fun flatten(): Set<Directory> {
            val structure = hashSetOf<Directory>()
            structure.add(this)
            return structure + children.map { c -> c.flatten() }.flatten().toHashSet()
        }

        fun getParentDirectory(): Directory = parentDirectory ?: this

        override fun equals(other: Any?): Boolean {
            if (this === other) return true
            if (javaClass != other?.javaClass) return false
            other as Directory
            if (name != other.name) return false
            if (parentDirectory == null && other.parentDirectory == null) return true
            return parentDirectory?.equals(other.parentDirectory) ?: false
        }

        override fun hashCode(): Int {
            var result = name.hashCode()
            result = 31 * result + (parentDirectory?.hashCode() ?: 0)
            return result
        }
    }

    class Command(input: String) {
        private val command: String
        private val arguments: List<String>
        private val output: List<String>
        init {
            val lines = input.split('\n').toMutableList()
            val commandLine = lines.removeFirst().trim().split(' ').toMutableList()
            command = commandLine.removeFirst()
            arguments = commandLine
            output = lines
        }

        fun run(terminal: Terminal) {
            when (command) {
                "cd" -> cd(terminal)
                "ls" -> ls(terminal)
                else -> throw IllegalArgumentException("Unrecognized command $command")
            }
        }

        private fun cd(terminal: Terminal) {
            terminal.changeDirectory(arguments.first())
        }

        private fun ls(terminal: Terminal) {
            val workingDirectory = terminal.getWorkingDirectory()
            output.map { l -> lsLineToFile(l, workingDirectory) }
        }

        private fun lsLineToFile(input: String, parentDirectory: Directory): IoObject {
            val parts = input.trim().split(' ')
            val name = parts[1]
            if (parts[0] == "dir") return parentDirectory.getOrCreateChild(name)
            val size = parts[0].toInt()
            return parentDirectory.createFile(size)
        }
    }

    class Terminal internal constructor() {
        private val rootDirectory: Directory = Directory("/")
        private var workingDirectory: Directory = rootDirectory

        fun changeDirectory(name: String) {
            workingDirectory = getDirectory(name)
        }

        private fun getDirectory(name: String): Directory {
            if (name == "/") return rootDirectory
            if (name == "..") return workingDirectory.getParentDirectory()
            return workingDirectory.getOrCreateChild(name)
        }

        fun getWorkingDirectory(): Directory = workingDirectory
        fun getRootDirectory(): Directory = rootDirectory
    }

    private val terminal = Terminal()
    private fun getCommands(input: String): List<Command> {
        return input.split('$')
            .map { l -> l.trim() }
            .filter { l -> l.isNotEmpty() }
            .map { l -> Command(l) }
    }

    fun getTotalSizeOfDirectoriesBelow(limit: Int): Int {
        var totalSize = 0
        for(d in terminal.getRootDirectory().flatten().sortedByDescending { d -> d.size() }) {
            val size = d.size()
            if (size > limit) continue
            totalSize += size
        }
        return totalSize
    }

    fun getTotalSizeOfDirectoryToDelete(totalDiskSpace: Int, requiredToUpdate: Int): Int {
        val usedSpace = terminal.getRootDirectory().size()
        val freeSpace = totalDiskSpace - usedSpace
        if (freeSpace >= requiredToUpdate) return 0
        val requiredToFree = requiredToUpdate - freeSpace
        for(d in terminal.getRootDirectory().flatten().sortedBy { d -> d.size() }) {
            if (d.size() < requiredToFree) continue
            return d.size()
        }
        return 0
    }

    fun runCommands(input: String) {
        getCommands(input).forEach { c -> c.run(terminal) }
    }
}

fun main() {
    val input = DaySeven::class.java.getResourceAsStream("input-07.txt")?.bufferedReader()?.readText() ?: return
    DaySeven.runCommands(input)
    println("Size: " + DaySeven.getTotalSizeOfDirectoriesBelow(100000))
    println("Size: " + DaySeven.getTotalSizeOfDirectoryToDelete(70000000, 30000000))
}
