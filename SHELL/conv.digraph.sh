echo "give a file name"
echo "digraph graph3 {" > tmp1.txt
echo "node [shape = circle, ordering=out];" > tmp1.txt
sed 's/ $//' $1 | sed 's/://g' | sed 's/ / -> /g' | sed 's/$/;/' >> tmp1.txt
echo \} >> tmp1.txt
touch out.svg



rm outDOT.svg
touch outDOT.svg
dot -Tsvg tmp1.txt -o outDOT.svg -Kneato
ls -l outDOT.svg
cp outDOT.svg /cygdrive/c/Users/conor.williams/Desktop/ut1.svg
echo "press.."
read



rm outNEATO.svg
touch outNEATO.svg
neato -Tsvg tmp1.txt -o outNEATO.svg
ls -l outNEATO.svg
cp outNEATO.svg /cygdrive/c/Users/conor.williams/Desktop/ut2.svg
echo "press.."
read



rm outSFDP.svg
touch outSFDP.svg
sfdp -Tsvg tmp1.txt -o outSFDP.svg
ls -l outSFDP.svg
cp outSFDP.svg /cygdrive/c/Users/conor.williams/Desktop/ut3.svg
