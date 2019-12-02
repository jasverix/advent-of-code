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
	input, err := makeIntegerArray(fileContents)

	// replacements due to earlier error (see task text)
	input[1] = 12
	input[2] = 2

	result := intcode(input)
	
	if err != nil {
		fmt.Printf("Error in intcode: %v", err)
		return
	}

	fmt.Printf("result[0] is: %v", result[0])
}
