;код программы	
						org code.adr
	

;конец игры		
game_over				call ext_mute_music
						call clear_screen
						ld hl,game_over_text
						ld de,#0c0c
						call drw_string
						
						ld b,25
						call pause
						
						ld hl,music_over.adr
						call ext_init_music
						
						ld bc,14*50 ;длительность трека game_over 14 секунд
						call interrupted_pause
						
						ld hl,music.adr
						call ext_init_music
						jr game_loop									
						
;потеря оружия, если оно было						
lose_weapon				ld a,(ix+object.weapon)
						or a
						ret z
						dec (ix+object.weapon)
						jr z,lose_weapon_1
						ld a,(ix+object.type)
						cp id_Guy
						ret nz
lose_weapon_1			ld h,0
						ld b,Items_Knife
						ld a,(ix+object.type)
						cp id_Cody
						jr z,$+4
						add a,b
						ld b,a
						ld a,id_Items
						call child_object
						ld a,(iy+object.status)
						xor 1
						ld (iy+object.status),a
						ld (iy+object.energy),0						
						ret		
						
;-----------------------------------------------------------------------------------------------------	

;старт игры
game_start				call init_level
		
;главный игровой цикл
game_loop				call init_hero

;новая локация
game_new_loc			call hero_go_start
						xor a
						ld (combo_active),a
						ld (game_screen_clear),a
						ld (drw_statusbar_hero+1),a
						ld (drw_statusbar_shuriken+1),a
						ld (drw_statusbar_rage+1),a
						inc a
						ld (frame_counter),a
						
						call init_location
						call init_offset_tab

game_loop_begin			ld hl,gameloop_cnt
						inc (hl)
						
						call spawn_enemy ;спавн врагов
						
						ld ix,objects
						
						ld a,(ix+object.type)
						or a
						jp z,game_over

						ld b,object.maxnum
game_loop_objects		push bc
						ld a,(ix+object.type)						
						or a
						jp z,game_loop_next
						exa
						
;расчёт физики объекта
object_physics			xor a
						ld b,(ix+object.yoffset)
						ld c,(ix+object.yaccel)
						cp b
						jr nz,object_physics_1
						cp c
						jp z,object_physics_end
object_physics_1		ld a,c
						add a,2
						jr nz,object_physics_2
						inc a
object_physics_2		cp 20
						jr c,object_physics_3
						cp 128
						jr nc,object_physics_3
						ld a,20
object_physics_3		ld (ix+object.yaccel),a
						sra a
						sra a
						ld c,a
						ld a,b
						sub c	
						jr nc,object_physics_5
						bit 7,c
						jr nz,object_physics_5
;соприкосновение с землей
						ld hl,0
						call get_tileprop
						cp TP_DEEP
						jr nz,object_physics_4
;падает в пропасть
						set_status IS_FALLDEEP
						
object_physics_4		xor a
						ld (ix+object.yaccel),a
						ld (ix+object.xaccel),a
						
						get_status IS_CRITDAMAGE
						call nz,sfx_fall
						
						get_status IS_SUPERDAMAGE
						jr z,object_physics_5
						ld (ix+object.yaccel),-15
						res_status IS_SUPERDAMAGE
						xor a
						
object_physics_5		ld (ix+object.yoffset),a

						or a
						jr z,object_physics_end
						ld a,(ix+object.xaccel)
						or a
						jr z,object_physics_end
						call add_x_dir
						
						ld a,(ix+object.xcord+1)
						or a
						jr nz,object_physics_7
						ld a,(ix+object.xcord)
						cp RIGHT_BORDER
						jr nc,object_physics_7
						cp LEFT_BORDER
						jr nc,object_physics_end
						
object_physics_7		ld (ix+object.xaccel),0
object_physics_end		
		
						ld a,(ix+object.undeadcntr) ;уменьшение undeadcntr
						sub 1
						adc a,0
						ld (ix+object.undeadcntr),a
						exa
						
;---------------------------- герои ----------------------------
hero_begin				cp id_Bred
						jp nc,enemy_begin
						
						ld a,(ix+object.stamina)
						sub 1
						adc a,0
						ld (ix+object.stamina),a
						
						ld a,(ix+object.twistercnt)
						sub 1
						adc a,0
						ld (ix+object.twistercnt),a
						
;отпускаем захваченного врага, если у него закончилась энергия или он падает в пропасть
						get_status IS_ENEMYCAPTURE
						jr z,hero_combo_life
						call get_iy
						get_enemy_status IS_FALLDEEP
						jr nz,hero_cancel_capture
						ld a,(iy+object.energy)
						or a
						jr nz,hero_combo_life
hero_cancel_capture		call capture_cancel
						
;комбо - пополнение жизни
hero_combo_life			ld hl,combo_active
						bit 1,(hl)
						jr z,hero_begin_1					
						ld a,(ix+object.energy)
						or a
						jr z,hero_begin_1						
						ld (hl),0		
						ld bc,65 + 60 * 256 ;стоимость твистера = 64, отнимает энергии 60
						call dec_rage
						jr c,hero_IsDamage_end
						
						ld a,(ix+object.maxenergy)
						ld b,a
						srl a
						add a,(ix+object.energy)
						cp (ix+object.maxenergy)
						jr c,hero_combo_life_1
						ld a,b
hero_combo_life_1		ld (ix+object.energy),a
						ld hl,restore_life_sfx
						call sfx_ay					
			
hero_begin_1			call deep_fall ;обработка падения в пропасть
						
						get_status IS_SCRIPTMODE ;герой в режиме скрипта
						jr z,hero_IsDamage
						call play_script
						jr hero_IsDamage_end
						
;получил повреждение						
hero_IsDamage			get_status IS_FALLDEEP
						jr nz,hero_IsDamage_end
						get_status IS_DAMAGE
						jr z,hero_IsDamage5
						res_status IS_DAMAGE
						res_status IS_SUPERMOVE
						call capture_cancel ;при получении урона отпускает захваченного врага
						get_status IS_CRITDAMAGE
						jr z,hero_IsDamage3

;критический урон		
						call lose_weapon
						ld (ix+object.yaccel),-20
						ld a,anim_damagefall
						jp game_loop_initanim
						
hero_IsDamage3			ld a,anim_damage
						jp game_loop_initanim
						
;пролёт по время критического повреждения			
hero_IsDamage5			get_status IS_SUPERDAMAGE
						jr nz,hero_super_move
						get_status IS_CRITDAMAGE
						jr z,hero_super_move
						ld a,(ix+object.yoffset)
						or a
						jr z,hero_IsDamage_end ;выходим если объект стоит на земле
						
						ld a,(ix+object.xcord)
						cp RIGHT_BORDER
						jr nc,hero_IsDamage_end	
						cp LEFT_BORDER
						jr c,hero_IsDamage_end	
						ld a,-1
						call add_x_dir
hero_IsDamage_end		jp game_loop_end
						
hero_super_move			get_status IS_SUPERMOVE
						jr z,hero_combo_throw_weapon
						ld a,6
						call add_x_dir
						ld de,#1010
						call weapon_damage
						ld a,(ix+object.xcord)
						cp LEFT_BORDER
						jr nc,hero_super_move_1
						ld (ix+object.xcord),LEFT_BORDER
						jr hero_super_move_2
hero_super_move_1		cp RIGHT_BORDER						
						jr c,hero_IsDamage_end
						ld (ix+object.xcord),RIGHT_BORDER
hero_super_move_2		res_status IS_SUPERMOVE
						res_status IS_NOINT
;падает в пропасть
						ld hl,0
						call get_tileprop
						cp TP_DEEP
						jr nz,hero_IsDamage_end
						set_status IS_FALLDEEP						
						jr hero_IsDamage_end
						

;комбо - бросок оружия
hero_combo_throw_weapon	ld hl,combo_active
						ld a,(ix+object.weapon) ;пропускаем комбо, если не вооружён
						or a
						jr z,hero_combo_twister
						ld a,(hl)
						and %00110000 ;combo_super_right или combo_super_left
						jr z,hero_combo_twister
						ld (hl),0
						
						ld a,(ix+object.type)
						cp id_Guy
						jr nz,hero_combo_throw_w_1
						dec (ix+object.weapon)
						jr hero_combo_throw_w_2
hero_combo_throw_w_1	ld (ix+object.weapon),0
hero_combo_throw_w_2	set_status IS_NOINT						
						ld a,anim_weapon_throw
						jp game_loop_initanim					
						
;комбо - твистер		
hero_combo_twister		call hero_is_armed ;пропускаем комбо, если вооружён
						jr nz,hero_keyread
						
						bit 0,(hl)
						jr z,hero_combo_super
						ld (hl),0
						
						ld a,(ix+object.yoffset)
						or a
						jr nz,hero_keyread
						
						ld bc,33 + 8 * 256 ;стоимость твистера = 32, отнимает энергии 8
						ld a,(ix+object.type)
						cp id_Haggar
						jr nz,$+4
						ld b,16
						call dec_rage
						jr c,hero_keyread_end
						
						call capture_cancel
						set_status IS_CRITHIT
						set_status IS_SUPERHIT
						res_status IS_NOINT
						ld a,(ix+object.type)
						cp id_Haggar
						jr nz,hero_combo_twister_1
						ld (ix+object.twistercnt),150
						jr hero_keyread_end
						
hero_combo_twister_1	ld (ix+object.yaccel),-30
						ld (ix+object.twistercnt),35
						ld a,anim_twister
						jp game_loop_initanim		
						
;комбо - суперудар
hero_combo_super		ld a,(hl)
						and %00001100 ;combo_super_right или combo_super_left
						jr z,hero_keyread

						ld a,(hl)
						and %11110011
						ld (hl),a
						
						ld bc,49 + 8 * 256 ;стоимость суперудара = 48, отнимает энергии 8
						call dec_rage
						jr c,hero_keyread_end
											
						ld a,(ix+object.type)
						cp id_Cody
						jr z,hero_combo_super_1
						set_status IS_SUPERMOVE
						set_status IS_CRITHIT
						set_status IS_SUPERHIT
hero_combo_super_1		set_status IS_NOINT
						call capture_cancel
						ld a,anim_super
						jp game_loop_initanim
						
;опрос клавиатуры
hero_keyread			ld a,(pressed_keys)
						ld c,a
						get_status IS_NOINT		
						jr z,hero_keyread1
						get_status IS_KNEE
						call z,turn_c ;разворот непрерываемой анимации
hero_keyread_end		jp game_loop_end

hero_keyread1			ld a,(ix+object.type) ;Хаггар может ходить с захваченным врагом
						cp id_Haggar
						jr z,hero_keyread2
						get_status IS_ENEMYCAPTURE
						jp nz,hero_enemycapture
						
hero_keyread2			bit KEY_FIRE,c ;обработка ударов
						jr z,hero_keyread3
						ld a,(ix+object.twistercnt)
						or a
						jr nz,hero_keyread3
						ld a,(ix+object.yoffset)
						or a
						jp z,hero_hits		
						
hero_keyread3			call turn_c ;разворот
						exa
						xor a ;признак, что персонаж двигался
						exa

hero_speed				call get_object_speed
						ld b,a
						
						ld a,(ix+object.yoffset)
						or a
						jr z,hero_moves_right
						ld b,2 ;ускорение во время прыжка

;вправо
hero_moves_right		bit KEY_RIGHT,c
						jr z,hero_moves_left
						
						ld a,(ix+object.xcord+1) ;ограничение выхода за экран
						or a
						jr nz,hero_moves_up
						ld a,(ix+object.xcord)
						cp RIGHT_BORDER
						jr nc,hero_moves_up
										
						ld a,b
						jr hero_moves_set_horiz						
;влево
hero_moves_left			bit KEY_LEFT,c
						jr z,hero_moves_up
						
						ld a,(ix+object.xcord+1) ;ограничение выхода за экран
						or a
						jr nz,hero_moves_up
						ld a,(ix+object.xcord)
						cp LEFT_BORDER
						jr c,hero_moves_up
						
						ld a,b
						neg
						
hero_moves_set_horiz	ld l,0 ;проверка на препятствие
						ld h,a
						call get_tileprop
						cp TP_EMPTY
						jr z,hero_moves_up
						cp TP_DEEP
						jr nz,hero_moves_set_horiz1
						ld a,(ix+object.yoffset)
						or a
						jr z,hero_moves_up
						
hero_moves_set_horiz1	bit KEY_FIRE,c
						jr nz,hero_moves_set_horiz2
						
						get_status IS_CRITHIT
						jr nz,hero_moves_set_horiz2
						call find_object_capture ;проверка на захват врага
						jp nz,game_loop_end
					
hero_moves_set_horiz2	ld a,h
						call add_x
						exa
						ld a,#ff
						exa
;вверх
hero_moves_up			ld a,(ix+object.yoffset)
						or a
						jr nz,hero_moves_end ;выход, если в прыжке

						bit KEY_UP,c
						jr z,hero_moves_down
						
						ld a,(ix+object.ycord+1) ;ограничение выхода за экран
						or a
						jr nz,hero_moves_end
						ld a,(ix+object.ycord)
						cp UP_BORDER
						jr c,hero_moves_end
												
						ld a,-1
						jr hero_moves_set_vert
;вниз
hero_moves_down			bit KEY_DOWN,c
						jp z,hero_moves_end
						
						ld a,(ix+object.ycord+1) ;ограничение выхода за экран
						or a
						jr nz,hero_moves_end
						ld a,(ix+object.ycord)
						cp DOWN_BORDER
						jr nc,hero_moves_end
												
						ld a,1
						
hero_moves_set_vert		ld h,0 ;проверка на препятствие
						ld l,a
						call get_tileprop
						jr nz,hero_moves_end	
						
						ld a,l
						call add_y
						exa
						ld a,#ff
						exa
						
hero_moves_end			ld a,(ix+object.yoffset)
						or a
						jp nz,game_loop_end

						ld a,(ix+object.twistercnt)
						or a
						jr z,hero_moves_end1
						ld a,anim_twister
						jp game_loop_initanim
						
hero_moves_end1			get_status IS_ENEMYCAPTURE
						jr nz,hero_moves_end8
						
						ld bc,anim_idle * 256 + anim_walk
						
						call hero_is_armed ;обработка анимаций с оружием
						jr z,hero_moves_end2
						ld bc,anim_weapon_idle * 256 + anim_weapon_walk

hero_moves_end2			exa
						or a
						ld a,c
						jp nz,game_loop_initanim
						ld (ix+object.hits),0
						ld a,b
						jp game_loop_initanim					
						
hero_moves_end8			exa ;Хаггар в режиме захвата врага
						or a
						ld a,anim_throw
						jr z,hero_moves_end9					
						ld a,anim_extmove
hero_moves_end9			call start_animation	
						ld a,(pressed_keys)
						ld c,a
						jp hero_enemycapture
						
;обработка ударов						
hero_hits				get_status IS_ENEMYCAPTURE ;бьём коленом, если герой держит врага
						jp nz,hero_enemycapture
						
						call turn_c ;разворот
						call hero_is_armed
						jp nz,hero_weapon_hit ;удар оружием, если оно есть
						
;прыжок вбок						
hero_jumpside			bit KEY_UP,c
						jr z,hero_punchleft
						bit KEY_RIGHT,c
						jr nz,hero_jumpside1
						bit KEY_LEFT,c
						jr z,hero_punchleft
hero_jumpside1			ld (ix+object.yaccel),-30
						set_status IS_CRITHIT
						ld a,anim_jumpkickside
						jp game_loop_initanim
								
hero_punchleft			set_status IS_NOINT
						ld b,4
						ld a,(ix+object.type) ;исключение для Хаггара
						cp id_Haggar
						jr nz,hero_punchleft_1
						ld b,2
hero_punchleft_1		ld a,(ix+object.hits)
						cp b
						jr nc,hero_punchright
						ld a,anim_punchleft
						jp game_loop_initanim

hero_punchright			set_status IS_CRITHIT
						ld (ix+object.hits),0
						bit KEY_UP,c
						jr nz,hero_jumpkick
						bit KEY_DOWN,c
						jr nz,hero_appercut
						ld a,(ix+object.type) ;дополнительный суперудар ногой у Гая
						cp id_Guy
						jr nz,hero_punchright1
						bit KEY_LEFT,c
						jr nz,hero_superkick
						bit KEY_RIGHT,c
						jr nz,hero_superkick				
hero_punchright1		ld a,anim_punchright
						jp game_loop_initanim
				
hero_jumpkick			ld (ix+object.yaccel),-23			
						ld a,anim_jumpkickup
						jp game_loop_initanim
						
;апперкот					
hero_appercut			ld a,(ix+object.type)
						cp id_Haggar
						jr z,hero_appercut1	
						ld bc,17 + 4 * 256 ;стоимость апперкота = 16, отнимает энергии 4
						call dec_rage
						jr c,hero_punchright1	
						set_status IS_SUPERHIT
						ld a,anim_uppercut
						jp game_loop_initanim
						
hero_appercut1			ld b,3 ;Хаггар хватает врага
hero_appercut2			push bc
						ld a,4
						call add_x_dir
						call find_object_capture
						pop bc
						jr nz,hero_appercut3
						djnz hero_appercut2
						ld a,-12
						call add_x_dir
						jr hero_punchright1
hero_appercut3			ld bc,17 + 8 * 256 ;стоимость захвата = 16, отнимает энергии 8
						call dec_rage
						jr nc,hero_appercut4
						call capture_cancel
						jr hero_punchright1
hero_appercut4			res_status IS_NOINT ;Хаггар хватает врага
						res_status IS_CRITHIT
						jp game_loop_end
						
;суперудар ногой у Гая					
hero_superkick			ld bc,17 + 8 * 256 ;стоимость апперкота = 16, отнимает энергии 8
						call dec_rage
						jr c,hero_punchright1
						res_status IS_CRITHIT
						ld a,anim_extmove
						jp game_loop_initanim			
;удар оружием			
hero_weapon_hit			set_status IS_NOINT
						set_status IS_CRITHIT
						ld a,anim_weapow_hit
						jp game_loop_initanim
				
;режим захвата врага				
hero_enemycapture		call get_iy
						call correct_enemy_cord
						ld a,(ix+object.cntr) ;таймер удержания врага
						or a
						jr z,hero_letgo
						dec (ix+object.cntr)
						jr nz,hero_letgo
						xor a ;anim_idle
						jp hero_capture_cancel
						
;отпускаем врага нажатием клавиши в противоположную сторону
hero_letgo				bit KEY_FIRE,c
						jr nz,hero_knee
						ld a,(ix+object.type) ;исключение для Хаггара
						cp id_Haggar
						jp z,game_loop_end						
						bit KEY_UP,c
						jr z,hero_letgo1
						ld a,(ix+object.ycord)
						cp UP_BORDER
						jr c,hero_letgo1
						ld hl,#00ff
						call get_tileprop
						ld a,anim_idle
						jr z,hero_capture_cancel						
hero_letgo1				bit KEY_DOWN,c
						jr z,hero_letgo2
						ld a,(ix+object.ycord)
						cp DOWN_BORDER
						jr nc,hero_letgo2
						ld hl,#0001
						call get_tileprop
						ld a,anim_idle
						jr z,hero_capture_cancel
hero_letgo2				get_status IS_DIRECT
						jr nz,hero_letgo3
						bit KEY_LEFT,c
						jr z,hero_enemycapture_end
						ld a,anim_idle
						jr hero_capture_cancel
hero_letgo3				bit KEY_RIGHT,c
						jr z,hero_enemycapture_end
						ld a,anim_idle
						jr hero_capture_cancel
						
;бьём коленом					
hero_knee				bit KEY_LEFT,c
						jr z,hero_knee1
						get_enemy_status IS_DIRECT
						jr nz,hero_throw
						jr hero_knee2
						
hero_knee1				bit KEY_RIGHT,c
						jr z,hero_knee2
						get_enemy_status IS_DIRECT
						jr z,hero_throw
						
hero_knee2				set_status IS_NOINT
						set_status IS_KNEE
						inc (ix+object.hits)
						ld a,(ix+object.hits)
						cp 5
						ld a,anim_throwkick
						jp c,game_loop_initanim
						set_status IS_CRITHIT
						jr hero_capture_cancel
												
;бросаем противника
hero_throw				set_status IS_NOINT
						set_enemy_status IS_THROW	
						set_enemy_status IS_CRITDAMAGE
						set_enemy_status IS_SUPERDAMAGE		
						ld a,anim_throwdown						
hero_capture_cancel		call capture_cancel		
						ld (ix+object.hits),0
						jp game_loop_initanim

hero_enemycapture_end	ld a,anim_throw			
						jp game_loop_initanim
						
;---------------------------------- враги ----------------------------------
enemy_begin				cp id_Items
						jp nc,item_begin

						ld a,(salute_on)
						or a
						jr z,enemy_scripted
						
						call rnd
						and 7
						jr nz,enemy_scripted
						ld hl,bum_sfx
						call sfx_ay
						ld hl,0
						ld a,id_Belger
						ld b,anim_kissed
						call child_object
						ld (iy+object.energy),0
						set_enemy_status IS_NOINT
						call rnd
						and 7
						sub 4
						ld (iy+object.xaccel),a
						call rnd
						and 15
						add a,15
						neg
						ld (iy+object.yaccel),a
						
						
enemy_scripted			get_status IS_SCRIPTMODE
						jr z,enemy_begin_1
						call play_script
						jr enemy_begin_2_end
						
enemy_begin_1			ld a,(ix+object.yoffset)
						or a
						jr nz,enemy_begin_2
						ld hl,0
						call get_tileprop
						cp TP_DEEP
						jr nz,enemy_begin_2
						set_status IS_FALLDEEP				
						
enemy_begin_2			call deep_fall ;обработка падения в пропасть
						
						get_status IS_NOINT
						jp nz,enemy_IsDamage
						get_status IS_THROW
						jp nz,enemy_IsThrow

						ld iy,objects
			
;последняя фаза Белгера			
						ld a,(ix+object.type)
						ld b,a
						cp id_Belger
						jr nz,enemy_belger_end
						ld a,(ix+object.weapon)
						or a
						jr z,enemy_belger_end
						ld hl,script_belger_end
						call start_script
enemy_begin_2_end		jp game_loop_end
			
enemy_belger_end		
						get_enemy_status IS_SUPERHIT
						jr nz,enemy_captured
						ld a,b ;блокирование удара для Акселя, Эндора и Белгера
						cp id_Axl
						jr z,enemy_begin_3
						cp id_Andore
						jr z,enemy_begin_3
						cp id_Belger
						jr nz,enemy_captured
enemy_begin_3			call distance_y
						ld a,l
						cp 4
						jr nc,enemy_captured
						call distance_x
						ld a,l
						cp 24
						jr nc,enemy_captured
						ld a,(iy+object.yoffset)
						or a
						jr z,enemy_captured
						bit IS_CRITDAMAGE,(iy+object.status)
						jr nz,enemy_captured
						ld (ix+object.faze),4			
					
enemy_captured			get_status IS_CAPTURED
						jr z,enemy_direction
						ld a,anim_idle					
						jp enemy_animnum_edit
									
;разворот в сторону героя
enemy_direction			ld a,(ix+object.faze)
						cp 5
						jr z,enemy_dicide
						call distance_x
						jr c,enemy_direction1
						set_status IS_DIRECT
						jr enemy_dicide
enemy_direction1		res_status IS_DIRECT
		
;фаза 0 - решение, атаковать или идти на сближение
enemy_dicide			ld a,(ix+object.faze)
						or a
						jp nz,enemy_path
						
						get_enemy_status IS_NOINT
						jr nz,enemy_dicide1
						call distance_x ;немедленный критический удар, если герой стоит рядом
						ld a,l
						cp 24
						jr nc,enemy_dicide1
						call distance_y
						ld a,l
						cp 4
						jr nc,enemy_dicide1
						ld a,(ix+object.undeadcntr)
						jp z,enemy_dicide1					
						set_status IS_CRITHIT
						jp enemy_attack0
						
enemy_dicide1			call rnd ;случайная пауза перед принятием решения
						and 15
						jr z,enemy_dicide2
						ld a,anim_idle
						jp enemy_animnum_edit
						
enemy_dicide2			ld a,(ix+object.type) ;движение к координате для Трешера и Китаны без мечей
						cp id_Trasher
						jr z,enemy_dicide3
						cp id_Kitana
						jr nz,enemy_dicide5
						ld a,(ix+object.weapon)
						or a
						jr z,enemy_dicide5
						jr enemy_dicide4
						
enemy_dicide3			call rnd
						and 1
						jr nz,enemy_dicide5
enemy_dicide4			ld a,(ix+object.movecord)
						cpl
						ld (ix+object.movecord),a
						ld a,(iy+object.ycord)
						ld (ix+object.movecord+1),a
						ld (ix+object.faze),5
						jr enemy_dicide_end												

enemy_dicide5			ld (ix+object.faze),1 ;сближение с противником
						call rnd
						rrca
						jr c,enemy_dicide6
						set_status IS_DIRENEMY
						call distance_x
						jr nc,enemy_dicide7
						jr enemy_dicide_end		
enemy_dicide6			res_status IS_DIRENEMY
						call distance_x
						jr nc,enemy_dicide_end

enemy_dicide7			call rnd
						rlca
						jr c,enemy_dicide8
						res_status IS_UPDOWNENEMY
						jr enemy_dicide9
enemy_dicide8			set_status IS_UPDOWNENEMY
enemy_dicide9			ld (ix+object.cntr),16
						ld (ix+object.faze),3 ;уйти с линии атаки
enemy_dicide_end		jp game_loop_end
						
;фаза 1 - сближение с противником					
enemy_path				cp 1
						jp nz,enemy_attack
						
						call distance_y ;отступление
						jr nz,enemy_path2
						call distance_x
						ld a,l
						cp 48
						jr nc,enemy_path2
						
						call rnd ;случайная атака во время отступления
						and 7
						jr nz,enemy_path1
						bit IS_NOINT,(iy+object.status)
						jr nz,enemy_dicide_end
						ld (ix+object.faze),2
						jr enemy_dicide_end
						
enemy_path1				call distance_x
						sbc a,a
						or 1
						jp enemy_x_walkanim
						
enemy_path2				ld a,(ix+object.type) ;Китана, ЭльГаде, Эндор и Белгер отходит на 128 пикселей перед атакой
						cp id_Kitana
						jr z,enemy_path2_1
						cp id_ElGade
						jr z,enemy_path2_1
						cp id_Andore
						jr z,enemy_path2_1
						cp id_Belger
						jr z,enemy_path2_1
						ld a,48
						jr enemy_path3
enemy_path2_1			ld a,120						
enemy_path3				ld b,a
						call compare_x
						jr z,enemy_path3_1
						ld a,b
						inc a
						call compare_x
						jp nz,enemy_path6
						
enemy_path3_1			call distance_y
						jp nz,enemy_path7

enemy_path4				call rnd ;случайная пауза перед атакой
						and 3
						ld a,anim_idle
						jp nz,enemy_animnum_edit
						
						ld (ix+object.faze),2 ;атака
			
;начальные проверки для стреляющих врагов			
						ld a,(ix+object.xcord+1)
						or a
						jp nz,enemy_path_end
						get_enemy_status IS_CRITDAMAGE
						jp nz,enemy_path_end
						call distance_x
						ld a,l
						cp 120
						jp nz,enemy_path_end

;Белгер запускает свой кулак
						ld a,(ix+object.type) 
						cp id_Belger
						jr nz,enemy_path4_2
						call rocket_strike
						jr enemy_path_end
			
;ЭльГаде бросает нож						
enemy_path4_2			cp id_ElGade
						jr nz,enemy_path_end

;проверка, нет-ли другого врага на пути
enemy_other_find		push iy

						ld iy,objects
						ld b,object.maxnum
						
enemy_other_find_loop	ld a,(iy+object.type)
						cp id_Bred
						jr c,enemy_other_end
						push ix,iy
						pop hl,de
						or a
						sbc hl,de
						jr z,enemy_other_end
						call distance_y
						ld a,l
						cp 4
						jr nc,enemy_other_end
						ld a,(ix+object.status)
						xor (iy+object.status)
						and 1
						jr z,enemy_other_find_exit
						
enemy_other_end			ld de,object.lenght
						add iy,de
						djnz enemy_other_find_loop
						inc b
							
enemy_other_find_exit 	pop iy
						jr z,enemy_path_end
						
						ld h,8
						ld b,Items_Knife
						ld a,id_Items
						call child_object
						ld (iy+object.hitpower),12
						ld (iy+object.damagearea),10
						ld (iy+object.damagearea+1),4
						ld (iy+object.itemspeed),4

						set_status IS_NOINT
						ld a,anim_punchleft
						jp enemy_animnum_edit
						
enemy_path_end			jp game_loop_end
						
enemy_path6				push hl
						ld a,(ix+object.type)
						cp id_Kitana
						jr z,enemy_path6_2
						cp id_Belger
						jr z,enemy_path6_2
						cp id_Trasher
						jr nc,enemy_path6_1
						and 1
						jr z,enemy_path6_2
enemy_path6_1			call distance_y
						jr z,enemy_path6_2
						ccf
						sbc a,a
						or 1
						ld l,a
						ld h,0
						exa
						call get_tileprop
						jr nz,enemy_path6_2
						exa
						add a,(ix+object.ycord)
						ld (ix+object.ycord),a
enemy_path6_2			pop hl
						
						rlc h
						sbc a,a
						or 1
						exa
						call distance_y
						jp z,enemy_path4
						exa
						jp enemy_x_walkanim

enemy_path7				ccf
						sbc a,a
						or 1
						jp enemy_y_walkanim
			
;фаза 2 - атака противника			
enemy_attack			cp 2
						jp nz,enemy_strafe

						ld a,(ix+object.type)
						cp id_Kitana
						jr nz,enemy_attack0_1
						
						ld a,63
						call compare_x
						jr z,enemy_attack0_0
						
						ld a,64
						call compare_x
						jr nz,enemy_attack0_1
						
enemy_attack0_0			ld (ix+object.faze),4 ;скользящая атака Китаны
						jr enemy_path_end
						
enemy_attack0_1			ld a,24
						call compare_x
						jr z,enemy_attack0_2
						
						ld a,23
						call compare_x
						jp nz,enemy_attack3
							
enemy_attack0_2			call distance_y
						jp nz,enemy_attack4
						
enemy_attack0			ld (ix+object.faze),0

						get_enemy_status IS_ENEMYCAPTURE ;сброс фазы, если герой кого-то держит
						jp nz,game_loop_end
						
						bit IS_CRITDAMAGE,(iy+object.status)
						jp nz,game_loop_end
						
						set_status IS_NOINT ;удар
						
						ld a,(ix+object.type) ;критический удар
						ld b,a
						
						cp id_Abigal ;поцелуй Абигейла
						jr nz,enemy_attack1_2
						ld a,(ix+object.undeadcntr)
						or a
						jr z,enemy_attack1_1
						ld a,anim_weapow_hit
						jp enemy_animnum_edit
						
enemy_attack1_1			call rnd
						and 1
						jr z,enemy_attack1_3

						ld a,(iy+object.yoffset)
						or a
						jr nz,enemy_attack1_3
						ld a,(ix+object.xcord+1)
						or a
						jr nz,enemy_attack1_3
						
						push bc,ix
						ld l,(ix+object.xcord)
						ld h,a
						ld e,(ix+object.ycord)
						ld d,(ix+object.ycord+1)
						ld a,(ix+object.status)
						cpl
						and 1
						ld b,a
						ld ix,objects
						ld a,(ix+object.status)
						and #fe
						or b
						ld (ix+object.status),a
						set_status IS_NOINT
						set_status IS_CRITDAMAGE
						ld (ix+object.xcord),l
						ld (ix+object.xcord+1),h
						ld (ix+object.ycord),e
						ld (ix+object.ycord+1),d
						xor a
						ld (ix+object.yoffset),a
						ld (ix+object.yaccel),a
						ld a,-16
						call add_x_dir
						ld a,anim_kissed
						call start_animation									
						pop ix,bc
						
enemy_attack1_2			ld a,b
						cp id_ElGade
						jr z,enemy_attack1	
						cp id_Axl
						jr z,enemy_attack1
						cp id_Andore						
						jr z,enemy_attack1
						cp id_Belger
						jr z,enemy_attack1
						cp id_Poison
						jr nz,enemy_attack2
						call rnd
						and 3
						jr nz,enemy_attack2
enemy_attack1_3			set_status IS_CRITHIT
						ld a,anim_punchright
						jp enemy_animnum_edit
						
enemy_attack1			set_status IS_CRITHIT						
enemy_attack2			ld a,anim_punchleft
						jp enemy_animnum_edit
						
enemy_attack3			rlc h
						sbc a,a
						or 1
						jp enemy_x_walkanim

enemy_attack4			ccf
						sbc a,a
						or 1
						jp enemy_y_walkanim						
			
;фаза 3 - уйти с линии атаки			
enemy_strafe			cp 3
						jr nz,enemy_block
						ld a,(ix+object.cntr)
						or a
						jr z,enemy_strafe3
						
						get_status IS_UPDOWNENEMY
						jr nz,enemy_strafe1
						
						ld hl,#0001
						call get_tileprop
						jr nz,enemy_strafe3
						jr enemy_strafe2
						
enemy_strafe1			ld hl,#00ff
						call get_tileprop
						jr nz,enemy_strafe3			
						
enemy_strafe2			dec (ix+object.cntr)
						ld a,l
						jp enemy_y_walkanim
						
enemy_strafe3			ld (ix+object.faze),1
						jp game_loop_end			
				
;фаза 4 - блокирование удара либо атака с разгона	
enemy_block				cp 4
						jr nz,enemy_move
						set_status IS_BLOCK
						set_status IS_NOINT
						ld (ix+object.faze),0
						set_status IS_CRITHIT
						ld a,anim_punchright
						jp enemy_animnum_edit		
			
;фаза 5 - перемещение к заданной координате			
enemy_move				cp 5
						jp nz,game_loop_end
						
						ld a,(ix+object.movecord)
						cpl
						and 1
						ld b,a
						ld a,(ix+object.status)
						and #fe
						or b
						ld (ix+object.status),a
						
						ld a,2
						call add_x_dir
						
;вправо						
						get_status IS_DIRECT
						jr nz,enemy_move_1
						ld a,(ix+object.xcord+1)
						cp 1
						jr z,enemy_move_4
						cp -1
						jr z,enemy_move_2
						ld a,(ix+object.xcord)
						cp RIGHT_BORDER
						jr nc,enemy_move_4
						jr enemy_move_2
;влево						
enemy_move_1			ld a,(ix+object.xcord+1)
						cp -1
						jr z,enemy_move_4
						cp 1
						jr z,enemy_move_2
						ld a,(ix+object.xcord)
						cp LEFT_BORDER
						jr c,enemy_move_4					

enemy_move_2			ld a,(ix+object.movecord+1)
						cp (ix+object.ycord)
						jr z,enemy_move_3
						sbc a,a
						or 1
						add a,(ix+object.ycord)
						ld (ix+object.ycord),a		
						
enemy_move_3			ld a,(ix+object.type)
						cp id_Kitana
						jr z,enemy_move_6
						
						ld a,(ix+object.yoffset)
						or a
						jr nz,enemy_move_5
						ld (ix+object.yaccel),-25					
						jr enemy_move_5
						
enemy_move_4			ld (ix+object.faze),0
						ld a,(ix+object.type)
						cp id_Kitana
						jr nz,enemy_move_5
						set_status IS_NOINT
						ld a,anim_idle
						jp enemy_animnum_edit
						
enemy_move_5			set_status IS_CRITHIT
						ld a,anim_jumpkickside
						jp enemy_animnum_edit
						
enemy_move_6			ld de,#1010
						ld (ix+object.hitpower),8
						call weapon_damage
						ld a,anim_weapon_walk
						jp enemy_animnum_edit
						
;влючение анимации ходьбы				
enemy_x_walkanim		ld l,0 ;проверка на препятствие
						ld h,a
						
						ld a,(ix+object.type)
						cp id_Abigal
						jr nz,enemy_x_walkanim0
						ld a,(ix+object.undeadcntr)
						or a
						jr z,enemy_x_walkanim1
						ld a,2
						jr enemy_x_walkanim2
						
enemy_x_walkanim0		cp id_Andore ;Эндор бежит во время атаки
						jr nz,enemy_x_walkanim1
						ld a,(ix+object.faze)
						cp 2
						jr nz,enemy_x_walkanim1
						ld a,2
						jr enemy_x_walkanim2					
						
enemy_x_walkanim1		push hl
						call get_object_speed
						pop hl
enemy_x_walkanim2		rlc h
						jr nc,enemy_x_walkanim3
						neg			
enemy_x_walkanim3		ld h,a					
						call get_tileprop
						jr z,enemy_x_walkanim5
						set_status IS_UPDOWNENEMY					
						ld (ix+object.cntr),100
						ld (ix+object.faze),3
						ld a,anim_idle
						get_status IS_DIRECT
						jr z,enemy_x_walkanim4
						set_status IS_DIRENEMY
						jp enemy_animnum_edit
enemy_x_walkanim4		res_status IS_DIRENEMY		
						jp enemy_animnum_edit
						
enemy_x_walkanim5		ld a,h						
						call add_x
						ld a,anim_walk
						jp enemy_animnum_edit
						
						
enemy_y_walkanim		ld h,0 ;проверка на препятствие
						ld l,a
						call get_tileprop
						jr z,enemy_y_walkanim1
						ld (ix+object.faze),0
						ld a,anim_idle
						jp enemy_animnum_edit
						
enemy_y_walkanim1		ld a,l
						call add_y
						ld a,anim_walk
						jp enemy_animnum_edit						
						
;получил повреждение						
enemy_IsDamage			get_status IS_DAMAGE
						jp z,enemy_IsDamage7
						
						ld a,(ix+object.type) ;второй Китана выбрасывает мечи
						ld b,a
						cp id_Belger
						jr z,enemy_IsDamage1
						cp id_Kitana
						jr nz,enemy_IsDamage4
						ld a,(current_level)
						cp '4'
						jr nz,enemy_IsDamage4		
						
enemy_IsDamage1			ld a,(ix+object.energy)
						cp 50
						jr nc,enemy_IsDamage4
						ld a,(ix+object.weapon)
						or a
						jr nz,enemy_IsDamage4
						inc (ix+object.weapon)					
						ld a,b
						cp id_Belger
						jr z,enemy_IsDamage2
						
						ld h,16
						ld b,Items_Blade
						ld a,id_Items
						call child_object
						ld (iy+object.energy),0
						ld (iy+object.faze),1
						ld (iy+object.yoffset),30
						ld (iy+object.xaccel),2
						ld (iy+object.yaccel),-25
						res_enemy_status IS_DIRECT
						ld h,-16
						ld b,Items_Blade
						ld a,id_Items
						call child_object
						ld (iy+object.energy),0
						ld (iy+object.faze),1
						ld (iy+object.yoffset),30
						ld (iy+object.xaccel),2
						ld (iy+object.yaccel),-25
						set_enemy_status IS_DIRECT
						
enemy_IsDamage2			set_status IS_CRITDAMAGE
						
enemy_IsDamage4			ld (ix+object.faze),0
						ld a,anim_damage
						res_status IS_BLOCK
						res_status IS_DAMAGE
						get_status IS_CAPTURED
						jr z,enemy_IsDamage5
						ld a,anim_captured					
enemy_IsDamage5			get_status IS_SUPERDAMAGE	
						jr nz,enemy_IsDamage10	
						get_status IS_CRITDAMAGE					
						jp z,enemy_animnum_edit
						
;критическое повреждение
						ld a,(ix+object.type) ;смерть Белгера
						cp id_Belger
						jr nz,enemy_IsDamage6
						ld a,(ix+object.energy)
						or a
						jr nz,enemy_IsDamage6
						ld hl,script_belger_salut
						call start_script
						ld a,anim_weapon_throw 
						jp enemy_animnum_edit

enemy_IsDamage6			ld (ix+object.undeadcntr),200 ;4 секунды бессмертия после падения
						set_status IS_NOINT
						ld (ix+object.yaccel),-20
						jr enemy_IsDamage11

;пролёт от удара
enemy_IsDamage7			get_status IS_BLOCK
						jp nz,enemy_IsBlock
						ld a,(ix+object.yoffset)
						or a
						jr z,enemy_IsDamage_end
						get_status IS_CRITDAMAGE
						jr z,enemy_IsDamage_end
						ld a,-1
						get_status IS_SUPERDAMAGE
						jr z,enemy_IsDamage8
						dec a
						jr enemy_IsDamage9
enemy_IsDamage8			get_status IS_THROW
						jr z,enemy_IsDamage9
						ld a,-3
enemy_IsDamage9			call add_x_dir
						get_status IS_THROW
						jr nz,enemy_fly
enemy_IsDamage_end		jp game_loop_end
			
;суперудар			
enemy_IsDamage10		set_status IS_NOINT
						ld (ix+object.yaccel),-25
				
enemy_IsDamage11		push ix ;отпускаем объект, если он был захвачен
						ld ix,objects
						call capture_cancel
						pop ix		
						ld a,anim_damagefall 
						jr enemy_animnum_edit
						
;врага бросили			
enemy_IsThrow			set_status IS_NOINT
						set_status IS_CRITDAMAGE
						ld a,anim_fly 
						jr enemy_animnum_edit		


;сбивание врагов другим врагом, летящим после броска
enemy_fly				ld (ix+object.hitpower),4 ;сила удара = 4
						set_status IS_CRITHIT
						ld iy,objects
						ld b,object.maxnum
enemy_fly_loop			push bc
						ld a,(iy+object.type)
						cp id_Bred
						jr c,enemy_fly_end
						push ix,iy
						pop hl,de
						or a
						sbc hl,de
						jr z,enemy_fly_end
						bit IS_CRITDAMAGE,(iy+object.status)
						jr nz,enemy_fly_end
						call distance_x
						ld a,l
						cp 8
						jr nc,enemy_fly_end
						call distance_y
						ld a,l
						cp 6
						jr nc,enemy_fly_end
						bit IS_CRITDAMAGE,(iy+object.status) ;не наносим урон лежащему объекту
						jr nz,enemy_fly_end						
						call set_damage
enemy_fly_end 			pop bc
						ld de,object.lenght
						add iy,de
						djnz enemy_fly_loop
						jr enemy_IsDamage_end

;перемещение по X во время блока
enemy_IsBlock			ld a,(ix+object.type)
						cp id_Andore
						jr z,enemy_IsBlock1
						cp id_Kitana
						jr z,enemy_IsBlock1
						ld a,-2
						jr enemy_IsBlock2
enemy_IsBlock1			ld a,2
enemy_IsBlock2			call add_x_dir
						jp game_loop_end
						
;корректировка номера анимации для врагов
enemy_animnum_edit		ld b,a
						ld a,(ix+object.type)
						
						cp id_Abigal
						jr nz,enemy_animnum_edit_2
						ld a,(ix+object.undeadcntr)
						or a
						jr z,enemy_animnum_edit_e
						ld a,b
						cp anim_idle
						jr nz,enemy_animnum_edit_1
						ld b,anim_weapon_idle
						jr enemy_animnum_edit_e
enemy_animnum_edit_1	cp anim_walk
						jr nz,enemy_animnum_edit_e
						ld b,anim_weapon_walk
						jr enemy_animnum_edit_e
						
enemy_animnum_edit_2	cp id_Kitana
						jr nz,enemy_animnum_edit_e
						ld a,(ix+object.weapon)
						or a
						jr z,enemy_animnum_edit_e
						ld a,b
						cp anim_idle
						jr nz,enemy_animnum_edit_e
						ld b,anim_weapon_idle
enemy_animnum_edit_e	ld a,b
						jp game_loop_initanim					
						
;------------------------------ предметы и оружие ------------------------------
item_begin				ld a,(ix+object.energy)
						or a
						jr nz,item_begin_5
;уничтожение предмета						
						ld a,(ix+object.faze)
						or a
						jr nz,item_begin_4		
						inc (ix+object.faze)
						
						ld a,(objects+object.status)
						cpl
						and 1
						ld b,a
						ld a,(ix+object.status)
						and #fe
						or b
						ld (ix+object.status),a
						
						ld (ix+object.yaccel),-25
						ld (ix+object.xaccel),-2	
						ld a,(ix+object.itemkind)
;уничтожение бочки или крыски				
						cp Items_RatIdle
						jr z,item_begin_2
						cp Items_Barrel
						jr nz,item_weapon_end
						ld a,Items_BarrelBreak
						call init_animation
						
item_begin_2			ld a,(ix+object.itemprop)
						and 7
						jr z,item_weapon_end
						ld b,a ;2 - Items_Eat, 3 - Items_Rage, 4 - Items_Heart или 5 - оружие
						cp Items_KnifeWeapon
						jr c,item_begin_3
						ld a,(main_hero) ;тип героя
						dec a
						add a,b
						ld b,a
						
;генерация поверапа						
item_begin_3			push ix
						ld l,(ix+object.xcord)
						ld h,(ix+object.xcord+1)
						ld e,(ix+object.ycord)
						ld d,16
						ld a,(ix+object.xaccel)
						push af
						ld a,(ix+object.status)
						and 1
						ld c,a
						ld a,id_Items
						call add_object
						pop af
						add a,a
						ld (ix+object.xaccel),a
						pop ix
						
						jr item_weapon_end
						
item_begin_4			ld a,(ix+object.yoffset)
						or a
						jr nz,item_weapon_end
						
						ld (ix+object.type),0 ;уничтожение объекта
						jr item_weapon_end
						
item_begin_5			ld a,(ix+object.itemkind)

;метательное оружие (нож, энергетическая волна, сюрикен, молот, перчатка Белгера)
item_weapon				cp Items_Knife
						jp c,item_bird
						ld a,(ix+object.xcord+1)
						or a
						jr z,item_weapon_1
						ld (ix+object.type),0 ;уничтожение объекта при выходе за пределы экрана
						jr item_weapon_end
		
item_weapon_1			ld a,(ix+object.itemspeed)
						call add_x_dir
						ld d,(ix+object.damagearea)
						ld e,(ix+object.damagearea+1)
						call weapon_damage
						jr nc,item_weapon_end
						ld a,(ix+object.itemkind)
						cp Items_Wave
						jr z,item_weapon_end
						ld (ix+object.type),0 ;уничтожение объекта после попадания
item_weapon_end			jp game_loop_end
						
;птичка		
item_bird				cp Items_BirdFly
						jr z,item_bird_fly
						cp Items_BirdIdle
						jr nz,item_rat
						
item_bird_wait			ld iy,objects
						ld b,object.maxnum
						ld c,0
item_bird_wait_loop		ld a,(iy+object.type)
						or a
						jr z,item_bird_wait_end ;выход, если объект пустой					
						cp id_Items
						jr z,item_bird_wait_end
						call distance_x
						ld a,l
						cp 32
						jr nc,item_bird_wait_end
						call distance_y
						ld a,l
						cp 32
						jr nc,item_bird_wait_end
						ld a,Items_BirdFly
						ld (ix+object.itemkind),a
						call init_animation
						jr item_bird_end
						
item_bird_wait_end		ld de,object.lenght
						add iy,de
						djnz item_bird_wait_loop
						
item_bird_idle			get_status IS_NOINT
						jr nz,item_bird_end
						call rnd
						and 31
						jr nz,item_bird_end
						set_status IS_NOINT
						call rnd
						and 3
						add a,Items_BirdIdle
						call init_animation
						jr item_bird_end
			
item_bird_fly			ld a,3
						call add_x_dir
						ld a,-2
						call add_y
						ld a,(ix+object.xcord+1)
						or a
						jr nz,item_bird_fly_1
						ld a,(ix+object.ycord+1)
						or a
						jr z,item_bird_end
item_bird_fly_1			ld (ix+object.type),0
item_bird_end			jp game_loop_end
		
;крыска		
item_rat				cp Items_RatIdle
						jr nz,item_barrel
						bit 3,(ix+object.itemprop)
						jr nz,item_bird_end ;выход, если крыска невидимая
						
						get_status IS_NOINT
						jr nz,item_bird_end
						call rnd
						and 63
						jr nz,item_rat_run
						set_status IS_NOINT
						ld a,Items_RatIdle
						call init_animation
						jr item_bird_end
						
item_rat_run			ld a,Items_RatRun
						call init_animation
						ld a,1
						call add_x_dir
						ld a,(ix+object.xcord+1)
						or a
						jr z,item_bird_end
						get_status IS_DIRECT
						jr nz,item_rat_run_1
						cp 1
						jr z,item_rat_run_2
						jr item_bird_end
item_rat_run_1			cp -1
						jr nz,item_bird_end
item_rat_run_2			ld (ix+object.type),0
						jr item_bird_end
			
;бочка		
item_barrel				cp Items_Barrel
						jp nz,items_powerups
						
;сбивание героя бочкой
						ld iy,objects
						ld b,object.maxnum
item_barrel_loop		ld a,(iy+object.type)
						or a
						jr z,item_barrel_loop_end ;выход, если объект пустой
						push ix,iy
						pop hl,de
						or a
						sbc hl,de
						jr z,item_barrel_loop_end
						ld a,(iy+object.yoffset)
						cp 24
						jr nc,item_barrel_loop_end ;не повреждаются объекты, у которых yoffset >= 24
						call distance_x
						ld a,l
						cp 15
						jr nc,item_barrel_loop_end
						call distance_y
						jr c,item_barrel_loop_end
						ld a,l
						cp 16
						jr nc,item_barrel_loop_end
						bit IS_CRITDAMAGE,(iy+object.status)
						jr nz,item_barrel_loop_end
						ld a,(iy+object.undeadcntr)
						or a
						jr nz,item_barrel_loop_end

						set IS_DAMAGE,(iy+object.status)
						set IS_CRITDAMAGE,(iy+object.status)

item_barrel_loop_end	ld de,object.lenght
						add iy,de
						djnz item_barrel_loop
						
						ld a,(ix+object.itemprop) ;подпрыгивающая бочка						
						ld b,a 
						and 16
						jr z,item_barrel_speed					
						ld a,(ix+object.yoffset) 
						or a
						jr nz,item_barrel_speed
						ld (ix+object.yaccel),-15				
						
item_barrel_speed		ld a,b ;скорость бочки
						and 8
						ld a,2
						jr nz,item_barrel_speed_1
						ld a,(gameloop_cnt)
						and 1
						inc a					
						
item_barrel_speed_1		call add_x_dir
						ld a,(ix+object.xcord+1)
						or a
						jr z,item_barrel_end
						get_status IS_DIRECT
						jr nz,item_barrel_1
						cp 1
						jr nz,item_barrel_end
						ld a,(ix+object.xcord)
						cp 30
						jr nc,item_barrel_end
						jr item_barrel_kill
						
item_barrel_1			cp -1
						jr nz,item_barrel_end
						ld a,(ix+object.xcord)
						cp -30
						jr c,item_barrel_end

item_barrel_kill		ld (ix+object.type),0
						jr game_loop_end

item_barrel_end			ld a,Items_Barrel
						call init_animation
						jr game_loop_end
			
;поверапы
items_powerups			exa
						ld a,(ix+object.yoffset)
						or a
						jr nz,game_loop_end
						ld iy,objects ;объект героя
						call distance_x
						ld a,h
						or a
						jr nz,game_loop_end
						ld a,l
						cp 16
						jr nc,game_loop_end
						call distance_y
						ld a,h
						or a
						jr nz,game_loop_end
						ld a,l
						cp 8
						jr nc,game_loop_end
						exa
;powerup еда						
						cp Items_Eat
						jr nz,items_powerups_2
						ld a,(iy+object.energy)
						add a,32
						cp (iy+object.maxenergy)
						jr c,items_powerups_1
						ld a,(iy+object.maxenergy)
items_powerups_1		ld (iy+object.energy),a
						jr items_powerups_end
;powerup ярость
items_powerups_2		cp Items_Rage
						jr nz,items_powerups_3
						ld (iy+object.rage),MAX_RAGE
						jr items_powerups_end

;powerup сердце
items_powerups_3		cp Items_Heart
						jr nz,items_powerups_4
						ld a,(iy+object.maxenergy)
						ld (iy+object.energy),a
						jr items_powerups_end
;powerup оружие
items_powerups_4		ld a,(main_hero)
						cp id_Guy
						ld a,3
						jr nz,items_powerups_5
						ld a,16
items_powerups_5		ld (iy+object.weapon),a
						
items_powerups_end		ld hl,powerup_sfx
						call sfx_ay
						ld (ix+object.type),0			
						jr game_loop_end
						
		
			
;инициализация анимации (номер анимации A)
game_loop_initanim		call start_animation			
			
;проигрывание анимации персонажа				
game_loop_end			call play_animation
						xor a
						ld (clicked_keys),a
						
;отрисовка экрана
game_loop_next			pop bc
						ld de,object.lenght
						add ix,de
						dec b
						jp nz,game_loop_objects

						ld hl,frame_counter
						ld a,(hl)					
						cp 4 ;кол-во кадров, с которого разрешается пропуск дельтатайминга
						jr nc,game_loop_next_1
						dec (hl)
						jp nz,game_loop_begin
						ld hl,skip_deltatime
						ld (hl),0
						jr game_loop_next_2
						
game_loop_next_1		dec (hl)
						ld hl,skip_deltatime
						ld a,(hl)
						ld (hl),0
						jr z,game_loop_next_2
						or a	
						jp z,game_loop_begin	
						
game_loop_next_2		xor a
						ld (pressed_keys),a
						
						call anim_tiles; анимирование тайлов
						call tiles_out ;перерисовка тайлов
						call objects_out ;вывод объектов
						call drw_statusbar ;вывод статус-баров
						
						ei
						halt					
						call view_screen ;отобразить виртуальный экран на реальный	
						
;игровая пауза			
pause_game				ld a,(clicked_keys)
						bit KEY_PAUSE,a
						jr z,kill_all_enemies
						
						ld a,(interupt_counter)
						and 16
						call drw_go_img_disable						
						
						ld de,screen+#800+10
						ld a,#04
						ld b,4
pause_game_loop1		push af,bc,de						
						ld b,12
pause_game_loop2		call drw_symbol						
						inc e,a
						djnz pause_game_loop2
						pop de,bc
						ld a,e
						add a,32
						ld e,a
						pop af
						add a,16
						cp #24
						jr nz,$+4
						ld a,#84
						djnz pause_game_loop1
						
						ld hl,screen+#1800+256+10
						ld bc,12
						ld a,%1000111
						call fill_attr
						ld hl,screen+#1800+256+64+10
						ld a,%1000111
						call fill_attr
										
						call ext_mute_music
						xor a
						ld (clicked_keys),a
						
pause_game_loop3		ld b,60
pause_game_loop4		ei
						halt
						ld a,(clicked_keys)
						bit KEY_PAUSE,a
						jr nz,pause_game_end
						djnz pause_game_loop4
						
						call sfx_fall
						
						ld b,15
pause_game_loop5		ei
						halt
						ld a,(clicked_keys)
						bit KEY_PAUSE,a
						jr nz,pause_game_end
						djnz pause_game_loop5
						
						call sfx_fall
						jr pause_game_loop3
						
pause_game_end			ld hl,(current_music)
						call ext_init_music
						ld a,1
						ld (refresh_gamescreen),a
						ld (frame_counter),a
						jr next_game_screen
						
;убийство всех врагов для тестирования					
kill_all_enemies		
						IF debug_mode = 1
						ld a,(pressed_keys)
						bit KEY_QUIT,a
						jr z,next_game_screen
						call delete_objects
						ld ix,objects
						;ld (ix+object.rage),80
						;ld (ix+object.weapon),16
						ENDIF
						
;проверка на возможность перехода на следующий экран
next_game_screen		ld a,(game_screen_clear)		
						or a
						jr z,next_game_screen_end
						
						ld hl,(location.currentadr) ;проверка конца локации
						ld de,16
						add hl,de
						ex de,hl
						ld hl,(location.startadr)
						ld a,(location.width)
						ld c,a
						ld b,0
						add hl,bc
						ex de,hl					
						or a
						sbc hl,de
						jr z,next_location
		
						ld ix,objects
						get_status IS_SCRIPTMODE
						call z,drw_go_img
						ld a,(go_visible)
						or a
						jr nz,next_game_screen_end
						ld a,(ix+object.xcord+1)
						or a
						jr nz,next_game_screen_end
						ld a,(ix+object.xcord)
						cp 224
						jr c,next_game_screen_end
						sub ONE_HSCROLL*16
						ld (ix+object.xcord),a
						
						call scroll_left
						call delete_objects
						xor a
						ld (game_screen_clear),a
						ld a,1
						ld (frame_counter),a			
next_game_screen_end		
						jp game_loop_begin

;переход на следующую локацию
next_location			ld ix,objects
						get_status IS_SCRIPTMODE
						jp nz,game_loop_begin
						
						ld a,(current_loc)
						cp 2
						jp z,level_clear
						
						ld a,(current_level)
						cp '1'
						jr nz,script_loc_2_1
						
;скрипт конца локации 1-1	
script_loc_1_1			ld a,(current_loc)
						or a
						jr nz,script_loc_1_2

						ld hl,script_hero_end_loc1_1 ;запуск скрипта выхода из локации
						jr script_start_script
						
;скрипт конца локации 1-2
script_loc_1_2			ld a,(ix+object.ycord)
						cp 120
						jr z,script_loc_1_2_1
						
						ld hl,script_hero_end_loc1_21 ;запуск первой части скрипта выхода из локации
						jr script_start_script
						
script_loc_1_2_1		ld hl,script_hero_end_loc1_22 ;запуск первой части скрипта выхода из локации
						call start_script
						
						ld hl,280
						ld de,120
						ld bc,0
						ld a,id_Trasher
						call add_object
						ld hl,script_Trasher
						jr script_start_script						
						
;скрипт конца локации 2-1 и 2-2
script_loc_2_1			cp '2'
						jr nz,script_loc_3_1

script_loc_2_1_1		ld hl,script_hero_end_loc2_1 ;запуск скрипта выхода из локации
						jr script_start_script

;скрипт конца локации 3-1
script_loc_3_1			cp '3'
						jr nz,script_loc_4_1
						
						ld a,(current_loc)
						cp 1
						jr nz,script_loc_2_1_1
						ld hl,script_hero_end_loc3_2
						jr script_start_script
						
;скрипт конца локации 4-1
script_loc_4_1			cp '4'
						jr nz,script_loc_5_1
						ld a,(current_loc)
						or a
						jr nz,script_loc_4_2
						ld hl,script_hero_end_loc4_1
						jr script_start_script
						
;скрипт конца локации 4-2
script_loc_4_2			cp 1
						jp nz,game_loop_begin
						ld hl,script_hero_end_loc4_2
						jr script_start_script
						
;скрипт конца локации 5-1
script_loc_5_1			ld a,(current_loc)
						or a
						jr nz,script_loc_5_2
						ld hl,script_hero_end_loc5_1
						jr script_start_script
script_loc_5_2			ld hl,script_hero_end_loc5_2
							
script_start_script		call start_script
						jp game_loop_begin
						
;уровень пройден						
level_clear				ld hl,current_level
						ld a,(hl)
						inc (hl)
						ld hl,music_win.adr
						ld b,1 ;продолжительность первой паузы для трека round_clear
						cp "5"
						push af
						jr nz,level_clear_1
						ld hl,music_final_win.adr
						ld b,150 ;продолжительность первой паузы для трека final_round_clear
level_clear_1			push bc
						call ext_init_music
						pop bc

						call pause
						ld b,250
						call pause
						
						xor a
						ld (current_loc),a
						
						IF version_type = tap
						call start_tape
						ENDIF
						
						IF version_type = trdos
						call clear_screen
						ENDIF
						
						pop af
						jp nz,game_start
;игра пройдена				
game_win				di
						im 1
						ei
						ld sp,#9fff
						ld iy,(iy_save)
						ld hl,ending
						push hl
						jp load_file
						
;------------------------------------- звуковые эффекты ----------------------------------------

;звук "Go!"							
sfx_clear				ld a,(sfx_on)
						or a
						ret z
						ld b,15
sfx_clear_loop1			ld a,#10
						out (254),a
						ld c,b
						sla c,c,c,c
						inc c
sfx_clear_loop2			dec c
						jr nz,sfx_clear_loop2
						xor a
						out (254),a
						ld c,b
						sla c,c,c,c
						inc c
sfx_clear_loop3			dec c
						jr nz,sfx_clear_loop3
						djnz sfx_clear_loop1
						ret

;звук удара	
sfx_hit					ld a,(sfx_on)
						or a
						ret z
						ld hl,#1000
						ld b,64
sfx_hit_loop			ld a,b
						dup 4
						rlca
						edup
						xor (hl)
						and #10
						out (254),a
						inc hl
						ld a,(hl)
						and #7f
sfx_hit_1				dec a
						jr nz,sfx_hit_1
						djnz sfx_hit_loop
						xor a
						out (254),a
						ret

;звук когда удар не попал в цель						
sfx_miss				ld a,(sfx_on)
						or a
						ret z
						ld hl,0
						ld bc,1000
sfx_miss_loop			ld a,(hl)
						and #10
						out (254),a
						inc hl
						dec bc
						xor a
						out (254),a
						ld a,b
						or c
						jr nz,sfx_miss_loop
						xor a
						out (254),a
						ret

;звук падения на землю						
sfx_fall				ld a,(sfx_on)
						or a
						ret z
						ld bc,1000
						ld a,#10
						out (254),a
sfx_fall_loop			ld a,b
						dup 4
						rlca
						edup
						and #10
						out (254),a
						dec bc
						ld a,b
						or c
						jr nz,sfx_fall_loop
						xor a
						out (254),a
						ret		
			
;звук голоса в диалогах, в bc - параметры голоса			
sfx_voice				ld a,(sfx_on)
						or a
						ret z
						push de
						ld d,c
sfx_voice_loop			ld a,#10
						out (254),a					
sfx_voice_loop_1		dec c
						jr nz,sfx_voice_loop_1				
						ld c,d
						xor a
						out (254),a
sfx_voice_loop_2		nop
						nop
						dec c
						jr nz,sfx_voice_loop_2							
						ld c,d
						djnz sfx_voice_loop
						pop de
						ret
					
						include "scripts.asm"
						include "animations.asm"
						include "events.asm"
						savebin "../bin/code.bin", code.adr, $ - code.adr

			
