package main

import (
	"strings"
	"strconv"
	"errors"
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

		if i + 2 >= length {
			return nil, errors.New("Index out of range")
		}

		firstPosition := inputs[i+1]
		secondPosition := inputs[i+2]
		resultPosition := inputs[i+3]

		if firstPosition >= length || secondPosition >= length || resultPosition >= length {
			return nil, errors.New("Index out of range")
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
