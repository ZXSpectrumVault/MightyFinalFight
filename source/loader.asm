;загрузчик игры
						org #6000
loader					di
						res 4,(IY+1)	;for ZX Spectrum +2, +3
						ld sp,#7fff

						xor a
						out (254),a					
						ld hl,#5800
						ld de,#5801
						ld bc,#2ff
						ld (hl),a
						ldir
						
;определение 128k
						ld hl,#c000
						ld bc,#7ffd
						ld a,#16
						out (c),a
						ld (hl),a
						ld a,#17
						out (c),a
						ld (hl),a
						ld a,#16
						out (c),a
						cp (hl)
						jr z,loader_2

;обнаружен 16/48k		
						ld hl,easteregg_screen
						ld de,#4000
						call loader_unzip
						
;обнаружен 16к
						ld hl,#8000
						ld (hl),#aa
						ld a,(hl)
						cp #aa
						jr nz,$						
						
loader_1				call loader_pause
						ld hl,beeper_music
						ld de,#8000
						push de
						jp loader_unzip
						
loader_pause			ld b,25
loader_pause_loop		ei
						halt
						djnz loader_pause_loop
						ret
						
loader_2				ld hl,loading_screen
						ld de,#4000
						call loader_unzip
						
						ld hl,files_len_tab
						ld de,files_length
						ld bc,128 ;максимальный размер таблицы
						ldir

						IF version_type = trdos
						ld a,2
						ld c,#08
						call #3d13
						ld hl,main_code
						xor a
						ld (#5cf9),a
						cpl
						ld c,#0e
						call #3d13
						ENDIF
						
						IF version_type = tap
loader_3				xor a
						in a,(254)
						cpl
						and #1f
						jr nz,loader_3
						ld ix,main_code
						ld de,(files_length)
						scf
						ld a,#ff
						inc d
						exa
						dec d
						call #0562
						xor a
						out (254),a
						ENDIF
						
						jp main_code
							
				
;распаковка MegaLZ (Z80 depacker for megalz V4 packed files   (C) fyrex^mhm) (только для загрузчика)
;вх  - hl - адрес архива
;      de - адрес распаковки
loader_unzip			ld a,#80
						exa
loader_unzip_1  		ldi
loader_unzip_2  		ld bc,#2ff
loader_unzip_3  		exa
loader_unzip_4  		add a,a
						jr nz,loader_unzip_5
						ld a,(hl)
						inc hl
						rla
loader_unzip_5  		rl c
						jr nc,loader_unzip_4
						exa
						djnz loader_unzip_6
						ld a,2
						sra c
						jr c,loader_unzip_9
						inc a
						inc c
						jr z,loader_unzip_8
						ld bc,#33f
						jr loader_unzip_3
loader_unzip_6  		djnz loader_unzip_12
						srl c
						jr c,loader_unzip_1
						inc b
						jr loader_unzip_3
loader_unzip_7			add a,c
loader_unzip_8			ld bc,#4ff
						jr loader_unzip_3
loader_unzip_9			inc c
						jr nz,loader_unzip_15
						exa
						inc b
loader_unzip_10 		rr c
						ret c
						rl b
						add a,a
						jr nz,loader_unzip_11
						ld a,(hl)
						inc hl
						rla
loader_unzip_11 		jr nc,loader_unzip_10
						exa
						add a,b
						ld b,6
						jr loader_unzip_3
loader_unzip_12			djnz loader_unzip_13
						ld a,1
						jr loader_unzip_16
loader_unzip_13 		djnz loader_unzip_14
						inc c
						jr nz,loader_unzip_15
						ld bc,#51f
						jr loader_unzip_3
loader_unzip_14			djnz loader_unzip_7
						ld b,c
loader_unzip_15 		ld c,(hl)
						inc hl
loader_unzip_16 		dec b
						push hl
						ld l,c
						ld h,b
						add hl,de
						ld c,a
						ld b,0
						ldir
						pop hl
						jr loader_unzip_2
				
files_len_tab			incbin "../bin/files_len.bin"
loading_screen			incbin "../res/screens/LoadingScreen.scr.mlz"
easteregg_screen		incbin "../res/screens/EasterEgg.scr.mlz"
beeper_music			incbin "../res/beeper/eastermuz.bin.mlz"
						savebin "../bin/loader.bin", loader, $ - loader