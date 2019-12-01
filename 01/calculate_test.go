package main

import "testing"

func assertResult(t *testing.T, expected int, input int) {
	result := calculateFuelUsage(input)
	if result != expected {
		t.Errorf("calculateFuelUsage(%v) failed, expected %v and got %v", input, expected, result)
	}
}

func TestCalculateFuelUsage(t *testing.T) {
	assertResult(t, 2, 12)
	assertResult(t, 2, 14)
	assertResult(t, 654, 1969)
	assertResult(t, 33583, 100756)
}
