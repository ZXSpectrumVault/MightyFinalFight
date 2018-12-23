;игровые скрипты

;формат скрипта
;первый байт - тип скрипта
;#FF - конец скрипта
;#00 - движение к координате, следующие два байта - координаты, если координата =0 по ней нет проверки
;#01 - ожидание приближения другого объекта
;#02 - пауза, следующий байт - время паузы во фреймах
;#03 - переход в следущую локацию
;#04 - выполнить подпрограмму, следущие 2 байта адрес

script_hero_start		db 0, 60, 0, #ff
script_enemy_wait		db 1, #ff
script_hero_end_loc1_1	db 0, 230, 110, 3, #ff
script_hero_end_loc1_21 db 0, 0, 120, #ff
script_hero_end_loc1_22 db 2, 100, 0, 55, 0, 4, low little_jump, high little_jump, 0, 168, 0, 4, low draw_hole, high draw_hole, 2, 20, 4, low hide_hero, high hide_hero, 3, #ff
script_Trasher			db 0, 160, 0, 2, 50, 0, 255, 0, 2, 50, #ff
script_boss_start		db 0, 220, 0, 2, 10, 4, low boss_dialog, high boss_dialog, #ff
script_hero_fall		db 2, 40, 4, low hero_lose_weapon, high hero_lose_weapon, #82, 60, 4, low hide_screen, high hide_screen, #ff
script_hero_end_loc1_3	db 2, 50, #ff
script_hero_end_loc2_1	db 0, 255, 0, 3, #ff
script_hero_end_loc3_2	db 0, 160, 150, 4, low draw_trap, high draw_trap, 0, 204, 0, 4, low little_jump, high little_jump, 0, 220, 0, 4, low hide_hero, high hide_hero, 3, #ff
script_hero_end_loc4_1  db 0, 0, 140, 0, 110, 0, 4, low andor_kick, high andor_kick, 2, 175, 3, #ff
script_Andore			db 0, 85, 0, 4, low andor_kick_1, high andor_kick_1, #ff
script_hero_end_loc4_2	db 3, #ff
script_hero_end_loc5_1	db 0, 216, 115, 3, #ff
script_hero_end_loc5_2	db 0, 212, 115, 3, #ff
script_belger_end		db 0, 15, 155, 0, 18, 0, 4, low rocket_strike, high rocket_strike, 0, 0, 140, 4, low rocket_strike, high rocket_strike, 0, 0, 125, 4, low rocket_strike, high rocket_strike, 4, low belger_ha_ha_ha, high belger_ha_ha_ha, 2, 100 
						db 0, 241, 0, 0, 238, 0, 4, low rocket_strike, high rocket_strike, 0, 0, 140, 4, low rocket_strike, high rocket_strike, 0, 0, 155, 4, low rocket_strike, high rocket_strike, 4, low belger_ha_ha_ha, high belger_ha_ha_ha, 2, 100
						db #ff
script_belger_salut		db 4, low salut, high salut, #82, 250, #82, 250, 4, low salut_end, high salut_end, #82, 200, 4, low kill_belger, high kill_belger, #ff

;запуск скрипта
;вх - ix - объект
;     hl - адрес скрипта
start_script			ld a,#ff
next_script_adr			ld (ix+object.scripttype),a
						ld (ix+object.scriptadr),l
						ld (ix+object.scriptadr+1),h
						set_status IS_SCRIPTMODE
						ret

;воспроизведение скрипта
;вх - ix - объект
play_script				ld a,(ix+object.scripttype)
						cp #ff
						jp nz,play_script_move

						ld l,(ix+object.scriptadr)
						ld h,(ix+object.scriptadr+1)
						ld a,(hl)
;конец скрипта
						cp #ff
						jr nz,play_script_0
						res_status IS_SCRIPTMODE
						ret
						
;инициализация движения к координате	
play_script_0			ld b,a
						and #7f
						jr nz,play_script_1
						inc hl
						ld a,(hl)
						ld (ix+object.movecord),a
						inc hl
						ld a,(hl)
						ld (ix+object.movecord+1),a
						inc hl
						xor a
						jp next_script_adr
						
;инициализация ожидания приближения другого объекта
play_script_1			cp 1
						jr nz,play_script_2
						inc hl
						jp next_script_adr
						
;инициализация паузы
play_script_2			cp 2
						jr nz,play_script_3
						inc hl
						ld a,(hl)
						ld (ix+object.movecord),a
						inc hl
						push hl
						bit 7,b
						jr nz,play_script_2_1
						ld a,(ix+object.yoffset) ;запрет Idle если объект в воздухе
						or a
						jr nz,play_script_2_1
						call hero_is_armed
						ld a,anim_idle
						jr z,$+4
						ld a,anim_weapon_idle
						call start_animation
play_script_2_1			pop hl
						ld a,2
						jp next_script_adr

;инициализация перехода в следущую локацию
play_script_3			cp 3
						jr nz,play_script_4
						
						pop hl ;забываем о RET
						call delete_objects
						
						ld b,7
play_script_3_1			push bc
						halt
						halt
						ld hl,screen+#1840
						ld bc,640
play_script_3_2			ld a,(hl)
						ld e,a
						and 7
						jr z,play_script_3_3
						dec a
play_script_3_3			ld d,a
						ld a,e
						and #38
						jr z,play_script_3_4
						sub 8
play_script_3_4			or d
						ld (hl),a
						inc hl
						dec bc
						ld a,b
						or c
						jr nz,play_script_3_2
						pop bc
						djnz play_script_3_1
						
						ld b,35
						call pause
						
						
						ld hl,current_loc
						inc (hl)
						jp game_new_loc

;выполнение подпрограммы
play_script_4			cp 4
						ret nz		
						
						inc hl
						ld e,(hl)
						inc hl
						ld d,(hl)
						inc hl
						ld a,#ff
						call next_script_adr
						ex de,hl
						jp (hl)
						
;движение к координате
play_script_move		or a
						jr nz,play_script_wait
						ld a,(ix+object.movecord) ;проверка достижения цели по X
						or a
						jr z,play_script_move_8
						ld a,(ix+object.xcord+1) 
						or a
						jr nz,play_script_move_2
						ld a,(ix+object.xcord)
						sub (ix+object.movecord)
						jr nc,play_script_move_1
						neg
play_script_move_1		cp 2
						jr c,play_script_move_8

play_script_move_2		ld a,(ix+object.xcord+1) ;разворот в сторону движения
						or a
						jr z,play_script_move_5
						cp 1
						jr nz,play_script_move_4
						set_status IS_DIRECT
						jr play_script_move_7
play_script_move_4		res_status IS_DIRECT
						jr play_script_move_7	
						
play_script_move_5		ld a,(ix+object.xcord)
						cp (ix+object.movecord)
						jr c,play_script_move_6
						set_status IS_DIRECT
						jr play_script_move_7
play_script_move_6		res_status IS_DIRECT						
		
play_script_move_7		call get_object_speed
						call add_x_dir
						jr play_script_move_e

play_script_move_8		ld a,(ix+object.movecord+1) ;проверка достижения цели по Y
						or a
						jr z,play_script_move_9
						ld a,(ix+object.ycord+1)
						or a
						jr nz,play_script_move_10
						ld a,(ix+object.ycord)
						cp (ix+object.movecord+1)
						jr nz,play_script_move_10	
						
play_script_move_9		ld (ix+object.scripttype),#ff
						ret
	
play_script_move_10		ld a,(ix+object.movecord+1)
						cp (ix+object.ycord)
						sbc a,a
						or 1
						call add_y

play_script_move_e		call hero_is_armed
						ld a,anim_walk
						jp z,start_animation
						ld a,anim_weapon_walk
						jp start_animation
						
;ожидание приближения другого объекта						
play_script_wait		cp 1
						jr nz,play_script_pause
						
						ld iy,objects
						ld b,object.maxnum
play_script_wait_loop	push ix,iy
						pop hl,de
						or a
						sbc hl,de
						jr z,play_script_wait_end
						ld a,(iy+object.type)
						or a
						jr z,play_script_wait_end

						call distance_x
						ld a,h
						or a
						jr nz,play_script_wait_end
						ld a,l
						cp 64
						jr nc,play_script_wait_end
						
						ld (ix+object.scripttype),#ff
						ret
						
play_script_wait_end	ld de,object.lenght
						add iy,de
						djnz play_script_wait_loop
						ret
						
;пауза					
play_script_pause		cp 2
						ret nz
						
						dec (ix+object.movecord)
						ret nz
						
						ld (ix+object.scripttype),#ff
						ret
						
						
;рисуем дыру на крыше в локации 1-2
draw_hole				ld hl,loc_buffer+(34*6)+18 ;рисуем дыру в крыше
						call drw_3x2_tiles
						
						set_status IS_FALLDEEP
						ld a,120
						ld (drw_sprite_lowbord_1+1),a
						ld (drw_sprite_lowbord_2+1),a
						ld (ix+object.yaccel),-20
						push ix
						ld hl,music_fall.adr
						call ext_init_music
						pop ix
						ld a,anim_damage
						jp start_animation
						
;рисуем люк в конце локации 3-2
draw_trap				ld hl,loc_buffer+(34*8)+26 ;рисуем люк в полу
						jp drw_3x2_tiles	
						
;прячем героя за границу экрана
hide_hero				ld a,160
						ld (drw_sprite_lowbord_1+1),a
						ld (drw_sprite_lowbord_2+1),a
						ld (ix+object.ycord),240
						ld (ix+object.yaccel),0
						ld (ix+object.yoffset),0
						res_status IS_FALLDEEP
						ret
						
;маленький прыжок героя
little_jump				ld (ix+object.yaccel),-15
						ret

;пинок от Эндора в конце локации 4-1
andor_kick				ld hl,-64
						ld de,140
						ld bc,0
						ld a,id_Andore
						call add_object
						ld hl,script_Andore
						call start_script
						ret
						
andor_kick_1			ld hl,objects+object.energy
						ld a,(hl)
						add a,6
						ld (hl),a ;компенсируем удар Эндора
						ld a,anim_punchright
						call start_animation
						res_status IS_SCRIPTMODE
						set_status IS_NOINT
						ld ix,objects
						ld a,anim_damagefall
						call start_animation
						ld (ix+object.yaccel),-25
						ld (ix+object.xaccel),-2
						ret
						
;сцена падения героя в начале локации 1-3
hide_screen				ld a,#7e ;инструкция ld a,(hl)
						ld (prepare_loc_zero),a
						call prepare_locbuffer
						call real_screen_draw
						push ix
						ld hl,music_boss.adr
						call ext_init_music
						pop ix
						ret
						
hero_lose_weapon		ld a,(ix+object.weapon)
						or a
						ret z
						ld (ix+object.weapon),1
						jp lose_weapon

;диалог с боссом перед боем						
boss_dialog				ld a,(ix+object.type)
						cp id_Trasher
						jr nz,kitana_dialog

						ld hl,trasher_text_1
						ld bc,#0800
						call print_rage_text
						jr nz,trasher_dialog_3
						
trasher_dialog_1		ld hl,trasher_text_2
						ld bc,#0800
						call print_rage_text
						jr nz,trasher_dialog_2

						ld bc,#0800
						call print_rage_text
						jp init_statusbar
						
trasher_dialog_2		ld hl,trasher_text_4
						ld bc,#0800
						call print_rage_text
						jp init_statusbar
						
trasher_dialog_3		ld hl,trasher_text_5
						ld bc,#0800
						call print_rage_text
						jr z,trasher_dialog_1
						
						ld bc,#0800
						call print_rage_text
						jp init_statusbar
						
kitana_dialog			cp id_Abigal
						jr nz,kitana_dialog_1

						exa
						ld a,(current_level)
						cp '5'
						jr z,abigal_dialog_1
						exa
						
kitana_dialog_1			push af,ix
						ld hl,music_boss.adr				
						call ext_init_music
						pop ix,af

						cp id_Kitana
						jr nz,abigal_dialog
						ld a,(current_level)
						cp '2'
						jr nz,kitana_dialog_2
						
						ld hl,kitana_text_1
						ld bc,#0800
						call print_rage_text
						ld bc,#0ac0
						call print_rage_text
						ld bc,#0800
						call print_rage_text
						jr exit_to_init_statusbar
						
kitana_dialog_2			ld hl,kitana_text_4
						ld bc,#0800
						call print_rage_text
						jr exit_to_init_statusbar
						
abigal_dialog			cp id_Abigal
						jr nz,belger_dialog
						ld hl,abigal_text_1
						ld bc,#0800
						call print_rage_text
						ld bc,#0ac0
						call print_rage_text
						ld bc,#0800
						call print_rage_text
						jr exit_to_init_statusbar
				
abigal_dialog_1			ld hl,abigal_text_4
						ld bc,#0800
						call print_rage_text
						jr exit_to_init_statusbar
			
belger_dialog			ld hl,belger_text_1
						ld bc,#0ac0
						call print_rage_text
						ld bc,#0800
						call print_rage_text
exit_to_init_statusbar	jp init_statusbar

rocket_strike			ld h,10
						ld b,Items_Arm
						ld a,id_Items
						call child_object
						ld (iy+object.hitpower),12
						ld (iy+object.damagearea),8
						ld (iy+object.damagearea+1),15
						ld (iy+object.itemspeed),4
						set_status IS_NOINT
						ld hl,rocket_sfx
						call sfx_ay
						ld a,anim_weapow_hit
						jp start_animation
						
belger_ha_ha_ha			set_status IS_NOINT
						ld a,anim_extmove
						jp start_animation
						
salut					ld a,#ff
						ld (salute_on),a
						call ext_mute_music
						ret
salut_end				xor a
						ld (salute_on),a						
						ret
						
kill_belger				ld (ix+object.type),0
						ret
						