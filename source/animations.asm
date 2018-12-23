;анимации игры

;персонажи
id_Cody					equ 1
id_Guy					equ 2
id_Haggar				equ 3
id_Bred					equ 4
id_TwoP					equ 5
id_ElGade				equ 6
id_Poison				equ 7
id_Axl					equ 8
id_Andore				equ 9
id_Trasher				equ 10
id_Kitana				equ 11
id_Abigal				equ 12
id_Belger				equ 13
id_Items				equ 14

;анимации
anim_idle = 0
anim_walk = 1
anim_punchleft = 2
anim_punchright = 3
anim_jumpkickside = 4
anim_jumpkickup = 5
anim_uppercut = 6
anim_damage = 7
anim_damagefall = 8
anim_throw = 9
anim_throwkick = 10
anim_throwdown = 11
anim_captured = 12
anim_fly = 13
anim_wait = 14
anim_sitting = 15
anim_twister = 16
anim_super = 17
anim_extmove = 18
anim_weapon_idle = 19
anim_weapon_walk = 20
anim_weapow_hit = 21
anim_weapon_throw = 22
anim_kissed = 23

;таблица анимаций по типам объектов
;структура: 
;   1-ый байт - сила удара
;   2-ой байт - номер анимации
anim_tab				
;walk						
						db 0,Cody_Walk, 0,Guy_Walk, 0,Haggar_Walk, 0,Bred_Walk
						db 0,TwoP_Walk, 0,ElGade_Walk, 0,Poison_Walk, 0,Axl_Walk
						db 0,Andore_Walk, 0,Trasher_Walk, 0,Kitana_Walk, 0,Abigal_Walk, 0,Belger_Walk
;punchleft				
						db 4,Cody_PunchLeft, 2,Guy_PunchLeft, 6,Haggar_PunchLeft, 2,Bred_Punch
						db 3,TwoP_Punch, 12,ElGade_Punch, 6,Poison_PunchLeft, 8,Axl_Kick
						db 4,Andore_Punch, 12,Trasher_Punch, 12,Kitana_PunchLeft, 12,Abigal_Kiss, 12,Belger_Punch
;punchright
						db 8,Cody_PunchRight, 4,Guy_PunchRight, 12,Haggar_PunchRight, 0,0 
						db 0,0, 0,0, 4,Poison_PunchRight, 0,Axl_Block
						db 6,Andore_Attack, 0,0, 12,Kitana_PunchRight, 12,Abigal_Uppercut, 0,Belger_Block
;jumpkickside						
						db 4,Cody_JumpKickSide, 4,Guy_JumpKickSide, 4,Haggar_JumpKickSide, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 8,Trasher_Jump, 0,0, 0,0, 0,0					
;jumpkickup						
						db 22,Cody_JumpKickUp, 20,Guy_JumpKickUp, 30,Haggar_JumpKickUp, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,0
;uppercut						
						db 25,Cody_Uppercut, 20,Guy_Uppercut, 30,Haggar_Uppercut, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,0						
;damage
						db 0,Cody_Damage, 0,Guy_Damage, 0,Haggar_Damage, 0,Bred_Damage
						db 0,TwoP_Damage, 0,ElGade_Damage, 0,Poison_Damage, 0,Axl_Damage
						db 0,Andore_Damage, 0,Trasher_Damage, 0,Kitana_Damage, 0,Abigal_Damage, 0,Belger_Damage
;damagefall
						db 0,Cody_DamageFall, 0,Guy_DamageFall, 0,Haggar_DamageFall, 0,Bred_DamageFall
						db 0,TwoP_DamageFall, 0,ElGade_DamageFall, 0,Poison_DamageFall, 0,Axl_DamageFall
						db 0,Andore_DamageFall, 0,Trasher_DamageFall, 0,Kitana_DamageFall, 0,Abigal_DamageFall, 0,Belger_DamageFall
;throw
						db 0,Cody_Throw, 0,Guy_Throw, 0,Haggar_Throw, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,0						
;throwkick
						db 8,Cody_ThrowKick, 6,Guy_ThrowKick, 8,Haggar_ThrowHit, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,0					
;throwdown
						db 0,Cody_ThrowDown, 0,Guy_ThrowDown, 0,Haggar_ThrowDown, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,0								
;captured
						db 0,0, 0,0, 0,0, 0,Bred_Captured
						db 0,TwoP_Damage, 0,ElGade_Damage, 0,Poison_Damage, 0,Axl_Damage
						db 0,Andore_Damage, 0,0, 0,0, 0,0, 0,0						
;fly
						db 0,0, 0,0, 0,0, 0,Bred_Fly
						db 0,TwoP_Fly, 0,ElGade_Fly, 0,Poison_Fly, 0,Axl_Fly
						db 0,Andore_Fly, 0,0, 0,0, 0,0, 0,0					
;wait
						db 0,0, 0,0, 0,0, 0,Bred_Wait
						db 0,TwoP_Wait, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,0
;sitting
						db 0,0, 0,0, 0,0, 0,Bred_Sitting
						db 0,TwoP_Sitting, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,0
;twister
						db 20,Cody_Twist, 18,Guy_Twist, 20,Haggar_Twist, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,0
;super
						db 0,Cody_Super, 24,Guy_Super, 24,Haggar_Super, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,0							
;extmove
						db 0,Cody_Super, 6,Guy_SuperKick, 0,Haggar_ThrowWalk, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,Belger_Laugh				
;weapon_idle
						db 0,Cody_IdleKnife, 0,0, 0,Haggar_IdleHummer, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,Kitana_Stay, 0,Abigal_IdleAngry, 0,0	
;weapon_walk
						db 0,Cody_WalkKnife, 0,0, 0,Haggar_WalkHummer, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,Kitana_WalkNoBlade, 0,Abigal_RunAngry, 0,Belger_Walk	
;weapow_hit
						db 20,Cody_PunchKnife, 0,0, 30,Haggar_HummerHit, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 12,Abigal_PunchAngry, 0,Belger_PunchRocket
;weapon_throw
						db 0,Cody_ThrowKnife, 0,Guy_ThrowShuriken, 0,Haggar_ThrowHummer, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,Belger_Lose
						
;kissed
						db 0,Cody_Kissed, 0,Guy_Kissed, 0,Haggar_Kissed, 0,0
						db 0,0, 0,0, 0,0, 0,0
						db 0,0, 0,0, 0,0, 0,0, 0,Belger_Explode
						
;старт анимации по типу объекта
;вх  - ix - объект
;    - a - анимация
start_animation			or a
						jp z,init_animation ;у всех объектов idle = 0
						dec a
						ld l,a
						ld h,0
						add hl,hl
						ld de,hl
						add hl,hl
						add hl,hl
						ld bc,hl
						add hl,hl
						add hl,bc
						add hl,de ;hl = a * 26
						ld de,anim_tab
						add hl,de
						ld a,(ix+object.type)
						dec a
						add a,a
						add a,l
						ld l,a
						jr nc,$+3
						inc h
						ld a,(hl)
						ld (ix+object.hitpower),a
						inc hl
						ld a,(hl)
						jp init_animation