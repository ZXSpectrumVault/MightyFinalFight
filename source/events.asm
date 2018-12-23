;события анимаций


;повреждение от падения после броска
event_damagefall		res_status IS_THROW
						ld a,(ix+object.energy)
						sub 10 ;сила удара = 10
						jr nc,event_damagefall1
						xor a
event_damagefall1		ld (ix+object.energy),a
						ld (drw_statusbar_energy+1),a
						ld a,(ix+object.type)
						ld (drw_statusbar_type+1),a
					
;звук падения					
event_falldown_sfx		call rnd
						and 63
						add a,(ix+object.frametime)
						ld (ix+object.frametime),a ;добавляем случайное значение
						ret
						
;прыжок во время суперудара			
event_jump				ld (ix+object.yaccel),-30
						jp event_hit
			
;бросок объекта
event_throw				ld (ix+object.yaccel),-25
						ld a,(ix+object.status)
						xor 1
						ld (ix+object.status),a
						ret
						
event_weaponhit			call event_skipdelta
						jr event_hit
						
;пропуск дельтатайминга во время кадра удара						
event_skipdelta			ld a,#ff
						ld (skip_deltatime),a
						ret

;супер-удар
event_superhit			set_status IS_CRITHIT
						set_status IS_SUPERHIT
						ld (ix+object.hitpower),20
;удар
event_hit				ld iy,objects
						ld b,object.maxnum
						ld c,#00
;сравнение типов						
event_hit_loop			ld a,(iy+object.type)
						or a
						jp z,event_hit_end
						cp id_Items
						jr nz,event_hit_loop_0
						ld a,(iy+object.itemkind)
						cp Items_Eat
						jr c,event_hit_loop_0
						cp Items_Blade
						jr c,event_hit_end			
event_hit_loop_0		cp (ix+object.type)
						jr z,event_hit_end
						get_enemy_status IS_BLOCK
						jr nz,event_hit_end
						
						ld a,(iy+object.yoffset)
						cp 24
						jr nc,event_hit_end ;не повреждаются объекты, у которых yoffset >= 24
						
						ld a,(iy+object.faze) ;удар не наносится врагу в 5-ой фазе
						cp 5
						jr z,event_hit_end
						
;сравнение координат Y
						call distance_y
						ld d,6
						jr nc,event_hit_loop_1
						ld a,(iy+object.type)
						cp id_Items
						jr nz,event_hit_loop_1
						ld a,(iy+object.itemkind)
						cp Items_Barrel
						jr nz,event_hit_loop_1
						ld d,20
event_hit_loop_1		ld a,h
						or a
						jr nz,event_hit_end
						ld a,l
						cp d
						jr nc,event_hit_end
						
;сравнение координат X
						call distance_x
						exa
						ld a,(ix+object.twistercnt)
						or a
						jr nz,event_hit2
						exa
						get_status IS_DIRECT
						jr nc,event_hit1
						jr nz,event_hit_end
						jr event_hit2
event_hit1				jr z,event_hit_end
event_hit2				ld a,h
						or a
						jr nz,event_hit_end
						
						call hero_is_armed
						jr z,event_hit3
						ld a,l
						cp 40
						jr nc,event_hit_end
						jr event_hit4
						
event_hit3				ld a,l		
						cp 28
						jr nc,event_hit_end

;удар достиг цели		
event_hit4				get_enemy_status IS_CRITDAMAGE ;не наносим урон лежащему объекту
						jr nz,event_hit_end
						ld a,(iy+object.undeadcntr)
						or a
						jr nz,event_hit_end ;не наносим урон объекту с бессмертием

						push bc
						call set_damage
						pop bc
						ld c,#ff
						
event_hit_end			ld de,object.lenght
						add iy,de
						dec b
						jp nz,event_hit_loop				
;звук удара					
						ld a,c
						or a
						jp z,sfx_miss
						inc (ix+object.hits)
						jp sfx_hit
						
;удар коленом
event_knee				call get_iy
						call set_damage
						jp sfx_hit
						
;убить объект
event_kill				ld a,(ix+object.energy)	
						or a
						jr nz,event_kill_1
						ld (ix+object.type),a
						ret
event_kill_1			ld a,(ix+object.type)
						cp id_Abigal
						jr nz,event_kill_2
						ld (ix+object.undeadcntr),250 ;5 секунд ярости для Абигейла
						ret
event_kill_2			cp id_Bred
						ret nc
						xor a
						ld (combo_active),a
						ret
;нож Коди
event_knife				ld b,Items_Knife
						ld hl,#2806
						jr make_hero_weapon

;сюрикен Гая
event_shuriken			call event_skipdelta
						ld b,Items_Shuriken
						ld hl,#2806
						jr make_hero_weapon


;молот Хаггара
event_hummer			ld b,Items_Hummer
						ld hl,#2806
						jr make_hero_weapon

;волна Коди					
event_wave				ld hl,wave_sfx
						call sfx_ay
						ld b,Items_Wave
						ld hl,#1808

make_hero_weapon		push hl
						ld h,16
						ld a,id_Items
						call child_object
						ld (iy+object.damagearea),16
						ld (iy+object.damagearea+1),16
						pop hl
						ld (iy+object.hitpower),h
						ld (iy+object.itemspeed),l
						ret
		
event_super				ld hl,wave_sfx
						jp sfx_ay

event_endkiss			ld iy,objects
						res_enemy_status IS_CRITDAMAGE
						set_status IS_CRITHIT
						set_status IS_SUPERHIT
						call event_hit
						ld a,-2
						ld (objects+object.xaccel),a
event_kiss				jp sfx_clear