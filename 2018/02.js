class BoxScanner {
  constructor (boxes) {
    this.boxes = boxes
    this.boxesIndexed = []
    
    this.doubleLetterBoxes = 0
    this.tripleLetterBoxes = 0

    this.similarBoxesLetters = null

    this._hasScanned = false
  }

  scan () {
    if (this._hasScanned) return
    this._hasScanned = true

    this.indexBoxes()
    this.readBoxes()

    for (const boxNumber of this.boxes) {
      this.readBoxNumber(boxNumber)
    }
  }

  indexBoxes () {
    for (const boxNumber of this.boxes) {
      this.boxesIndexed.push(boxNumber.split(''))
    }
  }

  readBoxes () {
    for (const chars of this.boxesIndexed) {
      this.readBoxNumber(chars)
      this.findSimilarBox(chars)
    }
  }

  readBoxNumber (chars) {
    const charIndex = {}

    for (const char of chars) {
      // add 1 to charIndex - with safe 'undefined' checker
      charIndex[char] = (charIndex[char] || 0) + 1
    }

    let hasDouble = false
    let hasTriple = false
    for (const [, count] of Object.entries(charIndex)) {
      if (count === 3) {
        hasTriple = true
      } else if (count === 2) {
        hasDouble = true
      }
    }

    if (hasDouble) ++this.doubleLetterBoxes
    if (hasTriple) ++this.tripleLetterBoxes
  }

  findSimilarBox (chars) {
    if (this.similarBoxesLetters !== null) return

    for (const otherChars of this.boxesIndexed) {
      if (this.similarBoxesLetters !== null) return
      if (chars.join('') === otherChars.join('')) continue;

      for (let i = 0; i < chars.length; ++i) {
        const tempChars = chars.map(i => i)
        const tempOtherChars = otherChars.map(i => i)

        tempChars.splice(i, 1)
        tempOtherChars.splice(i, 1)

        const charString = tempChars.join('')
        const otherCharString = tempOtherChars.join('')

        if (charString === otherCharString) {
          this.similarBoxesLetters = charString
        }
      }
    }
  }

  getChecksum () {
    this.scan()

    return this.doubleLetterBoxes * this.tripleLetterBoxes
  }

  getSimilarBoxesLetters () {
    this.scan()

    return this.similarBoxesLetters
  }
}

function parseInput () {
  return document.body.innerText
    .trim()
    .split(/\n/)
    .map(input => input.trim())
    .filter(input => input !== '')
}

function test () {
  const boxes = [
    'abcdef',
    'bababc',
    'abbcde',
    'abcccd',
    'aabcdd',
    'abcdee',
    'ababab',
  ]

  const scanner = new BoxScanner(boxes)

  console.log('Checksum:', scanner.getChecksum(), 'should be 12')

  const boxes2 = [
    'abcde',
    'fghij',
    'klmno',
    'pqrst',
    'fguij',
    'axcye',
    'wvxyz',
  ]

  const scanner2 = new BoxScanner(boxes2)

  console.log('Similar boxes:', scanner2.getSimilarBoxesLetters(), 'should be fgij')
}

function run() {
  const scanner = new BoxScanner(parseInput())

  console.log('Checksum:', scanner.getChecksum())
  console.log('Similar boxes:', scanner.getSimilarBoxesLetters())
}

console.log('TEST')
test()

console.log('')
console.log('RUN')
run()
