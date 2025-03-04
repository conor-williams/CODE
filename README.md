<br>
<h2>Notes (this README is a .html file (drag and drop into a browser)) lu:3-03-2025</h2>
<address><b>conor.williams@gmail.com</b></address><br>
<h3>Update (11::20-03-25):</h3>
<br>
<p>
	
<br>
<pre>
	o run "sh FIRST.sh" -- it does everything for you
	o length of filename getting you down? use ln and stop getting down (e.g.):
	         ln -s 2020.day20.i1.txt i1.txt
	o added year and day to input file names and year to c files (no code change just renaming of files)
	o dup2 needed an fflush -- now no debug -- sorry
	o if the file is not present, it has not been coded
	o all <s></s>Part 1's for every year working well, except:
		-- 2016 Day 11,1
	   	-- <s>2018 Day 21.1</s>
		-- <s>2019 Day 18 part1</S> *
		-- <s>2022 Day 22.1</s>
	   	-- <s>2023 Day 22.1</s>
	o all years days 1--10 Part 1&2 working well, except:
		-- <s>2017 Day 7.2</s>
		-- <s>2018 Day 7 1&2</s>
	   	-- 2022 Day 9.2
	o Timer has been added (an automatic kill, if you will) to the slow ones so they don't run too long
	        (but you might not get the answer)
		(timer length can be changed in code before a compile) TimerSet(Xseconds);
     <s>o looking now to tidy part2's (11-24) of every year (16-01-2025) </s>
	o Result of Tidy every year <s>but</s> including 2024:
	conor.williams@DESKTOP-MM5HH8I AOC-main
$ cat lool.txt
	2015
<s>2015&sol;day12&sol;day12.2.c:   printf("broken\n"); exit(0);</s>
<s>2015&sol;day19&sol;day19.2.c:   printf("broken\n"); exit(0);</s>
        2016
<s>2016&sol;day22&sol;day22.2.c:   printf("broken..\n"); exit(0);</s>
        2017
2017&sol;day16&sol;day16.2.c:   printf("broken...\n"); exit(0); (MANUAL CYCLE DETECTION)
<s>2017&sol;day18&sol;day18.2.c:   printf("broken...\n"); exit(0);</s>
        2018
2018&sol;day19&sol;day19.2.c:   printf("broken\n"); ex<s>it(0); (R</s>UNNING CURRENTLY ~2hours??)
2018&sol;day23&sol;day23.2.c:   printf("broken\n"); exit(<s>0</s>); (Running... (nanobots ~2.5hours??)
        2019
<s>2019&sol;day14&sol;day14.2.c:   printf("broken\n"); exit(0);</s>
<s>2019&sol;day20&sol;day20.2.c:   printf("broken\n"); exit(0);</s>
2019&sol;day23&sol;day23.2.c:   printf("broken\n"); exi<s>t(0); </s>(NETWORKING TRICKY USING TIMERS todo(2x-1 = idle)
        2021
<s>2021&sol;day12&sol;day12.2.c:   printf("broken...\n"); exit(0);</s>
        2022
<s>2022&sol;day11&sol;day11.2.c:   printf("broken\n"); exit(0);</s>
<s>2022&sol;day23&sol;day23.2.c:   printf("broken\n"); exit(0);</s>
        2023
<s>2023&sol;day11&sol;day11.2.c:   print("broken..\n"); exit(0); (work from the right m1)</s>
<s>2023&sol;day13&sol;day13.2.c:   printf("broken...\n"); exit(0);</s>
2023&sol;day14&sol;day14.2.c:   printf("broken...\n"); exit(0); (MANUAL CYCLE DETECTION)
<s>2023&sol;day15&sol;day15.2.c:   printf("broken...\n"); exit(0);</s>
2023&sol;day21&sol;day21.2.c:   printf("broken...\n"); exit(0); (DIAMOND CYCLE DETECTION)
2023&sol;day23&sol;day23.2.c:   printf("broken...\n"); exit(0);
</pre>
