package main

import (
	"strings"
	"strconv"
)

func makeIntegerArray(input string) ([]int64, error) {
	inputStrings := strings.Split(input, ",")
	inputs := make([]int64, len(inputStrings))

	for index, element := range inputStrings {
		inputInteger, parseIntError := strconv.ParseInt(element, 10, 64)
		if parseIntError != nil {
			return nil, parseIntError
		}

		inputs[index] = inputInteger
	}

	return inputs, nil
}

func execute(inputs []int64) []int64 {
	length := len(inputs)

	for i := 0; i < length; i += 4 {
		input := inputs[i]
		if input == 99 {
			break
		}

		firstPosition := inputs[i+1]
		secondPosition := inputs[i+2]
		resultPosition := inputs[i+3]
		firstValue := inputs[firstPosition]
		secondValue := inputs[secondPosition]

		switch input {
		case 1: // add
			inputs[resultPosition] = firstValue + secondValue
		case 2: // multiply
			inputs[resultPosition] = firstValue * secondValue
		}
	}

	return inputs
}

func intcode(input []int64) []int64 {
	return execute(input)
}
