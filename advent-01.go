package main

import (
	"fmt"
	"io/ioutil"
	"strings"
)

func main() {
	fileBinary, error := ioutil.ReadFile("./input-data/01.txt")
	if error != nil {
		fmt.Println("Error while reading input file")
	}

	// convert file contents to array of lines
	fileContents := strings.Trim(string(fileBinary), " \n\r")
	lines := strings.Split(fileContents, "\n")

	// print the lines
	for index, element := range lines {
		fmt.Printf("Line %d: %s\n", index+1, strings.Trim(element, " \n\r"))
	}
}
