;генерация процедуры вывода тайлов на виртуальный экран

						org tiles_out.adr
tilesout_b				
;						ld bc,#bf80
;						ld de,#407f
;						lua allpass
;							vscr = sj.get_label("vscreen")
;							buffer_adr = sj.get_label("loc_buffer") + 1
;							for y = 0, 9, 1 do
;								for x = 0, 15, 1 do	
;									_pc("ld hl," .. buffer_adr)									
;									_pc("ld a,(hl)")
;									_pc("and b")
;									_pc("ld (hl),a")
;									_pc("and c")
;									_pc("jp z,$+77")
;									_pc("ld sp,(" .. buffer_adr - 1 .. ")")
;									_pc("ld a,(hl)")
;									_pc("or d")
;									_pc("and e")
;									_pc("ld (hl),a");
;									buffer_adr = buffer_adr + 2
;									for i = 0, 15, 1 do
;										_pc("pop hl")
;										_pc("ld ("  .. vscr + y * 640 + x * 2 + i * 40 .. "),hl")
;									end							
;									_pc("ld sp,ix")
;								end
;								buffer_adr = buffer_adr + 2
;							end
;							_pc("ld sp,ix")
;						endlua

						ld a,#d5
						ld (interrupt_selector),a ;переключение прерываний на "pop de"

						ld hl,loc_buffer+1
						ld de,vscreen
						ld bc,#100a

tilesout_loop			push bc,de,hl
						res 6,(hl)
						bit 7,(hl)
						jr z,tilesout_end
						ld a,(hl)
						set 6,(hl)
						res 7,(hl)
						dec l
						ld l,(hl)
						ld h,a
						
						ld (tilesout_sp+1),sp
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
tilesout_sp				ld sp,0						

tilesout_end			pop hl,de,bc
						inc e,de,hl,l
						djnz tilesout_loop
						ld b,#10
						inc hl,l
						push hl
						ld hl,vscreen.width*16-32
						add hl,de
						ex de,hl
						pop hl
						dec c
						jp nz,tilesout_loop

						xor a
						ld (interrupt_selector),a ;переключение прерываний на "pop hl"

						ret
tilesout_e				
						savebin "../bin/tilesout.bin", tilesout_b, tilesout_e - tilesout_b