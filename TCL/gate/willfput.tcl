#!/bin/tclsh
######
# willfput.tcl - Conor Williams - conor.williams@usa.net
##########

puts "#######################################################################"
puts "     #     #     #  #  #      #      # # #  # # #  #   #  # # #"
puts "      # # # # # #   #  #      #      # #    # # #  #   #    #"
puts "        #     #     #  # # #  # # #  #      #      # # #    #"
puts "#######################################################################"

if {[llength $argv] < 3} {
        puts "Usage: tclsh willfput.tcl remoteipaddress remoteportnumber filename"
	puts "or   : willfput.exe remoteipaddress remoteportnumber filename - if running exe"
	puts ""
	puts " 		 implements put style ftp"
	puts "		 get soon to come"
	puts ""
	puts "	Conor Williams - will551@iol.ie 04/05/2003 0-4"
        exit 0
}

global blah
set blah "notready"
global start
set start "blankblank"
global filename
set filename [lindex $argv 2]
global fd
global done
set done "notdone"
#set message "Check server directory for file\nDone."
set message "Done."
global buffersize
#set buffersize 7168
set buffersize 2000000

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

proc write_channel {s} {
	puts -nonewline "in write..."
}

proc read_channel {s} {
	global fd
	global done
	global start
	global sfilename
	set gotline "doneman2"

#	puts ""
	
	set gotline [gets $s]
#	puts "Got -$gotline- from socket"
	if {[string compare $gotline "doneman"] == 0} {
		puts "Finished - closing socket"
		catch {puts $s "donEman"}
		puts "put donEman"
		catch {close $s}
		catch {close $fd}
		puts "about to exit"		
		exit
	} elseif {[string compare $gotline "donefile"] == 0} {
#		puts "Sent $sfilename"
		set done "done"
	} elseif {[string compare $gotline "fileerror"] == 0} {
		puts "Error: Server File \"[set sfilename [gets $s]]\" unwritable"
		close $fd
		catch {puts $s "donEman"}
		close $s
		exit
	} elseif {[string compare $gotline "fileexists"] == 0} {
		puts "Error: Server File \"[set sfilename [gets $s]]\" already exists"
		close $fd
		catch {puts $s "donEman"}
		close $s
		exit
	} elseif {[string compare $gotline "filecloseproblem"] == 0} {
		puts "Error: Couldnt close \"$sfilename\" on server"
		close $fd
		catch {puts $s "donEman"}
		close $s
		exit
	} elseif {[string compare $gotline "FilenamE"] == 0} {
		set sfilename [gets $s]
#		puts "Server File $sfilename"
		if {[string compare $sfilename ""] == 0} {
			puts "Error: Garbled Server File Name - aborting"
			close $fd
			catch {puts $s "donEman"}
			close $s
			exit
		}
		set start "go"
	} elseif {[string compare $gotline "sendinitstring"] == 0} {
		puts "Error: Send correct init sequence"
		catch {puts $s "donEman"}
		catch {close $s}
		catch {close $fd}
		exit
	} else {
		puts "Garbage from socket -$gotline- aborting"
		catch {puts $s "donEman"}
		catch {close $s}
		catch {close $fd}
		exit
	}
	flush stdout
}

regsub -all {\\} $filename {/} filename
set filenamelist [glob $filename]

foreach filename $filenamelist {
	global fd
	global done
	global start
	global sfilename
	global s
	global blah
	global buffersize

	#
	# Make sure file is present
	#
	if {[catch {set fd [open $filename r]}] == 1} {
		puts "Error: Cannot find file \"$filename\" - aborting"
		exit
	} else {
		fconfigure $fd -translation binary
	}

	puts $s "FilenamE"
	puts $s "$filename"
	flush $s
	vwait start

	puts "Sending $filename to $sfilename on server"
	file stat $filename fileinfo
	set filesize $fileinfo(size)
	if {$filesize != 0} {
	
		set looptimes [expr $filesize/$buffersize]
		set leftover [expr $filesize%$buffersize]
	
		if {$leftover != 0} {
			set looptimes [expr {$looptimes + 1}]
		}
	
		set startclock [clock seconds]
		while {$looptimes > 0} {
			set looptimes [expr {$looptimes - 1}]
			
			if {$looptimes == 0} {
				fconfigure $s -buffering line -buffersize $buffersize -blocking 1 -translation binary
				set bytestoread $leftover
				catch {puts $s $leftover}
#				puts "leftover is $leftover"
				catch {flush $s}
			} else {
				fconfigure $s -buffering line -buffersize $buffersize -blocking 1 -translation binary
				set bytestoread $buffersize
				catch {puts $s 0}
				catch {flush $s}
			}
			#
			# zero if fd not reached 
			# indicates whether last buffer being sent
			fconfigure $s -buffering full -buffersize $buffersize -blocking 1 -translation binary
#			puts "writing $bytestoread..."
		    	if {[catch {fcopy $fd $s -size $bytestoread} var] == 1} {
				puts "Error: Problem sending - quitting"
				puts "    : $var"
				fconfigure $s -buffering line -buffersize $buffersize -blocking 1 -translation binary
				catch {puts $s "donEman"}
				catch {close $s}
				catch {close $fd}
				exit 1
			}
			catch {flush $s}
			fconfigure $s -buffering line -buffersize $buffersize -blocking 1 -translation binary
		     	puts -nonewline stdout "."
			flush stdout
			catch {flush $s}
		}
	
		set finclock [clock seconds]
		set secs [expr {$finclock - $startclock}]
		if {$secs == 0} {
			set secs "1"
		}
	
#		puts ""
#		puts "$filesize Bytes transferred in $secs seconds"
		set speed [expr $filesize/$secs]
		puts "Average transfer rate $speed Bytes/sec]"
	        puts $s "$speed"
	} else {
		puts $s 0
		puts $s 0
	}
	flush $s
	catch {close $fd}
	vwait done
	set done "notdone"
}

#
# Tidy up time
#
puts $message
catch {puts $s "donEman"}
#puts "wrote donEman to socket"
catch {close $s}
#puts "exitting"
exit
#
# eof
#


