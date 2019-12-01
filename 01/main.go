package main

import (
	"fmt"
	"io/ioutil"
	"strconv"
	"strings"
)

func main() {
	fileBinary, fileReadingError := ioutil.ReadFile("./01.txt")
	if fileReadingError != nil {
		fmt.Println("Error while reading input file")
	}

	// convert file contents to array of lines
	fileContents := strings.Trim(string(fileBinary), " \n\r")
	lines := strings.Split(fileContents, "\n")

	var sum int = 0

	// print the lines
	for index, element := range lines {
		number, floatParsingError := strconv.ParseInt(strings.Trim(element, " \n\r"), 10, 64)
		if floatParsingError != nil {
			fmt.Println("Error while parsing input data on line " + string(index))
		}

		sum += calculateFuelUsage(int(number))
	}

	fmt.Printf("Result: %v\n", sum)
}
