main:
lda #$0
sta $2 
lda #$4
sta $10 
lda #$1
sta $20 

loop:
lda $10 
add  $2 
sub $20 
sta $10 
sub #$9 
be rehash 
ba out 

rehash:
sta $2 
and $20 
or $10 
sta $10

out:
lda $10 