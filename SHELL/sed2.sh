awk '{print $1}' i1.txt > icol1.txt
awk '{print $2}' i1.txt > icol2.txt
sed 's/2/b/g' icol1.txt | sed 's/3/c/g'| sed 's/4/d/g'| sed 's/5/e/g'| sed 's/6/f/g'| sed 's/7/g/g'| sed 's/8/h/g'| sed 's/9/i/g'| sed 's/T/j/g'| sed 's/J/a/g'| sed 's/Q/l/g'| sed 's/K/m/g'| sed 's/A/n/g' > icol1text.txt
paste icol1text.txt icol2.txt > ib4sort.txt
sort ib4sort.txt > is1.txt
sed 's/	/ /' is1.txt > isorted.txt
