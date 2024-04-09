echo "---TIMER TIMER TIMER-----------"
echo FROM $1
echo STEP $2
echo TO $3

FROM=$1
STEP=$2
TO=$3
if [ @$FROM = @ ]; then
	FROM=10
fi
if [ @$STEP = @ ]; then
	STEP=2
fi
if [ @$TO = @ ]; then
	TO=3600
fi

echo "-----------------"
echo "FOR EVERY $FROM TO $TO - STEP: $STEP-- press a key"
echo "-----------------"
read
if [ -d DONELESSTHAN10 ]; then
	echo "directory DONELESSTHAN10 already there"
else
	mkdir DONELESSTHAN10
fi
if [ -d PROBSHIGHERTHAN10 ]; then
	echo "directory PROBSHIGHERTHAN10 already there"
else
	mkdir PROBSHIGHERTHAN10
fi
echo "PRESS TO START"
echo "PRESS TO START"
read

echo "---------------TIMER TIMER TIMER single-----"
echo "----------RUNNING a MAX(FROM) time $FROM----"

for j in $(seq $FROM $STEP $TO); do for i in x* ; do ./a $i $j >> OUTPUT; if [ $? = 10 ]; then echo -n "[$i]:LEAVING "; else echo ""; echo "[$i]:SUCCESS -\> PROBSHIGHERTHAN10"; mv $i PROBSHIGHERTHAN10; fi  done; done

