#!/bin/tclsh
###############################################################################
#
# willsvmd.tcl - Conor Williams - will551@iol.ie
#
###############################################################################

puts "    #####################################################################"
puts "      #   #   #  #  #      #      # # #       #      # # #  # # #"
puts "       # # # #   #  #      #      #  # #    # # #      #    # # "
puts "        #   #    #  # # #  # # #   # # #   #     #     #    # # #"
puts "    #####################################################################"
   
if {[llength $argv ] != 3} {
        puts "Usage: tclsh willsvmd.tcl portnumber remoteip remoteport"
#        puts "or   : willsvmd.exe portnumber - if running the binary"
	puts ""
        puts "  Conor Williams - will551@iol.ie 08/10/2001 0-4"
        exit
}

global fcopyready
set fcopyready 0
global insize
set insize 0
global incomingsize
set incomingsize 0
global backsock
global buffersize
#set buffersize 7168
set buffersize 2000000
global remoteip
set remoteip [lindex $argv 1]
global remoteport
set remoteport [lindex $argv 2]
global hostport
set hostport [lindex $argv 0]
global expectfilename 
set expectfilename 0
global readyforsize
global x
set x "notdone"

################################################################################
#
# Allow connections to this server regardless of specified ip.
#
################################################################################

################################################################################
#
# Accept temporary on this port...
#
################################################################################
global servsocket
set servsocket [socket -server accept $hostport]
fconfigure $servsocket -buffering full -buffersize $buffersize -blocking 1 -translation binary

################################################################################
#
# Set up the remote socket
#
################################################################################
global rservsocket
if {[catch {set rservsocket [eval socket $remoteip $remoteport]} var] == 1} {
    puts "Socket to $remoteip on port $remoteport not ready to connect - aborting"
    puts $var
    exit 1
} else {
    puts "Connected socket successfully to $remoteip on port $remoteport"
    fileevent $rservsocket readable [list read_channel_remote $rservsocket]
}

###############################################################################
#
# This is the permanent socket function which connects to the server
#
###############################################################################
proc read_channel_remote {remoteservsocket} {
	global backsock
	global servsocket
	global buffersize
	global readinfo
	global incomingsize
	global insize
	global fcopyready 
	global expectfilename 
	global readyforsize
	
	
#	puts "backsock is $backsock"
#	puts "remoteservsocket is $remoteservsocket"
#	puts "servsocket is $servsocket"

	set readinfo "blah"
	if {$fcopyready == 0} {
		set catchval [catch {set readinfo [remove_ctrlm [gets $remoteservsocket]]} var]
		if {$catchval != 0} {
			catch {close $backsock}
			catch {close $servsocket}
			catch {close $remoteservsocket}
			puts "Problems reading from RemoteServer"
			puts "Exitting"
			exit 1
		}
#	        puts "Got \[$readinfo\] from remoteserversocket"
	}
#	puts "catchval is $catchval - var"

	if {[string match "*donEman*" $readinfo] == 1} {
		puts "Got doneman from Server"
		catch {flush $backsock}
		catch {flush $servsocket}
		set expectfilename 0
		set fcopyready 0
	} elseif {$readyforsize == 1} {
#		puts "Setting size"
		catch {puts $backsock $readinfo}
		catch {flush $backsock}
		catch {flush $servsocket}
		set insize $readinfo
		set readyforsize 0
		set fcopyready 1
	} elseif {[string match "fileexists" $readinfo] == 1} {
#		puts "Got fileexists from remote"
		catch {puts $backsock $readinfo}
		catch {flush $backsock}
		catch {flush $servsocket}
		catch {set readinfo [remove_ctrlm [gets $remoteservsocket]]}
		catch {puts $backsock $readinfo}
		catch {flush $backsock}
		catch {flush $servsocket}
		set expectfilename 0
		set fcopyready 0
	} elseif {[string match "globlistfailed" $readinfo] == 1} {
#		puts "Got globlistfailed from Server"
		catch {puts $backsock $readinfo}
		catch {flush $backsock}
		catch {flush $servsocket}
		set expectfilename 0
		set fcopyready 0
	} elseif {[string match "donefile" $readinfo] == 1} {
#		puts "Got donefile from Server"
		catch {puts $backsock $readinfo}
		catch {flush $backsock}
		catch {flush $servsocket}
		set expectfilename 0
		set fcopyready 0
	} elseif {[string match "*FilenamE*" $readinfo] == 1} {
#		puts "dealing with FilenamE"
		catch {flush $servsocket}
		catch {flush $rservsocket}
		catch {puts $backsock $readinfo}

		catch {set readinfo [remove_ctrlm [gets $remoteservsocket]]}
		catch {puts $backsock $readinfo}
		catch {flush $backsock}
		catch {flush $servsocket}
		set expectfilename 0
		set fcopyready 0
	} elseif {[string match "*BlAhhAlB*" $readinfo] == 1} {
		catch {flush $backsock}
		catch {flush $servsocket}
		catch {close $backsock}
		catch {close $servsocket}
		catch {flush $remoteservsocket}
		puts "Dealing with BlAhhAlB"
		set expectfilename 0
		set fcopyready 0
	} elseif {[string match "*globlistsuccess*" $readinfo] == 1} {
#		puts "Dealing with globlistsuccess"
		catch {puts $backsock $readinfo}
		catch {flush $servsocket}
		catch {flush $rservsocket}
		set expectfilename 1
		set fcopyready 0
	} elseif {[string match "*globlist*" $readinfo] == 1} {
#		puts "Dealing with globlist"
		catch {puts $backsock $readinfo}
		catch {flush $servsocket}
		catch {flush $rservsocket}
		set expectfilename 0
		set fcopyready 0
	} elseif {$expectfilename == 1} {
#		puts "Dealing with actual filename"
		catch {puts $backsock $readinfo}
		catch {flush $backsock}
		catch {flush $servsocket}
		catch {flush $rservsocket}
		set expectfilename 0
		set fcopyready 1
	} elseif {$fcopyready != 1} {
#		puts "Dealing with $readinfo"
		catch {puts $backsock $readinfo}
		catch {flush $backsock}
		catch {flush $servsocket}
		catch {flush $rservsocket}
		set expectfilename 0
		set fcopyready 0
	} elseif {$fcopyready == 1} {
#		puts "putting $readinfo to local"
#		set catchval [catch {puts $backsock [set incomingsize [gets $remoteservsocket]]} var]
#		puts "Doing Fcopy size $insize from remote to servsocket"
		set catchval [catch {fcopy $remoteservsocket $backsock -size $insize} var]
		set insize 0
		set expectfilename 0
		set fcopyready 0
		if {$catchval != 0} {
			puts $var
			puts "Closing local socket"
			catch {close $backsock}
			catch {close $servsocket}

			catch {flush $remoteservsocket}
			catch {close $remoteservsocket}
			puts "Problems on remote socket - exitting"
			puts ""
			exit 1
		} else {
			catch {flush $backsock}
		}
	} else {
		puts "Got Erroneous \[$readinfo\] from socket"
	}
#	puts "leaving - the reading of remote..."
}


proc accept {socket addr port} {
   global hostport
   global buffersize
   global backsock

   set backsock $socket
   puts "Socket connect from $addr: port $port to Server port: $hostport"
   fconfigure $socket -buffering full -buffersize $buffersize -blocking 1 -translation binary
   fileevent $socket readable [list read_channel $socket]
}

proc remove_ctrlm {instring} {
	return [string trimright $instring "\n\r"]
}
#	set stringlen [string length $instring]
#	set strrange [string index $instring [expr {$stringlen - 1}]] 
#	set ctrlm "\r"
#	set ctrln "\n"
#	puts "strrange is \"$strrange\""
#	if {[string match "*$$strrange] == 0} 
#		puts "removed r"
#		return [string range $instring 0 [expr {$stringlen - 2}]]
#	elseif {[string compare $strrange $ctrln] == 0} 
#		puts "removed n"
#		return [string range $instring 0 [expr {$stringlen - 2}]]
#	else 
#		puts "not"
#		return $instring
#	

###############################################################################
#
# Reads from the temporary client and writes to the server (remote socket)
#
###############################################################################
proc read_channel {servsocket} {
   global backsock
   global rservsocket
   global var
   global buffersize 
   global x
   global looptimes
   global incomingsize
   global insize
   global readyforsize

   set readyforsize 0
   catch {flush $servsocket}

   set input blah
   if {$incomingsize != 2} {
	   set input [remove_ctrlm [gets $servsocket]]
#	   puts "Got [string range $input 0 5] from servsocket"
   }

   if {[string match "*donEman*" $input] == 1} {
#	puts "Closing socket on client side after getting donEman"
#	puts stdout "Ready"
	catch {flush $servsocket}
#	catch {flush $rservsocket}
	catch {flush $backsock}
	catch {close $servsocket} 
	catch {close $backsock}
#	puts ""
	return
   } elseif {[string match "*donefile*" $input] == 1} {
#	puts "Dealing with $input"
	puts $rservsocket $input
	catch {flush $rservsocket}
	catch {close $backsock}
	catch {close $servsocket}
	set incomingsize 0
   } elseif {$incomingsize == 1} {
#	puts "Speed of transfer is \[$input\]"
	puts $rservsocket $input
	catch {flush $rservsocket}
	set incomingsize 0
   } elseif {[string match "*FilenamE*" $input] == 1} {
#	puts "Dealing with FilenamE"
	puts $rservsocket $input
	catch {flush $servsocket}
	catch {flush $rservsocket}
	set incomingsize 3
   } elseif {[string match "*oksend*" $input] == 1} {
	set readyforsize 1
#	puts "Dealing with oksend"
	puts $rservsocket $input
	catch {flush $servsocket}
	catch {flush $rservsocket}
	set incomingsize 0
   } elseif {[string match "*globlistsuccess*" $input] == 1} {
#	puts "Dealing with globlistsuccess"
	puts $rservsocket $input
	catch {flush $servsocket}
	catch {flush $rservsocket}
	set incomingsize 2
   } elseif {[string match "*globlist*" $input] == 1} {
#	puts "Dealing with globlist"
	puts $rservsocket $input
	catch {flush $servsocket}
	catch {flush $rservsocket}
	set incomingsize 3
   } elseif {$incomingsize == 4} {
#	puts "Dealing with size of transfer"
	set insize $input
	puts $rservsocket $input
	catch {flush $servsocket}
	catch {flush $rservsocket}
	set incomingsize 2
   } elseif {$incomingsize == 3} {
#	puts "Dealing with actual filename"
	puts $rservsocket $input
	catch {flush $servsocket}
	catch {flush $rservsocket}
	set incomingsize 4
   } elseif {$incomingsize == 2} {
#	puts "writing to rservsocket - $rservsocket"
#	puts "Dealing with Fcopy getting from serversocket"
#	puts "Size is \[$insize\]"
	fconfigure $servsocket -buffering full -buffersize $buffersize -blocking 1 -translation binary
	fconfigure $rservsocket -buffering full -buffersize $buffersize -blocking 1 -translation binary
	fcopy $servsocket $rservsocket -size $insize
   	catch {flush $rservsocket}
	catch {flush $servsocket}
	set insize 0
	set incomingsize 1
   } else {
	puts "Got Erroneous \[$input\] from servsocket"
#	puts "Closing socket on client side"
	puts stdout "Done2.\nReady."
	catch {flush $servsocket}
	catch {flush $backsock}
	catch {close $servsocket} 
	catch {close $backsock}
#	puts ""
	return
   }
#   puts "Leaving - reading servsocket..."
}

puts "Ready on port number $hostport"
set x "done"
vwait x
catch {close $servsocket}
catch {close $rservsocket}
exit 0

##############################################################################
# eof
##############################################################################
