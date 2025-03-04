<h3>Notes: c.w.</h3>
<br></br>
<h3>Update (11::18-01-25):</h3>
<pre>
	<s>o looking now to tidy part2's (11-24) of every year (16-01-2025) </s>
	o dup2 needed an fflush -- now no debug -- sorry!
	o Result of Tidy every year <s>but</s> including 2024: (+if the file is not present, it has not been coded )
	conor.williams@DESKTOP-MM5HH8I ~/AOC-main
$ cat lool.txt
        2015
2015/day12/day12.2.c:   printf("broken\n"); exit(0);
2015/day19/day19.2.c:   printf("broken\n"); exit(0);
        2016
2016/day22/day22.2.c:   printf("broken..\n"); exit(0);
        2017
2017/day16/day16.2.c:   printf("broken...\n"); exit(0);
2017/day18/day18.2.c:   printf("broken...\n"); exit(0);
        2018
2018/day23/day23.2.c:   printf("broken\n"); exit(0);
        2019
2019/day14/day14.2.c:   printf("broken\n"); exit(0);
2019/day20/day20.2.c:   printf("broken\n"); exit(0);
2019/day23/day23.2.c:   printf("broken\n"); exit(0);
        2021
2021/day12/day12.2.c:   printf("broken...\n"); exit(0);
        2022
2022/day23/day23.2.c:   printf("broken\n"); exit(0);
        2023
2023/day11/day11.2.c:   print("broken..\n"); exit(0);
2023/day13/day13.2.c:   printf("broken...\n"); exit(0);
2023/day14/day14.2.c:   printf("broken...\n"); exit(0);
2023/day15/day15.2.c:   printf("broken...\n"); exit(0);
2023/day21/day21.2.c:   printf("broken...\n"); exit(0);
2023/day23/day23.2.c:   printf("broken...\n"); exit(0);

	o all <s>day</s>Part 1's for every year working well, except:
		-- 2016 Day 11,1
	   	-- 2018 Day 21.1
		-- 2019 Day 18 part1
		-- <s>2022 Day 22.1</s>
	   	-- 2023 Day 22.1
	o all years days 1--10 Part 1&2 working well, except:
		-- 2017 Day 7.2
		-- <s>2018 Day 7 1&2</s>
	   	-- 2022 Day 9.2
	o Timer being added (an automatic kill, if you will) to the slow ones so they don't run too long
	        (but you might not get the answer)
		(timer length can be changed in code before a compile) TimerSet(Xseconds);
</pre>
<h3>Note: (of course you may already know this)</h3>     
<br>if you are not interested in waiting for one to finish
<br>and you want the system to continue, then use 
<br>ps and kill (in cygwin, ps,  and maybe ps -ef or ps -aux in linux) eg.:
               <hr>
