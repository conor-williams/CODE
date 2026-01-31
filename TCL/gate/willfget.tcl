#!/bin/tclsh
########## ########## ########## ########### ########## ##########
# willfget.tcl - Conor Williams - will551@iol.ie
########## ########## ########## ########### ########## ##########

puts "#######################################################################"
puts "     #     #     #  #  #      #      # # #   # # #  # # #  # # #"
puts "      # # # # # #   #  #      #      # #    # # #   # #      #"
puts "        #     #     #  # # #  # # #  #       # # #  # # #    #"
puts "#######################################################################"

if {[llength $argv] < 3} {
        puts "Usage: tclsh willfget.tcl remoteipaddress remoteportnumber filename"
	puts "or   : willfget.exe remoteipaddress remoteportnumber filename - if running exe"
	puts ""
	puts " 		 implements put style ftp"
	puts "		 get soon to come"
	puts ""
	puts "	Conor Williams - will551@iol.ie 22/10/2001 0-4"
        exit 0
}

global firstblock
set firstblock 0
global start
set start "blankblank"
global filename
set filename [lindex $argv 2]
global lfilename
global fd
global done
set done "notdone"
set message "Done."
global buffersize
#set buffersize 7168
set buffersize 2000000
global filenamelist ""

#
# Set up the socket
#
if {[catch {set s [eval socket  [lindex $argv 0] [lindex $argv 1]]} var] == 1} {
        puts "Socket not ready to receive - aborting"
        puts $var
        exit 1
} else {
#	puts "opened socket successfully"
}

fconfigure $s -buffering line -buffersize $buffersize -blocking 1 -translation binary
fileevent $s readable [list read_channel $s]
#fileevent $s writable [list write_channel $s]
puts stdout "Connected to computer: [lindex $argv 0] port: [lindex $argv 1]"

proc savefile {socket} {
	global lfilename
	global fd

	set finished 1
	catch {flush $fd}
        if {[catch {close $fd}] == 1} {
		puts ""
		puts "Error: closing file problem"
	} 
#	puts "Saved File $lfilename."
}

proc read_channel {s} {
	global fd
	global done
	global start
	global sfilename
	global lfilename
	global buffersize
	global firstblock
	global filenamelist

	set gotline "doneman2"

#	puts "In read about to read."
	set gotline [gets $s]
	
#	puts "Gotline is [string range $gotline 0 4]"

	if {[string range $gotline 0 6] == "doneman"} {
		puts "Finished - closing socket"
		catch {puts $s "donEman"}
		catch {close $s}
		catch {close $fd}
		exit
	} elseif {[string range $gotline 0 8] == "fileerror"} {
		puts "Error: Server File file unwritable"
		close $fd
		catch {puts $s "donEman"}
		close $s
		exit
	} elseif {[string range $gotline 0 7] == "globlist"} {
		set rescode [gets $s]
		if {$rescode == "globlistfailed"} {
			puts "Error Couldnt get file from server - files probably dont exist"
			catch {puts $s "donEman"}
			catch {close $s}
			exit
		} elseif {$rescode == "globlistsuccess"} {
			set filenamelist [gets $s]
		}
	} elseif {[string range $gotline 0 12] == "filenotexist"} {
		puts "Error: Server File file does not exist"
		close $fd
		catch {puts $s "donEman"}
		close $s
		exit
	} elseif {[string range $gotline 0 15] == "filecloseproblem"} {
		puts "Error: Couldnt close file on server"
		close $fd
		catch {puts $s "donEman"}
		close $s
		exit
	} elseif {[string range $gotline 0 8] == "FilenamEG"} {
		set tempfilename [gets $s]
		puts "Recieving $tempfilename from server"

		if {[string compare $tempfilename ""] == 0} {
			puts "Error: Garbled Server File Name - aborting"
			close $fd
			catch {puts $s "donEman"}
			close $s
			exit
		}
		# puts "sending back oksend message"
		puts $s "oksend"
		catch {flush $s}
		set firstblock 1
	 } elseif {[string compare $gotline "0"] == 0} {
#	 elseif {[string range $gotline 0 0] == "0"} 
#		puts "Got a block that isnt the last one ok."
		if {$firstblock == 1} {
			fconfigure $s -buffering full -buffersize $buffersize -blocking 1 -translation binary
		}
#		set gotline [read $s $buffersize]
		fcopy $s $fd -size $buffersize 
		# puts -nonewline $fd $gotline
		puts -nonewline stdout  "."
		#catch {flush $s} 
		#catch {flush $fd}
	        #flush stdout
		#after 500
		set firstblock 0
	} elseif {[string range $gotline 0 7] == "FilenamE"} {
		puts "Server File [set sfilename [gets $s]]"
		if {[string compare $sfilename ""] == 0} {
			puts "Error: Garbled Server File Name - aborting"
			close $fd
			catch {puts $s "donEman"}
			close $s
			exit
		}
		set firstblock 0
		set start "go"
	} elseif {[string range $gotline 0 13]  == "invalidinit"} {
		puts "Error: Send correct init sequence"
		catch {puts $s "donEman"}
		catch {close $s}
		catch {close $fd}
		exit
	} elseif {$gotline >  0} {
#		puts "got last block"
		if {$firstblock == 1} {
			fconfigure $s -buffering full -buffersize $buffersize -blocking 1 -translation binary
		}
#		puts "about to fcopy"
#		set gotline [read $s $gotline]
#		puts -nonewline $fd $gotline
		if {$gotline != 77777} {
			fcopy $s $fd -size $gotline
		       	puts -nonewline stdout  "."
		}
		#catch {flush $fd}
		puts ""
		puts "Average speed: [gets $s] Bytes/second"
#		puts "about to save"
		savefile $s
		catch {puts $s "donEman"}
		#catch {close $s}
		set done "done"
        	flush stdout
		#exit
	} else {
		puts "Garbage from socket -$gotline- aborting"
		catch {puts $s "donEman"}
		catch {close $s}
		catch {close $fd}
		exit
	}
	flush stdout
}


#
# Make sure file is present
#
regsub -all {\\} $filename {/} filename
#puts "Getting list: $filename from server"
puts $s "globlist"
puts $s "$filename"
flush $s

#
# This is done at the other side
# set filenamelist [glob $filename]
#
vwait filenamelist
foreach filename $filenamelist {
	global lfilename
	global done
	
	if {[file isdirectory "incoming"]} {
		set lfilename "incoming/[file tail $filename]"
	} else {
		set lfilename "[file tail $filename]"
	}
	if {[file exists $lfilename]} {
#		puts ""
		puts "Error: $lfilename already exists"
	} else {
		if {[catch {set fd [open $lfilename w]}] == 1} {
			puts ""
			puts "Error: Cannot open \"$lfilename\""
		} else {
			fconfigure $fd -translation binary
			# puts "Going to write to \"$lfilename\""
#			puts ""
#			puts "Requesting file \"$filename\""
			puts $s "FilenamEG"
			puts $s "$filename"
			flush $s
			vwait done
			set done "notdone"
		}
	}
}
#
# Tidy up time
#
#puts ""
puts $message
catch {puts $s "donEman"}
catch {close $s}
catch {close $fd}
exit
#
# eof
#
