;главный модуль игры
						device ZXSPECTRUM128
						include "macroses.asm"
;тип сборки
trdos					equ 0		;tr-dos версия
tap						equ 1		;tap версия
version_type			equ trdos

;режим debug (0 - выключен, 1 - включен)
debug_mode				equ 0

;нижняя оперативная память #4000-#bfff
screen					equ #4000	;адрес реального экрана
ending					equ #6000	;адрес модуля концовок (16384 байта)
temp_tile_prop			equ #6000	;временный буфер свойств тайлов
vscreen					equ #6008	;адрес виртуального экрана, размер #1a00
loc_buffer				equ #7a00	;адрес буфера локации, размер #0154
main_stek				equ #7bff   ;адрес стека
objects					equ #7c00   ;описатели объектов, размер #0100
game_screen_property	equ #7d00   ;таблица свойств тайлов на экране, 16x10, размер #0a0
anim_tile_list			equ #7da0   ;список анимированных тайлов, размер #160
files_length			equ #7f80   ;список длинн файлов проекта, максимум 64 файла (128 байт)
temp_arx				equ #8000	;адрес для временного хранения архивов для распаковки и таблицы смещений
main_code				equ #a000   ;начало программы, размер #1e00 (7680 байта)
int_vector				equ #be00	;вектор прерывания, размер #0200

;верхняя оперативная память #с000-#ffff для 48кб машин или доп. страница для машин >= 128кб
ext.page				equ 4
code.adr				equ #c000	;начало дополнительного кода, размер #1900 (6400 байт)
tileset					equ #d900	;адрес начала тайлсета, размер #2700 (256 tiles property + 256 tiles)

;расширенная оперативная, или постоянная память
extended.adr			equ #c000	;адрес начала расширенной памяти (#0000 или #c000)
vscreen.width			equ 40		;длина строки виртуального экрана
redraw.adr				equ #c000   ;адрес процедуры вывода виртуального экрана
redraw.page				equ 1		;номер страницы процедуры вывода виртуального экрана
sengine.adr				equ #c000	;адрес звукового движка (#0900)
sengine.page			equ 3		;номер страницы звукового движка
music.adr				equ #c900	;адрес музыкального трека локации (максимальный размер 4кб)
music_set.adr			equ #d900 	;адрес начала набора дополнительных треков (#1c00)
location.adr			equ #f500	;адрес локации размер #0b00 (2816 байт)
location.page			equ 3		;номер страницы локации, должен быть равен sengine.page
tilesets.adr			equ #dc00	;адрес архива тайлсетов для текущего уровня (максимальный размер 7кб)
tilesets.page			equ 7		;страница архива тайлсетов для текущего уровня
enemy_set_1.page		equ 6		;страница_1 набора врагов
enemy_set_2.page		equ 7		;страница_2 набора врагов
font.adr				equ #f800   ;адрес шрифта, размер #800
font.page				equ 7		;номер страницы шрифта
hero_1.adr				equ #c000	;адрес загрузки архива_1 с героями (страница enemy_set_1.page)
hero_2.adr				equ #c000	;адрес загрузки архива_2 с героями (страница enemy_set_2.page)

;распределение памяти для main_menu
main_menu.adr			equ #c000	;адрес модуля главного меню (6400 байт)
main_menu.page			equ 4		;номер страницы модуля главного меню
main_menu_res_1.adr     equ #d900   ;адрес ресурсов №1 главного меню (9984 байта)
main_menu_res_1.page	equ 3		;номер страницы ресурсов №1 главного меню
main_menu_res_2.adr     equ #d800   ;адрес ресурсов №2 главного меню (8192 байта)
main_menu_res_2.page	equ 7		;номер страницы ресурсов №2 главного меню

;дескриптор объекта
object.ycord			equ 0	;координата y в локации в пикселях (2 байта)
object.xcord			equ 2	;координата x в локации в пикселях (2 байта)
object.yoffset			equ 4	;высота объекта над поверхностью, вычитается из ycord (1 байт)
object.type				equ 5	;тип объекта (1 байт), если равно #00 - нет объекта
object.animation		equ 6   ;номер текущей анимации (1 байт)
object.frametime		equ 7   ;счетчик продолжительности вывода кадра (1 байт)
object.composition		equ 8   ;адрес текущей композиции (2 байта)
object.xaccel			equ 10  ;скорость перемещения объекта по координате X с учётом направления (-127;127) (1 байт)
object.yaccel			equ 11	;ускорение объекта по координате Y (гравитация) (1 байт)
object.hits				equ 12  ;счетчик непрерывно попадавших в цель ударов (1 байт)
object.animadr			equ 13  ;адрес начала анимаций объекта (2 байт)
object.animpage			equ 15  ;страница начала анимаций объекта (1 байт)
object.faze				equ 16  ;текущая фаза (1 байт) (для врагов)
object.twistercnt		equ 16  ;счётчик для суперудара "твистер" (1 байт) (для героев)
object.energy			equ 17  ;жизненная энергия объекта (1 байт)
object.maxenergy		equ 18  ;максимальная жизненная энергия объекта (1 байт)
object.hitpower			equ 19  ;сила текущего удара (1 байт)
object.cntr				equ 20  ;счётчик общего назначения (1 байт)
object.itemkind			equ 20	;(для Items) вид Items (1 байт)
object.rage				equ 21  ;накопленный Rage (1 байт)
object.itemprop			equ 21	;свойства Item, берется при создании из yoffset(1 байт)
object.stamina			equ 22	;выносливость, если больше 150, то герою наносится критический урон (1 байт)
object.undeadcntr		equ 23  ;счётчик бессмертия (1 байт)
object.enemyobject		equ 24  ;адрес вражеского объекта (2 байта)
object.movecord			equ 24  ;координаты, к которым стремится враг во время 5-ой фазы или герой в режиме скрипта
object.damagearea		equ 24	;размеры коллайдера поражения у Items (2 байта)
object.weapon			equ 26  ;оружие героя, количество (1 байт)
object.itemspeed		equ 26  ;скорость перемещения у Items (1 байт)
object.status			equ 27 	;статус объекта (2 байта)
object.scriptadr		equ 29	;текущий адрес описателя скрипта (2 байта)
object.scripttype		equ 31	;текущий тип скрипта (1 байт)

;статусы
IS_DIRECT				equ 0	;направление объекта по горизонтали (0 - вправо, 1 - влево)
IS_DAMAGE				equ 1   ;объект получил повреждение
IS_NOINT				equ 2   ;проигрывается анимация, которую нельзя прерывать
IS_THROW				equ 3   ;объект бросили
IS_CRITDAMAGE			equ 4   ;объект получил критический удар
IS_CAPTURED				equ 5	;объект захвачен (для вражеских объектов)
IS_SUPERMOVE			equ 5   ;объект в движении во время супер удара (для героев)
IS_SUPERDAMAGE			equ 6   ;объект получил супер удар (либо героя ударили сзади)
IS_SCRIPTMODE			equ 7	;объект в режиме скрипта
IS_DIRENEMY				equ 8	;с какой стороны подходит к врагу
IS_UPDOWNENEMY			equ 9   ;уходить с линии атаки вверх или вниз
IS_ENEMYCAPTURE			equ 10	;захвачен враг
IS_SUPERHIT				equ 11  ;объект наносит супер удар
IS_CRITHIT				equ 12  ;объект наносит критический удар
IS_KNEE					equ 13  ;объект наносит удар коленом
IS_FALLDEEP				equ 14	;объект падает в пропасть
IS_BLOCK				equ 15  ;объект в состоянии блока			
						
object.lenght			equ 32	;размер описателя объекта
object.maxnum			equ 8	;максимальное число объектов

;свойства тайлов
;D0..D4 - анимация
TP_ANIMMASK				equ #0f	;маска анимации
TP_RNDANIM				equ #10	;случайно включающаяся анимация

;D6..D7 - тип тайла
TP_TYPEMASK				equ #c0	;маска типа тайла
TP_EMPTY				equ #00	;пустой тайл, по нему нельзя ходить
TP_DEEP					equ #40	;пропасть, падает в неё
TP_PLATFORM				equ #80	;платформа

ONE_HSCROLL				equ 12	;кол-во тайлов горизонтального скролла
ONE_VSCROLL				equ 5	;кол-во тайлов вертикального скролла

LEFT_BORDER				equ 16	;левая граница экрана
RIGHT_BORDER			equ 240	;правая граница экрана
UP_BORDER				equ 40	;верхняя граница экрана
DOWN_BORDER				equ 159	;нижняя граница экрана

KEY_UP					equ 0	;кнопка вверх
KEY_DOWN				equ 1	;кнопка вниз
KEY_LEFT				equ 2	;кнопка влево
KEY_RIGHT				equ 3	;кнопка вправо
KEY_FIRE				equ 4	;кнопка огонь
KEY_PAUSE				equ 6   ;кнопка паузы
KEY_QUIT				equ 7   ;кнопка выхода

MAX_RAGE				equ 80	;максимальная величина Rage

;генерация бинарников
						include "loader.asm"
						include "main_menu.asm"
						include "ending.asm"
						include "redraw.asm"
						include "code.asm"
						include "bin_assembler.asm"
						include "sengine.asm"

						org main_code
main_begin				ld sp,main_stek
						ld (iy_save),iy
						jp main_start
						
						include "debug.asm"
		
						include "Cody_anim.asm"
						include "Cody_event.asm"
						
						include "Guy_anim.asm"
						include "Guy_event.asm"
						
						include "Haggar_anim.asm"
						include "Haggar_event.asm"
						
						include "Bred_anim.asm"
						include "Bred_event.asm"
						
						include "TwoP_anim.asm"
						include "TwoP_event.asm"
						
						include "ElGade_anim.asm"
						include "ElGade_event.asm"
						
						include "Poison_anim.asm"
						include "Poison_event.asm"
						
						include "Axl_anim.asm"
						include "Axl_event.asm"
						
						include "Andore_anim.asm"
						include "Andore_event.asm"
						
						include "Trasher_anim.asm"
						include "Trasher_event.asm"
						
						include "Kitana_anim.asm"
						include "Kitana_event.asm"
						
						include "Abigal_anim.asm"
						include "Abigal_event.asm"
						
						include "Belger_anim.asm"
						include "Belger_event.asm"
						
						include "Items_anim.asm"
						include "Items_event.asm"
		
						include "objects.asm"
						include "system.asm"
						include "gengine.asm"
						
;расположение ресурсов персонажей в памяти, страницы 0, 6 или 7
personages

Cody.evenst				dw Cody_events
Cody.adr				dw #c000
Cody.page				db 0
Cody.energy				db 70
Cody.speed				db 2

Guy.evenst				dw Guy_events
Guy.adr					dw #c000
Guy.page				db 0
Guy.energy				db 50
Guy.speed				db 3

Haggar.evenst			dw Haggar_events
Haggar.adr				dw #c000
Haggar.page				db 0
Haggar.energy			db 90
Haggar.speed			db 2

Bred.evenst				dw Bred_events
Bred.adr				dw tutorial_bred
Bred.page				db ext.page
Bred.energy				db 40
Bred.speed				db 1

TwoP.evenst				dw TwoP_events
TwoP.adr				dw #c000
TwoP.page				db 6
TwoP.energy				db 30
TwoP.speed				db 2

ElGade.evenst			dw ElGade_events
ElGade.adr				dw #c000
ElGade.page				db 6
ElGade.energy			db 30
ElGade.speed			db 2

Poison.evenst			dw Poison_events
Poison.adr				dw #c000
Poison.page				db 6
Poison.energy			db 20
Poison.speed			db 2

Axl.evenst				dw Axl_events
Axl.adr					dw #c000
Axl.page				db 6
Axl.energy				db 60
Axl.speed				db 1

Andore.evenst			dw Andore_events
Andore.adr				dw #c000
Andore.page				db 6
Andore.energy			db 70
Andore.speed			db 1

Trasher.evenst			dw Trasher_events
Trasher.adr				dw #c000
Trasher.page			db 6
Trasher.energy			db 90
Trasher.speed			db 2

Kitana.evenst			dw Kitana_events
Kitana.adr				dw #c000
Kitana.page				db 6
Kitana.energy			db 90
Kitana.speed			db 3

Abigal.evenst			dw Abigal_events
Abigal.adr				dw #c000
Abigal.page				db 6
Abigal.energy			db 90
Abigal.speed			db 2

Belger.evenst			dw Belger_events
Belger.adr				dw #c000
Belger.page				db 6
Belger.energy			db 90
Belger.speed			db 3

Items.evenst			dw Items_events
Items.adr				dw #f400
Items.page				db 0
Items.energy			db 1
Items.speed				db 0									

tutor_hero_events		dw tutorial_hit
						dw event_falldown_sfx
						dw event_kill
						dw tutorial_skip
						dw tutorial_jump
						dw tutorial_knee
						dw tutorial_wave
						dw tutorial_weapon_thow
						dw tutorial_superhit

tutor_bred_events		dw event_hit
						dw sfx_tutorial_fall
						dw tutorial_kill
						dw tutorial_throw
						dw sfx_tutorial_hit
game_over_text			db "GAME OVER",0	
						
;блок сохранений			
saves_begin

kempston_on				db #00 ;kempston joystick включен
music_on				db #ff ;музыка включена
sfx_on					db #ff ;звуковые эффекты включены

						IF version_type = trdos
Cody_level				db "1" ;прогресс персонажей
Cody_loc				db 0
Guy_level				db "1"
Guy_loc					db 0
Haggar_level			db "1"
Haggar_loc				db 0

saves_end
saves_filename			db "saves   C"
						ENDIF

;переменные
main_hero				db 1 ;тип главного героя
loc_pos					dw #0000 ;координаты начала отрисовки локации в символах
game_screen_clear		db #00 ;экран очищен от врагов, можно скроллировать
go_visible				db #00 ;на экране есть надпись Go!
max_enemy_onscreen		db 2 ;максимальное кол-во врагов на текущем игровом экране
skip_deltatime			db 0 ;пропустить дельтатайминг до следующего игрового цикла
location.startadr		dw 0 ;адрес начала текущей локации
current_enemy_adr		dw 0 ;текущий адрес в описателях врагов
current_music			dw 0 ;адрес инициализации текущей музыки
refresh_gamescreen		db 0 ;признак необходимости обновления экрана
pressed_keys			db 0 ;нажатые кнопки [D0-вверх,D1-вниз,D2-влево,D3-право,D4-огонь,D5-прыжок,D6-пауза,D7-выход]
clicked_keys			db 0 ;однократно нажатые кнопки [D0-вверх,D1-вниз,D2-влево,D3-право,D4-огонь,D5-прыжок,D6-пауза,D7-выход]
prev_keys				db 0 ;предыдущие нажатия кнопок
combo_active			db 0 ;активированные комбо
combo_twister			db 0,0,#10,#10,#ff ;комбо "твистер"
combo_super_right		db 0,0,#04,#08,#10,#ff ;комбо "супер-удар" вправо
combo_super_left		db 0,0,#08,#04,#10,#ff ;комбо "супер-удар" влево
combo_life				db 0,0,#02,#02,#01,#01,#10,#ff ;комбо "жизнь"
combo_weapon_right		db 0,0,#04,#04,#10,#ff ;комбо "бросок оружия" вправо
combo_weapon_left		db 0,0,#08,#08,#10,#ff ;комбо "бросок оружия" влево

location.currentadr		dw #00 ;текущий адрес в локации
location.width			db #00 ;ширина локации в тайлах
location.height			db #00 ;высота локации в тайлах
gameloop_cnt			db #00 ;счетчик игрового цикла
stek_save				dw 0 ;переменная для хранения стека
iy_save					dw 0 ;значение iy при запуске игры
salute_on				db 0 ;включение эффектов взрыва Белгера
						
;стартовая точка	
main_start				di

;загрузка сохранений
						IF version_type = trdos
						ld hl,saves_filename
						ld c,#13
						call #3d13
						ld c,#0a
						call #3d13
						inc c
						jr z,load_resources ;пропускаем загрузку, если файла ещё нет						
						ld hl,saves_begin
						xor a
						ld (#5cf9),a
						cpl
						ld c,#0e
						call #3d13  						
						ENDIF		
						
;загрузка ресурсов игры					
load_resources			ld a,(Items.page) 
						call ram_driver
						ld hl,(Items.adr)
						call load_file

						setpage font.page
						ld hl,font.adr
						call load_file
						
						setpage main_menu.page
						ld hl,main_menu.adr
						call load_file
						
						setpage main_menu_res_1.page
						ld hl,main_menu_res_1.adr
						call load_file
						
						setpage main_menu_res_2.page
						ld hl,main_menu_res_2.adr
						call load_file
						
						setpage redraw.page
						ld hl,redraw.adr
						call load_file
						
						setpage sengine.page
						ld hl,sengine.adr
						call load_file
						
						setpage ext.page ;ресурсы тренировочной комнаты
						ld hl,tileset
						call load_file
	
;загрузка архивов с героями	
						setpage enemy_set_1.page
						ld hl,hero_1.adr
						call load_file
						
						setpage enemy_set_2.page
						ld hl,hero_2.adr
						call load_file
						
;окончание загрузки основного блока
						IF version_type = tap
						call stop_tape
						ENDIF
						
						call wait_any_key
						call init_game
						
						setpage main_menu.page
						call main_menu.adr
						call clear_screen	
						
						IF version_type = tap
						call start_tape
						ENDIF
						
						setpage sengine.page
						ld hl,music_set.adr
						call load_file
						
						setpage ext.page
						ld hl,code.adr
						call load_file


						IF version_type = trdos	
						
						IF debug_mode = 0
						ld a,(main_hero)
						dec a
						add a,a
						ld e,a
						ld d,0
						ld hl,Cody_level
						add hl,de
						ld de,current_level
						ldi
						ldi
						ENDIF
						
						ld a,(current_level)
						;ld a,'6';;;;;;;;;;;;;;;;;;
						sub '1'
						add a,a
						ld b,a
						add a,a
						add a,b
						ld b,a
						ld hl,current_file_num
						add a,(hl)
						;inc a;;;;;;;;;;;;;;;;;;;;;
						ld (hl),a
						ENDIF
						
						setpage ext.page
						;jp game_win;;;;;;;;;;;;;;;;;;;;;;;
						jp game_start
						
;вызов инициализации музыкального трека и страницы sengine.page с возвратом в предыдущую страницу
ext_init_music			ld a,(current_page)
						push af
						setpage sengine.page
						call init_music		
						pop af
						call ram_driver
						ret

;вызов AY звука
;вх  - hl - адрес звукового эфекта						
sfx_ay					ld a,(current_page)
						push af
						setpage sengine.page
						call init_sfx	
						pop af
						call ram_driver
						ret
						
;вызов инициализации музыкального трека и страницы sengine.page с возвратом в предыдущую страницу
ext_mute_music			ld a,(current_page)
						push af
						setpage sengine.page
						call mute_music						
						pop af
						call ram_driver
						ret
						
ext_mute_sfx			ld a,(current_page)
						push af
						setpage sengine.page
						call mute_sfx						
						pop af
						call ram_driver
						ret						
						
;копирование блока c включением выбранной страницы с возвратом в предыдущую страницу
;вх  - hl - адрес источника
;      de - адрес приёмника
;      bc - длинна
;      a - номер страницы
ext_ldir				exa
						ld a,(current_page)
						exa
						call ram_driver
						ldir
						exa
						call ram_driver
						ret
						
;распаковка c включением выбранной страницы с возвратом в предыдущую страницу
;вх  - hl - адрес архива
;      de - адрес распаковки
;      a - номер страницы
ext_unzip				exa
						ld a,(current_page)
						push af
						exa
						call ram_driver
						call unzip
						pop af
						call ram_driver
						ret
						
;звуковые эффекты
select_sfx				incbin "../res/sfx/select.afx"					
wave_sfx				incbin "../res/sfx/wave.afx"		
restore_life_sfx		incbin "../res/sfx/restore_life.afx"	
powerup_sfx				incbin "../res/sfx/powerup.afx"	
rocket_sfx				incbin "../res/sfx/rocket.afx"	
bum_sfx					incbin "../res/sfx/bum.afx"	
			
main_end	
						savebin "../bin/main.bin", main_begin, main_end - main_begin