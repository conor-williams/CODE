#!/bin/sh

if [ @$1 = @ ]; then
        echo Usage: conv2.sh i1.txt
        echo "NOTE: change final location"
        exit 1
fi
rm tmp1.txt
rm tmp2.txt

sed 's/^\(...\): \(...\) \(...\) \(...\) \(...\) \(...\) \(...\) \(...\) \(...\)/\1 \2\n\1 \3\n\1 \4\n\1 \5\n\1 \6\n\1 \7\n\1 \8\n\1 \9/' $1 | sed 's/^\(...\): \(...\) \(...\) \(...\) \(...\) \(...\) \(...\) \(...\)/\1 \2\n\1 \3\n\1 \4\n\1 \5\n\1 \6\n\1 \7\n\1 \8/'| sed 's/^\(...\): \(...\) \(...\) \(...\) \(...\) \(...\) \(...\)/\1 \2\n\1 \3\n\1 \4\n\1 \5\n\1 \6\n\1 \7/' | sed 's/^\(...\): \(...\) \(...\) \(...\) \(...\) \(...\)/\1 \2\n\1 \3\n\1 \4\n\1 \5\n\1 \6/' | sed 's/^\(...\): \(...\) \(...\) \(...\) \(...\)/\1 \2\n\1 \3\n\1 \4\n\1 \5/' | sed 's/^\(...\): \(...\) \(...\) \(...\)/\1 \2\n\1 \3\n\1 \4/' | sed 's/^\(...\): \(...\) \(...\)/\1 \2\n\1 \3/' | sed 's/^\(...\): \(...\)/\1 \2/' >> tmp2.txt


echo "graph graph3 {" > tmp1.txt
sed 's/ $//' tmp2.txt | sed 's/://g' | sed 's/ / -- /g' | sed 's/$/;/' >> tmp1.txt
echo \} >> tmp1.txt
touch out.svg

rm outDOT.svg
touch outDOT.svg
dot -Tsvg tmp1.txt -o outDOT.svg -Kneato
ls -l outDOT.svg
cp outDOT.svg /cygdrive/c/Users/conor.williams/Desktop/xyzday25.svg
echo "DONE press a key..."
read


~         
