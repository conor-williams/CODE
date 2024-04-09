######## ######## ######### ########
#
# Moving train program
# changes: 3.2 has speed... def: 300
######## ######## ######### #########

frame .f3
pack configure .f3 -side bottom
frame .f1 
pack configure .f1 -side bottom
frame .f2
pack configure .f2 -side bottom
button .f3.but1 -text reset -command mainloop
pack .f3.but1 -side left -padx 120
button .f3.but2 -text "s:300" -command changeSpeed
pack .f3.but2 -side left -padx 120

label .f1.label1
label .f2.label1
.f1.label1 configure -text "Train1:     " 
.f2.label1 configure -text "Train2:     "
pack configure .f1.label1 -side left
pack configure .f2.label1 -side left

label .f3.lab1 -text "Speed:"
pack .f3.lab1 -side left
text .f3.t1 -width 4 -height 1
pack .f3.t1 -side left

set numlabels 31
for { set i  0 } { $i <= $numlabels } { incr i } {
	label .f1.l${i} -text " " -font bold -fg red
	pack configure .f1.l${i} -side left 

	label .f2.l${i} -text " " -font bold -fg green
	pack configure .f2.l${i} -side left 
}
set trainSpeed 300
set trainLength 1
set start1 0;
set start2 $numlabels

proc changeSpeed {} {
	global trainSpeed
	set trainSpeed  [.f3.t1 get 1.0 1.4]
}
proc getRandomStarts {} {
	global numlabels; global start1; global start2;

	set start1 [expr {int(rand() * 100) % $numlabels} ]
	set start2 [expr {int(rand() * 100) % $numlabels} ]	
}

proc blankLabels {} {
	global numlabels;

	for {set b 0} {$b <= $numlabels} {incr b} {
		.f1.l${b} configure -text " " -font bold
		.f2.l${b} configure -text " " -font bold
	}
}

proc mainloop {} {
	getRandomStarts
	global trainSpeed
	global numlabels; global start1; global start2

	set j $start1
	set k $start2
	
	blankLabels

	while { 1 == 1 } {
		.f1.l${j} configure -text " " -font bold
		.f2.l${k} configure -text " " -font bold
		#update ; #after 200; update
		if {$j < $numlabels} {incr j} else {set j 0}
		if {$k > 0} {incr k -1} else {set k $numlabels}
	
		#Train1
		.f1.l${j} configure -text "<" -font bold
		.f1.l[expr {[expr {$j + 1}] % $numlabels}] configure -text "=" -font bold
		.f1.l[expr {[expr {$j + 2}] % $numlabels}] configure -text "=" -font bold
		.f1.l[expr {[expr {$j + 3}] % $numlabels}] configure -text ">" -font bold
		.f1.l[expr {[expr {$j + 4}] % $numlabels}] configure -text " " -font bold
		#Train2
		.f2.l${k} configure -text ">" -font bold
		.f2.l[expr [expr {$k -1 + $numlabels} ] % $numlabels ] configure -text "=" -font bold
		.f2.l[expr [expr {$k -2 + $numlabels} ] % $numlabels ] configure -text "<" -font bold
		.f2.l[expr [expr {$k -3 + $numlabels} ] % $numlabels ] configure -text " " -font bold		
		update; after $trainSpeed; update
	}
}
mainloop
