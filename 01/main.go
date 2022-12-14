package main

import (
	"adventure-of-code/utils"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func calculate(input string) int {
	elves := strings.Split(input, "\n\n")
	maxCarryCapacity := 0
	for _, elf := range elves {
		carryCapacity := getCarryCapacity(elf)
		if carryCapacity > maxCarryCapacity {
			maxCarryCapacity = carryCapacity
		}
	}
	return maxCarryCapacity
}

func getCarryCapacity(elfInput string) int {
	carryCapacity := 0
	calories := strings.Split(elfInput, "\n")
	for _, cal := range calories {
		calValue, err := strconv.Atoi(cal)
		if err == nil {
			carryCapacity += calValue
		}
	}
	return carryCapacity
}

func main() {
	fileBinary, fileReadingError := os.ReadFile(utils.RelativeFile("input.txt"))
	if fileReadingError != nil {
		fmt.Println("Error while reading input file: " + fileReadingError.Error())
	}

	// convert file contents to array of lines
	fileContents := strings.Trim(string(fileBinary), " \n\r")
	sum := calculate(fileContents)

	fmt.Printf("Result: %v\n", sum)
}
