package main

import (
	"adventure-of-code/utils"
	"fmt"
	"io/ioutil"
	"math"
	"strconv"
	"strings"
)

func calculateFuelUsage(mass int) int {
	if mass == 0 {
		return 0
	}
	fuelRequirement := int(math.Floor(float64(mass)/3) - 2)

	if fuelRequirement > 0 {
		fuelRequirement = fuelRequirement + calculateFuelUsage(fuelRequirement)
	}

	if fuelRequirement < 0 {
		return 0
	}

	return fuelRequirement
}

func main() {
	fileBinary, fileReadingError := ioutil.ReadFile(utils.RelativeFile("input.txt"))
	if fileReadingError != nil {
		fmt.Println("Error while reading input file: " + fileReadingError.Error())
	}

	// convert file contents to array of lines
	fileContents := strings.Trim(string(fileBinary), " \n\r")
	lines := strings.Split(fileContents, "\n")

	var sum = 0

	// print the lines
	for index, element := range lines {
		number, floatParsingError := strconv.ParseInt(strings.Trim(element, " \n\r"), 10, 64)
		if floatParsingError != nil {
			fmt.Println("Error while parsing input data on line " + strconv.Itoa(index))
		}

		sum += calculateFuelUsage(int(number))
	}

	fmt.Printf("Result: %v\n", sum)
}
