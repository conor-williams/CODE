pack [button .b -text asfd -width 10 -height 5 -command exit]
while {1==1} {incr x_pos; incr y_pos; incr y_pos
  update; event generate .b <Motion> -warp 1 -x $x_pos -y $y_pos; after 100;
  if {$x_pos == 102} {set x_pos 2; set y_pos 1;}} 
#/n:c @aargh!!!
