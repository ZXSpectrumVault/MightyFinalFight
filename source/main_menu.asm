;модуль главного меню и выбора персонажей
						org main_menu.adr
						
						include "../res/screens/MainMenu.asm"	
						include "../res/screens/SelectPlayer.asm"
						include "../res/screens/Settings.asm"
						include "../res/screens/Intro.asm"
						include "../res/screens/Tutorial.asm"
						
						
;-------------------------------- главное меню --------------------------------						
						
						;call unzip_hero
						;jp tutorial
						
;распаковка музыки и экранов главного меню
						ld bc,0 ;b - номер выбранного пункта меню, c - счётчик анимации воды
main_menu_initmusic		push bc ;для красивого возврата из settings
						
						di
						call ext_mute_music
						ld hl,main_menu_music
						call init_menu_music	
				
main_menu_noinitmusic	call clear_screen
						ld hl,main_menu_screens
						ld de,temp_arx
						ld a,main_menu_res_1.page
						call ext_unzip					
						im 2
						ei
						
						ld de,MainMenu_offset+temp_arx
						ld a,MainLabel
						call drw_screen
						ld a,LightAnim0
						call drw_screen
						pop bc
						ld hl,0 ;h - счётчик фреймов до следующей анимации, l - анимация фонаря, если равен 1, пауза - если 2
						
main_menu_loop			ld a,PlaySelected
						add a,b
						call drw_screen						

;анимация воды						
main_menu_water			ld a,c
						and 7
						jr nz,main_menu_water_1
						ld a,c
						rrca
						rrca
						rrca
						and 3
						cp 3
						jr nz,$+4
						ld a,1
						add a,WaterAnim3			
						call drw_screen
						
main_menu_water_1		ld a,c
						and 15
						jr nz,main_menu_water_end
						ld a,c
						rrca
						rrca
						rrca
						rrca
						and 3
						cp 3
						jr nz,$+4
						ld a,1
						add a,WaterAnim0			
						call drw_screen
main_menu_water_end		inc c
				
;анимация персонажей				
main_menu_char			ld a,h
						or a
						jr z,main_menu_char_3
						dec h
						jr nz,main_menu_light
						
						ld a,l
						or a
						jr z,main_menu_char_3
						cp 1
						jp z,main_menu_char_1
						ld hl,#2000	
main_menu_char_1		ld a,LightAnim0
						call drw_screen
						jr main_menu_char_end
				
main_menu_char_3		call rnd
						and #0f
						jr nz,main_menu_char_4
						ld hl,#2001		
						jr main_menu_char_end
main_menu_char_4		cp 1
						jr nz,main_menu_char_5
						ld hl,#3202
						ld a,LightAnim1
						call drw_screen	
						jr main_menu_char_end
main_menu_char_5		cp 8
						jr nz,main_menu_char_end
						ld hl,#3202
						ld a,LightAnim2
						call drw_screen	
						jr main_menu_char_end
						
;анимация фонаря			
main_menu_light			ld a,l
						cp 1
						jr nz,main_menu_char_end
						ld a,h
						and 7
						jr nz,main_menu_light_1
						ld a,DarkAnim0
						call drw_screen	
						jr main_menu_char_end
main_menu_light_1		cp 4
						jr nz,main_menu_char_end
						ld a,LightAnim0
						call drw_screen							
main_menu_char_end	

;опрос контроллера
main_menu_key			call menu_navigation
						jr c,main_menu_fire
						or a
						jp z,main_menu_water
						jp main_menu_loop
					
main_menu_fire			ld a,b
						or a
						jr z,select_player
						dec a
						jp z,settings
						jp intro
						
;-------------------------------- выбор персонажа --------------------------------
select_player			call clear_screen			
								
;распаковка музыки и экранов выбора персонажа
						di
						call ext_mute_music
						ld hl,select_player_music
						call init_menu_music
						call clear_screen
						ld hl,select_player_screens
						ld de,temp_arx
						ld a,main_menu_res_2.page
						call ext_unzip
						im 2
						ei

;начальная анимация - выезжают надписи
						call init_any_key
						ld b,19
						ld de,#0814
select_anim_loop_1		ei
						halt
						push bc,de
						dec e,e
						ld a,sSelectLabel
						ld b,0
						call drw_color_spr
						pop de
						dec e
						call any_key
						pop bc
						jr nz,select_anim_init
						djnz select_anim_loop_1

						ld de,#1903
						ld bc,5 * 256 + sCodyEnable
						call move_hero_img
						jr nz,select_anim_init
						
						ld de,#190a
						ld bc,15 * 256 + sGuyEnable
						call move_hero_img
						jr nz,select_anim_init

						ld de,#1911
						ld bc,5 * 256 + sHaggarEnable
						call move_hero_img
						jr nz,select_anim_init
						
						call white_screen
						jr select_player_loop
						
						
select_anim_init		call clear_screen
						call start_select_screen					
								
select_player_loop		ld a,(main_hero)
						dec a
						ld b,a
						call menu_navigation
						push af
						ld a,b
						inc a
						ld (main_hero),a
						pop af
						jp nz,main_menu.adr
						jr c,tutorial_or_play ;переход в выбор туториала или игры
						or a
						jr z,select_player_loop
						
						ld a,b
						add a,CodySelect
						call drw_screen
						jp select_player_loop
						
;-------------------------------- выбор туториала или игры --------------------------------
						
tutorial_or_play		call enter_help
						
;распаковка экранов выбора туториала или игры
						ld hl,tutorial_screens
						ld de,temp_arx
						ld a,main_menu_res_1.page
						call ext_unzip
						
						ld de,Tutorial_offset+temp_arx

						ld b,0 ;номер пункта
tutorial_or_play_loop_1	ld a,b
						call drw_screen

tutorial_or_play_loop_2	xor a
						ld (clicked_keys),a						
						ei
						halt
						ld a,(clicked_keys)
						or a
						jr z,tutorial_or_play_loop_2
						
						push af,bc,de
						ld hl,select_sfx
						call sfx_ay
						pop de,bc,af
						
						bit KEY_PAUSE,a
						jr z,tutorial_or_play_loop_3
						
;возврат на select player
						ld hl,select_player_screens
						ld de,temp_arx
						ld a,main_menu_res_2.page
						call ext_unzip
						ld de,SelectPlayer_offset+temp_arx
						jp select_anim_init
						
tutorial_or_play_loop_3	bit KEY_FIRE,a
						jr nz,tutorial_or_play_fire

						ld a,b
						xor 1
						ld b,a
						jr tutorial_or_play_loop_1
						
tutorial_or_play_fire	push bc
						call ext_mute_sfx
						call ext_mute_music
						di
						im 1
						call unzip_hero			
						pop bc				
						dec b
						ret z
						
;-------------------------------- туториал --------------------------------
tutorial

;распаковка музыки туториала					
						ld hl,tutorial_music
						call init_menu_music
						im 2
						ei
						ld hl,tutor_hero_events
						ld (Cody.evenst),hl
						ld (Guy.evenst),hl
						ld (Haggar.evenst),hl
						ld hl,tutor_bred_events
						ld (Bred.evenst),hl
						
;инициализация комнаты для тренировки						
						call clear_screen
						call init_offset_tab
						ld hl,tutorial_location+4
						ld (location.currentadr),hl
						ld a,16
						ld (location.width),a	
						ld hl,tileset
						ld de,temp_tile_prop
						ld bc,#100
						ldir
						call prepare_locbuffer_ext
						ei
						halt
						call real_screen_draw
						call tiles_out
						call view_screen
						
;инициализация главного героя
						call clear_objects
						ld ix,objects
						ld hl,-64
						ld de,148 * 256
						ld a,(main_hero)
						call add_tutor_object
						ld (ix+object.faze),0
						ld (ix+object.cntr),90;90;64
						srl (ix+object.energy)
						ld a,anim_hero_walk
						call init_hero_animation

;главный цикл туториала
						ei
						halt
						xor a
						ld (pressed_keys),a
						inc a
						ld (frame_counter),a
						
;воспроизведение анимаций объектов
tutorial_loop			ld ix,objects
						ld b,object.maxnum
tutorial_loop_ap		push bc
						ld a,(ix+object.type)
						or a
						jr z,tutorial_loop_ap_e
						
;расчёт физики объекта для туториала
tutorial_physics		xor a
						ld b,(ix+object.yoffset)
						ld c,(ix+object.yaccel)
						cp b
						jr nz,tutorial_physics_1
						cp c
						jp z,tutorial_physics_end
tutorial_physics_1		ld a,c
						add a,2
						jr nz,tutorial_physics_2
						inc a
tutorial_physics_2		cp 20
						jr c,tutorial_physics_3
						cp 128
						jr nc,tutorial_physics_3
						ld a,20
tutorial_physics_3		ld (ix+object.yaccel),a
						sra a
						sra a
						ld c,a
						ld a,b
						sub c	
						jr nc,tutorial_physics_5
						bit 7,c
						jr nz,tutorial_physics_5
						
						xor a
						ld (ix+object.yaccel),a
						ld (ix+object.xaccel),a

						get_status IS_SUPERDAMAGE
						jr z,tutorial_physics_5
						ld (ix+object.yaccel),-15
						ld (ix+object.xaccel),-2
						ld (ix+object.energy),0
						res_status IS_SUPERDAMAGE
						xor a
						
tutorial_physics_5		ld (ix+object.yoffset),a

						or a
						jr z,tutorial_physics_end
						ld a,(ix+object.xaccel)
						or a
						jr z,tutorial_physics_end
						call add_x_dir
						
						ld a,(ix+object.xcord+1)
						or a
						jr nz,tutorial_physics_7
						ld a,(ix+object.xcord)
						cp RIGHT_BORDER
						jr nc,tutorial_physics_7
						cp LEFT_BORDER
						jr nc,tutorial_physics_end
						
tutorial_physics_7		ld (ix+object.xaccel),0					
tutorial_physics_end	call play_animation

tutorial_loop_ap_e		pop bc
						ld de,object.lenght
						add ix,de
						dec b
						jp nz,tutorial_loop_ap

						ld ix,objects
						ld b,(ix+object.faze)
						ld c,(ix+object.cntr)
						
;фаза 0 - начальный выход героя
						ld a,b
						or a
						jr nz,tutorial_loop_faze_1
						call tutorial_move_object
						dec c
						jr nz,tutorial_loop_faze_0_e
						inc b
						ld c,16
						
						;ld b,50
						
						xor a
						call init_hero_animation				
						ld ix,objects+object.lenght ;создаём объект врага
						xor a
						call tutorial_create_enemy
tutorial_loop_faze_0_e	jp tutorial_loop_end

;фаза 1 - приветственный текст						
tutorial_loop_faze_1	dec a
						jr nz,tutorial_loop_faze_2
						dec c
						jr nz,tutorial_loop_faze_0_e
						inc b
						ld c,90				
						ld de,tutorial_text_1
						call tutorial_print
tutorial_loop_faze_1_1	ld a,(pressed_keys)
						bit KEY_PAUSE,a
						jp nz,tutorial_quit
						ld a,(clicked_keys)
						bit KEY_FIRE,a
						jr z,tutorial_loop_faze_1_1
						call tutorial_clear
						jp tutorial_loop_rescntr
						
;фаза 2 - выход Бреда					
tutorial_loop_faze_2	dec a
						jp z,tutorial_wait_enemy
						
;фаза 3 - текст про непрерывное нажатие кнопки огонь				
tutorial_loop_faze_3	dec a
						jr nz,tutorial_loop_faze_4
						ld de,tutorial_text_2
						jp tutorial_text
						
;фаза 4 - тренировка непрерывного нажатия кнопки огонь				
tutorial_loop_faze_4	dec a
						jr nz,tutorial_loop_faze_5
						ld hl,anim_hero_left_punch * 256 + anim_hero_right_punch
						ld de,7 * 256 + #10
						ld a,140
						call tutorial_hero_hit
						jr tutorial_loop_faze_0_e						
						
;фаза 5 - текст про добивающий удар в прыжке		
tutorial_loop_faze_5	dec a
						jr nz,tutorial_loop_faze_6
						ld de,tutorial_text_3
						jp tutorial_text
						
;фаза 6 - тренировка добивающего удара в прыжке			
tutorial_loop_faze_6	dec a
						jr nz,tutorial_loop_faze_7		
						ld hl,anim_hero_left_punch * 256 + anim_hero_jumpkick
						ld de,3 * 256 + #11
						ld a,140
						call tutorial_hero_hit
						get_status IS_CRITHIT
						jr z,tutorial_loop_faze_0_e
						ld (ix+object.yaccel),-23
						jr tutorial_loop_faze_0_e
						
;фаза 7 - текст про удар в прыжке			
tutorial_loop_faze_7	dec a
						jr nz,tutorial_loop_faze_8	
						ld de,tutorial_text_4
						jp tutorial_text
						
;фаза 8 - тренировка удара в прыжке
tutorial_loop_faze_8	dec a
						jr nz,tutorial_loop_faze_9							
						ld a,(pressed_keys)
						cp %11001
						jr nz,tutorial_loop_faze_8_e
						inc b
						ld (ix+object.yaccel),-30
						ld (ix+object.xaccel),2
						set_status IS_CRITHIT
						ld c,170
						ld a,anim_hero_jumpsidekick
						call init_hero_animation
tutorial_loop_faze_8_e	jp tutorial_loop_end
						
;фаза 9 - текст про захват врага			
tutorial_loop_faze_9	dec a
						jr nz,tutorial_loop_faze_10
						ld de,tutorial_text_5
						jp tutorial_text

;фаза 10 - тренировка захвата врага			
tutorial_loop_faze_10	dec a
						jr nz,tutorial_loop_faze_11					
						ld hl,anim_hero_walk
						call tutorial_hero_walk
						ld a,(ix+object.xcord)
						cp 192
						jr nz,tutorial_loop_faze_8_e
						ld c,16
						inc b
						ld a,(main_hero)
						cp id_Haggar
						jr z,tutorial_loop_faze_10_5
						inc b ;пропускаем две фазы, если герой не Хаггар
						inc b
tutorial_loop_faze_10_5	ld a,anim_hero_throw
						call init_hero_animation
						jr tutorial_loop_faze_8_e

;фаза 11 - текст про возможность ходить с захваченным врагом для Хаггара	
tutorial_loop_faze_11	dec a
						jr nz,tutorial_loop_faze_12
						ld de,tutorial_text_6
						jp tutorial_text

;фаза 12 - тренировка возможности ходить с захваченным врагом для Хаггара	
tutorial_loop_faze_12	dec a
						jr nz,tutorial_loop_faze_13
						ld hl,anim_hero_throw * 256 + anim_hero_throw_walk
						call tutorial_hero_walk
						ld a,16
						get_status IS_DIRECT
						jr z,tutorial_loop_faze_12_1
						ld a,-16
tutorial_loop_faze_12_1	ld iy,objects+object.lenght
						add a,(ix+object.xcord)
						ld (iy+object.xcord),a
						ld a,(ix+object.status)
						xor 1
						and 1
						ld (iy+object.status),a
						get_status IS_DIRECT
						jr nz,tutorial_loop_faze_12_e
						ld a,(ix+object.xcord)
						cp 128
						jr nz,tutorial_loop_faze_12_e
						ld a,anim_hero_throw
						call init_hero_animation
						ld c,16
						inc b
tutorial_loop_faze_12_e	jp tutorial_loop_end
						
;фаза 13 - текст про удары по захваченному врагу
tutorial_loop_faze_13	dec a
						jr nz,tutorial_loop_faze_14
						ld de,tutorial_text_7
						jp tutorial_text
						
;фаза 14 - тренировка ударов по захваченному врагу							
tutorial_loop_faze_14	dec a
						jr nz,tutorial_loop_faze_15							
						get_status IS_NOINT
						jr nz,tutorial_loop_faze_12_e
						ld h,anim_hero_throw
						ld a,c
						cp 3
						jr nz,tutorial_loop_faze_14_1
						inc b
						ld c,30
						jr tutorial_loop_faze_14_2
tutorial_loop_faze_14_1	ld a,(pressed_keys)
						bit KEY_FIRE,a
						jp z,tutorial_loop_faze_14_2
						inc c
						ld h,anim_hero_knee
						set_status IS_NOINT
tutorial_loop_faze_14_2	ld a,h
						call init_hero_animation
						jr tutorial_loop_faze_12_e

;фаза 15 - текст про бросок врага
tutorial_loop_faze_15	dec a
						jr nz,tutorial_loop_faze_16
						ld de,tutorial_text_8
						jp tutorial_text

;фаза 16 - тренировка броска врага						
tutorial_loop_faze_16	dec a
						jr nz,tutorial_loop_faze_17
						ld a,(pressed_keys)
						bit KEY_FIRE,a
						jr z,tutorial_loop_faze_16_e
						ld a,(clicked_keys)
						bit KEY_LEFT,a
						jr z,tutorial_loop_faze_16_e
						set_status IS_DIRECT
						ld a,anim_hero_throw_down
						call init_hero_animation
						inc b
						ld c,150
						ld ix,objects+object.lenght
						set_status IS_SUPERDAMAGE
						ld a,Bred_Fly
						call init_enemy_animation
tutorial_loop_faze_16_e	jp tutorial_loop_end
						
;фаза 17 - создаём оружие						
tutorial_loop_faze_17	dec a
						jr nz,tutorial_loop_faze_18
						dec c
						jr nz,tutorial_loop_faze_16_e
						call tutorial_clear
						inc b
						ld c,50
						
						ld ix,objects+object.lenght ;создаём объект оружия
						ld hl,64
						ld de,148 * 256 + 160
						ld a,id_Items
						call add_tutor_object
						ld a,(main_hero)
						dec a
						add a,Items_KnifeWeapon
						call init_enemy_animation
						jp tutorial_loop_rescntr
						
;фаза 18 - текст про взятие оружия					
tutorial_loop_faze_18	dec a
						jr nz,tutorial_loop_faze_19
						ld de,tutorial_text_9
						jp tutorial_text
						
;фаза 19 - тренировка взятия оружия					
tutorial_loop_faze_19	dec a
						jr nz,tutorial_loop_faze_20
						ld hl,anim_hero_walk
						call tutorial_hero_walk
						ld a,(ix+object.xcord)
						cp 70
						jr nz,tutorial_loop_faze_16_e
						ld a,anim_hero_idle_weapon
						call init_hero_animation
						push hl
						ld hl,powerup_sfx
						call sfx_tutorial_ay
						pop hl
						inc b
						ld c,115
						ld (ix+object.weapon),1
						ld ix,objects+object.lenght ;создаём объект врага
						xor a
						call tutorial_create_enemy				
						ld a,(main_hero)
						cp id_Guy
						jr nz,tutorial_loop_faze_16_e
						ld bc,23 * 256 + 16
						jr tutorial_loop_faze_16_e
						
;фаза 20 - ожидание выхода нового врага				
tutorial_loop_faze_20	dec a
						jp z,tutorial_wait_enemy
						
;фаза 21 - текст про удар оружием					
tutorial_loop_faze_21	dec a
						jr nz,tutorial_loop_faze_22
						ld de,tutorial_text_10
						jp tutorial_text					
						
;фаза 22 - тренировка удара оружием				
tutorial_loop_faze_22	dec a
						jr nz,tutorial_loop_faze_23
						ld a,(pressed_keys)
						bit KEY_FIRE,a
						jr z,tutorial_loop_faze_22_e
						inc b
						ld c,170
						set_status IS_CRITHIT
						set_status IS_SUPERHIT
						ld a,anim_hero_hit_weapon
						call init_hero_animation
tutorial_loop_faze_22_e	jp tutorial_loop_end

;фаза 23 - создаём нового врага		
tutorial_loop_faze_23	dec a
						jr nz,tutorial_loop_faze_24
						dec c
						jr nz,tutorial_loop_faze_22_e
						inc b
						call tutorial_clear
						ld c,68
						ld ix,objects+object.lenght ;создаём объект врага
						xor a
						call tutorial_create_enemy				
						jp tutorial_loop_rescntr
						
;фаза 24 - ожидание выхода нового врага				
tutorial_loop_faze_24	dec a
						jp z,tutorial_wait_enemy						
						
;фаза 25 - текст про бросок оружия				
tutorial_loop_faze_25	dec a
						jr nz,tutorial_loop_faze_26
						ld de,tutorial_text_11
						jp tutorial_text	


;фаза 26 - тренировка броска оружия			
tutorial_loop_faze_26	dec a
						jr nz,tutorial_loop_faze_27
						ld a,(combo_active)
						bit 4,a
						jr z,tutorial_loop_faze_22_e
						ld c,255
						inc b
						ld a,anim_hero_thow_weapon
						call init_hero_animation
						jr tutorial_loop_faze_22_e
						
;фаза 27 - полёт оружия			
tutorial_loop_faze_27	dec a
						jr nz,tutorial_loop_faze_28					
						push bc
						ld ix,objects+(object.lenght*2)
						ld a,6
						call add_x
						pop bc
						dec c
						jr nz,tutorial_loop_faze_22_e
						ld c,200
						inc b
						xor a
						ld (objects+(object.lenght*2)+object.type),a
						ld ix,objects+object.lenght
						call tutorial_enemy_damage
						jr tutorial_loop_faze_22_e

;фаза 28 - текст про шкалу Rage		
tutorial_loop_faze_28	dec a
						jr nz,tutorial_loop_faze_29
						ld de,tutorial_text_12
						jp tutorial_text

;фаза 29 - демонстрация шкалы Rage
tutorial_loop_faze_29	dec a
						jr nz,tutorial_loop_faze_30
						ld a,c
						cp 100
						jr z,tutorial_loop_faze_29_1
						cp 101
						jr z,tutorial_loop_faze_29_2
						cp 102
						jr z,tutorial_loop_faze_29_3
						inc c
						jr tutorial_loop_faze_22_e
						
tutorial_loop_faze_29_1	call tutorial_clear
						push bc
						call init_rage_scale
						pop bc
						ld (ix+object.rage),0
						inc c
						jp tutorial_loop_end
						
tutorial_loop_faze_29_2	push bc
						call drw_statusbar_rage
						pop bc
						inc (ix+object.rage)
						ld a,(ix+object.rage)
						cp MAX_RAGE
						jr c,tutorial_loop_faze_34_e
						inc c
						jr tutorial_loop_faze_34_e
						
tutorial_loop_faze_29_3	push bc
						call drw_statusbar_rage
						pop bc
						dec (ix+object.rage)
						jr nz,tutorial_loop_faze_34_e
						inc b
						call tutorial_clear
						push bc
						call clear_ragebar
						pop bc
						ld c,114
						ld ix,objects+object.lenght ;создаём объект врага
						xor a
						call tutorial_create_enemy					
						jp tutorial_loop_rescntr

;фаза 30 - ожидание выхода нового врага				
tutorial_loop_faze_30	dec a
						jp z,tutorial_wait_enemy		
						
;фаза 31 - текст про фиолетовую зону Rage						
tutorial_loop_faze_31	dec a
						jr nz,tutorial_loop_faze_32
						ld de,tutorial_text_13
						ld a,(main_hero)
						cp id_Haggar
						jp nz,tutorial_text
						ld de,tutorial_text_14
						jp tutorial_text
						
;фаза 32 - демонстрация фиолетовой зоны Rage						
tutorial_loop_faze_32	dec a
						jr nz,tutorial_loop_faze_33
						ld a,20
						jp tutorial_rage_show
						
;фаза 33 - текст про апперкот или захват для Хаггара
tutorial_loop_faze_33	dec a
						jr nz,tutorial_loop_faze_34
						ld de,tutorial_text_15
						jp tutorial_text						
						
;фаза 34 - тренировка апперкота или захвата врага			
tutorial_loop_faze_34	dec a
						jr nz,tutorial_loop_faze_35
						ld hl,anim_hero_left_punch * 256 + anim_hero_uppercut
						ld de,4 * 256 + #12
						ld a,140
						call tutorial_hero_hit
						get_status IS_CRITHIT
						jr z,tutorial_loop_faze_34_e
						set_status IS_SUPERHIT
						ld b,41
						ld a,(main_hero)
						cp id_Cody
						jr z,tutorial_loop_faze_34_e
						cp id_Haggar
						jr nz,tutorial_loop_faze_34_1
						ld bc,39 * 256 + 16
						ld a,86
						ld (objects+object.lenght+object.xcord),a
						jr tutorial_loop_faze_34_e
tutorial_loop_faze_34_1	ld b,35	
tutorial_loop_faze_34_e	jp tutorial_loop_end

;фаза 35 - создаём объект врага
tutorial_loop_faze_35	dec a
						jr nz,tutorial_loop_faze_36
						dec c
						jr nz,tutorial_loop_faze_34_e
						inc b
						ld c,114
						ld ix,objects+object.lenght 
						xor a
						call tutorial_create_enemy
						call tutorial_clear
						jp tutorial_text

;фаза 36 - ожидание выхода нового врага				
tutorial_loop_faze_36	dec a
						jp z,tutorial_wait_enemy	

;фаза 37 - текст про второе добивание Гая
tutorial_loop_faze_37	dec a
						jr nz,tutorial_loop_faze_38
						ld de,tutorial_text_16
						jp tutorial_text

;фаза 38 - тренировка второго добивание Гая
tutorial_loop_faze_38	dec a
						jr nz,tutorial_loop_faze_39
						ld hl,anim_hero_left_punch * 256 + anim_hero_exthit
						ld de,4 * 256 + #18
						ld a,140
						call tutorial_hero_hit
						get_status IS_CRITHIT
						jr z,tutorial_loop_faze_34_e
						res_status IS_CRITHIT
						ld bc,41 * 256 + 140
						jr tutorial_loop_faze_34_e
						
;фаза 39 - текст finish him
tutorial_loop_faze_39	dec a
						jr nz,tutorial_loop_faze_40
						res_status IS_NOINT
						res_status IS_SUPERHIT
						ld de,tutorial_text_17
						jp tutorial_text						
						
;фаза 40 - прикончить врага		
tutorial_loop_faze_40	dec a
						jr nz,tutorial_loop_faze_41
						get_status IS_NOINT
						jr nz,tutorial_loop_faze_34_e
						ld h,anim_hero_throw
						ld a,(pressed_keys)
						bit KEY_FIRE,a
						jr z,tutorial_loop_faze_40_1
						ld h,anim_hero_knee
						set_status IS_NOINT
						inc c
						ld a,c
						cp 3
						jr nz,tutorial_loop_faze_40_1
						inc b
						ld c,140
						set_status IS_SUPERHIT
tutorial_loop_faze_40_1 ld a,h
						call init_hero_animation
						jr tutorial_loop_faze_34_e							
						
;фаза 41 - создаём двух врагов	
tutorial_loop_faze_41	dec a
						jr nz,tutorial_loop_faze_42
						dec c
						jr nz,tutorial_loop_faze_42_e
						call tutorial_clear
						inc b
						
						ld c,87
						ld a,(main_hero)
						cp id_Haggar
						jr nz,tutorial_loop_faze_41_1
						ld c,70
						
tutorial_loop_faze_41_1	ld ix,objects+object.lenght ;создаём объект врага
						xor a
						call tutorial_create_enemy	
						
						ld ix,objects+(object.lenght*2) ;создаём второй объект врага
						ld a,#ff
						call tutorial_create_enemy
						jp tutorial_loop_rescntr
						
;фаза 42 - перемещение врагов и героя в центр	
tutorial_loop_faze_42	dec a
						jr nz,tutorial_loop_faze_43				
						dec c
						jr nz,tutorial_loop_faze_42_1
						inc b
						ld c,16
						
						ld ix,objects+object.lenght
						xor a
						call init_enemy_animation
						ld ix,objects+(object.lenght*2)
						xor a
						call init_enemy_animation
tutorial_loop_faze_42_e	jp tutorial_loop_end
						
tutorial_loop_faze_42_1	ld a,(ix+object.xcord)
						cp 128
						jr nc,tutorial_loop_faze_42_2

						call tutorial_move_object
						ld a,anim_hero_walk
						call init_hero_animation
						jr tutorial_loop_faze_42_3

tutorial_loop_faze_42_2	xor a
						call init_hero_animation
						
tutorial_loop_faze_42_3	ld ix,objects+object.lenght
						call tutorial_move_object
						ld ix,objects+(object.lenght*2)
						call tutorial_move_object
						jr tutorial_loop_faze_46_e
						
;фаза 43 - текст про красную зону Rage						
tutorial_loop_faze_43	dec a
						jr nz,tutorial_loop_faze_44
						ld de,tutorial_text_18
						jp tutorial_text
						
;фаза 44 - демонстрация красной зоны Rage						
tutorial_loop_faze_44	dec a
						jr nz,tutorial_loop_faze_45
						ld a,36
						jp tutorial_rage_show

;фаза 45 - текст про твистер
tutorial_loop_faze_45	dec a
						jr nz,tutorial_loop_faze_46
						ld de,tutorial_text_19
						ld a,(main_hero)
						cp id_Haggar
						jp nz,tutorial_text
						ld de,tutorial_text_20
						jp tutorial_text	
						
;фаза 46 - тренировка твистера
tutorial_loop_faze_46	dec a
						jr nz,tutorial_loop_faze_47						
						ld a,(combo_active)
						bit 0,a
						jr z,tutorial_loop_faze_46_e
						ld (ix+object.hits),2
						ld bc,48 * 256 + 140
						ld h,-30
						ld a,(main_hero)
						cp id_Haggar
						jr nz,tutorial_loop_faze_46_1
						dec b
						ld h,0
tutorial_loop_faze_46_1	ld (ix+object.yaccel),h
						ld a,anim_hero_twister
						call init_hero_animation

						ld a,(main_hero)
						cp id_Haggar
						jr z,tutorial_loop_faze_46_e
						
						ld ix,objects+object.lenght
						call tutorial_enemy_damage					
						ld ix,objects+(object.lenght*2)
						call tutorial_enemy_damage
tutorial_loop_faze_46_e	jp tutorial_loop_end

;фаза 47 - перемещение Хаггара во время твистера
tutorial_loop_faze_47	dec a
						jr nz,tutorial_loop_faze_48
						ld hl,anim_hero_twister * 256 + anim_hero_twister
						call tutorial_hero_walk
						ld a,(ix+object.xcord)
						cp 160
						jr nz,tutorial_loop_faze_47_1
						ld ix,objects+object.lenght
						get_status IS_NOINT
						jr nz,tutorial_loop_faze_46_e
						call tutorial_enemy_damage
						set_status IS_NOINT
						jr tutorial_loop_faze_47_2
tutorial_loop_faze_47_1	cp 96
						jr nz,tutorial_loop_faze_46_e
						ld ix,objects+(object.lenght*2)
						get_status IS_NOINT
						jr nz,tutorial_loop_faze_46_e
						call tutorial_enemy_damage
						set_status IS_NOINT
tutorial_loop_faze_47_2	ld ix,objects
						dec (ix+object.hits)
						jr nz,tutorial_loop_faze_46_e
						inc (ix+object.hits)
						inc b
						xor a
						call init_hero_animation
						jr tutorial_loop_faze_46_e
						
;фаза 48 - окончание твистера
tutorial_loop_faze_48	dec a
						jr nz,tutorial_loop_faze_49
						ld a,c
						cp 140-35
						jr nz,tutorial_loop_faze_48_1
						xor a
						call init_hero_animation
						dec c
						jr tutorial_loop_faze_46_e
tutorial_loop_faze_48_1	cp 8
						jr nz,tutorial_loop_faze_48_2
						xor a
						call init_hero_animation
tutorial_loop_faze_48_2	dec c
						jr nz,tutorial_loop_faze_46_e
						inc b
						ld c,16
						xor a
						call init_hero_animation
						ld (ix+object.hits),0
						call tutorial_clear			
						ld ix,objects+object.lenght ;создаём объект врага
						xor a
						call tutorial_create_enemy
						ld ix,objects+(object.lenght*2) ;создаём объект второго врага
						xor a
						call tutorial_create_enemy
						ld (ix+object.xcord),160 ;сдвигаем, чтобы не вышли одновременно						
						jp tutorial_loop_rescntr

;фаза 49 - перемещение героя и врагов на позицию
tutorial_loop_faze_49	dec a
						jr nz,tutorial_loop_faze_50
						ld hl,0
						ld d,0
						ld a,(ix+object.xcord)
						cp 64
						jr z,tutorial_loop_faze_49_1
						set_status IS_DIRECT
						call tutorial_move_object
						ld h,anim_hero_walk
tutorial_loop_faze_49_1	ld a,h
						or a
						jr nz,tutorial_loop_faze_49_2
						res_status IS_DIRECT
tutorial_loop_faze_49_2	call init_hero_animation				
						ld ix,objects+object.lenght
						ld a,(ix+object.xcord)
						cp 128
						jr z,tutorial_loop_faze_49_3
						call tutorial_move_object
						ld l,Bred_Walk
tutorial_loop_faze_49_3	ld a,l
						call init_enemy_animation
						ld ix,objects+(object.lenght*2)
						ld a,(ix+object.xcord)
						cp 192
						jr z,tutorial_loop_faze_49_4
						call tutorial_move_object
						ld d,Bred_Walk
tutorial_loop_faze_49_4	ld a,d
						call init_enemy_animation
						ld a,h
						or l
						or d
						jr nz,tutorial_loop_faze_49_e
						inc b
						ld c,16
tutorial_loop_faze_49_e	jp tutorial_loop_end
						
;фаза 50 - текст про жёлтую зону Rage						
tutorial_loop_faze_50	dec a
						jr nz,tutorial_loop_faze_51
						ld de,tutorial_text_21
						jp tutorial_text
						
;фаза 51 - демонстрация жёлтой зоны Rage						
tutorial_loop_faze_51	dec a
						jr nz,tutorial_loop_faze_52
						ld a,52
						jp tutorial_rage_show

;фаза 52 - текст про супер-удар
tutorial_loop_faze_52	dec a
						jr nz,tutorial_loop_faze_53
						ld de,tutorial_text_22
						jp tutorial_text
						
;фаза 53 - тренировка супер-удара
tutorial_loop_faze_53	dec a
						jr nz,tutorial_loop_faze_54
						ld a,(combo_active)
						bit 3,a
						jr z,tutorial_loop_faze_49_e
						ld a,anim_hero_superhit
						call init_hero_animation
						inc b
						ld a,(main_hero)
						cp id_Cody
						jr z,tutorial_loop_faze_49_e
						ld hl,wave_sfx
						call sfx_tutorial_ay
						jr tutorial_loop_faze_49_e
						
;фаза 54 - перемещение во время супер-удара						
tutorial_loop_faze_54	dec a
						jr nz,tutorial_loop_faze_55
						ld a,(main_hero)
						cp id_Cody
						jr nz,tutorial_loop_faze_54_1
						ld ix,objects+(object.lenght*3)
						ld a,(ix+object.type)
						or a
						jr z,tutorial_loop_faze_54_e
tutorial_loop_faze_54_1	push bc
						ld a,6
						call add_x_dir
						pop bc
						ld a,(main_hero) ;выход и уничтожение энергетической волны 
						cp id_Cody
						jr nz,tutorial_loop_faze_54_2
						ld a,(ix+object.xcord+1)
						or a
						jr z,tutorial_loop_faze_54_2
						ld (ix+object.type),0
						inc b
						jr tutorial_loop_faze_54_e
tutorial_loop_faze_54_2	ld a,(ix+object.xcord)
						cp 110
						jr c,tutorial_loop_faze_54_e
						ld ix,objects+object.lenght
						get_status IS_NOINT
						jr nz,tutorial_loop_faze_54_3
						call tutorial_enemy_damage
						set_status IS_NOINT
						jr tutorial_loop_faze_54_e
tutorial_loop_faze_54_3	cp 174
						jr c,tutorial_loop_faze_54_e
						ld ix,objects+(object.lenght*2)
						get_status IS_NOINT
						jr nz,tutorial_loop_faze_54_4
						call tutorial_enemy_damage
						set_status IS_NOINT
						jr tutorial_loop_faze_54_e
tutorial_loop_faze_54_4	cp 220
						jr c,tutorial_loop_faze_54_e
						ld a,(main_hero)
						cp id_Cody
						jr z,tutorial_loop_faze_54_e
						xor a
						call init_hero_animation
						inc b
						ld c,140
tutorial_loop_faze_54_e	jp tutorial_loop_end
						
;фаза 55 - пустая фаза
tutorial_loop_faze_55	dec a
						jr nz,tutorial_loop_faze_56
						inc b
						jr tutorial_loop_faze_54_e
						
;фаза 56 - текст про белую зону Rage						
tutorial_loop_faze_56	dec a
						jr nz,tutorial_loop_faze_57
						ld de,tutorial_text_23
						jp tutorial_text
						
;фаза 57 - демонстрация белой зоны Rage						
tutorial_loop_faze_57	dec a
						jr nz,tutorial_loop_faze_58
						dec c
						jr nz,tutorial_loop_faze_54_e
						push bc
						call clear_ragebar
						call init_hero_scale
						call drw_statusbar_hero			
						pop bc
						inc c
						call tutorial_clear
						push bc
						call clear_ragebar
						ld a,68
						push af
						jr tutorial_rage_show_0

;фаза 58 - текст про пополнение жизни
tutorial_loop_faze_58	dec a
						jr nz,tutorial_loop_faze_59
						ld de,tutorial_text_24
						jp tutorial_text
						
;фаза 59 - тренировка пополнения жизней
tutorial_loop_faze_59	dec a
						jr nz,tutorial_loop_faze_60
						ld a,(combo_active)						
						bit 1,a
						jr z,tutorial_loop_faze_54_e
						ld hl,restore_life_sfx
						call sfx_tutorial_ay
						push bc
tutorial_loop_faze_59_1	ei
						halt
						inc (ix+object.energy)		
						call drw_statusbar_hero		
						ld a,(ix+object.energy)
						cp (ix+object.maxenergy)
						jr nz,tutorial_loop_faze_59_1
						pop bc		
						inc b
						jr tutorial_loop_faze_54_e

;фаза 59 - конец тренировки				
tutorial_loop_faze_60	call tutorial_clear
						ld de,tutorial_text_25
						call tutorial_print
						call wait_any_key
						jp tutorial_quit
					
;демонстрация Rage					
tutorial_rage_show		push bc
						push af
						ld b,100
						call pause
						call tutorial_clear
tutorial_rage_show_0	call init_rage_scale
						ld (ix+object.rage),0
						pop af
						ld b,a
tutorial_rage_show_1	push bc
						ei
						halt
						inc (ix+object.rage)
						call drw_statusbar_rage					
						pop bc
						djnz tutorial_rage_show_1
						
						ld b,3
tutorial_rage_show_2	push bc						
						ld b,10
						call pause
						call clear_ragebar
						ld b,10
						call pause
						call init_rage_scale
						xor a
						ld (drw_statusbar_rage+1),a
						call drw_statusbar_rage
						pop bc
						djnz tutorial_rage_show_2
						
						ld b,50
						call pause
						call tutorial_clear
						call clear_ragebar
						pop bc
						inc b
						ld c,16
						jp tutorial_loop_rescntr
						
;ожидание, когда выйдет враг						
tutorial_wait_enemy		res_status IS_DIRECT
						ld ix,objects+object.lenght
						call tutorial_move_object
						dec c
						jr nz,tutorial_loop_end
						ld c,16
						inc b
						xor a
						call init_enemy_animation
						jr tutorial_loop_end
						
;вывод текста для туториала, адрес в de
tutorial_text			dec c
						jr nz,tutorial_loop_end
						inc b
						push bc
						push de
						call tutorial_clear
						pop de
						call tutorial_print
						pop bc
						xor a
						ld (combo_active),a

;сброс счётчика фреймов	
tutorial_loop_rescntr	ld a,1
						ld (frame_counter),a	
						
;конец цикла
tutorial_loop_end		ld ix,objects
						ld (ix+object.faze),b
						ld (ix+object.cntr),c

						ld hl,frame_counter
						dec (hl)
						jp nz,tutorial_loop
						
;вывод тайлов и объектов
						ld a,(pressed_keys)
						bit KEY_PAUSE,a
						jr nz,tutorial_quit	
						xor a
						ld (pressed_keys),a
						ld (clicked_keys),a
						ei
						halt			
						call tiles_out
						call objects_out
						call view_screen
						jp tutorial_loop
		
;выход из тренировки		
tutorial_quit			ld hl,Cody_events
						ld (Cody.evenst),hl
						ld hl,Guy_events
						ld (Guy.evenst),hl
						ld hl,Haggar_events
						ld (Haggar.evenst),hl
						ld hl,Bred_events
						ld (Bred.evenst),hl
						
;распаковка музыки и экранов выбора персонажа
						di
						call ext_mute_music
						ld hl,select_player_music
						call init_menu_music
						call clear_screen
						ld hl,select_player_screens
						ld de,temp_arx
						ld a,main_menu_res_2.page
						call ext_unzip
						im 2
						ei
						jp tutorial_or_play
			
;добавить объект для туториала
;вх  - ix - адрес объекта
;      hl - коорд. X
;      d - коорд. Y
;      e - yoffset
;      a - тип объекта		
add_tutor_object		ld (ix+object.type),a
						ld (ix+object.xcord),l
						ld (ix+object.xcord+1),h
						ld (ix+object.ycord),d
						ld (ix+object.yoffset),e
						
						dec a
						ld l,a
						add a,a
						add a,a
						add a,a
						sub l
						add a,low (personages+2)
						ld l,a
						ld a,high (personages+2)
						adc a,0
						ld h,a
						ld a,(hl)
						ld (ix+object.animadr),a
						inc hl
						ld a,(hl)
						ld (ix+object.animadr+1),a
						inc hl
						ld a,(hl)
						ld (ix+object.animpage),a
						inc hl
						ld a,(hl)
						ld (ix+object.energy),a
						ld (ix+object.maxenergy),a
						ld (ix+object.animation),#ff
						ret			
	
;сдвинуть объект на 2 пикселя по направлению IS_DIRECT
;вх  - ix - адрес объекта
tutorial_move_object	push bc,de,hl
						ld a,2
						call add_x_dir
						pop hl,de,bc
						ret
						
;создать объект врага (Бред)
;вх  - ix - адрес объекта
;      hl - координата X
;      a - #00 - справа, #FF - слева

tutorial_create_enemy	push bc,de,hl
						or a
						jr nz,tutorial_create_enemy_1
						ld hl,320
						set_status IS_DIRECT
						jr tutorial_create_enemy_2
tutorial_create_enemy_1	ld hl,-64
						res_status IS_DIRECT
tutorial_create_enemy_2	ld de,147 * 256
						ld a,id_Bred
						call add_tutor_object
						ld a,Bred_Walk
						call init_animation
						pop hl,de,bc
						ret

;обработка движения героя
;вх  - h - номер анимации в покое
;      l - номер анимации в движении
tutorial_hero_walk		ld a,(pressed_keys)
						bit KEY_LEFT,a
						jr z,tutorial_hero_walk_1
						set_status IS_DIRECT
						ld a,(ix+object.xcord)
						cp 46
						jr c,tutorial_hero_walk_3
						ld h,l
						jr tutorial_hero_walk_2
						
tutorial_hero_walk_1	bit KEY_RIGHT,a
						jr z,tutorial_hero_walk_3
						res_status IS_DIRECT
						ld a,(ix+object.xcord)
						cp 210
						jr nc,tutorial_hero_walk_3
						ld h,l
						
tutorial_hero_walk_2	call tutorial_move_object
						
tutorial_hero_walk_3	ld a,h
						call init_hero_animation
						ret				
						
;обработка непрерывного нажатия на огонь и переход в следующую фазу
;вх  - h - номер анимации удара 1
;      l - номер анимации удара 2 (наносит критический урон)
;      d - значение счётчика для перехода в следущую фазу
;      e - код нажатых кнопок, для перехода в следущую фазу
;      a - начальное значение счётчика для следущей фазы
;вых - флаг Z
tutorial_hero_hit		exa
						ld a,(pressed_keys)
						bit KEY_FIRE,a
						jr nz,tutorial_hero_hit_1
						ld c,0
						ret
tutorial_hero_hit_1		get_status IS_NOINT
						ret nz	
						inc c
						cp e
						jr nz,tutorial_hero_hit_2
						ld a,c
						cp d
						jr c,tutorial_hero_hit_2
						set_status IS_CRITHIT
						ld h,l
						inc b
						exa
						ld c,a
tutorial_hero_hit_2		ld a,h
						call init_hero_animation
						set_status IS_NOINT
						ret				
						
;инициализация вражеской анимации
;вх  - ix - адрес объекта
;      a - номер анимации				
init_enemy_animation	push bc,de,hl
						call init_animation
						pop hl,de,bc
						ret

;инициализация вражеской анимации падения
;вх  - ix - адрес объекта				
tutorial_enemy_damage	set_status IS_SUPERDAMAGE
						ld a,Bred_DamageFall
						call init_enemy_animation	
						ld (ix+object.yaccel),-13
						ld (ix+object.xaccel),-2
						call sfx_tutorial_hit
						ret
						
						
;инициализация анимации по номеру в зависимости от типа героя
;вх  - a - номер анимации
init_hero_animation		push bc,de,hl
						ld ix,objects
						ld b,a
						add a,a
						add a,b
						add a,(ix+object.type)
						dec a
						add a,low init_hero_anim_tab
						ld l,a
						ld a,high init_hero_anim_tab
						adc a,0
						ld h,a
						ld a,(hl)
						call init_animation
						pop hl,de,bc
						ret

anim_hero_idle			equ 0
anim_hero_idle_weapon	equ 1
anim_hero_walk			equ 2
anim_hero_left_punch	equ 3
anim_hero_right_punch	equ 4
anim_hero_jumpkick		equ 5
anim_hero_jumpsidekick	equ 6
anim_hero_throw			equ 7
anim_hero_throw_walk	equ 8
anim_hero_knee			equ 9
anim_hero_throw_down	equ 10
anim_hero_hit_weapon	equ 11
anim_hero_thow_weapon	equ 12
anim_hero_uppercut		equ 13
anim_hero_exthit		equ 14
anim_hero_twister		equ 15
anim_hero_superhit		equ 16

init_hero_anim_tab		db Cody_Idle, Guy_Idle, Haggar_Idle
						db Cody_IdleKnife, Guy_Idle, Haggar_IdleHummer
						db Cody_Walk, Guy_Walk, Haggar_Walk				
						db Cody_PunchLeft, Guy_PunchLeft, Haggar_PunchLeft
						db Cody_PunchRight, Guy_PunchRight, Haggar_PunchRight
						db Cody_JumpKickUp, Guy_JumpKickUp, Haggar_JumpKickUp
						db Cody_JumpKickSide, Guy_JumpKickSide, Haggar_JumpKickSide
						db Cody_Throw, Guy_Throw, Haggar_Throw
						db 0, 0, Haggar_ThrowWalk
						db Cody_ThrowKick, Guy_ThrowKick, Haggar_ThrowHit
						db Cody_ThrowDown, Guy_ThrowDown, Haggar_ThrowDown
						db Cody_PunchKnife, 0, Haggar_HummerHit
						db Cody_ThrowKnife, Guy_ThrowShuriken, Haggar_ThrowHummer
						db Cody_Uppercut, Guy_Uppercut, Haggar_Throw
						db 0, Guy_SuperKick, 0
						db Cody_Twist, Guy_Twist, Haggar_Twist
						db Cody_Super, Guy_Super, Haggar_Super
				
;обработка супер-удара				
tutorial_superhit		set_status IS_CRITHIT
						set_status IS_SUPERHIT							
						
;обработка события удара						
tutorial_hit			ld a,(ix+object.hits)
						or a
						ret nz

						call sfx_tutorial_hit					
						ld b,Bred_Damage
						get_status IS_CRITHIT
						jr z,tutorial_hit_1
						
						ld b,Bred_DamageFall		
						ld a,(ix+object.faze)
						cp 6
						jr c,tutorial_hit_1	
						ld iy,objects+object.lenght
						ld (iy+object.yaccel),-18
						ld (iy+object.xaccel),-2
						
						get_status IS_SUPERHIT
						jr z,tutorial_hit_1
						set_enemy_status IS_SUPERDAMAGE

tutorial_hit_1			push ix	
						ld a,b
						ld ix,objects+object.lenght
						call init_animation						
						pop ix
						ret

;обработка события удара захваченного врага						
tutorial_knee			call sfx_tutorial_hit
						get_status IS_SUPERHIT
						push ix	
						ld a,Bred_Captured
						ld ix,objects+object.lenght
						jr z,tutorial_knee_1
						set_status IS_SUPERDAMAGE
						ld a,Bred_DamageFall
						ld (ix+object.yaccel),-13
						ld (ix+object.xaccel),-2
tutorial_knee_1			call init_animation						
						pop ix
						ret
						
;уничтожение объекта, если его энергия равна 0						
tutorial_kill			ld a,(ix+object.energy)
						or a
						ret nz
						ld (ix+object.type),a
						ret

;бросок объекта
tutorial_throw			ld (ix+object.yaccel),-25
						ld (ix+object.xaccel),-3
						ld a,(ix+object.status)
						xor 1
						ld (ix+object.status),a
						ret
	
;бросок оружия
tutorial_weapon_thow	ld a,(ix+object.type)
						cp id_Guy
						jr nc,tutorial_weapon_thow_1
						dec a
tutorial_weapon_thow_1 	add a,Items_Knife		
						push ix
						push af
						ld ix,objects+(object.lenght*2)
						ld hl,88
						ld de,148 * 256
						ld a,id_Items
						call add_tutor_object
						pop af
						call init_animation
						pop ix
						ld (ix+object.weapon),0
						ld (ix+object.cntr),16
						ret

;бросок энергетической волны Коди
tutorial_wave			ld a,(ix+object.type)
						cp id_Cody
						ret nz
						push ix
						ld ix,objects+(object.lenght*3)
						ld hl,88
						ld de,148 * 256
						ld a,id_Items
						call add_tutor_object
						res_status IS_DIRECT
						ld a,Items_Wave
						call init_animation
						ld hl,wave_sfx
						call sfx_tutorial_ay
						pop ix
						ret
						
;прыжок во время апперкота		
tutorial_jump			ld (ix+object.yaccel),-30
						jp tutorial_hit						
	
;звук удара	для тренировки
sfx_tutorial_hit		ld a,(sfx_on)
						or a
						ret z
						push bc,de,hl
						ld hl,#1000
						ld b,64
sfx_tutorial_hit_loop	ld a,b
						dup 4
						rlca
						edup
						xor (hl)
						and #10
						out (254),a
						inc hl
						ld a,(hl)
						and #7f
sfx_tutorial_hit_1		dec a
						jr nz,sfx_tutorial_hit_1
						djnz sfx_tutorial_hit_loop
						xor a
						out (254),a
						pop hl,de,bc
						ret				
				
tutorial_skip			ret				
				
;звук падения на землю для тренировки
sfx_tutorial_fall		ld a,(sfx_on)
						or a
						ret z
						ld bc,1000
						ld a,#10
						out (254),a
sfx_tutorial_fall_loop	ld a,b
						dup 4
						rlca
						edup
						and #10
						out (254),a
						dec bc
						ld a,b
						or c
						jr nz,sfx_tutorial_fall_loop
						xor a
						out (254),a
						ret	

;вызов AY звука
;вх  - hl - адрес звукового эфекта
sfx_tutorial_ay			push af,bc,de,hl
						call sfx_ay
						pop hl,de,bc,af
						ret

						
;-------------------------------- настройки игры --------------------------------				
settings				call enter_help

;распаковка экранов настройки
						ld hl,settings_screens
						ld de,temp_arx
						ld a,main_menu_res_1.page
						call ext_unzip

						ld b,0
						ld de,Settings_offset+temp_arx
						
settings_loop			ld a,KempstonFocus
						add a,b
						call drw_screen
						
						ld a,(kempston_on)
						ld c,a
						xor a
						cp c
						ld a,KempstonOff
						adc a,0
						call drw_screen
						
						ld a,(music_on)
						ld c,a
						xor a
						cp c
						ld a,MusicOff
						adc a,0
						call drw_screen
						
						ld a,(sfx_on)
						ld c,a
						xor a
						cp c
						ld a,SoundsOff
						adc a,0
						call drw_screen
					
settings_loop_1			call menu_navigation
						jr z,settings_loop_2
						
						ld bc,#0100 ;b - номер выбранного пункта меню, c - счётчик анимации воды
						push bc
						jp main_menu_noinitmusic
						
settings_loop_2			jr c,settings_loop_3 ;нажат Fire
						or a
						jr z,settings_loop_1
						jr settings_loop
					
settings_loop_3			ld a,b
						or a
						jr nz,settings_loop_4
						ld hl,kempston_on
						ld a,(hl)
						cpl
						ld (hl),a
						jr settings_loop
		
settings_loop_4			dec a
						jr nz,settings_loop_6
						ld hl,music_on
						ld a,(hl)
						cpl
						ld (hl),a
						or a
						jr nz,settings_loop_5
						push bc,de
						call ext_mute_music
						pop de,bc
						jr settings_loop
settings_loop_5			ld hl,music.adr
						push bc,de
						call ext_init_music
						pop de,bc
						jr settings_loop
						
settings_loop_6			ld hl,sfx_on
						ld a,(hl)
						cpl
						ld (hl),a
						jr settings_loop
						
;-------------------------------- интро --------------------------------
intro					call ext_mute_music
						call ext_mute_sfx
						ld b,25
						call intro_pause

;начальный кусок текста
						call clear_screen
						ld hl,#0604
						ld de,intro_text_1
						exa
						ld a,#ff
						exa
						call intro_print
						
						ld b,100
						call intro_pause

;распаковка музыки и экранов интро
						di
						call ext_mute_music
						ld hl,intro_music
						call init_menu_music	
						
						call clear_screen
						ld hl,intro_screens
						ld de,temp_arx
						ld a,main_menu_res_1.page
						call ext_unzip					
						im 2
						ei
						
						ld de,Intro_offset+temp_arx
						
						ld b,60
						call intro_pause
						
						ld a,Code ;кодер
						call drw_screen
						ld bc,#0a05
						call scroll_begin					
						ld b,11
						call scroll_next					
						ld b,25
						call intro_pause				
						ld a,Sanchez
						call drw_screen					
						ld b,25
						call intro_pause
						
						ld a,Graph ;художник
						call drw_screen
						ld bc,#180a
						call scroll_begin
						ld b,4
						call scroll_next					
						ld b,25
						call intro_pause					
						ld a,ER
						call drw_screen				
						ld b,25
						call intro_pause
						
						ld a,Music ;музыкант
						call drw_screen
						ld bc,#1210
						call scroll_begin
						ld b,7
						call scroll_next			
						ld b,25
						call intro_pause				
						ld a,NQ
						call drw_screen		
						ld b,25
						call intro_pause
						
						ld bc,#1e05 ;разбираем экран
						call scroll_next
						ld b,25
						call intro_pause				
						ld a,Sanchez_off
						call drw_screen		
						ld b,25
						call intro_pause			
						ld bc,#1e0a
						call scroll_next			
						ld b,25
						call intro_pause			
						ld a,ER_off
						call drw_screen			
						ld b,25
						call intro_pause		
						ld bc,#1e10
						call scroll_next			
						ld b,25
						call intro_pause	
						ld a,NQ_off
						call drw_screen
											
						call clear_screen
						ld de,Intro_offset+temp_arx
						ld b,115
						call intro_pause
						ld a,Screen1
						call drw_screen
						
						exa ;скрин с Белгером
						xor a
						exa
						ld hl,#0614
						ld de,intro_text_2
						call intro_print
						call intro_clear	
						ld hl,#0615
						ld de,intro_text_3
						call intro_print
						call intro_clear
						call clear_screen
						ld b,50
						call intro_pause
						ld hl,#060c
						ld de,intro_text_4
						call intro_print
						ld b,25
						call intro_pause
						call clear_screen
						ld b,25
						call intro_pause
										
						ld de,Intro_offset+temp_arx
						ld a,Screen2
						call drw_screen
						exa
						xor a
						exa
						ld b,25
						call intro_pause
						ld hl,#0616
						ld de,intro_text_5
						call intro_print
						call intro_clear	
						ld hl,#0616
						ld de,intro_text_6
						call intro_print
						call intro_clear	
						ld hl,#0615
						ld de,intro_text_7
						call intro_print
						call intro_clear	
						ld hl,#0616
						ld de,intro_text_8
						call intro_print
						call intro_clear
						ld hl,#0615
						ld de,intro_text_9
						call intro_print	
						ld b,100
						call intro_pause
						call clear_screen

intro_end				ld bc,#0200 ;b - номер выбранного пункта меню, c - счётчик анимации воды
						jp main_menu_initmusic

;-------------------------------- процедуры --------------------------------

;вывод медленного текста для туториала
;вх  - de - адрес текста
tutorial_print			push af,bc,de,hl
						ld hl,#0016
						ld bc,hl
tutorial_print_loop_1	call cord_to_adr
tutorial_print_loop_2	ei
						halt
						halt
						halt
						ld a,(de)
						inc de
						or a
						jr nz,tutorial_print_loop_3
						inc c
						ld hl,bc
						jr tutorial_print_loop_1
tutorial_print_loop_3	cp #ff
						jr z,tutorial_print_loop_4
						ex de,hl
						call drw_symbol
						ex de,hl
						inc l
						jr tutorial_print_loop_2
tutorial_print_loop_4	pop hl,de,bc,af
						ret

;очистка области текста	для туториала
tutorial_clear			push af,bc,de,hl
						ld hl,#0016
						ld b,16
						call cord_to_adr
tutorial_clear_loop		ei
						halt
						push bc,de,hl
						ld de,hl
						inc e
						ld bc,31
						ld (hl),0
						ldir
						pop hl,de,bc
						call down_hl
						djnz tutorial_clear_loop
						pop hl,de,bc,af
						ret

;распаковка выбранного героя		
unzip_hero				ld a,(main_hero)
;Коди
						cp id_Cody
						jr nz,unzip_hero_1
						ld hl,Cody.arx
						ld a,enemy_set_1.page
						jr unzip_hero_3
	
;Гай	
unzip_hero_1			cp id_Guy
						jr nz,unzip_hero_2
						ld hl,Guy.arx
						ld a,enemy_set_1.page
						jr unzip_hero_3
						
;Хаггар
unzip_hero_2			ld hl,Haggar.arx
						ld a,enemy_set_2.page
						
unzip_hero_3			ld de,temp_arx
						ld bc,#2000
						call ext_ldir
						xor a ;страница с выбранным героем
						ld hl,temp_arx
						ld de,extended.adr
						call ext_unzip
						ret

enter_help				call clear_screen
						ld hl,enter_help_text
						ld de,#0017
						call drw_string
						ret
enter_help_text			db "PRESS 'ENTER' OR 'H' FOR RETURN",0	
			
;очистка области текста	
intro_clear				ld hl,#0014
						ld b,32
						call cord_to_adr
intro_clear_loop		ei
						halt
						push bc
						call any_key
						pop bc
						jr nz,intro_clear_end
						push bc,de,hl
						ld de,hl
						inc e
						ld bc,31
						ld (hl),0
						ldir
						pop hl,de,bc
						call down_hl
						djnz intro_clear_loop
						ret
intro_clear_end			pop af
						jp intro_end
	
;вывод медленного текста для интро
;вх  - hl - координаты
;      de - адрес текста
;      a' - если 0, то вывод без звука
intro_print				ld bc,hl
intro_print_loop_1		call cord_to_adr
intro_print_loop_2		ei
						halt
intro_print_loop_3		ei
						halt
						halt
						push bc
						call any_key
						pop bc
						jr nz,intro_print_loop_6
						ld a,(de)
						inc de
						or a
						jr nz,intro_print_loop_4
						inc c
						ld hl,bc
						jr intro_print_loop_1
intro_print_loop_4		cp #ff
						jr z,intro_print_loop_end
						ex de,hl
						call drw_symbol
						ex de,hl
						inc l
						cp " "
						jr z,intro_print_loop_2
						exa
						push af
						exa
						pop af
						or a
						jr z,intro_print_loop_2
						call sfx_intro
						jr intro_print_loop_3
intro_print_loop_6		pop af
						jp intro_end
intro_print_loop_end	ld b,100
						jr intro_pause

;звук для интро		
sfx_intro				ld a,(sfx_on)
						or a
						ret z
						ei
						halt
						push bc,de,hl
						ld b,8
sfx_intro_loop			ld a,#10
						out (254),a					
sfx_intro_loop_1		dec c
						jr nz,sfx_intro_loop_1				
						ld c,0
						xor a
						out (254),a
sfx_intro_loop_2		nop
						nop
						dec c
						jr nz,sfx_intro_loop_2							
						ld c,0
						djnz sfx_intro_loop
						pop hl,de,bc
						ret
		
;прерываемая пауза для интро, при прерывании переход на intro_end
;вх  - b - длинна паузы в тактах
intro_pause				ld c,b
						ld b,0
						call interrupted_pause
						ret z
						pop af
						jp intro_end
		
;начальный выезд строк, при прерывании переход на intro_end
;вх  - b - ширина строки
;      c - номер первой скроллируемой линии
scroll_begin			push bc,de,hl
						ld hl,#0000
						ld d,b
						dec d
						ld e,1
scroll_begin_loop		ei
						halt
						push bc
						call any_key
						pop bc
						jr nz,scroll_begin_end
						ld a,c
						call scroll_line_left
						push de
						ld d,31
						ld e,c
						call copy_symbol
						pop de
						inc h					
						ex de,hl				
						ld a,c
						inc a
						call scroll_line_right
						push de
						ld d,0
						ld e,c
						inc e
						call copy_symbol
						pop de
						dec h						
						ex de,hl					
						djnz scroll_begin_loop
						xor a
scroll_begin_end		pop hl,de,bc
						ret z
						pop af
						jp intro_end

;продолжение скролла, при прерывании переход на intro_end
;вх  - b - кол-во шагов скролла
;      c - номер первой скроллируемой линии
scroll_next				push bc,de,hl
scroll_next_loop		ei
						halt
						push bc
						call any_key
						pop bc
						jr nz,scroll_next_end
						ld a,c
						call scroll_line_left
						ld a,c
						inc a
						call scroll_line_right
						djnz scroll_next_loop
						xor a
scroll_next_end			pop hl,de,bc
						ret z
						pop af
						jp intro_end
						
;скроллирование строки символов с номером a вправо
scroll_line_right		push bc,de,hl
						ld l,a
						ld h,30
						call cord_to_adr
						ld b,8
scroll_line_right_loop	push bc,hl
						ld de,hl
						inc e
						ld bc,31
						lddr
						inc l
						ld (hl),0
						pop hl,bc
						inc h
						djnz scroll_line_right_loop
						pop hl,de,bc
						ret
						
;скроллирование строки символов с номером a влево
scroll_line_left		push bc,de,hl
						ld l,a
						ld h,1
						call cord_to_adr
						ld b,8
scroll_line_left_loop	push bc,hl
						ld de,hl
						dec e
						ld bc,31
						ldir
						dec l
						ld (hl),0
						pop hl,bc
						inc h
						djnz scroll_line_left_loop
						pop hl,de,bc
						ret						
						
						
;копирование символа по координатам hl в координаты в de						
copy_symbol				push bc,de,hl
						call cord_to_adr
						ex de,hl
						call cord_to_adr
						ex de,hl
						ld b,8
copy_symbol_loop		ld a,(hl)
						ld (de),a
						inc h,d
						djnz copy_symbol_loop
						pop hl,de,bc
						ret

;преобразование координат в hl в адрес на реальном экране
cord_to_adr				ld a,l
						rrca
						rrca
						rrca
						and #e0
						or h
						ld h,a
						ld a,l
						and #18
						add a,high screen
						ld l,h
						ld h,a
						ret
				
;навигация по меню из 3 пунктов
;вх  - b - номер предыдущего пункта меню
;вых - b - номер текущего пункта меню
;      a - если а = 0, ни одна кнопка не была нажата
;      флаг C - нажата кнопка "FIRE"
;      флаг NZ - нажата кнопка "PAUSE"
menu_navigation			xor a
						ld (clicked_keys),a						
						ei
						halt
						ld a,(clicked_keys)
						or a
						ret z
						
						bit KEY_PAUSE,a
						ret nz
						
						push af,bc,de,hl
						ld hl,select_sfx
						call sfx_ay
						pop hl,de,bc,af						
						
						bit KEY_FIRE,a
						jr z,menu_navigation_1
						xor a
						scf
						jr menu_navigation_end

menu_navigation_1		bit KEY_RIGHT,a
						jr nz,menu_navigation_2
						bit KEY_DOWN,a
						jr z,menu_navigation_3
						
menu_navigation_2		inc b
						ld a,b
						cp 3
						jr nz,$+4
						ld b,0
						xor a
						jr menu_navigation_end

menu_navigation_3		dec b
						ld a,b
						inc a
						jr nz,$+4
						ld b,2
						xor a
menu_navigation_end		ld a,#ff
						ret
					
						
;скролл пиктограммы героя для меню выбора						
;вх  - de - начальные координаты
;      b - время паузы перед скроллом во фреймах
;	   c - номер спрайта с пиктограммой	
;вых - флаг Z - не было нажатия, NZ - было нажатие		
move_hero_img			push bc
						ld c,b
						ld b,0
						call interrupted_pause
						pop bc
						ret nz

						ld b,24
move_hero_img_loop		ei
						halt
						push bc,de
						ld a,c
						ld b,0
						call drw_color_spr
						ld a,d
						add a,6
						ld d,a
						ld a,sSelFrameLeft
						ld b,#40
						call drw_color_spr
						pop de
						dec d
						call any_key
						pop bc
						ret nz
						djnz move_hero_img_loop
						ret						
	
;двойная белая вспышка
white_screen			call white_screen_1
white_screen_1			ld a,#3f
						call white_screen_cls
						xor a
						call white_screen_cls
start_select_screen		ld de,SelectPlayer_offset+temp_arx
						ld a,SelectLabel
						call drw_screen
						ld a,(main_hero)
						dec a
						add a,CodySelect
						call drw_screen
						ret
						
white_screen_cls		ld b,5
						call pause
						ld b,a
						and 7
						out (254),a
						ld a,b
						ld hl,screen+#1800
						ld de,screen+#1801
						ld bc,#2ff
						ld (hl),a
						ldir
						ret
						
;инициализация выбранного трека из ресурсов главного меню
;вх  - hl - адрес трека
init_menu_music			ld de,temp_arx
						ld bc,#1000 ;максимально возможный размер трека
						ld a,main_menu_res_2.page
						call ext_ldir
						ld hl,temp_arx
						ld de,music.adr
						ld a,sengine.page
						call ext_unzip
						ld hl,music.adr
						call ext_init_music
						ret
						
intro_text_1			db "THIS IS METRO CITY.",0
						db "WITHIN THE WALLS",0
						db "OF THE CITY LIVES",0				
						db "THE DAUGHTER OF",0
						db "THE MAYOR, JESSICA.",0
						db "HER BEAUTY RADI -",0			
						db "ATES THROUGHOUT",0
						db "THE CITY AND GIVES",0
						db "THE CITIZENS THE",0				
						db "POWER TO SURVIVE.",0
						db "BUT NOW A VILLAIN",0
						db "ATTEMPTS TO HAVE",0			
						db "THIS BEACON OF",0
						db "LIGHT ALL TO",0
						db "HIMSELF.",255
						
intro_text_2			db "I CAN'T BELIEVE",0 ;тут идёт первая картинка
						db "THAT I'VE ACTUALLY",0
						db "FALLEN IN LOVE",0
						db "WITH THE GIRL.",255
						
intro_text_3			db "SHE WILL BE MINE..",0
						db "...",255
						
intro_text_4			db "A COUPLE DAYS",0 ;тут экран гаснет
						db "LATER...",255
						
intro_text_5			db "WHAT?! JESSICA...",0 ;тут идёт вторая картинка
						db "KIDNAPPED...!?!",255
						
intro_text_6			db "WHAT HAVE YOU DONE",0
						db "TO HER, CODY?",255
						
intro_text_7			db "ME?!! THIS IS THE",0
						db "WORK OF THE MAD",0
						db "GEAR GANG.",255
						
intro_text_8			db "WHAT DO YOU THINK,",0
						db "GUY?",255
						
intro_text_9			db "YES. THERE'S NOT A",0 ;тут экран гаснет
						db "MOMENT TO SPARE.",0
						db "LET'S GO!",255			
						
tutorial_text_1			db "HI! LET'S TRAINING!",0
						db 'PRESS "FIRE" FOR CONTINUE.',255
						
tutorial_text_2			db 'PRESS AND HOLD "FIRE" FOR',0
						db 'BEAT ENEMY.',255	

tutorial_text_3			db "FOR FINISH-JUMP-KICK PRESS AND",0
						db 'HOLD "FIRE"+"UP".',255	

tutorial_text_4			db "FOR JUMP-SIDE-KICK PRESS",0
						db '"FIRE"+"UP"+"RIGHT".',255
						
tutorial_text_5			db "FOR GRAB THE ENEMY",0
						db "JUST GO TO HIM.",255

tutorial_text_6			db "YOU CAN WALK WITH GRABBED ENEMY.",255

tutorial_text_7			db "FOR BEAT GRABBED ENEMY",0
						db 'PRESS "FIRE".',255
						
tutorial_text_8			db 'FOR THROW ENEMY HOLD "FIRE"',0
						db 'AND PRESS "LEFT".',255
						
tutorial_text_9			db "JUST GO TO THE WEAPON",0
						db "TO TAKE IT.",255

tutorial_text_10		db 'PRESS "FIRE".',255

tutorial_text_11		db "FOR THROW WEAPON PRESS",0
						db '"RIGHT"-"RIGHT"-"FIRE".',255
						
tutorial_text_12		db "AT THE BOTTOM OF THE SCREEN",0
						db 'THERE IS "RAGE" POWER:',255
						
tutorial_text_13		db 'IF "RAGE" IN MAGENTA ZONE,',0
						db "YOU CAN DO UPPERCUT:",255						
						
tutorial_text_14		db 'IF "RAGE" IN MAGENTA ZONE, YOU',0
						db "CAN GRAB ENEMY ON MORE DISTANCE:",255						
						
tutorial_text_15		db 'PRESS AND HOLD "FIRE"+"DOWN"',255

tutorial_text_16		db "ALSO YOU CAN DO BACK KICK,",0
						db 'PRESS AND HOLD "FIRE"+"RIGHT".',255

tutorial_text_17		db "FINISH HIM!",255

tutorial_text_18		db 'IF "RAGE" IN RED ZONE, YOU',0
						db "CAN DO TWIST-STRIKE:",255	

tutorial_text_19		db 'PRESS "FIRE"-"FIRE".',255

tutorial_text_20		db 'PRESS "FIRE"-"FIRE".',0
						db "YOU CAN MOVE.",255
						
tutorial_text_21		db 'IF "RAGE" IN YELLOW ZONE, YOU',0
						db "CAN DO SUPER-STRIKE:",255
						
tutorial_text_22		db 'PRESS "LEFT"-"RIGHT"-"FIRE".',255

tutorial_text_23		db 'IF "RAGE" IN WHITE ZONE, YOU',0
						db "CAN REFILL HALF LIFE:",255
	
tutorial_text_24		db 'PRESS',0
						db '"DOWN"-"DOWN"-"UP"-"UP"-"FIRE".',255
						
tutorial_text_25		db "WELL DONE!",0
						db "NOW YOU READY!",255
						
						savebin "../bin/mainmenu.bin", main_menu.adr, $ - main_menu.adr
						
						org main_menu_res_1.adr
main_menu_screens		incbin "../res/screens/MainMenu.bin.mlz"
intro_screens			incbin "../res/screens/Intro.bin.mlz"
settings_screens		incbin "../res/screens/Settings.bin.mlz"
tutorial_screens		incbin "../res/screens/Tutorial.bin.mlz"
						savebin "../bin/menures1.bin", main_menu_res_1.adr, $ - main_menu_res_1.adr
						
						org main_menu_res_2.adr
select_player_screens	incbin "../res/screens/SelectPlayer.bin.mlz"
select_player_music		incbin "../music/nq-mff-player-select-menu.pt3.mlz"	
main_menu_music			incbin "../music/nq-mff-title-screen.pt3.mlz"
intro_music				incbin "../music/nq-mff-prologue.pt3.mlz"
tutorial_music			incbin "../music/nq-mff-training.pt3.mlz"
						savebin "../bin/menures2.bin", main_menu_res_2.adr, $ - main_menu_res_2.adr