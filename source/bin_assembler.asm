;создание бинарников для загрузки в образ диска

;архив_1 с героями
						org hero_1.adr
Cody.arx				incbin "../bin/Cody.bin.mlz"
Guy.arx					incbin "../bin/Guy.bin.mlz"
						savebin "../bin/heroes_1.bin", hero_1.adr, $ - hero_1.adr
				
;архив_2 с героями
						org hero_2.adr
Haggar.arx				incbin "../bin/Haggar.bin.mlz"
						savebin "../bin/heroes_2.bin", hero_2.adr, $ - hero_2.adr

;архив c тайлами и локацией тренировочной комнаты + тренировочный Брэд
						org tileset
						incbin "../res/tileset/0-0.tiles"
tutorial_location		incbin "../bin/location_0.bin"
						align 256
tutorial_bred			incbin "../bin/Bred.bin"
						savebin "../bin/tutorial.bin", tileset, $ - tileset
						
;набор дополнительных музыкальных треков
						org music_set.adr
music_over.adr			incbin "../music/nq-mff-game-over.pt3"
music_boss.adr			incbin "../music/nq-mff-boss-theme.pt3"
music_win.adr			incbin "../music/nq-mff-round-clear.pt3"
music_fall.adr			incbin "../music/_sfx/down-fx.pt3"
						savebin "../bin/music_set.bin", music_set.adr, $ - music_set.adr
						
;набор дополнительных музыкальных треков для финального босса
						org music_set.adr
						incbin "../music/nq-mff-game-over.pt3" ;трек game over там-же, где и в обычном наборе
						incbin "../music/nq-mff-final-boss-theme.pt3" ;трек финального босса там же, где трек босса в обычном наборе
music_final_win.adr		incbin "../music/nq-mff-final-round-clear.pt3"
						savebin "../bin/final_music_set.bin", music_set.adr, $ - music_set.adr
				
;уровень 1
						org extended.adr
						db id_Bred,enemy_set_1.page
						dw enemy_set_1_1
						db id_TwoP,enemy_set_1.page
						dw enemy_set_1_2
						db id_Andore,enemy_set_1.page
						dw enemy_set_1_3
						db id_Axl,enemy_set_1.page
						dw enemy_set_1_4
						db id_Trasher,enemy_set_2.page
						dw enemy_set_1_5
						db 0
						align 256
enemy_set_1_1			incbin "../bin/Bred.bin"
						align 256
enemy_set_1_2			incbin "../bin/TwoP.bin"
						align 256
enemy_set_1_3			incbin "../bin/Andore.bin"
						align 256
enemy_set_1_4			incbin "../bin/Axl.bin"				
						savebin "../bin/enemy_set_1_1.bin", extended.adr, $ - extended.adr
						org extended.adr
enemy_set_1_5			incbin "../bin/Trasher.bin"	

yesno_text				db " YES  NO",0
trasher_text_1			db "HEH-HEH. I AM THRASHER,",0
						db "RULER OF THIS CITY.",0
						db "BOW DOWN BEFORE ME!",255
trasher_text_2			db "I AM SUPERIOR.",0
						db "MAYBE YOU'RE STRONGER THAN",0
						db "YOU LOOK.",0
						db "WHY DON'T YOU JOIN US?",255
trasher_text_3			db "HEH-HEH. I DON'T BELIVE",0
						db "YOU. YOU'RE TOO MUCH OF A",0
						db "MAMA'S BOY.",254
trasher_text_4			db "YOU DARE REFUSE THE MAD GEAR",0
						db "GANG?",0
						db "THOSE WHO OFFEND ME",0
						db "MUST DIE!",254
trasher_text_5			db "WAHHHHH!",0
						db "DON'T YOU KNOW I RULE THE",0
						db "STREETS?!",255
trasher_text_6			db "ARGHHHH!",0
						db "THOSE WHO OFFEND ME",0
						db "MUST DIE!",254

						savebin "../bin/enemy_set_1_2.bin", extended.adr, $ - extended.adr
				
;уровень 2
						org extended.adr
						db id_Bred,enemy_set_1.page
						dw enemy_set_2_1
						db id_TwoP,enemy_set_1.page
						dw enemy_set_2_2
						db id_ElGade,enemy_set_1.page
						dw enemy_set_2_3
						db id_Axl,enemy_set_1.page
						dw enemy_set_2_4
						db id_Poison,enemy_set_1.page
						dw enemy_set_2_5
						db id_Kitana,enemy_set_2.page
						dw enemy_set_2_6
						db 0
						align 256
enemy_set_2_1			incbin "../bin/Bred.bin"
						align 256
enemy_set_2_2			incbin "../bin/TwoP.bin"
						align 256
enemy_set_2_3			incbin "../bin/ElGade.bin"
						align 256
enemy_set_2_4			incbin "../bin/Axl.bin"
						align 256
enemy_set_2_5			incbin "../bin/Poison.bin"
						savebin "../bin/enemy_set_2_1.bin", extended.adr, $ - extended.adr
						org extended.adr
enemy_set_2_6			incbin "../bin/Kitana.bin"	

kitana_text_1			db "YOU'RE TOO FAT AND LAZY TO",0
						db "OPPOSE ME!",254
kitana_text_2			db "BIG MOUTH, LITTLE SWORD",0
						db "TELL ME WHAT YOU KNOW",0
						db "OR I'LL CLOBBER YOU!",254
kitana_text_3			db "ARE YOU DONE FLAPPIN' THOSE",0
						db "LIPS OF YOURS?",0
						db "GOOD, BECAUSE I'M GONNA CHOP",0
						db "YOU DOWN TO SIZE!",254

						savebin "../bin/enemy_set_2_2.bin", extended.adr, $ - extended.adr
				
;уровень 3
						org extended.adr
						db id_TwoP,enemy_set_1.page
						dw enemy_set_3_1
						db id_Andore,enemy_set_1.page
						dw enemy_set_3_2
						db id_Abigal,enemy_set_1.page
						dw enemy_set_3_3
						db id_ElGade,enemy_set_2.page
						dw enemy_set_3_4
						db id_Poison,enemy_set_2.page
						dw enemy_set_3_5
						db 0
						align 256
enemy_set_3_1			incbin "../bin/TwoP.bin"
						align 256
enemy_set_3_2			incbin "../bin/Andore.bin"
						align 256
enemy_set_3_3			incbin "../bin/Abigal.bin"			
						savebin "../bin/enemy_set_3_1.bin", extended.adr, $ - extended.adr
						org extended.adr
enemy_set_3_4			incbin "../bin/ElGade.bin"	
						align 256
enemy_set_3_5			incbin "../bin/Poison.bin"	

abigal_text_1			db "I'M ABIGAIL, THE TOUGHEST",0
						db "FIGHTER IN THE MAD GEAR.",0
						db "YOU'VE BEEN LUCKY SO FAR....",0
						db "BUT YOUR LUCK HAS JUST RUN",0
						db "OUT!",254
abigal_text_2			db "THE OTHERS LOOKED TOUGHER",0
						db "THAN YOU, JERK!",254
abigal_text_3			db "I'M GONNA SHOVE THOSE WORDS",0
						db "DOWN YOUR THROAT!",254
						
						savebin "../bin/enemy_set_3_2.bin", extended.adr, $ - extended.adr
				
;уровень 4
						org extended.adr
						db id_Bred,enemy_set_1.page
						dw enemy_set_4_1
						db id_Andore,enemy_set_1.page
						dw enemy_set_4_2
						db id_Axl,enemy_set_1.page
						dw enemy_set_4_3
						db id_ElGade,enemy_set_1.page
						dw enemy_set_4_4
						db id_Kitana,enemy_set_2.page
						dw enemy_set_4_5
						db 0
						align 256
enemy_set_4_1			incbin "../bin/Bred.bin"
						align 256
enemy_set_4_2			incbin "../bin/Andore.bin"
						align 256
enemy_set_4_3			incbin "../bin/Axl.bin"
						align 256
enemy_set_4_4			incbin "../bin/ElGade.bin"		
						savebin "../bin/enemy_set_4_1.bin", extended.adr, $ - extended.adr
						org extended.adr
enemy_set_4_5			incbin "../bin/Kitana.bin"	

kitana_text_4			db "JESSICA HAS ALREADY BEEN",0
						db "TAKEN TO OUR SECRET HIDEOUT.",0
						db "MY BROTHER'S DEFEAT WILL BE",0
						db "AVENGED!",0
						db "READY!?",254

						savebin "../bin/enemy_set_4_2.bin", extended.adr, $ - extended.adr
				
;уровень 5
						org extended.adr
						db id_TwoP,enemy_set_1.page
						dw enemy_set_5_1
						db id_Abigal,enemy_set_1.page
						dw enemy_set_5_2
						db id_Belger,enemy_set_1.page
						dw enemy_set_5_3
						db id_Poison,enemy_set_2.page
						dw enemy_set_5_4
						db id_ElGade,enemy_set_2.page
						dw enemy_set_5_5
						db 0
						align 256
enemy_set_5_1			incbin "../bin/TwoP.bin"
						align 256
enemy_set_5_2			incbin "../bin/Abigal.bin"
						align 256
enemy_set_5_3			incbin "../bin/Belger.bin"			
						savebin "../bin/enemy_set_5_1.bin", extended.adr, $ - extended.adr
						org extended.adr
enemy_set_5_4			incbin "../bin/Poison.bin"
						align 256
enemy_set_5_5			incbin "../bin/ElGade.bin"

abigal_text_4			db "YOU'RE NO MATCH FOR ME!",0
						db "READY!?",254
belger_text_1			db "YOU'RE THE BOSS?",0
						db "WHERE'S JESSICA?",254
belger_text_2			db "RELAX. JESSICA IS MY SPECIAL",0
						db "GUEST.",0
						db "SHE IS IN THE NEXT ROOM.",0
						db "JESSICA AND I ARE SCHEDULED",0
						db "TO BE MARRIED TOMORROW!",0
						db "HA-HA-HA!",0
						db "LET US BEGIN OUR",0
						db "FINAL FIGHT!",254
belger_text_3			db "FINAL VICTORY WILL BE MINE!",254		
				
						savebin "../bin/enemy_set_5_2.bin", extended.adr, $ - extended.adr
				