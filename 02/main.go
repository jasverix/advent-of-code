package main

import (
	"fmt"
	"io/ioutil"
	"strings"
)

func main() {
	fileBinary, fileReadingError := ioutil.ReadFile("./input.txt")
	if fileReadingError != nil {
		fmt.Println("Error while reading input file")
	}

	// convert file contents to array of lines
	fileContents := strings.Trim(string(fileBinary), " \n\r")
	originalInput, err := makeIntegerArray(fileContents)

	// replacements due to earlier error (see task text)
	success := false
	noun := 1
	verb := 1
	expectedResult := 19690720

	fmt.Printf("Memory size: %v\n", len(originalInput))

	for !success {
		input := make([]int, len(originalInput))
		copy(input, originalInput)

		input[1] = noun
		input[2] = verb

		fmt.Printf("Trying noun: %v and verb %v ... - ", noun, verb)

		result, err := intcode(input)
		if err != nil {
			fmt.Printf("could not read - got error\n")
		} else {
			fmt.Printf("result: %v\n", result[0])

			if result[0] == expectedResult {
				break
			}
		}

		if noun < 99 {
			noun++
		} else {
			noun = 1
			verb++
		}

		if verb > 99 {
			fmt.Printf("Could not resolve values")
			return
		}
	}
	
	if err != nil {
		fmt.Printf("Error in intcode: %v", err)
		return
	}

	fmt.Printf("Result: %v", 100 * noun + verb)
}
