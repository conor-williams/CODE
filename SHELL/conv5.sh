#!/bin/sh 
set -xv

if [ @$1 = @ ]; then
        echo Usage: conv2.sh i1.txt
        echo "NOTE: change final location"
        exit 1
fi
rm tmp1.txt
rm tmp2.txt

sed 's/  / /g' $1 | sed 's/^\([A-Za-z]\): \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\)/\1 \2\n\1 \3\n\1 \4\n\1 \5\n\1 \6\n\1 \7\n\1 \8\n\1 \9/' | sed 's/^\([A-Za-z]\): \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\)/\1 \2\n\1 \3\n\1 \4\n\1 \5\n\1 \6\n\1 \7\n\1 \8/'| sed 's/^\([A-Za-z]\): \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\)/\1 \2\n\1 \3\n\1 \4\n\1 \5\n\1 \6\n\1 \7/' | sed 's/^\([A-Za-z]\): \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\)/\1 \2\n\1 \3\n\1 \4\n\1 \5\n\1 \6/' | sed 's/^\([A-Za-z]\): \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\)/\1 \2\n\1 \3\n\1 \4\n\1 \5/' | sed 's/^\([A-Za-z]\): \([A-Za-z]\) \([A-Za-z]\) \([A-Za-z]\)/\1 \2\n\1 \3\n\1 \4/' | sed 's/^\([A-Za-z]\): \([A-Za-z]\) \([A-Za-z]\)/\1 \2\n\1 \3/' | sed 's/^\([A-Za-z]\): \([A-Za-z]\)/\1 \2/' >> tmp2.txt
#    (with-open-file (dotfile "2018-07-1.dot"
#                                     :direction :output
#                                     :if-exists :supersede)
#            (format dotfile "digraph aoc_2018_07_1 {~%")
#            (format dotfile "    rankdir=\"LR\"~%")
#            (format dotfile "~:{    ~A -> ~A~%~}~%" edges-to-graph)
#            (when in-order
#              (iter (for c in-string (step-order! (make-graph-from-edges edges)))
#                    (for cp previous c initially nil)
#                    (when cp
#                      (format dotfile "    ~A -> ~A [style=\"invis\"]~%" cp c))))
#            (format dotfile "}~%"))))
#

#echo "graph graph3 {" > tmp1.txt
echo "digraph graph3 {" > tmp1.txt
#echo "node [shape = circle, ordering=out];" >> tmp1.txt
echo "	rankdir=\"LR\";" >> tmp1.txt
sed 's/ $//' tmp2.txt | sed 's/://g' | sed 's/ / -> /g' | sed 's/$/;/' | sed 's/^/{/' | sed 's/$/}/' >> tmp1.txt
echo \} >> tmp1.txt
touch out.svg

rm outDOT.svg
touch outDOT.svg
dot -Tsvg tmp1.txt -o outDOT.svg -Kneato
ls -l outDOT.svg
cp outDOT.svg /cygdrive/c/Users/conor.williams/Desktop/xyzday25.svg
echo "DONE press a key."
#read
cp outDOT.svg /cygdrive/c/Users/conor.williams/Desktop/
