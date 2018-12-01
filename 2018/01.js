// should be a class with properties - but I don't want to continue on this right now - I got two stars
// class FrequencyCalculator
//  calculate()
//  getSum()
//  getEarliestReachedFrequency()
let reachedFrequencies = {}
let earliestReachedDoubleFrequency = null

function reset () {
  earliestReachedDoubleFrequency = null
  reachedFrequencies = {}
}

function logFrequency (sum) {
  if (reachedFrequencies[sum] === void 0) {
    reachedFrequencies[sum] = 0
  }

  ++reachedFrequencies[sum]

  if (earliestReachedDoubleFrequency === null && reachedFrequencies[sum] > 1) {
    earliestReachedDoubleFrequency = sum
  }
}

function calculateFrequency (...args) {
  let sum = 0
  logFrequency(0)

  for (const num of args) {
    sum += num
    logFrequency(sum)
  }

  // the first star of the task is to get the 'sum' - don't modify that value after the first loop
  let freq = sum

  // the second star is to find the first double frequency reached - continue looping until a double frequency is reached
  while (earliestReachedDoubleFrequency === null) {
    for (const num of args) {
      freq += num
      logFrequency(freq)

      if (earliestReachedDoubleFrequency !== null) {
        break
      }
    }
  }

  return sum
}

function test () {
  console.log(calculateFrequency(1, -2, 3, 1, 1, -2), 'should be 2')
  console.log('Earliest frequency ', earliestReachedDoubleFrequency, 'should be 2')

  reset()

  console.log(calculateFrequency(1, -1), 'should be 0')
  console.log('Earliest frequency ', earliestReachedDoubleFrequency, 'should be 0')
  reset()

  console.log(calculateFrequency(3, 3, 4, -2, -4), 'should be 4')
  console.log('Earliest frequency ', earliestReachedDoubleFrequency, 'should be 10')
  reset()

  console.log(calculateFrequency(-6, 3, 8, 5, -6), 'should be 4')
  console.log('Earliest frequency ', earliestReachedDoubleFrequency, 'should be 5')
  reset()

  console.log(calculateFrequency(7, 7, -2, -7, -4), 'should be 1')
  console.log('Earliest frequency ', earliestReachedDoubleFrequency, 'should be 14')
  reset()
}

function parseInput () {
  return document.body.innerText
    .trim()
    .split(/\n/)
    .map(input => parseInt(input, 10))
}

function run () {
  reset()
  console.log('Output from body content is: ', calculateFrequency(...parseInput()))
  console.log('Earliest double frequency is: ', earliestReachedDoubleFrequency)
}

test()
run()
