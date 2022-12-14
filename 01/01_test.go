package main

import (
	"github.com/stretchr/testify/assert"
	"testing"
)

func Test_First_day(t *testing.T) {
	input := "1000\n2000\n3000\n\n4000\n\n5000\n6000\n\n7000\n8000\n9000\n\n10000"
	assert.Equal(t, 24000, calculate(input))
}
