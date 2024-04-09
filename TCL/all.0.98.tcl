#!/usr/bin/wish
# steak knife... 0.99 cwinc 2202 Des 10 0.99 all.tcl

wm positionfrom . user

frame .f1; 
button .f1.1 -text 1 -command {catch {c1p;};catch {wm state .t1 withdraw;}; catch {wm state .t1 normal;};}
button .f1.1on -text 1on -command {catch {c1p;}; catch {wm state .t1 withdraw;}; catch {wm state .t1 normal};}
button .f1.1off -text 1off -command {catch {wm state .t1 withdraw;};} 
pack .f1; pack .f1.1 -side left;pack .f1.1on -side right;pack .f1.1off -side right

frame .f2; 
button .f2.2 -text 2 -command {catch {c2p};catch {wm state .t2 withdraw;}; catch {wm state .t2 normal;};}
button .f2.2on -text 2on -command {catch {c2p};catch {wm state .t2 withdraw};catch {wm state .t2 normal;};}
button .f2.2off -text 2off -command {catch {wm state .t2 withdraw;};} 
pack .f2; pack .f2.2 -side right;pack .f2.2on -side right;pack .f2.2off -side right

frame .f3; 
button .f3.3 -text 3 -command {catch {c3p};catch {wm state .t3 withdraw;}; catch {wm state .t3 normal;};}
button .f3.3on -text 3on -command {catch {c3p};catch {wm state .t3 withdraw;}; catch {wm state .t3 normal;};}
button .f3.3off -text 3off -command {catch {wm state .t3 withdraw;};} 
pack .f3; pack .f3.3 -side left;pack .f3.3on -side right;pack .f3.3off -side right

#
#1
#aargh dont press it else crash r.s.
proc c1p {} {
		toplevel .t1
		wm geometry .t1 239x121
		button .t1.button20
		update
		.t1.button20 configure -activebackground {#cd1ad9} \
		  -font {Helvetica 20} -foreground {#00fe12} \
		  -text {n1c} -width {800} -height 900 -command changeit
		pack configure .t1.button20;
		update
		proc changeit {} {set counter 0; while {1 == 1} {
			incr counter; puts -nonewline $counter 
			if {$counter % 16 == 0} {puts "\n"}
			if {$counter % 4 == 0} {flush stdout}
			.t1.button20 configure -width 30 -height 40 -cursor clock
				wm state .t1 normal
			update;
	  for {set j 0; j < 4; incr j 1} { after 1000; update }
			.t1.button20 configure -width 50 -height 49 -cursor circle
			update;
	  for {set j2 0; j2 < 4; incr j2 1} { after 1000; update }
		} } 
		changeit
#/c:n 
}
#2
proc c4pc {} {
				wm state .t1 normal
}

proc c2p {} {
	toplevel .t2
	pack [button .t2.b -text asfd -width 1 -height 1 -command exit]
	while {1==1} { 
	  incr x_pos; incr y_pos; incr y_pos
	  update; event generate .t2.b <Motion> -warp 1 -x $x_pos -y $y_pos;
	  #after 30000;
	  for {set j 0; j < 300; incr j 2} { after 1000; update }
	  if {$x_pos == 102} {set x_pos 2; set y_pos 1;}}
	#sleep bug
	#/n:c
}
#3
proc c3p {} {
	toplevel .t3

	proc createGroup {no} {
		frame .t3.frame${no}1; pack .t3.frame${no}1 -side left;
		text .t3.frame${no}1.text1; pack .t3.frame${no}1.text1;
		.t3.frame${no}1.text1 configure -width 5 -height 2 -font tiny
		frame .t3.frame${no}1.frame1 -bd 3 -relief solid; pack .t3.frame${no}1.frame1
		button .t3.frame${no}1.frame1.but1 -text cut1 -command "highlight .t3.frame${no}1"; pack .t3.frame${no}1.frame1.but1 -side left;
		button .t3.frame${no}1.frame1.but2 -text paste1 -command "paste1 .t3.frame${no}1" ; pack .t3.frame${no}1.frame1.but2 -side right;
		#
		#2nd
		frame .t3.frame${no}2; pack .t3.frame${no}2 -side right;
		text .t3.frame${no}2.text1; pack .t3.frame${no}2.text1;
		.t3.frame${no}2.text1 configure -width 4 -height 1 -font tiny
		frame .t3.frame${no}2.frame1 -bd 3 -relief ridge; pack .t3.frame${no}2.frame1
		button .t3.frame${no}2.frame1.but1 -text cut2 -command "highlight .frame${no}2" ; pack .t3.frame${no}2.frame1.but1 -side right;
		button .t3.frame${no}2.frame1.but2 -text paste1 -command "paste2 .t3.frame${no}2" ; pack .t3.frame${no}2.frame1.but2 -side right;
	}
	createGroup 1
	createGroup 2

	proc highlight {fr} {
		clipboard clear
		clipboard append [$fr.text1 get 0.0 1000.1000]
	}

	proc paste1 {fr} {
		puts [clipboard get]
		$fr.text1 delete 0.0 1000.1000
		$fr.text1 insert 0.0 [clipboard get]
	}
	proc paste2 {fr} {
		puts [clipboard get]
		$fr.text1 delete 0.0 1000.1000
		$fr.text1 insert 0.0 [selection get]
	}
	proc paste3 {fr} {
		puts [selection get ]
		$fr.text1 delete 0.0 1000.1000
		$fr.text1 insert 0.0 [selection get] # -selection SECONDARY
	}
	label .t3.label1 -text master@conorwilliams.fun; pack .t3.label1
}
######### ####### ######### 
#EOF
######### ####### ######### 
