#!/bin/tclsh
#
################## ################## ################## ##################
#
# willftsv.tcl - Conor Williams - will551@iol.ie
#
################## ################## ################## ##################

puts "#######################################################################"
puts "     #     #     #  #  #      #      # # #  # # #  # # #  #     #"
puts "      # # # # # #   #  #      #      # #      #      # #   # # #"
puts "        #     #     #  # # #  # # #  #        #    # # #     #"
puts "#######################################################################"

if {[llength $argv ] != 1} {
        puts "Usage: tclsh willftsv.tcl portnumber"
#        puts "or   : willftsv.exe portnumber - if running the binary"
	puts ""
	puts "	Conor Williams - will551@iol.ie 22/10/2001 0-4"
        exit 0
}

global buffersize
#set buffersize 7168
set buffersize 2000000
global finished
set finished 0
global sizeline
set sizeline "notquit"
global fd
global filename
global x
set x "started"
global getready
set getready 0
#global myhostaddress
#set myhostaddress [lindex $argv 0]
global hostport
set hostport [lindex $argv 0]
global ready
set ready 0

###############################################################################
#
# Allow connections to this server regardless of specified ip.
#
###############################################################################
set servsocket [socket -server accept $hostport]
fconfigure $servsocket -buffering line -buffersize $buffersize -blocking 1 -translation binary

proc accept {socket addr port} {
   global hostport
   global buffersize
#   puts ""
   puts "Socket connect from $addr: port $port - Server port: $hostport"
   fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
   fileevent $socket readable [list read_channel $socket]
}

proc mywrite_channel {socket whattowrite} {
	catch {puts $socket $whattowrite}
   	catch {flush $socket}
}

proc savefile {socket} {
	global filename
	global fd

	set finished 1
	catch {flush $fd}
        if {[catch {close $fd}] == 1} {
		puts "Error: closing file problem"
		mywrite_channel $socket "filecloseproblem"
	} else {
	       # puts "Saving File $filename."
	   	catch {flush $socket}
		mywrite_channel $socket "donefile"
		#catch {close $socket}
	}

        set ready 0
}

proc remove_ctrlm {instring} {
	set stringlen [string length $instring]
	set strrange [string index $instring [expr {$stringlen - 1}]] 
	set ctrlm "\r"
#	puts "strrange is \"$strrange\""
	if {[string compare $strrange $ctrlm] == 0} {
		return [string range $instring 0 [expr {$stringlen - 2}]]
	} else {
		return $instring
	}
}

proc read_channel {socket} {
   global sizeline
   global filename
   global ofd
   global x
   global fd
   global ready
   global finished
   global sfilename
   global getready
   global ingotline
   global buffersize
	

   ####################################################################   
   # The following is for putting and getting a file to and from 
   # the server 
   ####################################################################   
#   puts "in read on server\n"
   if {$ready == 0} {
	fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
	flush $socket
	set gotline [remove_ctrlm [gets $socket]]
#	set gotline [remove_ctrlm $gotline]
	set ingotline [string range $gotline 0 11]
	fconfigure $socket -buffering full -buffersize $buffersize -blocking 1 -translation binary
   } elseif {$getready == 1} {
	fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
	flush $socket
	set gotline [remove_ctrlm [gets $socket]]
#	set gotline [string range $gotline 0 [expr [string length $gotline] - 2]]
#	set gotline [remove_ctrlm $gotline]
	set ingotline [string range $gotline 0 11]
	fconfigure $socket -buffering full -buffersize $buffersize -blocking 1 -translation binary
   } else {
	flush $socket
	fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
	set sizeline [remove_ctrlm [gets $socket]]
	fconfigure $socket -buffering full -buffersize $buffersize -blocking 1 -translation binary
	
#	puts "sizeline is $sizeline"
	if {$sizeline < $buffersize} {
		if {[string compare $sizeline "0"] == 0} {
			set sizeline $buffersize
		}

		set gotline "reading"
		#puts "sizeline less than buffersize"
		# [read $socket $buffersize]
	} else {
		set gotline "reading"
		fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
		set gotline [remove_ctrlm [gets $socket]]
		fconfigure $socket -buffering full -buffersize $buffersize -blocking 1 -translation binary
	}

	set ingotline [string range $gotline 0 11]

	if {[string length $sizeline] > 11} {
		set ingotline [string range $sizeline 0 11]
   	}	
#   	puts "gotline is [string range $gotline 0 11] ."
   	#puts "ingotline [string range $gotline 0 11] ."
   }

   if {$getready == 1 && [string compare $gotline "oksend"] == 0} {
		puts "Sending $sfilename to client"
		fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
		flush stdout
		file stat $sfilename fileinfo
		set filesize $fileinfo(size)

		set looptimes [expr $filesize/$buffersize]
		set leftover [expr $filesize%$buffersize]
		if {$leftover != 0} {
		    set looptimes [expr {$looptimes + 1}]
		}

		set startclock [clock seconds]
		while {$looptimes > 0} {
			#
			# zero if fd not reached - 
			# indicates whether last buffer being sent
			#
			set looptimes [expr {$looptimes - 1}]

			if {$looptimes == 0} {
			        set bytestowrite $leftover
			        catch {puts $socket $leftover}
			} else {
			        set bytestowrite $buffersize
			        catch {puts $socket "0"}
			}
			catch {flush $socket}
	   		#set line [read $ofd $buffersize]
	    		if {[catch {fcopy $ofd $socket -size $bytestowrite} var] == 1} {
				catch {puts $socket "fileerror"}
				puts "Error: Problem sending - aborting"
				puts "    : $var"
				catch {close $socket}
				catch {close $ofd}
				set ready 0
				set getready 0
				return
			}
			puts -nonewline stdout "."
			flush stdout
#			catch {flush $socket}
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
#		puts "Ready."
#		puts ""
		mywrite_channel $socket $speed
		catch {close $ofd}
		catch {flush $socket}
		flush stdout
		set ready 0
		set getready 0
    } elseif {[string compare $ingotline "quitQuiTquit"] == 0} {
		puts "Received quit command - quitting."
		mywrite_channel $socket "doneman" 
		catch {flush $fd}
      		catch {close $fd}
		catch {flush $ofd}
      		catch {close $ofd}
		catch {close $socket}
     		set ready 0
		set x "done"
   } elseif {[string compare $gotline "globlist"] == 0} {
#		puts "Processing globlist"
		fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
		set gotline2 [remove_ctrlm [gets $socket]]
		fconfigure $socket -buffering full -buffersize $buffersize -blocking 1 -translation binary
		set res1 [catch {set globlist [glob $gotline2]}]
		if {$res1 == 1} {
			catch {puts $socket "globlist"}
			catch {puts $socket "globlistfailed"}
			flush $socket
		} else {
			catch {puts $socket "globlist"}
			catch {puts $socket "globlistsuccess"}
			catch {puts $socket $globlist}
			flush $socket
		}
		return

   } elseif {[string compare $gotline "FilenamEG"] == 0} {
		set getready 0
		set ready 0
		fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
	        set sfilename [remove_ctrlm [gets $socket]]
		fconfigure $socket -buffering full -buffersize $buffersize -blocking 1 -translation binary
#		set sfilename [string range $sfilename 0 [expr [string length $sfilename] -2]]
#		puts "Got request for $sfilename"
		regsub -all {\\} $sfilename {/} sfilename

		if {[catch {set ofd [open $sfilename]}] == 1} {
			puts "Error: Couldnt open file $sfilename"
   			mywrite_channel $socket "filenotexist"
			catch {flush $socket}
			return
		} else {
			fconfigure $ofd -translation binary
#			puts "Opened $sfilename - Ready to send"
		}
		set getready 1
		set ready 1
		fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
		puts $socket "FilenamEG"
		puts $socket "$sfilename"
		catch {flush $socket}
		catch {flush $socket}
		#puts "Fired back Filename to client."
   } else {
        if {[string match "*FilenamE*" $gotline] == 1} {
	    #puts "after compare"
	    #puts "Getting filename."
	    flush stdout
	    puts -nonewline "Current directory is [pwd] "
		fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
            while {[set filename [remove_ctrlm [gets $socket]]] == ""} {}
		fconfigure $socket -buffering full -buffersize $buffersize -blocking 1 -translation binary
	    puts "- File: $filename"
            set ready 1
	    regsub -all {/} $filename {\\} filename
	    set filename [split $filename \\]
	    set filename [lindex $filename [expr [llength $filename] - 1]]
	    if {[file isdirectory "incoming"] == 1} {
		     set filename [format "%s/%s" "incoming" $filename]
    	    }

	    if {[file exists $filename]} {
		puts "Error: File Already Exists - Aborting Transfer"
		puts $socket "fileexists"
		puts $socket "$filename"
		catch {flush $socket}
		set ready 0
		return
	    } else {
		puts $socket "FilenamE"
		puts $socket "$filename"
		catch {flush $socket}
	    }

#            puts "Incoming File is \"$filename\""
            if {[catch {set fd [open $filename "w+"]}] == 1} {
		puts "Cant open file $filename - aborting client"
		puts $socket "fileerror"
		catch {flush $socket}
		set ready 0
	     } else {
		fconfigure $fd -translation binary
	     }
        } else {
            if {$ready == 1} {
	        if {[catch {fcopy $socket $fd -size $sizeline} var] == 1} {
			puts "Problems on socket - restart transfer"
			puts "	:$var"
			catch {close $fd}
			catch {close $ofd}
			catch {close $socket}
			return
		}
       		puts -nonewline stdout  "."
		#catch {flush $fd}
                flush stdout
		#after 3000	
		catch {flush $socket}

		if {[string compare $sizeline $buffersize] != 0} {
#			puts ""
			fconfigure $socket -buffering line -buffersize $buffersize -blocking 1 -translation binary
			puts "Average Transfer speed: [remove_ctrlm [gets $socket]] Bytes/sec"
			fconfigure $socket -buffering full -buffersize $buffersize -blocking 1 -translation binary
#			puts "Ready."
#			puts ""
			savefile $socket
			set ready 0
		}
            } else {
		puts "Ready."
		puts "##############################################"
		puts "all done - writing Blahhalb to socket"
		if {[catch {puts $socket "BlAhhAlB"}] != 0} {
			puts "closing socket..."		
			catch {close $socket}
		} else {
			puts "flushing socket..."
			catch {flush $socket}
		}
            }
        }
   }
}

puts "Ready on port number $hostport"
vwait x
catch {close $servsocket}
exit 0

###############################################################################
# eof
###############################################################################
