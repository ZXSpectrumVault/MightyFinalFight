;макросы работы со статусом объекта

;проверить состояние объекта (IX)
						MACRO get_status property
							IF property < 8
								bit property,(ix+object.status)					
							ELSE
								bit property-8,(ix+object.status+1)			
							ENDIF
						ENDM
						
;проверить состояние объекта (IY)
						MACRO get_enemy_status property
							IF property < 8
								bit property,(iy+object.status)					
							ELSE
								bit property-8,(iy+object.status+1)			
							ENDIF
						ENDM
						
;включить свойство
						MACRO set_status property
							IF property < 8
								set property,(ix+object.status)					
							ELSE
								set property-8,(ix+object.status+1)			
							ENDIF
						ENDM
						
;включить свойство (IY)
						MACRO set_enemy_status property
							IF property < 8
								set property,(iy+object.status)					
							ELSE
								set property-8,(iy+object.status+1)			
							ENDIF
						ENDM
;выключить свойство
						MACRO res_status property
							IF property < 8
								res property,(ix+object.status)					
							ELSE
								res property-8,(ix+object.status+1)			
							ENDIF
						ENDM
;выключить свойство (IY)
						MACRO res_enemy_status property
							IF property < 8
								res property,(iy+object.status)					
							ELSE
								res property-8,(iy+object.status+1)			
							ENDIF
						ENDM
;включение страницы памяти с текущей локацией						
						MACRO set_loc_page
							ld a,(prepare_locbuffer_page+1)
							call ram_driver					
						ENDM	
					
;включение страницы page_num					
						MACRO setpage page_num
							IF page_num = 0
								xor a					
							ELSE
								ld a, page_num					
							ENDIF
							call ram_driver						
						ENDM