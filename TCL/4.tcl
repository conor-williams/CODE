######## ######## ######### ########
#
# Random at finder
#
######## ######## ######### #########

frame .fR1
pack configure .fR1 -side bottom

set numframesY 6
set numlabelsX 10

set RXPOS 0
set RYPOS 0

proc startGameProc {} {
	global numframesY; global numlabelsX
	global RXPOS; global RYPOS;
	set RXPOS [expr { int(rand() * 100) % ($numlabelsX + 1) } ]
	set RYPOS [expr { int(rand() * 100) % ($numframesY + 1) } ]	
}

for { set i $numframesY } { $i >= 0 } { incr i -1 } {
	frame .f${i}
	pack configure .f${i} -side bottom
}

for { set i 0 } { $i <= $numframesY } { incr i } {	
	for { set j 0 } { $j <= $numlabelsX } { incr j } {
		label .f${i}.l${j} -text " " -font bold -fg red
		pack configure .f${i}.l${j} -side left
	}
}

while { 1 == 1 } {	
	global numframesY; global numlabelsX; global RXPOS; global RYPOS

	.f${RYPOS}.l${RXPOS} configure -text " " -font bold -fg grey
	startGameProc; update
	.f${RYPOS}.l${RXPOS} configure -text "@" -font bold -fg red
	update; after 2000
	set xpos [expr (-1 * ([winfo rootx .] - [winfo pointerx .])  / (116 / $numlabelsX)) ]
	set ypos [expr (-1 * ([winfo rooty .] - [winfo pointery .]) / 23) ]
	#.f${RYPOS}.l${RXPOS} configure -text "$xpos $RXPOS $ypos $RYPOS" -font bold -fg red
	update ; after 2000; update
	#tk_messageBox -message "$xpos $RXPOS"
	if { ($xpos == $RXPOS ) && ($ypos == 
		$RYPOS) } {
		tk_messageBox -message "gotchu"
	}
	update
}