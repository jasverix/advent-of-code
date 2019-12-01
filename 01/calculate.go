package main

import "math"

func calculateFuelUsage(mass int) int {
	return int(math.Floor(float64(mass)/3) - 2)
}
