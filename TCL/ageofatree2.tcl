############ ############ ############
# conor.williams@gmail.com
# tcl script to tell age of a tree
############ ############ ############

proc rest123 {filename} {
	set img [image create photo img -file $filename]

	catch {
	    canvas .con
	   .con create image 0 0 -anchor nw -image $img
	    pack .con
	    update
	}

	set myvar 1
	wm title . "age of tree"
	catch {destroy .but}
	######## ######### ########
	# Main Procedural Loop
	######## ######### ########
	set flag 0
	set whiteFound 0
	for {set i 100} {$i < 400} {incr i 4} {
		for {set j 100} {$j < 350} {incr j 3} {
		    set color [$img get $i $j]
		    #set color 1
		    set data [$img get $i $j]
		    
		    set red1 [lindex $data 0]
		    set green2 [lindex $data 1]
		    set blue3 [lindex $data 2]
		#	    puts "$red1  $green2  $blue3"

		    if {$red1 == 255 && $green2 == 255 && $blue3 == 255} {
		#	    puts white
		    } elseif {$red1 == 0 && $green2 == 0 && $blue3 == 0} {
		#	    puts black
		    } elseif {$red1 == 217 && $green2 == 217 && $blue3 == 217} {
		#	    puts grey
		    } elseif {$red1 == 253 && $green2 == 33 && $blue3 == 13} {
			    set age 0
			    puts "red pith located @ $i -- $j"
			    
			    for {set jv $j} {$jv > 9} {incr jv -1} {
				    #puts "$i $jv"
				set data2 [$img get $i $jv]
		    
				set red1h [lindex $data2 0]
				set green2h [lindex $data2 1]
				set blue3h [lindex $data2 2]
				if {$red1h == 253 && $green2h == 33 && $blue3h == 13} {
				      #puts "red"
				} elseif {$red1h == 255 && $green2h == 255 && $blue3h == 255} {
				      #puts white
				      set whiteFound 1
				} else {
					puts "$red1h $green2h $blue3h other : $i $jv"
					if {$whiteFound == 1} {
						incr age
						set whiteFound 0
					}
				}
				set flag 1
			    }
		    } else {
	#		    puts "$red1 $green2 $blue3 other"
		    }
		    if {$flag == 1} {break}
		}
		if {$flag == 1} {break}
	}
	puts "age of the tree seen on screen is: $age"
	update
	after 3000
	button .but
	.but configure -text $age -background red
	pack .but
	update
}

frame .frame1
pack .frame1 -side top
label .frame1.filename -text "png filename:"
pack .frame1.filename -side left
text .frame1.text2 -height 1 -width 30
pack .frame1.text2 -side right
#KeyPress is the event not the binding
#.frame1.text2 tag bind KeyPress {rest123 [.frame1.text2 get -displaychars 1.0 1.100]}
button .but10
.but10 configure -text "press me" -command {rest123 [.frame1.text2 get -displaychars 1.0 1.100]}
pack .but10
update
bind . <Key-Return> {rest123 [.frame1.text2 get -displaychars 1.0 1.100]}

#########
#EOF
##########