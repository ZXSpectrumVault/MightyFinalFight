;модуль концовок
						org ending
						
						include "../res/screens/Ending.asm"	

						IF version_type = tap	
						call clear_screen			
						ld hl,stop_tape_text
						ld de,#020c
						call drw_string
						call wait_any_key
						ENDIF
						
						call clear_screen
						
;сброс прогресса выбранного героя						
						IF version_type = trdos
						ld a,(main_hero)
						dec a
						add a,a
						ld e,a
						ld d,0
						ld hl,Cody_level
						add hl,de
						ld (hl),"1"
						inc hl
						ld (hl),0
						di
						im 1
						ld hl,saves_filename
						ld c,#13
						call #3d13
						ld c,#12
						call #3d13
						ld hl,saves_begin
						ld de,saves_end-saves_begin
						ld c,#0b
						call #3d13
						im 2
						ENDIF		

;распаковка музыки и экранов концовки
						di
						setpage sengine.page
						ld hl,ending_music
						ld de,music.adr
						call unzip
						ld hl,music.adr
						call init_music
						ld hl,ending_screens
						ld de,temp_arx
						call unzip
						im 2
						ei
						ld b,200
						call pause
						
;картинка героя						
						ld de,Ending_offset+temp_arx
						ld a,(main_hero)
						dec a
						add a,CodyEnding
						call drw_screen
						ld b,25
						call pause
		
;вычисляем адрес текста		
						ld a,(main_hero)
						dec a
						jr nz,ending_1
						ld hl,ending_text_Cody
						jr ending_3
ending_1				dec a
						jr nz,ending_2
						ld hl,ending_text_Guy
						jr ending_3
ending_2				ld hl,ending_text_Haggar

;печатаем текст
ending_3				ld de,screen+#1066
						ld b,5
ending_loop_1			push bc,de
ending_loop_2			ld a,(hl)
						inc hl
						cp #ff
						jr z,titles_scroll
						or a
						jr z,ending_loop_3
						call drw_symbol
						inc e
						cp " "
						jr z,ending_loop_2		
						ld b,3
						call pause
						jr ending_loop_2					
ending_loop_3			pop de,bc
						ld a,e
						add a,32
						ld e,a
						djnz ending_loop_1
						
;очистка текста			
						ld b,250
						ld a,(main_hero)
						cp id_Haggar
						jr nz,ending_loop_4
						ld b,150
ending_loop_4			call pause
						
						push bc,de,hl
						ld hl,screen+#1066
						ld b,40
ending_loop_5			push bc,hl						
						ld de,hl
						inc e
						ld bc,24
						ld (hl),0
						ldir
						pop hl,bc
						call down_hl
						djnz ending_loop_5
						pop hl,de,bc					
						jr ending_3
						
;запуск титров					
titles_scroll			pop de
						ld b,250
						call pause
						call clear_screen
						ld b,150
						call pause
						ld hl,screen+#1800
						call black_line
						ld hl,screen+#1ae0
						call black_line

						setpage sengine.page
						ld hl,titles_music
						ld de,music.adr
						call unzip
						ld hl,music.adr
						call init_music

						ld b,75
						call pause
						
						setpage font.page
						ld hl,titles_text
						
titles_scroll_loop_1	ei
						halt
						halt
						ld de,#0417
						call drw_title_string
						call scroll_screen_up
						ld b,7
titles_scroll_loop_2	ei
						halt
						halt
						call scroll_screen_up_wait
						djnz titles_scroll_loop_2
						ld a,(hl)
						inc a
						jr nz,titles_scroll_loop_1
						
						ld b,100
						call scroll_screen_up_djnz						
						ld de,Ending_offset+temp_arx
						ld a,EndImg1
						call drw_screen	
						call scroll_screen_up
						ld b,7
						call scroll_screen_up_djnz
						
						ld de,Ending_offset+temp_arx
						ld a,EndImg2
						call drw_screen	
						call scroll_screen_up
						ld b,7
						call scroll_screen_up_djnz
						
						ld de,Ending_offset+temp_arx
						ld a,EndImg3
						call drw_screen	
						call scroll_screen_up
						ld b,7
						call scroll_screen_up_djnz
						
						ld b,82
						call scroll_screen_up_djnz
						
						IF version_type = tap
						jr $
						ENDIF
						
						IF version_type = trdos
						ld b,100
						call pause
						di
						im 1
						ld a,"1"
						ld (current_level),a
						ld a,2
						ld (current_file_num),a
						ld hl,main_code
						push hl
						jp load_file				
						ENDIF
						
;скроллировать титры несколько раз
scroll_screen_up_djnz	ei
						halt
						halt
						call scroll_screen_up_wait
						djnz scroll_screen_up_djnz
						ret
						
;нарисовать одну строку титров
;вх  - de - координаты в символах на реальном экране
;      hl - адрес строки
;вых - hl - адрес следующей строки
drw_title_string		ld a,e
						rrca
						rrca
						rrca
						and #e0
						or d
						ld d,a
						ld a,e
						and #18
						add a,high screen
						ld e,d
						ld d,a
						ld c,a
						
drw_title_string_loop	ld a,(hl)
						inc hl
						or a
						ret z
						
						push hl
						ld l,a
						ld h,0
						add hl,hl
						add hl,hl
						add hl,hl
						ld a,high font.adr
						add a,h
						ld h,a
						dup 7
						ld a,(hl)
						ld (de),a
						inc l,d
						edup
						ld a,(hl)
						ld (de),a
						pop hl
						ld d,c
						
						inc e
						jr drw_title_string_loop
						
;линия 32 знакоместа с нулевым атрибутом						
black_line				ld de,hl
						inc de
						ld bc,31
						ld (hl),b
						ldir
						ret

scroll_screen_up_wait	push af,bc
						ld bc,700
scroll_screen_up_wait_1 dec bc
						ld a,b
						or c
						jr nz,scroll_screen_up_wait_1
						pop bc,af
						
;скролл экрана на один пиксель вверх						
scroll_screen_up		push bc,hl
						ld de,screen+#0704
						exx
						ld b,185
scroll_screen_up_loop	exx
						push de

						ld hl,de
						inc h
						ld a,h
						and 7
						jp nz,$+13
						ld a,l
						add a,32
						ld l,a
						jr c,$+6
						ld a,h
						sub 8
						ld h,a

						dup 24
						ldi
						edup
						
						pop de
						
						inc d
						ld a,d
						and 7
						jp nz,$+13
						ld a,e
						add a,32
						ld e,a
						jr c,$+6
						ld a,d
						sub 8
						ld d,a
						
						exx
						djnz scroll_screen_up_loop
						pop hl,bc
						ret
						
ending_text_Cody		db "FINALLY WE ARE",0
						db "TOGETHER AGAIN, MY",0
						db "LOVE! NOTHING AND NO",0				
						db "ONE CAN STAND",0					
						db "BETWEEN US FROM NOW.",0
						
						db "AND IF SOMEONE TRIES",0				
						db "ONCE MORE, I WILL",0
						db "DEAL WITH HIM.",0
						db 0
						db 0

						db "LET'S HURRY BACK",0
						db "HOME, DADDY IS",0
						db "PROBABLY GOING CRAZY",0
						db "WAITING FOR HIS",0
						db "PRINCESS TO COME BACK.",255
						
ending_text_Guy			db "I HAVE WALKED THIS",0
						db "PATH FROM THE",0
						db "BEGINNING TILL THE",0
						db "END, THE STREETS OF",0
						db "OUR BELOVED TOWN",0
						
						db "BECAME MUCH CLEANER,",0
						db "I GREW STRONGER AND",0
						db "NEW ROADS ARE LYING",0
						db "IN FRONT OF ME RIGHT",0
						db "NOW.",0
						
						db "I JUST NEED TO",0
						db "CHOOSE THE ONE TO",0
						db "GO ON...",255
						
ending_text_Haggar		db "DADDY, WHAT TOOK YOU",0
						db "SO LONG.",0
						db 0
						db 0
						db 0
						
						db "SORRY, PRINCESS, I",0
						db "HAD TO CLEAN UP THE",0
						db "HOUSE AND TAKE THE",0
						db "GARBAGE OUT.",0
						db 0
						
						db "DAD, AND WHERE IS",0
						db "CODY?",0
						db 0
						db 0
						db 0
						
						db "CODY? I LOST THE",0
						db "SIGHT OF HIM ON MY",0
						db "WAY HERE.",0
						db 0
						db 0		

						db "YOU ARE SO GREAT,",0
						db "DADDY!",255
		
titles_text				DB "THIS GAME WAS CONVERTED ",0
						DB " FROM THE NES PLATFORM  ",0
						DB "   TO ZX SPECTRUM 128   ",0
						DB "      SPECIALLY FOR     ",0
						DB "   ZX-DEV CONVERSIONS   ",0
						DB "      COMPETITION.      ",0
						DB "                        ",0
						DB "                        ",0
						DB "  WE ARE VERY GRATEFUL  ",0
						DB "      FOR THE HELP:     ",0
						DB "                        ",0
						DB "        KOVALSKY        ",0
						DB "                        ",0
						DB "         JERRI          ",0
						DB "                        ",0
						DB "        EPSILON         ",0
						DB "                        ",0
						DB "    MICHAIL SUDAKOV     ",0
						DB "                        ",0
						DB "         DIVER          ",0
						DB "                        ",0
						DB "        CHAVES          ",0
						DB "                        ",0
						DB "          VBI           ",0
						DB "                        ",0
						DB "    LERA SUHANOVA       ",0
						DB "                        ",0
						DB "       INTROSPEC        ",0
						DB "                        ",0
						DB "         SHIRU          ",0
						DB "                        ",0
						DB "      CAPCOM TEAM       ",0
						DB "                        ",0
						DB "  AND ALSO FOR ANYBODY  ",0
						DB "        WHO READS       ",0
						DB "   THIS FINAL MESSAGE.  ",0
						DB "                        ",0
						DB " BEST WISHES AND SEE YA ",0
						DB "    IN NEW RELEASES     ",0
						DB "    DEVELOPERS TEAM     ",0
						DB '  "SANCHEZ CREW 2018"!  ',0,#ff

ending_screens			incbin "../res/screens/Ending.bin.mlz"
titles_music			incbin "../music/nq-mff-bonus-round.pt3.mlz"
ending_music			incbin "../music/nq-mff-ending-theme.pt3.mlz"						
						savebin "../bin/ending.bin", ending, $ - ending
						
						
						
						
						
						
						
						