;генерация процедур вывода виртуального экрана на реальный частично
						org redraw.adr
redraw_b				ld (stek_save),sp
						ld b,#c0
						lua allpass
							screen = sj.get_label("screen")
							vscreen = sj.get_label("vscreen")
							buffer = sj.get_label("loc_buffer") + 1
							for y = 1, 10, 1 do
								for x = 0, 15, 1 do
									_pc("ld a,(" .. buffer .. ")")
									_pc("and b")
									_pc("jr z,$+98")
									for i = 0, 1, 1 do
										for j = 0, 7, 1 do
											_pc("ld hl,(" .. vscreen + (y - 1) * 640 + x * 2 + (i * 8 + j) * 40 .. ")")
											_pc("ld (" .. screen + x * 2 + j * 256 + i * 32 + (math.fmod(y, 4) * 64) + (math.floor(y / 4) * 2048) .. "),hl")
										end
									end
									buffer = buffer + 2
								end
								buffer = buffer + 2
							end
						endlua
						ld sp,(stek_save)
						ret
redraw_e											
						savebin "../bin/redraw.bin", redraw_b, redraw_e - redraw_b
						
