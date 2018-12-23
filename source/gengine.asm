;графический движок
				
;инициализация игры
init_game

;инициализация прерываний		
init_interrupt			di
						ld hl,int_vector
						ld b,l
						ld a,high int_vector+1
init_interrupt1   		ld (hl),a
						inc hl
						djnz init_interrupt1
						ld (hl),a
						ld l,h
						ld (hl),#c3
						inc hl
						ld (hl),low interrupt
						inc hl
						ld (hl),high interrupt
						dec a
						ld i,a
						ret	

;инициализация таблицы смещения для вывода спрайтов
init_offset_tab			ld hl,temp_arx
						xor a
init_offset_tab_loop1	call init_offset_tab_byte
						inc hl
						inc a
						jr nz,init_offset_tab_loop1
						ld hl,temp_arx+#1000
						ld c,0
init_offset_tab_loop2	ld b,8
init_offset_tab_loop3	rrc c
						rla
						djnz init_offset_tab_loop3
						call init_offset_tab_byte
						inc hl
						inc c
						jr nz,init_offset_tab_loop2
						ret			

init_offset_tab_byte	push af,bc,de,hl
						ld d,a
						ld e,0
						ld b,8
init_offset_tab_byte_1	ld (hl),d
						inc h
						ld (hl),e
						inc h
						srl d
						rr e
						djnz init_offset_tab_byte_1
						pop hl,de,bc,af
						ret

;полная очистка реального экрана
clear_screen			xor a
						ld hl,screen+#1800
						ld de,screen+#1801
						ld bc,#2ff
						ld (hl),a
						ldir
						ld hl,screen
						ld de,screen+1
						ld bc,#1800
						ld (hl),a
						ldir
						ld bc,#2ff
						ld (hl),%1000111
						ldir
						ret
		
;смещение hl на строку вниз
down_hl					inc h
						ld a,h
						and 7
						ret nz
						ld a,l
						add a,32
						ld l,a
						ret c
						ld a,h
						sub 8
						ld h,a
						ret
		
;вывод символа на реальный экран
;вх  - de - адрес на реальном экране
;      a - код символа
drw_symbol				push af,de,hl
						ld l,a
						ld h,0
						add hl,hl
						add hl,hl
						add hl,hl
						ld a,high font.adr
						add a,h
						ld h,a
						setpage font.page
						dup 7
						ld a,(hl)
						ld (de),a
						inc l,d
						edup
						ld a,(hl)
						ld (de),a
						setpage ext.page
						pop hl,de,af
						ret	
	
;вывод статус-баров
drw_statusbar			setpage ext.page
						call drw_statusbar_hero
						jr drw_statusbar_enemy
;энергия героя
drw_statusbar_hero		ld b,0
						ld a,(objects+object.energy)
						ld c,a
						rrca
						rrca
						and #3f
						cp b
						jr z,drw_statusbar_shuriken ;равно предыдущему значению
						or a
						jr nz,drw_statusbar_hero1
						ld a,c
						or a
						jr z,drw_statusbar_hero1
						ld a,1						
drw_statusbar_hero1		ld (drw_statusbar_hero+1),a
						ld c,a
						ld b,12
						ld de,screen+2 ;de = screen.adr, b = scale.width, c = value
						ld hl,#e201
						call drw_scale
						ld b,255
						jr drw_statusbar_shur_0
						
;кол-во сюрикенов у Гая
drw_statusbar_shuriken	ld b,0
drw_statusbar_shur_0	ld a,(objects+object.type)
						cp id_Guy
						ret nz
						ld a,(objects+object.weapon)
						cp b
						ret z
						ld (drw_statusbar_shuriken+1),a
						ld c,a
						ld hl,screen+9+32
						ld de,screen+9
						ld b,8
drw_statusbar_shur_1	ld a,c
						or a
						jr z,drw_statusbar_shur_2
						dec c
						ld a,#5e
drw_statusbar_shur_2	call drw_symbol
						ex de,hl
						ld a,c
						or a
						jr z,drw_statusbar_shur_3
						dec c
						ld a,#5e
drw_statusbar_shur_3	call drw_symbol
						ex de,hl
						inc l,e
						djnz drw_statusbar_shur_1
						ret
						
;энергия врага						
drw_statusbar_enemy		ld b,0
drw_statusbar_energy	ld a,0
						ld c,a
						rrca
						rrca
						and #3f
						cp b
						jr z,drw_statusbar_icon ;равно предыдущему значению
						or a
						jr nz,drw_statusbar_enemy1
						ld a,c
						or a
						jr z,drw_statusbar_enemy1
						ld a,1	
drw_statusbar_enemy1	ld (drw_statusbar_enemy+1),a
						ld c,a
						ld b,12
						ld de,screen+29 ;de = screen.adr, b = scale.width, c = value
						ld hl,#e1ff
						call drw_scale
;иконка врага
drw_statusbar_icon		ld b,0
drw_statusbar_type		ld a,0
						cp b
						jr z,drw_statusbar_rage ;равно предыдущему значению
						ld (drw_statusbar_icon+1),a
						ld de,screen+30
drw_statusbar_i2		call drw_icon
					
;шкала Rage					
drw_statusbar_rage		ld b,0
						ld a,(objects+object.rage)
						rrca
						rrca
						and #3f
						cp b
						jr z,drw_statusbar_end ;равно предыдущему значению
						ld (drw_statusbar_rage+1),a
						ld c,a
						ld b,10
						ld de,screen+#10d1 ;de = screen.adr, b = scale.width, c = value
						ld hl,#cc01
						push bc
						call drw_scale
						pop bc
						ld de,screen+#10ce
						ld hl,#cbff
						call drw_scale	

drw_statusbar_end		ret				
				
				
				
;вывод тайлов на виртуальный экран
tiles_out				ld a,(refresh_gamescreen)
						or a
						jp z,tiles_out_1
						call prepare_locbuffer
						
tiles_out_1				ld a,#d5
						ld (interrupt_selector),a ;переключение прерываний на "pop de"

						ld hl,loc_buffer+1
						ld de,vscreen
						ld bc,#100a
						
						ld ix,-6
						add ix,sp

tiles_out_loop			push bc,de,hl
						res 6,(hl)
						bit 7,(hl)
						jp z,tiles_out_end
						set 6,(hl)
						ld a,(hl)
						res 7,(hl)
						dec l
						ld l,(hl)
						ld h,a
						
						ld sp,hl
						ex de,hl
						ld bc,vscreen.width-1
						dup 15
						pop de
						ld (hl),e
						inc l
						ld (hl),d
						add hl,bc
						edup
						pop de
						ld (hl),e
						inc l
						ld (hl),d
						ld sp,ix					

tiles_out_end			pop hl,de,bc
						inc e,de,hl,l
						djnz tiles_out_loop
						ld b,#10
						inc hl,l
						push hl
						ld hl,vscreen.width*16-32
						add hl,de
						ex de,hl
						pop hl
						dec c
						jp nz,tiles_out_loop
						xor a
						ld (interrupt_selector),a ;переключение прерываний на "pop hl"
						
						ld a,(refresh_gamescreen)
						or a
						ret z
						
						ld hl,loc_buffer+1
						ld bc,#100a
tiles_out_2				set 7,(hl)
						inc hl,l
						djnz tiles_out_2
						ld b,#10
						inc hl,l
						dec c
						jp nz,tiles_out_2					
						ret
						
;вывод виртуального экрана на реальный
view_screen				ld a,(refresh_gamescreen)
						or a
						jr z,view_screen_2
						xor a
						ld (refresh_gamescreen),a
						ld hl,loc_buffer
						ld de,screen+#1800+64
						ld bc,#100a
view_screen_1			push hl
						ld a,(hl)
						inc l
						ld h,(hl)
						ld l,a
						call drw_tile_attr
						pop hl
						inc hl,hl,de,de
						djnz view_screen_1
						inc hl,hl
						push hl
						ld hl,32
						add hl,de
						ex de,hl						
						pop hl
						ld b,#10
						dec c
						jr nz,view_screen_1

;выключение обновления экрана под надписью Go!
view_screen_2			ld a,(go_visible) ;не обновляем атрибуты когда надпись Go!
						or a
						jr z,view_screen_4
						
						ld hl,loc_buffer+34*3+24+1
						ld bc,#0302
view_screen_3			ld a,(hl)
						and #3f
						ld (hl),a
						inc hl,l
						djnz view_screen_3
						ld b,3
						ld de,34-6
						add hl,de
						dec c
						jr nz,view_screen_3

view_screen_4			setpage redraw.page ;отобразить виртуальный экран на реальный
						call redraw.adr
						setpage ext.page												
						ret

;заливка прямоугольника аттрибутов реального экрана высотой 2 аттрибута
;вх  - hl - адрес верхнего правого угла						
;      bc - ширина прямоугольника
;      a - значение
fill_attr				call fill_attr1
						and #3f
fill_attr1				push bc,hl
						ld de,hl
						inc de
						ld (hl),a
						dec bc
						ldir
						pop hl
						ld bc,32
						add hl,bc
						pop bc
						ret
					
;инициализация уровня
init_level				di
						im 1
						call ext_mute_music
						ld iy,(iy_save)
						
;загрузка тайлов карты
						setpage ext.page
						ld hl,tileset
						call load_file
						
;загрузка тайлсетов
						setpage tilesets.page
						ld hl,tilesets.adr
						call load_file
						
;загрузка локаций						
						setpage location.page
						ld hl,location.adr
						call load_file
						
;загрузка музыкального трека
						setpage sengine.page
						ld hl,music.adr
						call load_file
						
						ld a,(current_level)
						cp "5"
						jr nz,init_level_1
						ld hl,music_set.adr ;загрузка финального набора треков
						call load_file

;загрузка набора врагов
init_level_1			setpage enemy_set_1.page
						ld hl,extended.adr
						push hl						
						call load_file
						pop hl
						
personages_define_loop	ld a,(hl) ;определение типов врагов в наборе				
						or a
						jr z,personages_define_end
						dec a
						ld b,a
						add a,a
						add a,a
						add a,a
						sub b
						add a,low (personages+4)
						ld e,a
						ld a,high (personages+4)
						adc a,0
						ld d,a
						inc hl
						ld a,(hl)
						ld (de),a ;page
						inc hl
						dec de,de
						ld a,(hl)
						ld (de),a ;low adr
						inc hl,de
						ld a,(hl)
						ld (de),a ;high adr
						inc hl					
						jr personages_define_loop
personages_define_end
						setpage enemy_set_2.page
						ld hl,extended.adr
						call load_file
						
;карта-заставка с номером уровня						
level_map				im 2
						ei
						call init_offset_tab
						ld hl,location.adr+10
						ld (location.currentadr),hl
						ld a,16
						ld (location.width),a		
						call prepare_locbuffer
						call real_screen_draw
						
						IF version_type = tap
						call stop_tape
						ENDIF
						
						call clear_objects
						
						ld hl,32
						ld de,40
						ld bc,0
						ld a,(main_hero)
						call add_object
						
						call tiles_out
						call objects_out
						call view_screen
						call wait_any_key
						
						ld hl,screen+#1ae0
						ld de,screen+#1ae1
						ld bc,31
						ld (hl),b
						ldir
						
						ld a,(main_hero)
						cp id_Cody
						jr nz,level_map_1
						ld a,anim_super
						jr level_map_3
level_map_1				cp id_Guy
						jr nz,level_map_2
						ld hl,rocket_sfx
						call sfx_ay
						ld a,anim_weapon_throw
						jr level_map_3
level_map_2				ld a,anim_twister		
level_map_3				call start_animation
											
						ld b,75
level_map_loop			push bc
						halt
						call view_screen
						call tiles_out
						call objects_out
						ld ix,objects
						call play_animation
						ld a,(main_hero)
						cp id_Haggar
						jr nz,level_map_loop_1
						ld a,4
						call add_x_dir
						call play_animation
						jr level_map_loop_2			
level_map_loop_1		ld ix,objects+object.lenght
						ld a,12
						call add_x_dir
level_map_loop_2		pop bc
						djnz level_map_loop

;в TAP версии музыка запускается сразу после загрузки уровня
						IF version_type = tap
						ld hl,music.adr
						call ext_init_music
						ENDIF
						
						setpage ext.page
						ret

						IF version_type = tap
;сообщение "stop tape"						
stop_tape				ld hl,screen+#1ae0
						ld de,screen+#1ae1
						ld bc,31
						ld (hl),%11010111
						ldir
						
						ld hl,stop_tape_text
						ld de,#0217
						call drw_string
						ret
stop_tape_text			db "STOP TAPE AND PRESS ANY KEY",0
						
;сообщение "start tape"
start_tape				call clear_screen
						call ext_mute_music
						call ext_mute_sfx
						ld hl,start_tape_text
						ld de,#010c
						call drw_string
						ld b,150
						call pause
						ret
start_tape_text			db "START TAPE AND DON'T TOUCH KEYS",0	
				
						ENDIF
					
;инициализация локации			
init_location			

;сохранение прогресса
						IF version_type = trdos
						di
						im 1
						call ext_mute_music
						
						IF debug_mode = 0			
						ld a,(main_hero)
						dec a
						add a,a
						ld e,a
						ld d,0
						ld hl,Cody_level
						add hl,de
						ex de,hl
						ld hl,current_level
						ldi
						ldi

						ld iy,(iy_save)
						ld hl,saves_filename
						ld c,#13
						call #3d13			
						ld c,#12
						call #3d13
						ld hl,saves_begin
						ld de,saves_end-saves_begin
						ld c,#0b
						call #3d13
						ENDIF
						
;в TR_DOS версии музыка запускается в начале каждой локации
						ld hl,music.adr
						call ext_init_music
						im 2
						ei
						ENDIF

						ld a,(current_level)
						cp '1'
						jr nz,init_location_1
						ld a,(current_loc)
						cp 2
						jr nz,init_location_1
						ld a,#af ;инструкция xor a
						ld (prepare_loc_zero),a

;распаковка тайлсета
init_location_1			setpage tilesets.page
						ld hl,tilesets.adr
						ld de,temp_arx
						ld bc,7168 ;7кб
						ldir
						setpage ext.page
						ld a,(current_loc)
						add a,a
						ld l,a				
						ld h,high temp_arx
						ld a,(hl)
						inc l
						ld h,(hl)
						ld l,a
						ld de,temp_arx
						add hl,de
						ld de,tileset
						call unzip	
						
;установка адреса локации
						setpage location.page
						ld a,(current_loc)
						inc a
						add a,a
						ld l,a		
						ld h,high location.adr
						ld a,(hl)
						inc l
						ld h,(hl)
						ld l,a
						ld de,location.adr
						add hl,de		
						ld de,location.width
						ldi
						ldi			
						ld (location.startadr),hl
						ld (location.currentadr),hl				
						call prepare_locbuffer
						
						call init_statusbar

;установка текущего адреса в описателях врагов
						setpage location.page
						ld a,(location.height)
						ld b,a
						ld a,(location.width)
						ld e,a
						ld d,0
						ld hl,(location.startadr)
init_enemy_adr			add hl,de
						djnz init_enemy_adr
						ld a,(hl)
						ld (max_enemy_onscreen),a
						inc hl					
						ld (current_enemy_adr),hl

;прорисовка локации на реальном экране						
real_screen_draw		setpage ext.page
						halt
						ld hl,screen+#1840 ;зачерняем атрибуты, чтобы небыло артефактов
						ld de,screen+#1841
						ld bc,639
						ld (hl),0
						ldir
						
						ld a,#d5
						ld (interrupt_selector),a ;переключение прерываний на "pop de"
						ld de,screen+64
						ld hl,loc_buffer
						ld b,10
init_location1			push bc,de
						ld b,16
init_location2			push hl
						ld a,(hl)
						inc l
						ld h,(hl)
						ld l,a
						call drw_tile
						pop hl
						inc l,hl
						inc e,e
						djnz init_location2
						pop de,bc
						inc l,hl
						ld a,e
						add a,64
						ld e,a
						jp nc,init_location3
						ld a,d
						add a,8
						ld d,a
init_location3			djnz init_location1
						xor a
						ld (interrupt_selector),a ;переключение прерываний на "pop hl"
						ret

;вывод иконки персонажа и значка Rage
init_statusbar			call init_hero_scale
						jr init_rage_scale

init_hero_scale			xor a
						ld (drw_statusbar_rage+1),a
						ld a,1
						ld (frame_counter),a
						setpage ext.page
						ld de,screen
						ld a,(main_hero)
						call drw_icon
					
;аттрибуты статус-баров
						ld hl,screen+#1802
						ld de,screen+#1803
						ld bc,27
						ld (hl),%1000011
						ldir
												
						ld hl,screen+#1822
						ld de,screen+#1823
						ld bc,27
						ld (hl),%1000010
						ldir
						
						ld a,(main_hero)
						cp id_Guy
						ret nz
				
						ld hl,screen+#1809
						ld bc,8
						ld a,%0000101
						call fill_attr
						ret
			
init_rage_scale			call clear_ragebar
						ld de,screen+#10cf
						ld a,16
						call drw_icon
						
						ld hl,screen+#1ac5
						ld bc,22
						ld a,%1000111
						call fill_attr
						ld hl,screen+#1ac7
						ld bc,18
						ld a,%1000110
						call fill_attr
						ld hl,screen+#1ac9
						ld bc,14
						ld a,%1000010
						call fill_attr
						ld hl,screen+#1acb
						ld bc,10
						ld a,%1000011
						call fill_attr
						ld hl,screen+#1acd
						ld bc,6
						ld a,%1000001
						call fill_attr
						ld hl,screen+#1acf
						ld bc,2
						ld a,%1000111
						call fill_attr
						ret
					

;получить свойста тайла по координатам объекта со смещением в hl
;вх  - ix - описатель объекта
;      h - смещение X относительно xcord (-127;127)
;      l - смещение Y относительно ycord (-127;127)
;вых - a - свойства тайла
;      флаг Z - свойство = TP_PLATFORM
get_tileprop			push hl

						ld a,(ix+object.xcord+1) ;если X<0 or X>255 => X=0
						or a
						jr z,get_tileprop1
						ld h,0
						jr get_tileprop2
						
get_tileprop1			ld a,(ix+object.xcord)
						add a,h				
						dup 4
						rrca
						edup
						and #0f
						ld h,a
											
get_tileprop2			ld a,(ix+object.ycord)
						add a,l
						and #f0
						add a,h
						ld l,a

						ld h,high game_screen_property
						ld a,(hl)
						and TP_TYPEMASK
						cp TP_PLATFORM
						pop hl
						ret	
						
;скроллинг влево по локации на реальном экране
scroll_left				ld a,#d5
						ld (interrupt_selector),a ;переключение прерываний на "pop de"

						ld hl,(location.currentadr)	
						ld de,ONE_HSCROLL
						add hl,de
						ld (location.currentadr),hl
						call prepare_locbuffer
						setpage ext.page
						
						ld de,loc_buffer+(16-ONE_HSCROLL)*2
						ld b,ONE_HSCROLL
						
scroll_left_loop1		push bc,de
						
						halt

;пауза, чтобы пропустить луч вперед						
						ld bc,600
scroll_left_pause		dec bc
						ld a,b
						or c
						jr nz,scroll_left_pause
				
						exx
						ld hl,screen+#1800+64+2
						ld de,32
						exx
						
						ld hl,screen+64+2
						ld b,10
						
scroll_left_loop2		push bc,de,hl
						ld bc,#0802						
						
scroll_left_loop3		push bc,hl
						ld de,hl
						dec e,e
						dup 30
						ldi
						edup
						pop hl,bc
						inc h
						djnz scroll_left_loop3
						ld a,h
						sub 8
						ld h,a
						ld a,l
						add a,32
						ld l,a
						ld b,8
						dec c
						jp nz,scroll_left_loop3
						
						exx
						ld b,2
scroll_left_loop4		push bc,de,hl
						ld de,hl
						dec e,e
						dup 30
						ldi
						edup
						pop hl,de,bc
						add hl,de
						djnz scroll_left_loop4
						exx
						
						pop hl,de,bc
						
						push bc,de,hl ;вывод тайла
						ld bc,28
						add hl,bc
						ex de,hl
						ld a,(hl)
						inc l
						ld h,(hl)
						ld l,a
						call drw_tile
						pop hl,de
						
						ld a,l
						add a,64
						ld l,a
						jp nc,scroll_left_2
						ld a,h
						add a,8
						ld h,a
scroll_left_2			ex de,hl
						ld bc,34
						add hl,bc
						ex de,hl
						pop bc
						dec b
						jp nz,scroll_left_loop2
		
						pop de,bc
						inc e,de
						dec b
						jp nz,scroll_left_loop1
						xor a
						ld (interrupt_selector),a ;переключение прерываний на "pop hl"
						setpage ext.page
						ret

		
;подготовка буфера локации
prepare_locbuffer		setpage ext.page ;копируем свойства тайлов в temp_tile_prop
						ld hl,tileset
						ld de,temp_tile_prop
						ld bc,#100
						ldir
						
						setpage location.page
prepare_locbuffer_ext	ld hl,(location.currentadr)	
						ld de,game_screen_property
						ld bc,#100a
						exx
						ld hl,loc_buffer
						ld de,anim_tile_list
						exx
						
prepare_loc_loop		push bc,hl ;вычисляем адрес тайла
prepare_loc_zero		ld a,(hl)
						ld l,a 
						ld h,0
						add hl,hl
						ld bc,hl
						add hl,hl
						add hl,hl
						add hl,hl
						add hl,hl
						add hl,bc
						add hl,bc
						add hl,bc
						ld bc,tileset+#102
						add hl,bc
						push hl
						exx
						pop bc
						ld (hl),c
						inc hl
						ld (hl),b
						inc hl
						exx
						pop hl,bc
						
						push hl ;свойство тайла
						ld l,a
						ld h,high temp_tile_prop
						ld a,(hl)
						ld (de),a
						pop hl
						
						and TP_ANIMMASK ;если тайл анимированный, заносим его в список
						jr z,prepare_loc_end
						ld a,(de)
						exx
						ld (de),a ;свойства тайла
						inc de
						exx
						ld a,e
						exx
						ld (de),a ;координаты тайла
						inc de
						ex de,hl
						ld (hl),c
						inc hl
						ld (hl),b ;адрес тайла
						inc hl
						ex de,hl
						exx
						
prepare_loc_end			inc hl,e
						djnz prepare_loc_loop
						ld b,#10
						ld a,(location.width)
						sub 16
						add a,l
						ld l,a
						jr nc,$+3
						inc h
						exx
						inc hl,hl
						exx
						dec c
						jr nz,prepare_loc_loop
						
						exx	;устанавливаем признак окончания списка
						xor a
						ld (de),a
						;call init_offset_tab
						setpage ext.page
						ret

;анимирование тайлов на экране по списку
anim_tiles				setpage ext.page
						ld hl,anim_tile_list
						
anim_tiles_loop			ld a,(hl)
						or a
						ret z ;выход, если конец списка

						push hl ;вычисление номера анимации
						ld hl,anim_counters+16
						bit 4,a ;TP_RNDANIM
						jr z,anim_tiles_1
						ld hl,anim_counters+48
anim_tiles_1			and TP_ANIMMASK
						add a,l
						ld l,a
						jr nc,$+3
						inc h
						ld a,(hl)
						and 3
						add a,a
						ld e,a
						add a,a
						ld d,a
						add a,a
						add a,a
						add a,a
						add a,e
						add a,d
						ld e,a
						ld d,0 ; de = anim_num * 38						
						pop hl
						
						inc hl
						ld a,(hl) ;координаты тайла
						inc hl
						ld c,(hl)
						inc hl
						ld b,(hl) ;bc = адрес базового тайла
						inc hl
						
						push hl
						
						ex de,hl
						add hl,bc ;hl = адрес тайла анимации
						ld b,a ;b = координаты тайла
						
						ld a,(go_visible) ;не обновляем атрибуты когда надпись Go!
						or a
						jr z,anim_tiles_3
						ld a,b
						and #0f
						cp 12
						jr c,anim_tiles_3
						cp 15
						jr nc,anim_tiles_3
						ld a,b
						rrca
						rrca
						rrca
						rrca
						and #0f
						cp 3
						jr c,anim_tiles_3
						cp 5
						jr c,anim_tiles_end	
						
anim_tiles_3			ld a,b
						rrca
						rrca
						rrca
						rrca
						and #0f
						ld d,a
						ld a,b
						and #f0
						add a,d
						ld d,a
						ld a,b
						and #0f
						add a,d
						ld e,a
						ld d,0
						ex de,hl
						add hl,hl
						ld a,high loc_buffer
						add a,h
						ld h,a ;hl = адрес тайла в loc_buffer
						ld (hl),e
						inc l
						ld (hl),d ;обновляем адрес тайла в loc_buffer					
						
						ld a,b
						and #0f
						add a,a
						ld c,a
						ld a,b
						and #f0
						ld l,a
						ld h,0
						add hl,hl
						add hl,hl
						ld b,0
						add hl,bc
						ld bc,screen+#1840
						add hl,bc
						ex de,hl
						call drw_tile_attr
	
anim_tiles_end			pop hl						
						jp anim_tiles_loop
						
;вывод атрибутов тайла на реальный экран
;вх  - hl - адрес тайла
;      de - адрес атрибутов на реальном экране
drw_tile_attr			push bc,de,hl
						ld bc,32
						add hl,bc
						ldi
						ldi
						ex de,hl
						ld bc,30
						add hl,bc
						ex de,hl
						ldi
						ldi
						pop hl,de,bc
						ret
						
;вывод тайла на реальный экран
;вх  - hl - адрес тайла
;      de - адрес на реальном экране
drw_tile				push af,bc,de,hl
						push de
						ld a,d
						rrca
						rrca
						rrca
						and 3
						add a,high (screen+#1800)
						ld d,a
						ld (drw_tile_1+1),de
						pop de
						ld (drw_tile_sp+1),sp
						ld sp,hl
						ex de,hl
						
						ld b,4
drw_tile_loop_1			pop de
						ld (hl),e
						inc l
						ld (hl),d
						inc h
						pop de
						ld (hl),d
						dec l
						ld (hl),e
						inc h
						djnz drw_tile_loop_1

						ld a,h
						dec a
						and #f8
						ld h,a
						ld a,l
						add a,32
						ld l,a
						
						ld b,4				
drw_tile_loop_2			pop de
						ld (hl),e
						inc l
						ld (hl),d
						inc h
						pop de
						ld (hl),d
						dec l
						ld (hl),e
						inc h
						djnz drw_tile_loop_2
						
drw_tile_1				ld hl,0
						pop de
						ld (hl),e
						inc l
						ld (hl),d
						ld a,l
						add a,31
						ld l,a
						pop de
						ld (hl),e
						inc l
						ld (hl),d
drw_tile_sp				ld sp,0
						pop hl,de,bc,af
						ret
		
;инициализация анимации
;вх  - ix - описатель объекта
;      a - номер анимации
init_animation			cp (ix+object.animation)
						ret z ;выход, если происходит попытка повторно инициализировать уже идущую анимацию
						
init_animation_hard		ld (ix+object.animation),a ;насильное включение анимации
						ld h,(ix+object.animadr+1)
						add a,a
						ld l,a
						jr nc,$+3
						inc h
						ld a,(ix+object.animpage)
						call ram_driver
						ld a,(hl)
						inc l
						ld h,(hl)
						ld l,a
						ld e,0
						ld d,(ix+object.animadr+1)
						add hl,de
						ld a,(hl)
init_animation_nextfrm	ld (ix+object.frametime),a
						;ld de,9	;смещение адреса на sprites_count
						;add hl,de
						inc hl
	
;обработка события анимации	
						ld a,(hl) ;номер события
						or a
						jr z,init_animation_end
						push hl
						dec a
						add a,a
						ld e,a
						ld d,0
						ld a,(ix+object.type)
						dec a
						ld l,a
						add a,a
						add a,a
						add a,a
						sub l
						add a,low personages
						ld l,a
						ld a,high personages
						adc a,0
						ld h,a
						ld a,(hl)
						inc hl
						ld h,(hl)
						ld l,a
						add hl,de
						ld a,(hl)
						inc hl
						ld h,(hl)
						ld l,a
						ld (init_animation_event+1),hl
						setpage ext.page
init_animation_event	call 0 ;вызов события			
						pop hl
						
init_animation_end		inc hl
						ld (ix+object.composition),l
						ld (ix+object.composition+1),h
						setpage ext.page
						ret
		
;проигрывание анимации
;вх  - ix - описатель объекта	
play_animation			dec (ix+object.frametime)
						ret nz ;выход, если время воспроизведения кадра не закончилось
						
;вычисляем начало следующего кадра
						ld a,(ix+object.animpage)
						call ram_driver
						ld l,(ix+object.composition)
						ld h,(ix+object.composition+1)
						ld b,(hl) ;кол-во спрайтов
						inc hl
						ld de,5 ;длинна одного описателя спрайта
play_animation_1		add hl,de
						djnz play_animation_1
						ld a,(hl)
						or a
						jr nz,play_animation_4
						
;это последний кадр анимации	
						res_status IS_NOINT
						res_status IS_CRITHIT
						res_status IS_SUPERHIT
						res_status IS_CRITDAMAGE
						res_status IS_SUPERDAMAGE
						res_status IS_THROW
						res_status IS_KNEE
						res_status IS_BLOCK

						ld a,(ix+object.type)
						cp id_Items
						jr nz,play_animation_2
						ld a,(ix+object.itemkind)
						jp init_animation_hard
						
play_animation_2		ld a,(ix+object.energy)
						or a
						jr z,play_animation_3
						
						call hero_is_armed
						ld a,anim_idle
						jp z,init_animation_hard
						inc a
						jp init_animation_hard
						
play_animation_3		setpage ext.page
						ret
						
play_animation_4		cp 255
						jp nz,init_animation_nextfrm
						ld a,(ix+object.animation) ;зацикленная анимация
						jp init_animation_hard											
						
;проверка, вооружён-ли герой (исключение для Гая)
;вх -  ix - объект героя
;вых - флаг Z - не вооружён
hero_is_armed			ld a,(ix+object.type)
						cp id_Guy
						ret z
						ld a,(ix+object.weapon)
						or a
						ret					
						
;вывод объектов на виртуальный экран c сортировкой по высоте
objects_out				ld hl,objects+object.lenght*(object.maxnum-1)
						ld de,object.lenght
						ld b,object.maxnum		
objects_out_1			push hl
						or a
						sbc hl,de
						djnz objects_out_1
						
;сортировка "гномьим" алгоритмом

;void gnomeSort(int[] a) {
;	int i = 1;
;	while(i < a.length) {
;		if(i == 0 || a[i - 1] <= a[i])
;			i++;
;       else {
;			int temp = a[i];
;           a[i] = a[i - 1];
;           a[i - 1] = temp;
;           i--;
;       }
;   }
;}
						ld hl,2
						add hl,sp
						exx
						ld c,1
objects_out_2			ld a,c
						exx
						cp object.maxnum
						jp nc,objects_out_5
						or a
						jp z,objects_out_3
						inc hl
						ld b,(hl)
						dec hl
						ld c,(hl)
						dec hl
						ld d,(hl)
						dec hl
						ld e,(hl)
						inc hl,hl
						
						;inc bc,de
						;ex de,hl
						;ld a,(bc)
						;cp (hl)
						;ex de,hl
						;dec bc,de
						;jp c,objects_out_4
						ex de,hl
						ld a,(bc)
						cp (hl)
						ex de,hl
						jp c,objects_out_4

objects_out_3			inc hl,hl
						exx
						inc c
						jp objects_out_2
						
objects_out_4			inc hl
						ld b,(hl)
						ld (hl),d
						dec hl
						ld c,(hl)
						ld (hl),e
						dec hl
						ld (hl),b
						dec hl
						ld (hl),c
						exx
						dec c
						jp objects_out_2
						
;вывод объектов
objects_out_5			ld b,object.maxnum
objects_out_6			pop ix
						ld a,(ix+object.type)
						or a
						jr z,objects_out_10
						
;не выводим невидимую крыску
						cp id_Items
						jr nz,objects_out_6_1
						ld a,(ix+object.itemkind)					
						cp Items_RatIdle
						jr nz,objects_out_6_1
						bit 3,(ix+object.itemprop)
						jr nz,objects_out_10
						
;если энергия объекта равно нулю или он только что проинициализирован, выводим его мерцающим
objects_out_6_1			ld a,(ix+object.energy)
						or a
						jr nz,objects_out_7
						ld a,(ix+object.type)
						cp id_Belger
						jr z,objects_out_7
						
						ld a,(ix+object.yoffset)
						or a
						jr nz,objects_out_9
						jr objects_out_8
						
objects_out_7			ld a,(ix+object.undeadcntr)
						or a
						jr z,objects_out_9
						ld a,(ix+object.type)
						cp id_Bred
						jr nc,objects_out_9
						
objects_out_8			ld a,(interupt_counter)
						rrca
						rrca
						rrca
						rrca
						jr c,objects_out_10
objects_out_9			push bc
						call drw_object
						pop bc
objects_out_10			djnz objects_out_6
						setpage ext.page
						ret
						
;вывод объекта		
;вх  - ix - ссылка на объект	
drw_object				ld a,(ix+object.animpage)
						call ram_driver
						ld l,(ix+object.composition)
						ld h,(ix+object.composition+1)
						ld b,(hl) ;кол-во спрайтов
drw_object_loop			push bc
						inc hl
						ld c,(hl) ;тип спрайта и смещение страницы
						inc hl
						ld d,(hl) ;смещение X
						inc hl	
						ld e,(hl) ;смещение Y
						inc hl
						ld a,(hl)
						inc hl
						push hl
						ld h,(hl)
						ld l,a ;адрес спрайта
						push de
						ld e,0
						ld d,(ix+object.animadr+1)
						add hl,de
						pop de
						ld a,c
						and #0f
						add a,(ix+object.animpage)
						call ram_driver
						ld a,c
						and #f0
						jp nz,drw_object_loop_end ;выход, если тип спрайта не 0
						
;тип спрайта monochrome_with_mask
						ld a,(hl) ;width
						add a,7
						rrca
						rrca
						rrca
						and #1f
						ld b,a

						get_status IS_DIRECT ;корректировка смещения X
						jr z,drw_object_type0
						
						rlca
						rlca
						rlca
						and #f8
						add a,d
						neg
						ld d,a
						
drw_object_type0		inc hl
						ld c,(hl) ;height
						inc hl
						ex de,hl
;расчёт координат			
						push de
						ld e,h
						rl h
						sbc a,a
						ld d,a
						ld a,l
						ld l,(ix+object.xcord)
						ld h,(ix+object.xcord+1)
						add hl,de
						
;выход при слишком больших значениях X				
						exa
						ld a,h
						or a
						jr z,drw_object_type0_2
						inc a
						jr nz,drw_object_type0_1
						ld a,l
						cp -63
						jr nc,drw_object_type0_2
						
drw_object_type0_1		pop de ;выход по X
						jr drw_object_loop_end
						
drw_object_type0_2		exa
						push hl
						ld e,a
						rla
						sbc a,a
						ld d,a
						ld l,(ix+object.ycord)
						ld h,(ix+object.ycord+1)
						add hl,de
						ld e,(ix+object.yoffset) ;вычитаем высоту объекта над поверхностью
						ld d,0
						or a
						sbc hl,de
						
;выход при слишком больших значениях Y
						ld a,h
						or a
						jr z,drw_object_type0_3
						inc a
						jr nz,drw_object_type0_4
						ld a,l
						cp -63
						jr nc,drw_object_type0_5
						jr drw_object_type0_4
						
drw_object_type0_3		ld a,l
						cp 160
						jr c,drw_object_type0_5
											
drw_object_type0_4		pop hl,de ;выход по Y
						jr drw_object_loop_end
						
drw_object_type0_5		pop hl,de

						or a ;сбрасываем флаг C
						get_status IS_DIRECT
						jr z,$+3
						scf		
						call drw_sprite
						
						
drw_object_loop_end		pop hl,bc
						dec b
						jp nz,drw_object_loop
						ret
		
;вывод спрайта на виртуальный экран
;вх:  de - адрес данных спрайта
;     hl - координата x
;     a  - координата y
;     bc - размеры спрайта
;     nc - прямое отображение, c - зеркальное
drw_sprite				push af
						push hl
						
;обрезка по вертикали
						ld l,a
drw_sprite_lowbord_1	cp 160
						jr nc,drw_sprite2 ;возможно пересекается с верхней границей
						dec c
						add a,c
						inc c
drw_sprite_lowbord_2	sub 160
						jp c,drw_sprite5 ;точно не пересекается						
;пересекается с нижней границей
						inc a
						neg
						add a,c
						ld c,a
						jp drw_sprite5
drw_sprite2				dec c
						add a,c
						inc c
						jp c,drw_sprite3
						pop hl
						pop af
						ret ;находится вне видимости
						
;пересекается с верхней границей
drw_sprite3				inc a
						ld l,a
						neg
						add a,c
;сдвигаем адрес начала данных спрайта						
						push bc,hl
						ld l,b,h,0,b,a
						add hl,hl
						ex de,hl
drw_sprite4				add hl,de						
						djnz drw_sprite4
						ex de,hl
						pop hl,bc						
						ld c,l
						ld l,0
drw_sprite5				ld a,l
						pop hl
					
;помечаем область перерисовки экрана						
drw_sprite_range		push af,bc,de,hl ;a - YCORD, hl - XCORD, b - XSIZE, c - YSIZE

						ex de,hl	
						ld h,a
						
						ld a,e
						rrca
						rrca
						rrca
						and 1
						add a,b
						add a,2
						srl a
						ld b,a
											
						ld a,h
						and #0f
						add a,c
						add a,15
						dup 4
						rrca
						edup
						and #0f
						ld c,a
						
						ld a,h
						and #f0
						ld l,a
						ld a,h
						dup 4
						rrca
						edup
						and #0f
						or l
						ld l,a
						ld h,0
						add hl,hl
						ld a,h
						add a,high loc_buffer
						ld h,a 
						ex de,hl ; de = ycord / 16 * 17 * 2 + loc_buffer
						dup 4
						sra h
						rr l
						edup ; hl = xcord / 16
drw_sprite_range1		push bc,hl						
drw_sprite_range2		push hl
						ld a,l
						cp 17
						jr nc,drw_sprite_range3
						add hl,hl
						add hl,de
						inc hl
						set 7,(hl)
drw_sprite_range3		pop hl
						inc hl
						djnz drw_sprite_range2
						ld hl,34
						add hl,de
						ex de,hl
						pop hl,bc
						dec c
						jp nz,drw_sprite_range1				
						pop hl,de,bc,af
						
;вычисляем адрес vscreen
						push de,hl
						ld l,a
						ld h,0
						add hl,hl,hl,hl,hl,hl ;ycord * 40 (vscreen.width = 40)
						ld de,hl
						add hl,hl,hl,hl,hl,de
						ld de,vscreen
						add hl,de
						ld de,hl
						pop hl
						ld a,l
						and 7
						dup 3
						sra h
						rr l
						edup
						add hl,de
						pop de
						
						exa				
						pop af ;hl - vscreen adres, de - sprite data, bc - sprite size
						jp c,drw_sprite_mirror ;переход на зеркальное отображение спрайта
		
						exa
						add a,a
						add a,high temp_arx
						push de
						exx
						pop hl
						ld d,a
						ld bc,0
						exx 
						ld a,vscreen.width
						sub b
						ld e,a
						ld d,0 ;hl - vscreen, bc - size, de - (vscreen.width - sprite.width), 'hl - sprite_data, 'de - offset_tab, 'bc = 0

						push hl
						ld a,b
						dec a
						add a,a
						add a,low drw_sprite_dir_tab
						ld l,a
						ld a,high drw_sprite_dir_tab
						adc a,0
						ld h,a
						ld a,(hl)
						inc hl
						ld h,(hl)
						ld l,a
						ld (drw_sprite_dir_jp+1),hl
						ld (drw_sprite_dir_ret+1),hl
						pop hl
drw_sprite_dir_jp		jp 0						
						
drw_sprite_dir_tab		dw drw_sprite_dir_1, drw_sprite_dir_2, drw_sprite_dir_3, drw_sprite_dir_4, drw_sprite_dir_5, drw_sprite_dir_6, drw_sprite_dir_7
						
;ширина 7 байт						
drw_sprite_dir_7		ld a,(hl)
						exx						
						or b
						xor c						
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						inc h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						dec h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						inc hl
;ширина 6 байт							
drw_sprite_dir_6		ld a,(hl)
						exx						
						or b
						xor c						
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						inc h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						dec h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						inc hl
;ширина 5 байт	
drw_sprite_dir_5		ld a,(hl)
						exx						
						or b
						xor c						
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						inc h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						dec h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						inc hl
;ширина 4 байта	
drw_sprite_dir_4		ld a,(hl)
						exx						
						or b
						xor c						
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						inc h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						dec h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						inc hl
;ширина 3 байта	
drw_sprite_dir_3		ld a,(hl)
						exx						
						or b
						xor c						
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						inc h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						dec h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						inc hl
;ширина 2 байта	
drw_sprite_dir_2		ld a,(hl)
						exx						
						or b
						xor c						
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						inc h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						dec h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						inc hl
;ширина 1 байт	
drw_sprite_dir_1		ld a,(hl)
						exx						
						or b
						xor c						
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						inc h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						dec h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						inc hl					

						ld a,(hl)
						exx
						or b
						xor c
						ld bc,0
						exx
						ld (hl),a
						
						dec c
						ret z
						add hl,de
drw_sprite_dir_ret		jp 0
						
;зеркальное отображение спрайта											
drw_sprite_mirror		exa
						add a,a
						add a,#11+(high temp_arx)
						push de
						exx
						pop hl
						ld d,a
						ld bc,0
						exx
						ld e,b
						ld d,0
						add hl,de 
						ld a,vscreen.width
						add a,e
						ld e,a ;hl - (vscreen + sprite.width), bc - size, de - (vscreen.width + sprite.width), 'hl - sprite_data, 'de - offset_tab + #1100, 'bc = 0
						
						push hl
						ld a,b
						dec a
						add a,a
						add a,low drw_sprite_mir_tab
						ld l,a
						ld a,high drw_sprite_mir_tab
						adc a,0
						ld h,a
						ld a,(hl)
						inc hl
						ld h,(hl)
						ld l,a
						ld (drw_sprite_mir_jp+1),hl
						ld (drw_sprite_mir_ret+1),hl
						pop hl
drw_sprite_mir_jp		jp 0						
						
drw_sprite_mir_tab		dw drw_sprite_mir_1, drw_sprite_mir_2, drw_sprite_mir_3, drw_sprite_mir_4, drw_sprite_mir_5, drw_sprite_mir_6, drw_sprite_mir_7
						
;ширина 7 байт						
drw_sprite_mir_7		ld a,(hl)
						exx						
						or b
						xor c
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						dec h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						inc h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						dec hl						
;ширина 6 байт						
drw_sprite_mir_6		ld a,(hl)
						exx						
						or b
						xor c
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						dec h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						inc h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						dec hl
;ширина 5 байт						
drw_sprite_mir_5		ld a,(hl)
						exx						
						or b
						xor c
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						dec h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						inc h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						dec hl
;ширина 4 байта						
drw_sprite_mir_4		ld a,(hl)
						exx						
						or b
						xor c
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						dec h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						inc h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						dec hl
;ширина 3 байта						
drw_sprite_mir_3		ld a,(hl)
						exx						
						or b
						xor c
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						dec h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						inc h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						dec hl
;ширина 2 байта						
drw_sprite_mir_2		ld a,(hl)
						exx						
						or b
						xor c
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						dec h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						inc h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						dec hl
;ширина 1 байт						
drw_sprite_mir_1		ld a,(hl)
						exx						
						or b
						xor c
						ld e,(hl)
						inc l
						ld c,(hl)
						inc hl
						ex de,hl												
						or (hl)
						dec h
						ld b,(hl)						
						ld l,c
						ld c,(hl)
						inc h
						xor (hl)
						ex de,hl
						exx
						ld (hl),a
						dec hl

						ld a,(hl)
						exx
						or b
						xor c
						ld bc,0
						exx
						ld (hl),a
						
						dec c
						ret z
						add hl,de
drw_sprite_mir_ret		jp 0
	

;вывод строки на реальный экран
;вх  - de - координаты в символах на реальном экране
;      hl - адрес строки
;вых - hl - адрес следующей строки
drw_string				ld a,e
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
drw_string_loop			ld a,(hl)
						inc hl
						or a
						ret z
						call drw_symbol
						inc e
						jr drw_string_loop
						
;вывод иконки персонажа
;вх  - de - адрес на реальном экране
;      a - номер персонажа
drw_icon				or a
						jr z,drw_icon1
						dec a
						ld b,a
						rlca
						rlca
						and #60
						ld c,a
						ld a,b
						and 7
						add a,a
						add a,c
						add a,#a0
						call drw_symbol
						inc e,a
						call drw_symbol
						set 5,e
						add a,16
						call drw_symbol
						dec e,a
						call drw_symbol
						ret
drw_icon1				xor a
						call drw_symbol
						inc e
						call drw_symbol
						set 5,e
						call drw_symbol
						dec e
						call drw_symbol
						ret

;вывод шкалы
;вх  - de - адрес на реальном экране
;      b - ширина шкалы
;      с - значение
;      h - первый символ
;      l - направление вывода (1 - направо, -1 - налево)
drw_scale				ld a,c
						or a
						jr z,drw_scale1
						cp 2
						ld a,h
						jr nc,drw_scale1
						add a,l
drw_scale1				call drw_symbol
						set 5,e
						or a
						jr z,drw_scale2
						add a,16
drw_scale2				call drw_symbol
						res 5,e
						ld a,c
						sub 2
						jr nc,drw_scale3
						xor a		
drw_scale3				ld c,a
						ld a,e
						add a,l
						ld e,a
						djnz drw_scale
						ret											

;вывод картинки Go!
drw_go_img				ld a,(interupt_counter)
						and 16
						jr nz,drw_go_img_disable

;показываем Go!
drw_go_img_enable		ld de,screen+#800+24
						ld a,#e4
						ld b,4
drw_go_img_en_loop1		push bc,de						
						ld b,6
drw_go_img_en_loop2		call drw_symbol						
						inc e,a
						djnz drw_go_img_en_loop2
						pop de,bc
						exa
						ld a,e
						add a,32
						ld e,a
						exa
						cp #f0
						jr nz,$+4
						ld a,#f4
						djnz drw_go_img_en_loop1
						
						ld hl,screen+#1800+256+24
						ld bc,6
						ld a,%1000111
						call fill_attr
						ld hl,screen+#1800+256+64+24
						ld a,%1000111
						ld (go_visible),a ;надпись Go! на экране
						call fill_attr
						
;звуковой эффект								
drw_go_img_enable_s		ld a,0
						or a
						ret nz
						inc a
						ld (drw_go_img_enable_s+1),a
						jp sfx_clear
;стираем Go!						
drw_go_img_disable		ld a,(drw_go_img_enable_s+1)
						or a
						ret z
						xor a
						ld (drw_go_img_enable_s+1),a
						ld de,screen+#800+24
						ld hl,loc_buffer+17*3*2+24
						ld b,2
drw_go_img_di_loop1		push bc
						push de,hl
						ld b,3
drw_go_img_di_loop2		push hl
						ld c,(hl)
						inc l
						ld a,(hl)
						and #bf
						or #c0
						ld h,a
						ld l,c
						call drw_tile
						pop hl
						inc e,e,l,hl
						djnz drw_go_img_di_loop2
						pop hl,de
						ld bc,34
						add hl,bc
						ex de,hl
						ld bc,64
						add hl,bc
						ex de,hl
						pop bc
						djnz drw_go_img_di_loop1
						xor a
						ld (go_visible),a ;нет надписи Go! на экране
						ret
						
;очистка Rage-бара
clear_ragebar			ld hl,screen+#1ac0
						ld de,screen+#1ac1
						ld bc,63
						ld (hl),#47
						ldir
						ld hl,screen+#10c0
						ld b,16
clear_ragebar_loop		push bc,hl
						ld de,hl
						inc e
						ld bc,31
						ld (hl),b
						ldir
						pop hl,bc
						call down_hl
						djnz clear_ragebar_loop
						ret

;скролл Rage-бара вверх
scroll_ragebar			push af,bc,de,hl
						ld hl,screen+#10e0
						ld de,screen+#10c0
						ld bc,#2008
scroll_ragebar_loop_1	push bc,de,hl
scroll_ragebar_loop_2	ld a,(hl)
						ld (hl),0
						ld (de),a
						inc l,e
						djnz scroll_ragebar_loop_2
						pop hl,de,bc
						inc h,d
						dec c
						jr nz,scroll_ragebar_loop_1
						pop hl,de,bc,af
						ret
						
						
;печать строки, которая заканчивается #00
;вх  - hl - адрес текста
;      de - адрес на экране
print_string			setpage enemy_set_2.page
						ld a,(hl)
						inc hl
						or a
						jr z,print_string_end
						call drw_symbol
						inc e
						jr print_string
print_string_end		setpage ext.page
						ret
						
;печать текста в Rage-баре
;вх  - hl - адрес текста
;      bc - параметры голоса
;вых - флаг Z - yes, NZ - no
;      hl - адрес следущего текста
print_rage_text			ld (print_rage_voice_param+1),bc
						push hl
						call clear_ragebar
						pop hl
						ld b,25
						call pause
						
print_rage_text_new		call scroll_ragebar
						ld de,screen+#10e0
print_rage_text_loop	setpage enemy_set_2.page
						ld a,(hl)
						inc hl
						or a
						jr z,print_rage_text_new
						cp 254
						jr nz,print_rage_text_loop_1
						call wait_any_key
						jr print_rage_text_end

print_rage_text_loop_1	cp 255
						jr z,print_rage_text_select
						call drw_symbol
						cp " "
						jr z,print_rage_text_pause
	
print_rage_voice_param	ld bc,0	
						call sfx_voice
						
print_rage_text_pause	ld b,7
print_rage_text_pause_l	xor a
						ld (pressed_keys),a
						halt
						ld a,(pressed_keys)
						or a
						jr nz,print_rage_text_pause_e
						djnz print_rage_text_pause_l
						
print_rage_text_pause_e	inc e
						jr print_rage_text_loop

print_rage_text_select	push hl
						call scroll_ragebar
						ld hl,yesno_text
						ld de,screen+#10e0
						call print_string
						
						ld hl,screen+#1ae0
						ld (hl),#43
						ld l,#e5
						ld (hl),#43
						
print_rage_text_s_yes	ld hl,#5f20
						call print_rage_text_sel
print_rage_text_s_yes_l	xor a
						ld (clicked_keys),a
						halt
						ld a,(clicked_keys)
						bit KEY_RIGHT,a
						jr nz,print_rage_text_s_no
						bit KEY_FIRE,a
						jr z,print_rage_text_s_yes_l
						xor a
						jr print_rage_text_s_end
						
print_rage_text_s_no	ld hl,#205f
						call print_rage_text_sel
print_rage_text_s_no_l	xor a
						ld (clicked_keys),a
						halt
						ld a,(clicked_keys)
						bit KEY_LEFT,a
						jr nz,print_rage_text_s_yes
						bit KEY_FIRE,a
						jr z,print_rage_text_s_no_l
print_rage_text_s_end	pop hl
						
print_rage_text_end		push af
						setpage ext.page
						pop af
						ret			
						
print_rage_text_sel		ld de,screen+#10e0
						ld a,h
						call drw_symbol
						ld de,screen+#10e5
						ld a,l
						call drw_symbol
						call sfx_clear
						ret
						
;рисование блока тайлов 3х2 начиная со второго по счёту тайла в тайлсете
;вх  - hl - адрес в loc_buffer
drw_3x2_tiles			ld de,tileset+#102+38
						ld bc,#0302
drw_3x2_tiles_loop		ld (hl),e
						inc hl
						ld (hl),d
						inc hl						
						push hl
						ld hl,38
						add hl,de
						ex de,hl
						pop hl
						djnz drw_3x2_tiles_loop
						push de
						ld de,28
						add hl,de
						pop de
						ld b,3
						dec c
						jr nz,drw_3x2_tiles_loop					
						ret
						
;вывод экрана
;вх - de - адрес таблицы экранов
;     a - номер экрана
drw_screen				push af,bc,de,hl

						add a,a
						ld l,a
						ld h,0
						add hl,de
						ld a,(hl)
						inc hl
						ld h,(hl)
						ld l,a
						add hl,de
						
drw_screen_loop			ld a,(hl)
						cp #ff
						jp z,drw_screen_end
						inc hl
						ld d,(hl)
						inc hl
						ld e,(hl)
						inc hl
						ld b,(hl)
						inc hl
						call drw_color_spr
						jp drw_screen_loop

drw_screen_end			pop hl,de,bc,af
						ret
			
;вывод цветного спрайта
;вх - de - координаты в символах
;     a - номер спрайта	(адрес вычисляется по таблице с адреса temp_arx)	
;     b - значение мастер-атрибута, если = 0, то атрибуты спрайта		
drw_color_spr			push af,bc,de,hl

						push de
						add a,a
						ld l,a
						ld h,high temp_arx
						ld e,(hl)
						inc l
						ld d,(hl)
						ld hl,temp_arx
						add hl,de
						pop de ;hl = sprite adres
						
						ld a,b
						ld c,0 ;nop
						or a
						jr z,drw_color_spr_1
						ld c,#3e ;ld a,*					
drw_color_spr_1			ld (drw_color_spr_attr),bc ;модифицируем вывод

						ld a,e
						rrca
						rrca
						rrca
						ld b,a
						and #e0
						or d
						ld c,a
						ld d,a
						ld a,b
						and #03
						add a,high (screen + #1800)
						ld b,a 
						push bc
						exx
						pop bc
						exx ;bc,'bc = attr adres
						
						ld a,e
						ld e,d
						and #18
						add a,high screen
						ld d,a 
						push de
						exx
						pop de
						exx ;de,'de = screen adres
						
						ld a,(hl)
						inc hl
						exx
						ld h,a
						exx
						exa
						ld a,(hl)
						inc hl
						exx
						ld l,a
						exx ;'hl = sprite size, 'a = sprite width
						
drw_color_spr_loop		ld a,(hl)
						inc hl
						or a
						jp z,drw_color_spr_end ;пропускаем символ, если атрибут = 0

						bit 7,a
drw_color_spr_attr		ld a,0
						jp z,drw_color_spr_2
						and #7f ;пустой символ
						ld (bc),a
						jp drw_color_spr_end
						
drw_color_spr_2			ld (bc),a ;вывод целого символа						
						dup 7
						ld a,(hl)
						ld (de),a
						inc hl,d
						edup
						ld a,(hl)
						ld (de),a
						inc hl
						ld a,d
						and #f8
						ld d,a
						
drw_color_spr_end		inc c,e
						exx
						dec h
						exx
						jp nz,drw_color_spr_loop
						
						exx
						ld a,c
						add a,32
						ld c,a
						ld e,a
						jr nc,$+7
						inc b
						ld a,d
						add a,8
						ld d,a
						exa
						ld h,a
						exa
						dec l					
						push bc,de
						exx
						pop de,bc
						jp nz,drw_color_spr_loop
						
						pop hl,de,bc,af
						ret							
						
;обработка прерывания
interrupt       		ex (sp),hl
						ld (int_end+1),hl
						pop hl
						ld (int_sp+1),sp
interrupt_selector		nop ;выбор типа восстановления стека: #00 - HL, #d5 - DE
						ld sp,int_stek+31
						push af,bc,de,hl,ix
						exx 
						exa 
						push af,bc,de,hl
						ld a,(current_page) ;сохранияем текущую страницу памяти
						ld (int_page),a
						
						call get_input

;воспроизведение музыки и звуков						
interrupt_music			setpage sengine.page
						ld a,(music_ready)
						or a
						jr z,interrupt_music1
						ld a,(music_on)
						or a
						call nz,play_music
interrupt_music1		ld a,(sfx_on)
						or a
						call nz,play_sfx
						call ay_out

						ld hl,interupt_counter
						inc (hl)
						inc hl
						inc (hl)

;обработка таймеров
interrupt_timers		ld a,(timer_statusbar) ;таймер статус-бара
						or a
						jr z,init_rnd_anim
						dec a			
						jr nz,interrupt_timers1
						ld (drw_statusbar_type+1),a
						ld (drw_statusbar_energy+1),a
interrupt_timers1		ld (timer_statusbar),a
			
;инициализация сч-ков для случайной анимации
init_rnd_anim			call rnd
						and 63
						jr nz,inc_regular_anim
						
						ld hl,anim_counters+48
						ld bc,#1004
						ld d,0
init_rnd_anim_loop		ld a,(hl)						
						cp c
						jr nz,init_rnd_anim_loop_end
						ld (hl),d
init_rnd_anim_loop_end	inc hl
						djnz init_rnd_anim_loop
						
;счётчики регулярной анимации
inc_regular_anim		ld de,anim_counters
						ld hl,anim_counters+16
						ld bc,#1005
inc_regular_anim_loop	ld a,(de)
						inc a
						cp c
						jr nz,inc_regular_anim_loop_2
						inc (hl)
						ld a,(hl)
						cp 4
						jr nz,inc_regular_anim_loop_1
						ld (hl),0
inc_regular_anim_loop_1	xor a
inc_regular_anim_loop_2	ld (de),a						
						inc hl,de,c
						djnz inc_regular_anim_loop
			
;счётчики случайной анимации
inc_rnd_anim			ld de,anim_counters+32
						ld hl,anim_counters+48
						ld bc,#1005
inc_rnd_anim_loop		ld a,(de)
						inc a
						cp c
						jr nz,inc_rnd_anim_loop_2
						ld a,(hl)
						cp 4
						jr z,inc_rnd_anim_loop_1
						inc (hl)
inc_rnd_anim_loop_1		xor a
inc_rnd_anim_loop_2		ld (de),a						
						inc hl,de,c
						djnz inc_rnd_anim_loop
				
interrupt_end			ld a,(int_page) ;восстанавливаем страницу памяти
						call ram_driver
						pop hl,de,bc,af
						exx 
						exa 
						pop ix,hl,de,bc,af
int_sp          		ld sp,0
						ei 
int_end         		jp 0
int_stek        		ds 32
interupt_counter		db 0 ;счётчик прерываний
frame_counter			db 0 ;счётчик фреймов
anim_counters			ds 64 ;счётчики анимаций тайлов
int_page				db 0
timer_statusbar			db 0 ;таймер для статусбара				
						
						
						