main:
  ! Push an array onto the stack (from Stack[0] to Stack[15]
  ! Stack[0] = 10
  ! ...
  ! Stack[15] = 25
  lda #$10  
  sta $0
  lda #$11
  sta $1
  lda #$12
  sta $2
  lda #$13
  sta $3
  lda #$14
  sta $4
  lda #$15
  sta $5
  lda #$16
  sta $6
  lda #$17
  sta $7
  lda #$18
  sta $8
  lda #$19
  sta $9
  lda #$20
  sta $10
  lda #$21
  sta $11
  lda #$22
  sta $12
  lda #$23
  sta $13
  lda #$24
  sta $14
  lda #$25
  sta $15
  ! Cache filled

  ! Program demonstrates extreme locality
  
  loop:
    lda $0
    add #$1
    sta $0  ! Stack[0]++

    ! Go through the array incrementing every other value 
    ! (Every other value to keep this file sane...)
    lda $1
    add #$1
    sta $1

    lda $3
    add #$1
    sta $3

    lda $5
    add #$1
    sta $5

    lda $7
    add #$1
    sta $7

    lda $9
    add #$1
    sta $9

    lda $11
    add #$1
    sta $11

    lda $13
    add #$1
    sta $13

    lda $15
    add #$1
    sta $15

  check:
    lda $0  ! Place 10+ into the acc - do loop will run from 10 to 20...
    sub #$20  ! 20 - 10, 11 etc until 20-20 = 0; notice we do not store; just use the CC for the be
    be out
    ba loop
    
  out:
    lda $0

