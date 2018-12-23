;+++++++++++++++ SOUND ENGINE ++++++++++++
;звуковой движок на основе
;vortex tracker ii v1.0 pt3 player
;for zx spectrum
;(c)2004,2007 s.v.bulba
;<vorobey@mail.khstu.ru>
;http://bulba.untergrund.net
;(http://bulba.at.kz)

						org sengine.adr

;--------------- константы ---------------
tona=0
tonb=2
tonc=4
noise=6
mixer=7
ampla=8
amplb=9
amplc=10
env=11
envtp=13
psinor=0
psinsm=1
cramsl=2
crnssl=3
crensl=4
tslcnt=5
crtnsl=6
tnacc=8
conoff=10
onoffd=11
offond=12
ornptr=13
samptr=15
nntskp=17
note=18
sltont=19
env_en=20
flags=21
tnsldl=22
tslstp=23
tndelt=25
ntskcn=27
volume=28
chp=29
;-------------------- длинна модуля 2163 байта ---------------------

;set bit0 to 1, if you want to play without looping
;bit7 is set each time, when loop point is passed
setup   				db 0
crpsptr 				dw 0
music_ready				db 0 ;трек готов к воспроизведению

;identifier
						db "=vtii pt3 player r.7="

checklp         		ld hl,setup
						set 7,(hl)
						bit 0,(hl)
						ret z
						pop hl
						ld hl,delycnt
						inc (hl)
						ld hl,chana+ntskcn
						inc (hl)
mute_music
						xor a
						ld h,a
						ld l,a
						ld (music_ready),a
						ld (ayregs+ampla),a
						ld (ayregs+amplb),hl
						jp ay_out


;инициализация музыки
init_music				push hl
						call mute_music
						pop hl
						ld (current_music),hl
						ld (modaddr),hl
						ld (mdaddr2),hl
						push hl
						ld de,100
						add hl,de
						ld a,(hl)
						ld (delay),a
						push hl
						pop ix
						add hl,de
						ld (crpsptr),hl
						ld e,(ix+102-100)
						add hl,de
						inc hl
						ld (lposptr),hl
						pop de
						ld l,(ix+103-100)
						ld h,(ix+104-100)
						add hl,de
						ld (patsptr),hl
						ld hl,169
						add hl,de
						ld (ornptrs),hl
						ld hl,105
						add hl,de
						ld (samptrs),hl
						ld hl,setup
						res 7,(hl)

;note table data depacker
						ld de,t_pack
						ld bc,t1_+(2*49)-1
tp_0            		ld a,(de)
						inc de
						cp 15*2
						jr nc,tp_1
						ld h,a
						ld a,(de)
						ld l,a
						inc de
						jr tp_2

tp_1		            push de
						ld d,0
						ld e,a
						add hl,de
						add hl,de
						pop de
tp_2            		ld a,h
						ld (bc),a
						dec bc
						ld a,l
						ld (bc),a
						dec bc
						sub #f0
						jr nz,tp_0

						ld hl,vars
						ld (hl),a
						ld de,vars+1
						ld bc,var0end-vars-1
						ldir 
						inc a
						ld (delycnt),a
						ld hl,#f001 ;h - volume, l - ntskcn
						ld (chana+ntskcn),hl
						ld (chanb+ntskcn),hl
						ld (chanc+ntskcn),hl

						ld hl,emptysamorn
						ld (adinpta),hl ;ptr to zero
						ld (chana+ornptr),hl ;ornament 0 is "0,1,0"
						ld (chanb+ornptr),hl ;in all versions from
						ld (chanc+ornptr),hl ;3.xx to 3.6x and vtii

						ld (chana+samptr),hl ;s1 there is no default
						ld (chanb+samptr),hl ;s2 sample in pt3, so, you
						ld (chanc+samptr),hl ;s3 can comment s1,2,3; see
											;also emptysamorn comment
						ld a,(ix+13-100) ;extract version number
						sub #30
						jr c,l20
						cp 10
						jr c,l21
l20			            ld a,6
l21         		    ld (version),a
						push af
						cp 4
						ld a,(ix+99-100) ;tone table number
						rla 
						and 7

;notetablecreator (c) ivan roshin
;a - notetablenumber*2+versionfornotetable
;(xx1b - 3.xx..3.4r, xx0b - 3.4x..3.6x..vtii1.0)

						ld hl,nt_data
						push de
						ld d,b
						add a,a
						ld e,a
						add hl,de
						ld e,(hl)
						inc hl
						srl e
						sbc a,a
						and #a7 ;#00 (nop) or #a7 (and a)
						ld (l3),a
						ex de,hl
						pop bc ;bc=t1_
						add hl,bc
						ld a,(de)
						add a,low t_
						ld c,a
						adc a,high t_
						sub c
						ld b,a
						push bc
						ld de,nt_
						push de

						ld b,12
l1              		push bc
						ld c,(hl)
						inc hl
						push hl
						ld b,(hl)

						push de
						ex de,hl
						ld de,23
						ld hx,8

l2              		srl b
						rr c
l3              		db #19 ;and a or nop
						ld a,c
						adc a,d ;=adc 0
						ld (hl),a
						inc hl
						ld a,b
						adc a,d
						ld (hl),a
						add hl,de
						dec hx
						jr nz,l2

						pop de
						inc de
						inc de
						pop hl
						inc hl
						pop bc
						djnz l1

						pop hl
						pop de

						ld a,e
						cp low tcold_1
						jr nz,corr_1
						ld a,#fd
						ld (nt_+#2e),a

corr_1      		    ld a,(de)
						and a
						jr z,tc_exit
						rra 
						push af
						add a,a
						ld c,a
						add hl,bc
						pop af
						jr nc,corr_2
						dec (hl)
						dec (hl)
corr_2          		inc (hl)
						and a
						sbc hl,bc
						inc de
						jr corr_1

tc_exit
						pop af

;voltablecreator (c) ivan roshin
;a - versionforvolumetable (0..4 - 3.xx..3.4x;
                           ;5.. - 3.5x..3.6x..vtii1.0)

						cp 5
						ld hl,#11
						ld d,h
						ld e,h
						ld a,#17
						jr nc,m1
						dec l
						ld e,l
						xor a
m1              		ld (m2),a

						ld ix,vt_+16
						ld c,#10

initv2          		push hl

						add hl,de
						ex de,hl
						sbc hl,hl

initv1          		ld a,l
m2              		db #7d
						ld a,h
						adc a,0
						ld (ix),a
						inc ix
						add hl,de
						inc c
						ld a,c
						and 15
						jr nz,initv1

						pop hl
						ld a,e
						cp #77
						jr nz,m3
						inc e
m3              		ld a,c
						and a
						jr nz,initv2

;инициализирована
						ld a,#ff
						ld (music_ready),a
						ei
init_musend     		ret 

;pattern decoder
pd_orsm         		ld (ix-12+env_en),0
						call setorn
						ld a,(bc)
						inc bc
						rrca 

pd_sam          		add a,a
pd_sam_         		ld e,a
						ld d,0
samptrs=$+1
						ld hl,#2121
						add hl,de
						ld e,(hl)
						inc hl
						ld d,(hl)
modaddr=$+1
						ld hl,#2121
						add hl,de
						ld (ix-12+samptr),l
						ld (ix-12+samptr+1),h
						jr pd_loop

pd_vol          		rlca 
						rlca 
						rlca 
						rlca 
						ld (ix-12+volume),a
						jr pd_lp2

pd_eoff         		ld (ix-12+env_en),a
						ld (ix-12+psinor),a
						jr pd_lp2

pd_sore         		dec a
						jr nz,pd_env
						ld a,(bc)
						inc bc
						ld (ix-12+nntskp),a
						jr pd_lp2

pd_env          		call setenv
						jr pd_lp2

pd_orn          		call setorn
						jr pd_loop

pd_esam         		ld (ix-12+env_en),a
						ld (ix-12+psinor),a
						call nz,setenv
						ld a,(bc)
						inc bc
						jr pd_sam_

ptdecod         		ld a,(ix-12+note)
						ld (prnote+1),a
						ld l,(ix-12+crtnsl)
						ld h,(ix-12+crtnsl+1)
						ld (prslide+1),hl

pd_loop         		ld de,#2010
pd_lp2          		ld a,(bc)
						inc bc
						add a,e
						jr c,pd_orsm
						add a,d
						jr z,pd_fin
						jr c,pd_sam
						add a,e
						jr z,pd_rel
						jr c,pd_vol
						add a,e
						jr z,pd_eoff
						jr c,pd_sore
						add a,96
						jr c,pd_note
						add a,e
						jr c,pd_orn
						add a,d
						jr c,pd_nois
						add a,e
						jr c,pd_esam
						add a,a
						ld e,a
						ld hl,spccoms-#20e0 ;+#df20
						add hl,de
						ld e,(hl)
						inc hl
						ld d,(hl)
						push de
						jr pd_loop

pd_nois         		ld (ns_base),a
						jr pd_lp2

pd_rel          		res 0,(ix-12+flags)
						jr pd_res

pd_note         		ld (ix-12+note),a
						set 0,(ix-12+flags)
						xor a

pd_res          		ld (pdsp_+1),sp
						ld sp,ix
						ld h,a
						ld l,a
						push hl
						push hl
						push hl
						push hl
						push hl
						push hl
pdsp_           		ld sp,#3131

pd_fin          		ld a,(ix-12+nntskp)
						ld (ix-12+ntskcn),a
						ret 

c_portm         		res 2,(ix-12+flags)
						ld a,(bc)
						inc bc
;skip precalculated tone delta (because
;cannot be right after pt3 compilation)
						inc bc
						inc bc
						ld (ix-12+tnsldl),a
						ld (ix-12+tslcnt),a
						ld de,nt_
						ld a,(ix-12+note)
						ld (ix-12+sltont),a
						add a,a
						ld l,a
						ld h,0
						add hl,de
						ld a,(hl)
						inc hl
						ld h,(hl)
						ld l,a
						push hl
prnote          		ld a,#3e
						ld (ix-12+note),a
						add a,a
						ld l,a
						ld h,0
						add hl,de
						ld e,(hl)
						inc hl
						ld d,(hl)
						pop hl
						sbc hl,de
						ld (ix-12+tndelt),l
						ld (ix-12+tndelt+1),h
						ld e,(ix-12+crtnsl)
						ld d,(ix-12+crtnsl+1)
version=$+1
						ld a,#3e
						cp 6
						jr c,oldprtm ;old 3xxx for pt v3.5-
prslide         		ld de,#1111
						ld (ix-12+crtnsl),e
						ld (ix-12+crtnsl+1),d
oldprtm         		ld a,(bc) ;signed tone step
						inc bc
						exa 
						ld a,(bc)
						inc bc
						and a
						jr z,nosig
						ex de,hl
nosig           		sbc hl,de
						jp p,set_stp
						cpl 
						exa 
						neg 
						exa 
set_stp         		ld (ix-12+tslstp+1),a
						exa 
						ld (ix-12+tslstp),a
						ld (ix-12+conoff),0
						ret 

c_gliss         		set 2,(ix-12+flags)
						ld a,(bc)
						inc bc
						ld (ix-12+tnsldl),a
						and a
						jr nz,gl36
						ld a,(version) ;alco pt3.7+
						cp 7
						sbc a,a
						inc a
gl36            		ld (ix-12+tslcnt),a
						ld a,(bc)
						inc bc
						exa 
						ld a,(bc)
						inc bc
						jr set_stp

c_smpos         		ld a,(bc)
						inc bc
						ld (ix-12+psinsm),a
						ret 

c_orpos        			ld a,(bc)
						inc bc
						ld (ix-12+psinor),a
						ret 

c_vibrt         		ld a,(bc)
						inc bc
						ld (ix-12+onoffd),a
						ld (ix-12+conoff),a
						ld a,(bc)
						inc bc
						ld (ix-12+offond),a
						xor a
						ld (ix-12+tslcnt),a
						ld (ix-12+crtnsl),a
						ld (ix-12+crtnsl+1),a
						ret 

c_engls         		ld a,(bc)
						inc bc
						ld (env_del),a
						ld (curedel),a
						ld a,(bc)
						inc bc
						ld l,a
						ld a,(bc)
						inc bc
						ld h,a
						ld (esldadd),hl
						ret 

c_delay         		ld a,(bc)
						inc bc
						ld (delay),a
						ret 

setenv          		ld (ix-12+env_en),e
						ld (ayregs+envtp),a
						ld a,(bc)
						inc bc
						ld h,a
						ld a,(bc)
						inc bc
						ld l,a
						ld (envbase),hl
						xor a
						ld (ix-12+psinor),a
						ld (curedel),a
						ld h,a
						ld l,a
						ld (curesld),hl
c_nop           		ret 

setorn          		add a,a
						ld e,a
						ld d,0
						ld (ix-12+psinor),d
ornptrs=$+1
						ld hl,#2121
						add hl,de
						ld e,(hl)
						inc hl
						ld d,(hl)
mdaddr2=$+1
						ld hl,#2121
						add hl,de
						ld (ix-12+ornptr),l
						ld (ix-12+ornptr+1),h
						ret 

;all 16 addresses to protect from broken pt3 modules
spccoms         		dw c_nop
						dw c_gliss
						dw c_portm
						dw c_smpos
						dw c_orpos
						dw c_vibrt
						dw c_nop
						dw c_nop
						dw c_engls
						dw c_delay
						dw c_nop
						dw c_nop
						dw c_nop
						dw c_nop
						dw c_nop
						dw c_nop

chregs          		xor a
						ld (ampl),a
						bit 0,(ix+flags)
						push hl
						jp z,ch_exit
						ld (csp_+1),sp
						ld l,(ix+ornptr)
						ld h,(ix+ornptr+1)
						ld sp,hl
						pop de
						ld h,a
						ld a,(ix+psinor)
						ld l,a
						add hl,sp
						inc a
						cp d
						jr c,ch_orps
						ld a,e
ch_orps         		ld (ix+psinor),a
						ld a,(ix+note)
						add a,(hl)
						jp p,ch_ntp
						xor a
ch_ntp          		cp 96
						jr c,ch_nok
						ld a,95
ch_nok          		add a,a
						exa 
						ld l,(ix+samptr)
						ld h,(ix+samptr+1)
						ld sp,hl
						pop de
						ld h,0
						ld a,(ix+psinsm)
						ld b,a
						add a,a
						add a,a
						ld l,a
						add hl,sp
						ld sp,hl
						ld a,b
						inc a
						cp d
						jr c,ch_smps
						ld a,e
ch_smps         		ld (ix+psinsm),a
						pop bc
						pop hl
						ld e,(ix+tnacc)
						ld d,(ix+tnacc+1)
						add hl,de
						bit 6,b
						jr z,ch_noac
						ld (ix+tnacc),l
						ld (ix+tnacc+1),h
ch_noac         		ex de,hl
						exa 
						ld l,a
						ld h,0
						ld sp,nt_
						add hl,sp
						ld sp,hl
						pop hl
						add hl,de
						ld e,(ix+crtnsl)
						ld d,(ix+crtnsl+1)
						add hl,de
csp_            		ld sp,#3131
						ex (sp),hl
						xor a
						or (ix+tslcnt)
						jr z,ch_amp
						dec (ix+tslcnt)
						jr nz,ch_amp
						ld a,(ix+tnsldl)
						ld (ix+tslcnt),a
						ld l,(ix+tslstp)
						ld h,(ix+tslstp+1)
						ld a,h
						add hl,de
						ld (ix+crtnsl),l
						ld (ix+crtnsl+1),h
						bit 2,(ix+flags)
						jr nz,ch_amp
						ld e,(ix+tndelt)
						ld d,(ix+tndelt+1)
						and a
						jr z,ch_stpp
						ex de,hl
ch_stpp         		sbc hl,de
						jp m,ch_amp
						ld a,(ix+sltont)
						ld (ix+note),a
						xor a
						ld (ix+tslcnt),a
						ld (ix+crtnsl),a
						ld (ix+crtnsl+1),a
ch_amp          		ld a,(ix+cramsl)
						bit 7,c
						jr z,ch_noam
						bit 6,c
						jr z,ch_amin
						cp 15
						jr z,ch_noam
						inc a
						jr ch_svam
ch_amin         		cp -15
						jr z,ch_noam
						dec a
ch_svam         		ld (ix+cramsl),a
ch_noam         		ld l,a
						ld a,b
						and 15
						add a,l
						jp p,ch_apos
						xor a
ch_apos         		cp 16
						jr c,ch_vol
						ld a,15
ch_vol          		or (ix+volume)
						ld l,a
						ld h,0
						ld de,vt_
						add hl,de
						ld a,(hl)
ch_env          		bit 0,c
						jr nz,ch_noen
						or (ix+env_en)
ch_noen         		ld (ampl),a
						bit 7,b
						ld a,c
						jr z,no_ensl
						rla 
						rla 
						sra a
						sra a
						sra a
						add a,(ix+crensl) ;see comment below
						bit 5,b
						jr z,no_enac
						ld (ix+crensl),a
no_enac         		ld hl,addtoen
						add a,(hl) ;bug in pt3 - need word here.
;fix it in next version?
						ld (hl),a
						jr ch_mix
no_ensl         		rra 
						add a,(ix+crnssl)
						ld (addtons),a
						bit 5,b
						jr z,ch_mix
						ld (ix+crnssl),a
ch_mix          		ld a,b
						rra 
						and #48
ch_exit         		ld hl,ayregs+mixer
						or (hl)
						rrca 
						ld (hl),a
						pop hl
						xor a
						or (ix+conoff)
						ret z
						dec (ix+conoff)
						ret nz
						xor (ix+flags)
						ld (ix+flags),a
						rra 
						ld a,(ix+onoffd)
						jr c,ch_ondl
						ld a,(ix+offond)
ch_ondl         		ld (ix+conoff),a
						ret 

;проигрыватель музыки
play_music      		xor a
						ld (addtoen),a
						ld (ayregs+mixer),a
						dec a
						ld (ayregs+envtp),a
						ld hl,delycnt
						dec (hl)
						jr nz,pl2
						ld hl,chana+ntskcn
						dec (hl)
						jr nz,pl1b
adinpta=$+1
						ld bc,#0101
						ld a,(bc)
						and a
						jr nz,pl1a
						ld d,a
						ld (ns_base),a
						ld hl,(crpsptr)
						inc hl
						ld a,(hl)
						inc a
						jr nz,plnlp
						call checklp
lposptr=$+1
						ld hl,#2121
						ld a,(hl)
						inc a
plnlp           		ld (crpsptr),hl
						dec a
						add a,a
						ld e,a
						rl d
patsptr=$+1
						ld hl,#2121
						add hl,de
						ld de,(modaddr)
						ld (psp_+1),sp
						ld sp,hl
						pop hl
						add hl,de
						ld b,h
						ld c,l
						pop hl
						add hl,de
						ld (adinptb),hl
						pop hl
						add hl,de
						ld (adinptc),hl
psp_            		ld sp,#3131
pl1a            		ld ix,chana+12
						call ptdecod
						ld (adinpta),bc

pl1b            		ld hl,chanb+ntskcn
						dec (hl)
						jr nz,pl1c
						ld ix,chanb+12
adinptb=$+1
						ld bc,#0101
						call ptdecod
						ld (adinptb),bc

pl1c            		ld hl,chanc+ntskcn
						dec (hl)
						jr nz,pl1d
						ld ix,chanc+12
adinptc=$+1
						ld bc,#0101
						call ptdecod
						ld (adinptc),bc

delay=$+1
pl1d            		ld a,#3e
						ld (delycnt),a

pl2             		ld ix,chana
						ld hl,(ayregs+tona)
						call chregs
						ld (ayregs+tona),hl
						ld a,(ampl)
						ld (ayregs+ampla),a
						ld ix,chanb
						ld hl,(ayregs+tonb)
						call chregs
						ld (ayregs+tonb),hl
						ld a,(ampl)
						ld (ayregs+amplb),a
						ld ix,chanc
						ld hl,(ayregs+tonc)
						call chregs
		;               ld a,(ampl) ;ampl = ayregs+amplc
		;               ld (ayregs+amplc),a
						ld (ayregs+tonc),hl

						ld hl,(ns_base_addtons)
						ld a,h
						add a,l
						ld (ayregs+noise),a

addtoen=$+1
						ld a,#3e
						ld e,a
						add a,a
						sbc a,a
						ld d,a
						ld hl,(envbase)
						add hl,de
						ld de,(curesld)
						add hl,de
						ld (ayregs+env),hl

						xor a
						ld hl,curedel
						or (hl)
						ret z
						dec (hl)
						ret nz
env_del=$+1
						ld a,#3e
						ld (hl),a
esldadd=$+1
						ld hl,#2121
						add hl,de
						ld (curesld),hl
						ret 


;вывод на ay из буфера
ay_out

;вывод переменных в порты ay
ay_out1        		 	xor a
						ld de,#ffbf
						ld bc,#fffd
						ld hl,ayregs
ay_out2         		out (c),a
						ld b,e
						outi 
						ld b,d
						inc a
						cp 13
						jr nz,ay_out2
						out (c),a
						ld a,(hl)
						and a
						ret m
						ld b,e
						out (c),a
						ret 

;stupid alasm limitations
nt_data 				db 50*2 ;(t_new_0-t1_)*2
						db tcnew_0-t_
						db 50*2+1 ;(t_old_0-t1_)*2+1
						db tcold_0-t_
						db 0*2+1 ;(t_new_1-t1_)*2+1
						db tcnew_1-t_
						db 0*2+1 ;(t_old_1-t1_)*2+1
						db tcold_1-t_
						db 74*2;(t_new_2-t1_)*2
						db tcnew_2-t_
						db 24*2 ;(t_old_2-t1_)*2
						db tcold_2-t_
						db 48*2 ;(t_new_3-t1_)*2
						db tcnew_3-t_
						db 48*2 ;(t_old_3-t1_)*2
						db tcold_3-t_

t_

tcold_0 				db #00+1,#04+1,#08+1,#0a+1,#0c+1,#0e+1,#12+1,#14+1
						db #18+1,#24+1,#3c+1,0
tcold_1 				db #5c+1,0
tcold_2 				db #30+1,#36+1,#4c+1,#52+1,#5e+1,#70+1,#82,#8c,#9c
						db #9e,#a0,#a6,#a8,#aa,#ac,#ae,#ae,0
tcnew_3 				db #56+1
tcold_3 				db #1e+1,#22+1,#24+1,#28+1,#2c+1,#2e+1,#32+1,#be+1,0
tcnew_0 				db #1c+1,#20+1,#22+1,#26+1,#2a+1,#2c+1,#30+1,#54+1
						db #bc+1,#be+1,0
tcnew_1=tcold_1
tcnew_2 				db #1a+1,#20+1,#24+1,#28+1,#2a+1,#3a+1,#4c+1,#5e+1
						db #ba+1,#bc+1,#be+1,0

emptysamorn=$-1
						db 1,0,#90 ;delete #90 if you don't need default sample

;first 12 values of tone tables (packed)

t_pack  				db #0d,#d8 ;#06ec*2/256,#06ec*2
						db #0755-#06ec
						db #07c5-#0755
						db #083b-#07c5
						db #08b8-#083b
						db #093d-#08b8
						db #09ca-#093d
						db #0a5f-#09ca
						db #0afc-#0a5f
						db #0ba4-#0afc
						db #0c55-#0ba4
						db #0d10-#0c55
						db #0c,#da ;#066d*2/256,#066d*2
						db #06cf-#066d
						db #0737-#06cf
						db #07a4-#0737
						db #0819-#07a4
						db #0894-#0819
						db #0917-#0894
						db #09a1-#0917
						db #0a33-#09a1
						db #0acf-#0a33
						db #0b73-#0acf
						db #0c22-#0b73
						db #0cda-#0c22
						db #0e,#08 ;#0704*2/256,#0704*2
						db #076e-#0704
						db #07e0-#076e
						db #0858-#07e0
						db #08d6-#0858
						db #095c-#08d6
						db #09ec-#095c
						db #0a82-#09ec
						db #0b22-#0a82
						db #0bcc-#0b22
						db #0c80-#0bcc
						db #0d3e-#0c80
						db #0f,#c0 ;#07e0*2/256,#07e0*2
						db #0858-#07e0
						db #08e0-#0858
						db #0960-#08e0
						db #09f0-#0960
						db #0a88-#09f0
						db #0b28-#0a88
						db #0bd8-#0b28
						db #0c80-#0bd8
						db #0d60-#0c80
						db #0e10-#0d60
						db #0ef8-#0e10

;vars from here can be stripped
;you can move vars to any other address

vars

chana   				ds chp
chanb   				ds chp
chanc   				ds chp

;globalvars
delycnt 				db 0
curesld 				dw 0
curedel 				db 0
ns_base_addtons
ns_base 				db 0
addtons 				db 0
ayregs
vt_     				ds 256 ;createdvolumetableaddress
envbase=vt_+14
t1_=vt_+16 ;tone tables data depacked here
t_old_1=t1_
t_old_2=t_old_1+24
t_old_3=t_old_2+24
t_old_0=t_old_3+2
t_new_0=t_old_0
t_new_1=t_old_1
t_new_2=t_new_0+24
t_new_3=t_old_3
nt_     ds 192 ;creatednotetableaddress
;local var
ampl=ayregs+amplc
var0end=vt_+16 ;init zeroes from vars to var0end-1
varsend=$








;инициализация эффектов
;вх  - hl - адрес эффекта
init_sfx        		push hl
						call mute_sfx
						pop hl
						ld (sfx_pos),hl
						xor a
						ld (sfx_end),a
						ret 

;проигрыватель эффекта
play_sfx        		ld a,(sfx_end)
						or a
						ret nz

						ld a,(music_on)
						or a
						jr nz,play_sfz_tone
						cpl
						ld (ayregs+mixer),a				
						
play_sfz_tone			ld hl,(sfx_pos) ;tone
						ld b,(hl)
						inc hl
						
						bit 5,b 
						jr z,play_sfx_noize
						ld a,(hl)
						inc hl
						ld c,(hl)
						inc hl
						bit 4,b
						jr nz,play_sfx_noize				
						ld (ayregs+tonb),a
						ld a,c
						ld (ayregs+tonb+1),a
						ld a,(ayregs+mixer)
						and %11111101
						ld (ayregs+mixer),a
					
play_sfx_noize			bit 6,b ;noize
						jr z,play_sfx_volume
						ld a,(hl)
						inc hl
						cp #20
						jr nc,mute_sfx
						bit 7,b
						jr nz,play_sfx_volume				
						ld (ayregs+noise),a
						ld a,(ayregs+mixer)
						and %11101111
						ld (ayregs+mixer),a

play_sfx_volume			ld a,b ;volume
						and #0f
						ld (ayregs+amplb),a
						ld (sfx_pos),hl
						ret
						
;выключение эффекта						
mute_sfx        		xor a
						ld (ayregs+amplb),a
						ld (ayregs+tonb),a
						ld (ayregs+tonb+1),a
						ld (ayregs+noise),a
						ld (ayregs+mixer),a
						inc a
						ld (sfx_end),a
						ret
sfx_pos         		dw 0
sfx_end         		db #ff
						savebin "../bin/sengine.bin", sengine.adr, $ - sengine.adr
