echo "---TIMER TIMER TIMER-----------"
echo FROM $1
echo STEP $2
echo TO $3

FROM=$1
STEP=$2
TO=$3
if [ @$FROM = @ ]; then
	FROM=30
fi
if [ @$STEP = @ ]; then
	STEP=30
fi
if [ @$TO = @ ]; then
	TO=3600
fi

echo "-----------------"
echo "FOR EVERY $FROM TO $TO - STEP: $STEP-- press a key"
echo "-----------------"
read

echo "PRESS TO START"
read

echo ""
echo "---------------TIMER TIMER TIMER single-----"
echo "----------RUNNING a MAX(FROM) time $FROM----"
for j in $(seq $FROM $STEP $TO); do 
	echo ""
	echo "---------------TIMER TIMER TIMER single-----"
	echo "----------RUNNING a MAX(FROM) time $j----"
	if [ -d PROBSLOWERTHAN$j ]; then
		echo "directory PROBSLOWERTHAN$j already there"
	else
		mkdir PROBSLOWERTHAN$j
	fi
	for i in x* ; do
		 ./a $i $j >> OUTPUT; 
		if [ $? = 10 ]; then
			echo -n "[$i]:LEAVING ";
		else echo ""; echo "[$i]:SUCCESS -\> PROBSLOWERTHAN$j";
			mv $i PROBSLOWERTHAN$j;
		fi
	done;
done


