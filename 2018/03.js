class Claim {
  constructor (claimString) {
    const [raw, index, fromLeft, fromTop, width, height] = /#(\d+) @ (\d+),(\d+): (\d+)x(\d+)/g.exec(claimString)

    this.claimString = raw
    this.index = index
    this.fromLeft = fromLeft
    this.fromTop = fromTop
    this.width = width
    this.height = height
  }

  leftPoint () {
    return this.fromLeft
  }
  rightPoint () {
    return this.fromLeft + this.width
  }
  topPoint () {
    return this.fromTop
  }
  bottomPoint () {
    return this.fromTop + this.height
  }
}

class FabricClaimCalculator {
  constructor (claims) {
    this.rawClaims = claims
    this.claims = {}

    this.mapWidth = 0
    this.mapHeight = 0

    this._claimsIndexed = false
  }

  indexClaims () {
    if (this._claimsIndexed) return
    this._claimsIndexed = true

    for (const claimString in this.rawClaims) {
      const claim = new Claim(claimString)
      this.claims[claim.index] = claim

      if (claim.rightPoint() > this.mapWidth) {
        this.mapWidth = claim.rightPoint()
      }
      if (claim.bottomPoint() > this.mapHeight) {
        this.mapHeight = claim.bottomPoint()
      }
    }
  }

  getOverlappedSquareCount () {
    return 0
  }
}

function test () {
  const calculator = new FabricClaimCalculator([
    '#1 @ 1,3: 4x4',
    '#2 @ 3,1: 4x4',
    '#3 @ 5,5: 2x2',
  ])

  console.log('Overlapped squares are: ', calculator.getOverlappedSquareCount(), 'should be 4')
}
