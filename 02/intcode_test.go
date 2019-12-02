package main

import (
	"testing"
	"reflect"
)

func assertResult(t *testing.T, input string, expected []int) {
	inputArray, err := makeIntegerArray(input)
	if err != nil {
		t.Errorf("makeIntegerArray(%v) failed, error message %v", input, err)
		return
	}

	result, err := intcode(inputArray)
	if err != nil {
		t.Errorf("intcode(%v) failed, error message %v", input, err)
	} else if !reflect.DeepEqual(result, expected) {
		t.Errorf("intcode(%v) failed, expected %v and got %v", input, expected, result)
	}
}

func TestIntcode(t *testing.T) {
	assertResult(t, "1,9,10,3,2,3,11,0,99,30,40,50", []int{3500,9,10,70,2,3,11,0,99,30,40,50})
	assertResult(t, "1,0,0,0,99", []int{2,0,0,0,99})
	assertResult(t, "2,3,0,3,99", []int{2,3,0,6,99})
	assertResult(t, "2,4,4,5,99,0", []int{2,4,4,5,99,9801})
	assertResult(t, "1,1,1,4,99,5,6,0,99", []int{30,1,1,4,2,5,6,0,99})
}
