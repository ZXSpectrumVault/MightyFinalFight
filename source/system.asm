;системные подпрограммы

ram_driver				push bc
						ld (current_page),a
						or #10
						ld bc,#7ffd
						out (c),a
						pop bc
						ret 
current_page    		db 0

						
			
;загрузка файла (последовательная загрузка)
;вх  - hl - адрес загрузки
;      (current_file_num) - номер файла на диске
						IF version_type = trdos
load_file       		push hl
						ld hl,current_file_num
						ld a,(hl)
						inc (hl)				
						ld c,#08
						call #3d13
						pop hl
						
						xor a
						ld (#5cf9),a
						cpl
						ld c,#0e
						jp #3d13 
current_file_num		db 3					
						ENDIF
						
						
;загрузчик с ленты
;вх  - hl - адрес загрузки
;      (current_file_adr) - указатель на длинну файла				
						IF version_type = tap
load_file				xor a
						in a,(254)
						cpl
						and #1f
						jr nz,load_file
						push hl
						pop ix
						ld hl,(current_file_adr)
						ld e,(hl)
						inc l
						ld d,(hl)
						inc l
						ld (current_file_adr),hl
						
						di
						scf
						ld a,#ff
						inc d
						exa
						dec d
						call #0562
						xor a
						out (254),a
						ret
current_file_adr		dw files_length+2			
						ENDIF
				
;распаковка MegaLZ (Z80 depacker for megalz V4 packed files   (C) fyrex^mhm)
;вх  - hl - адрес архива
;      de - адрес распаковки
unzip					ld a,#80
						exa
unzip_1    				ldi
unzip_2    				ld bc,#2ff
unzip_3    				exa
unzip_4    				add a,a
						jr nz,unzip_5
						ld a,(hl)
						inc hl
						rla
unzip_5    				rl c
						jr nc,unzip_4
						exa
						djnz unzip_6
						ld a,2
						sra c
						jr c,unzip_9
						inc a
						inc c
						jr z,unzip_8
						ld bc,#33f
						jr unzip_3
unzip_6    				djnz unzip_12
						srl c
						jr c,unzip_1
						inc b
						jr unzip_3
unzip_7					add a,c
unzip_8					ld bc,#4ff
						jr unzip_3
unzip_9					inc c
						jr nz,unzip_15
						exa
						inc b
unzip_10   				rr c
						ret c
						rl b
						add a,a
						jr nz,unzip_11
						ld a,(hl)
						inc hl
						rla
unzip_11   				jr nc,unzip_10
						exa
						add a,b
						ld b,6
						jr unzip_3
unzip_12				djnz unzip_13
						ld a,1
						jr unzip_16
unzip_13   				djnz unzip_14
						inc c
						jr nz,unzip_15
						ld bc,#51f
						jr unzip_3
unzip_14				djnz unzip_7
						ld b,c
unzip_15   				ld c,(hl)
						inc hl
unzip_16   				dec b
						push hl
						ld l,c
						ld h,b
						add hl,de
						ld c,a
						ld b,0
						ldir
						pop hl
						jr unzip_2
						
;случайное число в a
rnd             		push bc
						ld a,r
rnd_1           		ld b,0
						add a,b
						ld b,a
						add a,a
						add a,a
						add a,b
						add a,7
						ld (rnd_1+1),a
						pop bc
						ret	
						
;опрос джойстиков и клавиатуры
;биты: 0 - вверх (Q, 4, 9)
;      1 - вниз (A, 3, 8)
;      2 - влево (O, K, 1, 6)
;      3 - право (P, L, 2, 7)
;      4 - огонь (Space, M, 0, 5)
;      6 - пауза (Enter, H)
;      7 - чит (Z)

get_input				ld d,0

get_input_up			ld bc,#fbfe ;q
						in a,(c)
						and 1
						jr z,get_input_up_ok
						ld b,#f7 ;4
						in a,(c)
						and 8
						jr z,get_input_up_ok
						ld b,#ef ;9
						in a,(c)
						and 2
						jr nz,get_input_down
get_input_up_ok		    set 0,d

get_input_down			ld b,#fd ;a
						in a,(c)
						and 1
						jr z,get_input_down_ok
						ld b,#f7 ;3
						in a,(c)
						and 4
						jr z,get_input_down_ok
						ld b,#ef ;8
						in a,(c)
						and 4
						jr nz,get_input_left
get_input_down_ok		set 1,d						
						
get_input_left			ld b,#df ;o
						in a,(c)
						and 2
						jr z,get_input_left_ok
						ld b,#bf ;k
						in a,(c)
						and 4
						jr z,get_input_left_ok
						ld b,#f7 ;1
						in a,(c)
						and 1
						jr z,get_input_left_ok
						ld b,#ef ;6
						in a,(c)
						and 16
						jr nz,get_input_right
get_input_left_ok		set 2,d		
						
get_input_right			ld b,#df ;p
						in a,(c)
						and 1
						jr z,get_input_right_ok
						ld b,#bf ;l
						in a,(c)
						and 2
						jr z,get_input_right_ok
						ld b,#f7 ;2
						in a,(c)
						and 2
						jr z,get_input_right_ok
						ld b,#ef ;7
						in a,(c)
						and 8
						jr nz,get_input_fire
get_input_right_ok		set 3,d								
						
get_input_fire			ld b,#7f ;Space
						in a,(c)
						and 1
						jr z,get_input_fire_ok
						in a,(c) ;m
						and 4
						jr z,get_input_fire_ok
						ld b,#f7 ;5
						in a,(c)
						and 16
						jr z,get_input_fire_ok
						ld b,#ef ;0
						in a,(c)
						and 1
						jr nz,get_input_pause
get_input_fire_ok		set 4,d

get_input_pause			ld b,#bf ;Enter
						in a,(c)
						and 1
						jr z,get_input_pause_ok
						in a,(c) ;h
						and 16
						jr nz,get_input_quit
get_input_pause_ok		set 6,d	

get_input_quit			ld b,#fe ;z
						in a,(c)
						and 2
						jr nz,get_input_kempston
						set 7,d	

;читаем джойстик
get_input_kempston		call read_kempston
						or d
						ld d,a
						ld hl,pressed_keys
						or (hl)
						ld (hl),a
						
						ld a,(prev_keys)
						ld e,a
						ld a,d
						ld (prev_keys),a
						ld a,e
						cpl
						and d
						ld hl,clicked_keys
						or (hl)
						ld (hl),a
												
						exx
						ld b,0
						exx
						
						ld hl,combo_weapon_right
						call read_combo_string
						exx
						rl b
						exx
						
						ld hl,combo_weapon_left
						call read_combo_string
						exx
						rl b
						exx
					
						ld hl,combo_super_right
						call read_combo_string
						exx
						rl b
						exx
						
						ld hl,combo_super_left
						call read_combo_string
						exx
						rl b
						exx
						
						ld hl,combo_life
						call read_combo_string
						exx
						rl b
						exx
						
						ld hl,combo_twister
						ld a,d
						and #10
						ld d,a
						ld a,e
						and #10
						ld e,a
						call read_combo_string
						exx
						rl b
						
						ld hl,combo_active
						ld a,(hl)
						or b
						ld (hl),a
						ret

;чтение комбо-строки
;вх - hl - адрес комбо-строки. формат: 1ый байт счетчик, 2ой смещение, дальше строка, оканчивающаяся #ff
;     d - текущие кнопки
;     e - предыдущие кнопки
;вых - флаг C - комбо выполнено
read_combo_string		ld bc,hl
						ld a,(hl)
						sub 1
						adc a,0
						ld (hl),a
						inc hl
						jr z,read_combo_string_1
						
						ld a,(hl) ;вычисляем адрес следующей кнопки в строке
						add a,l
						ld l,a
						ld a,h
						adc a,0
						ld h,a
						jr read_combo_string_2
						
read_combo_string_1		ld (hl),0 ;если счётчик = 0, начинаем последовательность сначала
						
read_combo_string_2		ld a,e ;если в прошлом цикле какая-то кнопка была нажата, то выход
						or a
						ret nz
						ld a,d ;если в текущем цикле ни одна кнопка не нажата, то выход
						or a
						ret z
						
						inc hl ;сравниваем текущую кнопку со строкой
						ld a,(hl)
						cp d
						jr z,read_combo_string_3
						
						ld hl,bc ;если кнопка не совпадает, то сбрасываем счётчик в ноль
						xor a
						ld (hl),a
						ret
						
read_combo_string_3		inc hl ;если совпадает, проверяем на конец строки
						ld a,(hl)
						cp #ff
						jr z,read_combo_string_4

						ld hl,bc ;устанавливаем счётчик и увеличиваем адрес
						ld (hl),20
						inc hl
						inc (hl)
						xor a
						ret
						
read_combo_string_4		ld hl,bc
						ld (hl),0
						scf
						ret
	
;простая пауза
;вх - b - кол-во фреймов
pause					ei
						halt
						djnz pause
						ret
				
;прерываемая пауза				
;вх  - bc - кол-во фреймов
;вых - Z - пауза была до конца, NZ - было прерывание паузы
interrupted_pause		call init_any_key
interrupted_pause_loop	ei
						halt
						push bc
						call any_key
						pop bc
						ret nz
						dec bc
						ld a,b
						or c
						jr nz,interrupted_pause_loop
						ret
				
;опрос kempston-joystick
read_kempston			ld a,(kempston_on)
						or a
						ret z		
						push bc
						in a,(#1f)
						and #1f
						ld c,a
						ld b,0
						dup 4
						rrca
						rl b
						edup
						ld a,c
						and #10
						or b
						pop bc
						ret
				
;проверка нажатия на любую кнопку
;вых - Z - нет нажатия, NZ - есть
any_key					ld a,#ff
						or a
						jr z,any_key1
						call any_key1
						xor a
						ret
any_key1				xor a
						in a,(254)
						cpl				
						ld b,a
						call read_kempston
						or b				
						and #1f		
						ld (any_key+1),a
						ret
init_any_key			ld a,#ff
						ld (any_key+1),a
						ret
wait_any_key			call init_any_key
wait_any_key_loop		ei
						halt
						call any_key
						jr z,wait_any_key_loop
						ret