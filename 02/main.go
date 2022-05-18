package main

import (
	"adventure-of-code/utils"
	"errors"
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

func makeIntegerArray(input string) ([]int, error) {
	inputStrings := strings.Split(input, ",")
	inputs := make([]int, len(inputStrings))

	for index, element := range inputStrings {
		inputInteger, parseIntError := strconv.Atoi(element)
		if parseIntError != nil {
			return nil, parseIntError
		}

		inputs[index] = inputInteger
	}

	return inputs, nil
}

func execute(inputs []int) ([]int, error) {
	length := len(inputs)

	for i := 0; i < length; i += 4 {
		input := inputs[i]
		if input == 99 {
			break
		}

		if i+2 >= length {
			return nil, errors.New("index out of range")
		}

		firstPosition := inputs[i+1]
		secondPosition := inputs[i+2]
		resultPosition := inputs[i+3]

		if firstPosition >= length || secondPosition >= length || resultPosition >= length {
			return nil, errors.New("index out of range")
		}

		firstValue := inputs[firstPosition]
		secondValue := inputs[secondPosition]

		switch input {
		case 1: // add
			inputs[resultPosition] = firstValue + secondValue
		case 2: // multiply
			inputs[resultPosition] = firstValue * secondValue
		}
	}

	return inputs, nil
}

func intcode(input []int) ([]int, error) {
	return execute(input)
}

func main() {
	fileBinary, fileReadingError := ioutil.ReadFile(utils.RelativeFile("/input.txt"))
	if fileReadingError != nil {
		fmt.Println("Error while reading input file")
	}

	// convert file contents to array of lines
	fileContents := strings.Trim(string(fileBinary), " \n\r")
	originalInput, err := makeIntegerArray(fileContents)

	// replacements due to earlier error (see task text)
	noun := 1
	verb := 1
	expectedResult := 19690720

	fmt.Printf("Memory size: %v\n", len(originalInput))

	for true {
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

	fmt.Printf("Result: %v", 100*noun+verb)
}
