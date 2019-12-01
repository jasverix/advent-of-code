package main

import (
	"math"
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
