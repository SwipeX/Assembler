main:
  ! Push some values on the stack randomly
  lda #$10  
  sta $0
  lda #$11
  sta $1
  lda #$12
  sta $2

  lda #$13
  sta $16
  lda #$14
  sta $17
  lda #$15
  sta $18

  ! Program demonstrates some locality
  
  loop:
    lda $0
    add 1
    sta $0  ! Stack[0]++

    lda $1
    add 1
    sta $1

    lda $15
    add 1
    sta $15

    lda $2
    add 1
    sta $2

    lda $16
    add 1
    sta $16

  check:
    lda $0  ! Place 10+ into the acc - do loop will run from 10 to 20...
    sub 20  ! 20 - 10, 11 etc until 20-20 = 0
    be out
    ba loop
    
  out:
    lda $0

