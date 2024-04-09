#
##
## Guess the speed of the moving minuses
##
#############
#
#
#

set framesY 10
set labelsX 20

for {set i 0} {$i <= $framesY} {incr i} {
	frame .f$i
	pack .f$i -side bottom
}

for {set i 0} {$i <= $framesY} {incr i} {
	for {set j 0} {$j <= $labelsX} {incr j} {
		label .f$i.l$j
		pack .f$i.l$j -side left
	}
}

set RXPOS 0
set RYPOS 0

proc startGameProc {} {
        global framesY; global labelsX
        global RXPOS; global RYPOS;
        set RXPOS [expr { int(rand() * 100) % ($labelsX + 1) } ]
        set RYPOS [expr { int(rand() * 100) % ($framesY + 1) } ]
}

frame .f[expr ($framesY + 1)]
pack .f[expr ($framesY + 1)]
text .t[expr ($framesY + 1)] -width 5 -height 1
pack .t[expr ($framesY + 1)]

set times 0
set randomTime 0
while { 1 == 1 } {
	  incr times
        global framesY; global labelsX; global RXPOS; global RYPOS

        .f${RYPOS}.l${RXPOS} configure -text " " -font bold -fg grey
        startGameProc; update
        .f${RYPOS}.l${RXPOS} configure -text "-" -font bold -fg red

        set xpos [expr (-1 * ([winfo rootx .] - [winfo pointerx .])  / (116 / $labelsX)) ]
        set ypos [expr (-1 * ([winfo rooty .] - [winfo pointery .]) / 23) ]
        
	  #.f${RYPOS}.l${RXPOS} configure -text "$xpos $RXPOS $ypos $RYPOS" -font bold -fg red

        #tk_messageBox -message "$xpos $RXPOS"
        if { ($xpos == $RXPOS ) && ($ypos == $RYPOS) } {
                tk_messageBox -message "gotchu"
        }
        update
	  	tk_messageBox -message "what.s the speed"
	  	set times 0
		set randomTime [expr { int(rand() * 100) % $labelsX}]
		#[expr ( int(rand() * 100) % ($labelsX + 1) ) ]
		set secondTimes [expr (500 * $randomTime)]
        update ; after $randomTime; update
	  
	  tk_messageBox -message "guess the time... (into the text area)"
	  set loopTime 10
	  for {set k 0} {$k < $loopTime} {incr k} {
	  	after 500;
		update
	  }
	  set value [.t[expr ($framesY + 1)] get 1.0 1.4]

		if {$value == $randomTime} {tk_messageBox -message "well done..."} else {
	  		tk_messageBox -message "$value ? no $randomTime"
		}	


}

