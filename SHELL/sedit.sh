sed 's/{/,/g' $1 | sed 's/\[/,/g' | sed 's/\]/,/g' | sed 's/}/,/g' | sed 's/X/,/g' | sed 's/"[a-z][a-z]*"/,/g' | sed 's/:/,/g' | sed 's/ //g' | sed 's/T/,/g' | sed 's/S/,/g' | sed 's/,[,]*/,/g' | sed 's/,/+/g' | sed 's/+$//' | sed 's/^/echo $((/' | sed 's/$/))/' 