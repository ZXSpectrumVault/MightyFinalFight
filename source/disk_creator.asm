;образ диска (на основе кода nikphe^any 2oo1 remixed for sjasmplus by aprisobal 2006)

				device zxspectrum128
				
				org #4000
				disp #5d3b
	
basic			dw #100,end_basic-begin_basic
begin_basic		dw #30fd,#0e,#b300,#5f,#f93a,#30c0,#0e,#5300,#5d,#ea3a

;очистка экрана
				di
				xor a
				out (254),a
				ld hl,#5aff
				ld (hl),a
				or (hl)
				dec hl
				jr z,$-3

;адрес загрузки
				ld hl,#6000
;количество секторов
				ld b,high loader_len + 1
				call load
;старт загрузчика
				jp #6000

load			ld de,(#5cf4)
				ld c,#05
				jp #3d13

				db #0d
end_basic					
				
;создаем trd образ			
				ent
				emptytrd "../game.trd"
				savetrd "../game.trd","boot.B",#4000,end_basic-basic	
				
				org 0	
				incbin "../bin/loader.bin"
loader_len		savetrd "../game.trd","loader.C",0,$
				
				org 0	
				incbin "../bin/main.bin"
fsz_0			savetrd "../game.trd","main.C",0,$
				
				org 0	
				incbin "../bin/Items.bin"
fsz_1			savetrd "../game.trd","Items.C",0,$
				
				org 0	
				incbin "../res/font/font.ch8"
fsz_2			savetrd "../game.trd","font.C",0,$
				
				org 0	
				incbin "../bin/mainmenu.bin"
fsz_3			savetrd "../game.trd","mainmenu.C",0,$
	
				org 0	
				incbin "../bin/menures1.bin"
fsz_4			savetrd "../game.trd","menures1.C",0,$
				
				org 0	
				incbin "../bin/menures2.bin"
fsz_5			savetrd "../game.trd","menures2.C",0,$

				org 0	
				incbin "../bin/redraw.bin"
fsz_6			savetrd "../game.trd","redraw.C",0,$
				
				org 0	
				incbin "../bin/sengine.bin"
fsz_7			savetrd "../game.trd","sengine.C",0,$

				org 0	
				incbin "../bin/tutorial.bin"
fsz_8			savetrd "../game.trd","tutorial.C",0,$

				org 0	
				incbin "../bin/heroes_1.bin"
fsz_9			savetrd "../game.trd","hero_1.C",0,$
				
				org 0	
				incbin "../bin/heroes_2.bin"
fsz_10			savetrd "../game.trd","hero_2.C",0,$
				
				org 0	
				incbin "../bin/music_set.bin"
fsz_11			savetrd "../game.trd","musset.C",0,$
				
				org 0	
				incbin "../bin/code.bin"
fsz_12			savetrd "../game.trd","code.C",0,$
				
;level 1					
				org 0	
				incbin "../res/tileset/1-0.tiles"
fsz_13			savetrd "../game.trd","map_1.C",0,$	
				org 0	
				incbin "../bin/tileset_1.bin"
fsz_14			savetrd "../game.trd","tiles_1.C",0,$			
				org 0	
				incbin "../bin/location_1.bin"
fsz_15			savetrd "../game.trd","loc_1.C",0,$
				org 0	
				incbin "../music/nq-mff-round-01-slum.pt3"
fsz_16			savetrd "../game.trd","music_1.C",0,$
				org 0	
				incbin "../bin/enemy_set_1_1.bin"
fsz_17			savetrd "../game.trd","enemy1_1.C",0,$
				org 0	
				incbin "../bin/enemy_set_1_2.bin"
fsz_18			savetrd "../game.trd","enemy1_2.C",0,$
;level 2				
				org 0	
				incbin "../res/tileset/2-0.tiles"
fsz_19			savetrd "../game.trd","map_2.C",0,$	
				org 0	
				incbin "../bin/tileset_2.bin"
fsz_20			savetrd "../game.trd","tiles_2.C",0,$
				org 0	
				incbin "../bin/location_2.bin"
fsz_21			savetrd "../game.trd","loc_2.C",0,$
				org 0	
				incbin "../music/nq-mff-round-02-riverside.pt3"
fsz_22			savetrd "../game.trd","music_2.C",0,$
				org 0	
				incbin "../bin/enemy_set_2_1.bin"
fsz_23			savetrd "../game.trd","enemy2_1.C",0,$	
				org 0	
				incbin "../bin/enemy_set_2_2.bin"
fsz_24			savetrd "../game.trd","enemy2_2.C",0,$
;level 3			
				org 0	
				incbin "../res/tileset/3-0.tiles"
fsz_25			savetrd "../game.trd","map_3.C",0,$	
				org 0	
				incbin "../bin/tileset_3.bin"
fsz_26			savetrd "../game.trd","tiles_3.C",0,$
				org 0	
				incbin "../bin/location_3.bin"
fsz_27			savetrd "../game.trd","loc_3.C",0,$
				org 0	
				incbin "../music/nq-mff-round-03-old-town.pt3"
fsz_28			savetrd "../game.trd","music_3.C",0,$
				org 0	
				incbin "../bin/enemy_set_3_1.bin"
fsz_29			savetrd "../game.trd","enemy3_1.C",0,$		
				org 0	
				incbin "../bin/enemy_set_3_2.bin"
fsz_30			savetrd "../game.trd","enemy3_2.C",0,$
;level 4				
				org 0	
				incbin "../res/tileset/4-0.tiles"
fsz_31			savetrd "../game.trd","map_4.C",0,$	
				org 0	
				incbin "../bin/tileset_4.bin"
fsz_32			savetrd "../game.trd","tiles_4.C",0,$
				org 0	
				incbin "../bin/location_4.bin"
fsz_33			savetrd "../game.trd","loc_4.C",0,$
				org 0	
				incbin "../music/nq-mff-round-04-factory.pt3"
fsz_34			savetrd "../game.trd","music_4.C",0,$
				org 0	
				incbin "../bin/enemy_set_4_1.bin"
fsz_35			savetrd "../game.trd","enemy4_1.C",0,$		
				org 0	
				incbin "../bin/enemy_set_4_2.bin"
fsz_36			savetrd "../game.trd","enemy4_2.C",0,$
;level 5
				org 0	
				incbin "../res/tileset/5-0.tiles"
fsz_37			savetrd "../game.trd","map_5.C",0,$	
				org 0	
				incbin "../bin/tileset_5.bin"
fsz_38			savetrd "../game.trd","tiles_5.C",0,$
				org 0	
				incbin "../bin/location_5.bin"
fsz_39			savetrd "../game.trd","loc_5.C",0,$
				org 0	
				incbin "../music/nq-mff-round-05-bay-area.pt3"
fsz_40			savetrd "../game.trd","music_5.C",0,$
				org 0	
				incbin "../bin/final_music_set.bin"
fsz_41			savetrd "../game.trd","fmusset.C",0,$
				org 0	
				incbin "../bin/enemy_set_5_1.bin"
fsz_42			savetrd "../game.trd","enemy5_1.C",0,$
				org 0	
				incbin "../bin/enemy_set_5_2.bin"
fsz_43			savetrd "../game.trd","enemy5_2.C",0,$
;ending
				org 0	
				incbin "../bin/ending.bin"
fsz_44			savetrd "../game.trd","ending.C",0,$

;таблица размеров файлов
				org 0
				dw fsz_0, fsz_1, fsz_2, fsz_3, fsz_4, fsz_5, fsz_6, fsz_7, fsz_8, fsz_9
				dw fsz_10, fsz_11, fsz_12, fsz_13, fsz_14, fsz_15, fsz_16, fsz_17, fsz_18
				dw fsz_19, fsz_20, fsz_21, fsz_22, fsz_23, fsz_24, fsz_25, fsz_26, fsz_27
				dw fsz_28, fsz_29, fsz_30, fsz_31, fsz_32, fsz_33, fsz_34, fsz_35, fsz_36
				dw fsz_37, fsz_38, fsz_39, fsz_40, fsz_41, fsz_42, fsz_43, fsz_44
				savebin "../bin/files_len.bin", 0, $