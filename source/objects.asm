;процедуры для работы с объектами

;добавление нового объекта
;вх  - a - тип объекта
;	   hl - координата X
;      e - координата Y
;      d - смещение по Y
;      b - начальная анимация
;      с - статус
;вых - ix - новый объект
;      флаг NC - объект не создан
add_object				push af,bc,de,hl
						ld ix,objects
						ld de,object.lenght
						ld b,object.maxnum
add_object1				ld a,(ix+object.type)
						or a
						jr z,add_object2
						add ix,de
						djnz add_object1
						pop hl,de,bc,af
						or a
						ret ;выход, если все объекты заняты
						
;очистка слота объекта
add_object2				push ix
						pop hl
						ld de,hl
						inc de
						ld bc,object.lenght-1
						ld (hl),0
						ldir
						
						pop hl,de,bc,af
						
						push af,hl
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
						pop hl,af
			
						ld (ix+object.type),a
						ld (ix+object.status),c
						ld (ix+object.xcord),l
						ld (ix+object.xcord+1),h
						ld (ix+object.ycord),e
						ld (ix+object.ycord+1),0
						ld (ix+object.yoffset),d
						ld (ix+object.animation),#ff
			
						cp id_Trasher
						jr c,add_object7
						cp id_Items
						jr nc,add_object7
						ld a,b
						cp anim_kissed
						jr z,add_object9 ;исключение для фиктивного объекта салюта
						
						ld a,(current_loc)
						or a
						jr z,add_object3
						cp 1
						jr z,add_object7
						
						ld a,(ix+object.type)
						ld (drw_statusbar_type+1),a
						ld a,(ix+object.energy)
						ld (drw_statusbar_energy+1),a
						xor a
						ld (timer_statusbar),a
						ld (drw_statusbar_icon+1),a
						ld (drw_statusbar_enemy+1),a

add_object3				push af,bc,de,hl,ix
						setpage ext.page
						ld hl,script_boss_start
						call start_script
						ld ix,objects
						ld hl,script_hero_end_loc1_3
						call start_script					
						pop ix,hl,de,bc,af
						jr add_object9
			
add_object7				get_status IS_SCRIPTMODE
						jr z,add_object8
						push af,bc,de,hl
						ld hl,script_enemy_wait
						call start_script
						pop hl,de,bc,af
						
;предметы и оружие						
add_object8				cp id_Items
						ld a,b
						jr nz,add_object9
						ld (ix+object.itemkind),a
						ld (ix+object.itemprop),d
						call init_animation_hard
						jr add_object_end
						
add_object9				call start_animation
						ld (ix+object.rage),24;80
add_object_end			scf
						ret
						
;спавн врагов на игровой экран
;формат описателей врагов:
;байт 1 - тип врага, если = 0 - враги не спавнятся пока не будут убиты те, что уже на экране
;                    если = 255 - на этом экране закончились враги
;байт 2 - координата X на экране (если = 0, враг выйдет слева, если = 15 - справа)
;байт 3 - координата Y на экране (если = 0, координата выбирается случайно)
;байт 4 - смещение по Y
;байт 5 - номер анимации
;байт 6 -статус
spawn_enemy				setpage location.page
						ld a,(game_screen_clear)
						or a
						jp nz,spawn_enemy_end
						
						ld hl,(current_enemy_adr)
						ld a,(hl)
						
;ожидание, пока все враги будут убиты							
						or a
						jr nz,spawn_enemy_1			
						call enemy_nums
						or a
						jp nz,spawn_enemy_end
						inc hl
						ld (current_enemy_adr),hl
						jr spawn_enemy_end
						
;закончились все враги для этого экрана
spawn_enemy_1			cp 255
						jr nz,spawn_enemy_2
						call enemy_nums
						or a
						jr nz,spawn_enemy_end
						inc hl
						ld a,(hl)
						ld (max_enemy_onscreen),a
						inc hl
						ld (current_enemy_adr),hl
						ld a,255
						ld (game_screen_clear),a
						jr spawn_enemy_end
						
;создание объекта врага					
spawn_enemy_2			exa
						call enemy_nums
						ld b,a
						ld a,(max_enemy_onscreen)
						dec a
						cp b
						jr c,spawn_enemy_end
						
						inc hl
						ld a,(hl) ;координата X
						inc hl
						ld e,(hl) ;координата Y
						inc hl
						ld d,(hl) ;смещение по Y
						inc hl
						ld b,(hl) ;номер анимации
						inc hl
						ld c,(hl) ;статус
						inc hl
						ld (current_enemy_adr),hl				
						
;координата X						
						or a
						jr nz,spawn_enemy_3
						ld hl,-30
						jr spawn_enemy_5
spawn_enemy_3			cp 255
						jr nz,spawn_enemy_4
						ld hl,270
						jr spawn_enemy_5
spawn_enemy_4			ld l,a
						ld h,0
						
;координата Y
spawn_enemy_5			ld a,e
						or a
						jr nz,spawn_enemy_7
						
spawn_enemy_6			call rnd ;координата Y
						ld e,a
						
spawn_enemy_7			setpage ext.page
						exa
						push af,bc,de,hl
						call add_object
						pop hl,de,bc,af
						cp id_Items
						jr z,spawn_enemy_end
						exa
						setpage location.page
						
						push hl
						ld hl,0
						call get_tileprop
						pop hl
						jr z,spawn_enemy_end
						ld (ix+object.type),0 ;новая попытка создать объект
						jr spawn_enemy_6

spawn_enemy_end			setpage ext.page
						ret	

;расчёт кол-ва врагов
;вых - a - кол-во врагов
enemy_nums				ld iy,objects
						ld de,object.lenght
						ld b,object.maxnum
						ld c,0
enemy_nums_loop			ld a,(iy+object.type)
						cp id_Bred
						jr c,enemy_nums_end
						cp id_Items ;Items не считаются, за исключением Barrels
						jr nz,enemy_nums_loop_1
						ld a,(iy+object.itemkind)
						cp Items_Barrel
						jr nz,enemy_nums_end	
enemy_nums_loop_1		inc c			
enemy_nums_end			add iy,de
						djnz enemy_nums_loop
						ld a,c
						ret
		
;причинение урона объектом IX объекту IY
;вх  - ix - объект1
;      iy - объект2	
;вых - флаг Z - удар успешно нанесён	
set_damage				ld c,(iy+object.type)
						ld b,(ix+object.hitpower)					
						ld a,(iy+object.energy)
						or a
						ret z
						sub b
						jr z,set_damage1
						jr nc,set_damage2	
						
;у объекта2 закончилась энергия
set_damage1				set_enemy_status IS_CRITDAMAGE
						res_enemy_status IS_SCRIPTMODE
						ld a,(ix+object.rage) ;добавляем объекту силу Rage
						add a,4
						cp MAX_RAGE+1 ;максимальная величина Rage
						jr c,$+4
						ld a,MAX_RAGE
						ld (ix+object.rage),a
						xor a
						ld c,a					
set_damage2				ld (iy+object.energy),a
						ld b,a
						
;урон наносится герою						
						ld a,(iy+object.type) ;запись данных для статус-бара							
						cp id_Bred
						jr nc,set_damage6
						ld a,(ix+object.status)
						xor (iy+object.status)
						bit IS_DIRECT,a
						jr nz,set_damage3
			
;героя ударили сзади
						set_enemy_status IS_CRITDAMAGE
						ld a,(iy+object.status)
						xor 1
						ld (iy+object.status),a
						
;героя ударили когда он был в прыжке						
set_damage3				ld a,(iy+object.yoffset)
						or a
						jr z,set_damage4
						set_enemy_status IS_CRITDAMAGE
;выносливость героя
set_damage4				ld a,(iy+object.stamina)
						add a,70
						ld (iy+object.stamina),a
						cp 150
						jr c,set_damage5
						set_enemy_status IS_CRITDAMAGE
						ld (iy+object.stamina),0		
set_damage5				ld c,(ix+object.type)
						ld b,(ix+object.energy)
					
set_damage6				ld a,c
						cp id_Items
						jr nz,set_damage7
						xor a
						ld b,a
set_damage7				ld (drw_statusbar_type+1),a
						ld a,b
						ld (drw_statusbar_energy+1),a
						
						ld a,(current_loc)
						or a
						jr z,set_damage8
						ld a,c
						cp id_Trasher
						jr nc,set_damage9
						
set_damage8				ld a,250
						ld (timer_statusbar),a

						
set_damage9				set_enemy_status IS_DAMAGE
						set_enemy_status IS_NOINT
						get_status IS_CRITHIT
						jr z,set_damage10
						set_enemy_status IS_CRITDAMAGE
set_damage10			get_status IS_SUPERHIT
						ret z
						set_enemy_status IS_SUPERDAMAGE
						ret										

;удалить все объекты, кроме первого						
delete_objects			ld hl,objects+object.lenght
						ld de,hl
						inc de
						ld bc,object.lenght*(object.maxnum-1)-1
						ld (hl),0
						ldir
						ret

;узнать скорость перемещения объекта
;вх  - ix - объект
;вых - a - скорость перемещения по X
get_object_speed		ld a,(ix+object.type) 
						dec a
						ld l,a
						add a,a
						add a,a
						add a,a
						sub l
						add a,low (personages+6)
						ld l,a
						ld a,high (personages+6)
						adc a,0
						ld h,a
						ld a,(hl)
;скорость = 1						
						cp 1
						ret z
;скорость = 2
						cp 2
						jr nz,get_object_speed_1
						ld a,(gameloop_cnt)
						and 1
						inc a
						ret
;скорость = 3
get_object_speed_1		dec a
						ret

;удаление всех объектов							
clear_objects			ld hl,objects
						ld de,objects+1
						ld bc,object.lenght*object.maxnum-1
						ld (hl),0
						ldir
						ret						
						
;инициализация главного героя
init_hero				call clear_objects
						ld a,(main_hero)
						ld b,anim_idle
						ld c,0
						ld hl,-100
						ld de,140
						call add_object
						ret
						
;герой на стартовую позицию
hero_go_start			ld ix,objects
						ld b,140
						ld a,(current_level)
						cp '3'
						jr nz,hero_go_start_1
						ld a,(current_loc)
						cp 2
						jr nz,hero_go_start_1
						
						xor a
						ld (ix+object.status),a
						ld (ix+object.status+1),a
						ld (ix+object.xcord+1),a
						ld (ix+object.xcord),64
						ld (ix+object.ycord+1),a
						ld (ix+object.ycord),140
						ld (ix+object.yoffset),160
						ld a,anim_idle
						jp start_animation
						
hero_go_start_1			cp '4'
						jr nz,hero_go_start_2
						ld a,(current_loc)
						cp 1
						jr nz,hero_go_start_2

						xor a
						ld (ix+object.status),a
						ld (ix+object.status+1),a
						ld (ix+object.xaccel),a
						ld (ix+object.xcord+1),a
						ld (ix+object.xcord),64
						ld (ix+object.ycord+1),a
						ld (ix+object.ycord),120
						ld (ix+object.yoffset),160
						ld a,anim_idle
						jp start_animation


hero_go_start_2			cp '1'
						jr nz,hero_go_start_e
						ld a,(current_loc)

						cp 2 ;падение в начале локации 1-3
						jr nz,hero_go_start_9
						xor a
						ld (ix+object.status),a
						ld (ix+object.status+1),a
						ld (ix+object.xcord+1),a
						ld (ix+object.xcord),64
						ld (ix+object.ycord+1),a
						ld (ix+object.ycord),140
						ld (ix+object.yoffset),190
						set_status IS_NOINT
						set_status IS_SUPERDAMAGE
						ld hl,script_hero_fall
						call start_script
						ld a,anim_damagefall
						jp start_animation
						
						
hero_go_start_9			cp 1
						jr nz,hero_go_start_e
						ld b,120											
hero_go_start_e			xor a
						ld (ix+object.status),a
						ld (ix+object.status+1),a
						ld (ix+object.xcord+1),-1
						ld (ix+object.xcord),-100
						ld (ix+object.ycord+1),a
						ld (ix+object.ycord),b
						
						ld hl,script_hero_start ;запуск скрипта выхода главного героя
						jp start_script
						
;проверить хватает ли Rage для удара, и отнять его
;вх  - ix - объект
;      c - стоимость удара
;      b - сколько энергии отнимает
;вых - флаг c - не хватает Rage
dec_rage				ld a,(ix+object.rage)
						cp c
						ret c
						sub b
						ld (ix+object.rage),a
						ret

;создать объект на основе родительского объекта
;вх  - ix - родительский объект, от него берутся координаты и направление
;	   h - смещение координаты X относительно родительского объекта, учитывается направление
;      b - начальная анимация или itemkind
;      a - тип нового объекта
;вых - iy - новый объект
;      флаг NC - объект не создан
child_object			push bc,de,hl,ix

						push hl
						ld l,(ix+object.xcord)
						ld h,(ix+object.xcord+1)
						ld e,(ix+object.ycord)
						ld d,0
						push af
						ld a,(ix+object.status)
						and 1 ;IS_DIRECT
						ld c,a
						pop af
						call add_object		
						pop hl
						jr nc,child_object_end
						
						ld a,h
						call add_x_dir
						
						push ix
						pop iy

child_object_end		pop ix,hl,de,bc
						ret

;поиск объектов целей для метательного оружия
;вх -  ix - объект оружия
;      de - размеры XY коллайдера
;вых - флаг NC - целей не найдено
weapon_damage			ld iy,objects					
						ld b,object.maxnum
						xor a
						exa
						
weapon_damage_loop		ld a,(iy+object.type)
						or a
						jr z,weapon_damage_end ;выход, если объект пустой
						
						push de
						push ix,iy
						pop hl,de
						or a
						sbc hl,de
						pop de
						jr z,weapon_damage_end ;объект не повреждает сам себя
						
						push de
						call distance_x
						pop de
						ld a,h
						or a
						jr nz,weapon_damage_end
						ld a,l
						cp d
						jr nc,weapon_damage_end ;выход, если не попали в коллайдер по X
						
						push de
						call distance_y
						pop de
						ld a,h
						or a
						jr nz,weapon_damage_end
						ld a,l
						cp e
						jr nc,weapon_damage_end ;выход, если не попали в коллайдер по Y

						get_enemy_status IS_CRITDAMAGE
						jr nz,weapon_damage_end
						;ld a,(iy+object.undeadcntr)
						;or a
						;jr nz,weapon_damage_end ;выход, если у вражеского объекта IS_CRITDAMAGE или он бессмертный					
						
						ld a,(iy+object.yoffset)
						cp 15
						jr nc,weapon_damage_end ;не повреждаются объекты, у которых yoffset >= 15
						
;повреждение						
						set_status IS_CRITHIT
						set_status IS_SUPERHIT
						call set_damage
						call sfx_hit
						exa
						or 1
						exa
						
weapon_damage_end		push de
						ld de,object.lenght
						add iy,de
						pop de
						djnz weapon_damage_loop
						
						exa
						rrca
						ret

;расстояние X между объектами IX и IY
;вх  - ix - первый объект
;      iy - второй объект
;вых - hl - abs(xcord1 - xcord2)
;      de - координаты xcord2
;      флаг c - отрицательное значение
;      флаг z - координаты равны
distance_x				ld l,(ix+object.xcord)
						ld h,(ix+object.xcord+1)
						ld e,(iy+object.xcord)
						ld d,(iy+object.xcord+1)
						or a
						sbc hl,de
						ret z
						bit 7,h
						jr nz,neg_hl
						xor a
						inc a
						ret
						
;расстояние Y между объектами IX и IY
;вх  - ix - первый объект
;      iy - второй объект
;вых - hl - abs(ycord1 - ycord2)
;      de - координаты ycord2
;      флаг c - отрицательное значение
;      флаг z - координаты равны
distance_y				ld l,(ix+object.ycord)
						ld h,(ix+object.ycord+1)
						ld e,(iy+object.ycord)
						ld d,(iy+object.ycord+1)
						or a
						sbc hl,de
						ret z
						bit 7,h
						jr nz,neg_hl
						xor a
						inc a
						ret
						
;сравнить IX.xcord и IY.xcord+a
;вх  - ix - объект 1
;вх  - iy - объект 2
;      a - значение (-127;127)
;вых - hl - IY.xcord+a - IX.xcord
;      флаг z - координаты равны
compare_x				ld e,(iy+object.xcord)
						ld d,(iy+object.xcord+1)
						get_status IS_DIRENEMY
						jr z,compare_x1
						neg
compare_x1				ld l,a					
						rlca
						sbc a,a
						ld h,a
						add hl,de
						ld e,(ix+object.xcord)
						ld d,(ix+object.xcord+1)
						or a
						sbc hl,de
						ret
						
;прибавить к координате X объекта IX значение A с учётом направления объекта					
;вх  - ix - объект
;      a - значение
;вых - de - предыдущий xcord
add_x_dir				get_status IS_DIRECT
						jr z,add_x
						neg
						
;прибавить к координате X объекта IX значение A (-127;127)
;вх  - ix - объект
;      a - значение
;вых - de - предыдущий xcord
add_x					ld l,a
						rlca
						sbc a,a
						ld h,a
						ld e,(ix+object.xcord)
						ld d,(ix+object.xcord+1)
						add hl,de
						ld (ix+object.xcord),l
						ld (ix+object.xcord+1),h
						ret					

;прибавить к координате Y объекта IX значение A (-127;127)	
;вх  - ix - объект
;      a - значение
;вых - de - предыдущий ycord
add_y					ld l,a
						rlca
						sbc a,a
						ld h,a
						ld e,(ix+object.ycord)
						ld d,(ix+object.ycord+1)
						add hl,de
						ld (ix+object.ycord),l
						ld (ix+object.ycord+1),h
						ret

;изменить знак значения hl
;вх  - hl - значение
;вых - hl - инвентированное значение
;      флаг c = 1
;      флаг z = 0
neg_hl					ld a,l
						cpl
						ld l,a
						ld a,h
						cpl
						ld h,a
						inc hl
						xor a
						inc a
						scf
						ret
						
;запись в IY из object.enemyobject
;вх  - ix - объект1
;вых - iy - объект2
get_iy					ld l,(ix+object.enemyobject)
						ld h,(ix+object.enemyobject+1)
						push hl
						pop iy
						ret
	
;разворот объекта, значение клавиш в регистре C
turn_c					bit KEY_RIGHT,c
						jr z,turn_c1
						res_status IS_DIRECT
						ret
turn_c1					bit KEY_LEFT,c
						ret z
						set_status IS_DIRECT
						ret
						
;падение объекта в пропасть
;вх  - ix - объект
deep_fall				get_status IS_FALLDEEP
						ret z
						ld a,(ix+object.ycord)
						add a,3
						ld (ix+object.ycord),a
						cp 192
						ret c
						
;переинициализация героя после падения
deep_fall_hero			ld a,(ix+object.type)
						cp id_Bred
						jr nc,deep_fall_enemy
						
						res_status IS_SUPERMOVE
						ld a,(ix+object.energy)
						sub 16
						jr z,deep_fall_hero_1
						jr nc,deep_fall_hero_2
deep_fall_hero_1		pop hl ;забываем о ret
						jp game_over
						
deep_fall_hero_2		ld (ix+object.energy),a
						ld (ix+object.undeadcntr),200 ;4 секунды бессмертия после перерождения
						jp hero_go_start				
;удаление врага
deep_fall_enemy			xor a
						ld (ix+object.type),a
						ld (drw_statusbar_type+1),a
						ld (drw_statusbar_energy+1),a
						ret	
						
;поиск и захват объекта для последующего броска
;вх  - ix - объект захватчик
;вых - флаг Z - объект не найден
find_object_capture		call hero_is_armed ;выход, если герой вооружён
						jr nz,find_object_capture_q

						get_status IS_ENEMYCAPTURE ;выход, если враг уже захвачен
						jr z,find_object_capture_0

find_object_capture_q	xor a
						ret

find_object_capture_0	push bc,de,hl
						ld iy,objects
						ld b,object.maxnum
						
find_object_capture_l	ld a,(iy+object.type)
						cp id_Bred
						jp c,find_object_capture_e
						cp id_Items
						jp z,find_object_capture_e

						ld a,(iy+object.yoffset)
						or a
						jp nz,find_object_capture_e
						
						ld a,(iy+object.energy)
						or a
						jp z,find_object_capture_e
						
						get_enemy_status IS_CRITDAMAGE
						jp nz,find_object_capture_e
					
						get_enemy_status IS_SUPERDAMAGE
						jr nz,find_object_capture_e
					
						ld a,(ix+object.type)
						cp id_Haggar
						jr z,find_object_capture_h
					
						bit IS_NOINT,(iy+object.status)
						jr nz,find_object_capture_e
						
find_object_capture_h	call distance_y
						ld a,h
						or a
						jp nz,find_object_capture_e
						ld a,l
						cp 5
						jr nc,find_object_capture_e
						
						call distance_x
						get_status IS_DIRECT
						jr nc,find_object_capture_1
						jr nz,find_object_capture_e
						jr find_object_capture_2
find_object_capture_1	jr z,find_object_capture_e
find_object_capture_2	ld a,h
						or a
						jr nz,find_object_capture_e
						ld a,l
						cp 8
						jr c,find_object_capture_e
						cp 16
						jr nc,find_object_capture_e

						ld a,(iy+object.type)
						cp id_Poison
						jr z,find_object_capture_3
						cp id_Trasher
						jr c,find_object_capture_4
find_object_capture_3	ld (iy+object.faze),2
						jr find_object_capture_e
						
find_object_capture_4	set IS_CAPTURED,(iy+object.status)
						set_status IS_ENEMYCAPTURE
						ld a,(iy+object.type)
						ld (drw_statusbar_type+1),a
						ld a,(iy+object.energy)
						ld (drw_statusbar_energy+1),a
						ld a,(ix+object.type)
						cp id_Haggar
						ld a,250 ;пауза удержания для Хаггара
						ld (timer_statusbar),a
						jr z,$+4
						ld a,150 ;пауза удержания для Коди и Гая
						ld (ix+object.cntr),a 
						call correct_enemy_cord

						push iy
						pop hl
						ld (ix+object.enemyobject),l
						ld (ix+object.enemyobject+1),h
						ld (ix+object.hits),0
						or h
						jr find_object_capture_f
						
find_object_capture_e	ld de,object.lenght
						add iy,de
						dec b
						jp nz,find_object_capture_l
find_object_capture_f	pop hl,de,bc
						ret
						
;отмена удержания врага						
capture_cancel			get_status IS_ENEMYCAPTURE ;выход, если нет захваченного врага
						ret z
						res_status IS_ENEMYCAPTURE
						call get_iy
						inc (iy+object.ycord)
						res_enemy_status IS_CAPTURED
						ret					
						
;корректировка координат врага при захвате
;вх  - ix - объект героя
;      iy - объект врага
correct_enemy_cord		ld a,(ix+object.status)
						cpl
						and #01
						ld d,a
						ld a,(iy+object.status)
						and #fe
						or d
						ld (iy+object.status),a
						
						ld a,(ix+object.ycord)
						dec a
						ld (iy+object.ycord),a
						
						ld hl,16
						get_status IS_DIRECT
						jr z,correct_enemy_cord_1
						call neg_hl
correct_enemy_cord_1	ld e,(ix+object.xcord)
						ld d,(ix+object.xcord+1)
						add hl,de
						ld (iy+object.xcord),l
						ld (iy+object.xcord+1),h
						ret