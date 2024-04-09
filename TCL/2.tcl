label .lab3 -text "Ta... St..." -font bold
pack .lab3

frame .fram1 
pack .fram1
button .fram1.but3 -text reset -command bellloop
pack .fram1.but3

label .fram1.lab1 -text 0 -font bold
pack .fram1.lab1 

label .fram1.lab2 -text INFO -font bold
pack .fram1.lab2

proc bellloop args {
	set tim1 0
	while 1 {
		.fram1.lab1 configure -text $tim1
		update
		after 998
		update
		incr tim1
		if { $tim1 == 60 || $tim1 == 1 } {
			bell;
		} elseif { $tim1 == 121 } {
			.fram1.lab2 configure -text "restarting in 3..."
			update; after 1000; bell;
			.fram1.lab2 configure -text "restarting in 2..."
			update; after 1000; bell;
			.fram1.lab2 configure -text "restarting in 1..."
			update; after 1000;
			.fram1.lab2 configure -text "INFO"			
			set tim1  0; update
		}
	}
}

bellloop