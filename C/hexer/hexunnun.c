#include <stdio.h>
#include <stdlib.h>
#include <map>
#include <assert.h>
#include <cstring>
#include <ctype.h>
#include <stdio.h>
#include <unistd.h>
#include <string>

using namespace std;

//enum legend {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};
//enum legend {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, a, b, c, d, e, f};
enum legend {one, two};
void convertF(char *hex);
void convert2(char *nonhex);
#define SZ 1000

int main(int argc, char **argv) {
	int opt; 

	// put ':' in the starting of the 
	// string so that program can  
	//distinguish between '?' and ':'  
	//0f717dc2
	char hex[SZ];
	char nonhex[SZ];
	int giveH = 0;
	int forward = 0;
	int back = 0;
	while((opt = getopt(argc, argv, "fbh:i")) != -1)  
	{  
		switch(opt)  
		{  
			case 'f':  
				forward = 1;
				//printf("option: forward %c\n", opt);  
				break;  
			case 'b':
				back = 1;
				//printf("option: back %c\n", opt);
				break;
			case 'h':  
				giveH = 1;
				//printf("conver hex: %s\n", optarg);  
				if ((int)strlen(optarg) > SZ-1) {
					printf("hex too long...\n");
					exit(0);
				}
				strcpy(hex, optarg);
				strcpy(nonhex, optarg);
				//convertForward(hex);
				break;  
			case '?':  
				printf("unknown option: %c (continueing)\n", optopt); 
				break;  
		}  
	}  
	//if (forward && back) {perror("error cannot do both\n");}
	if (giveH == 0) {perror("must give h string\n");exit(0);}
	if (back == 1) {/*printf("converting forward:\n"); */ convertF(hex);}
	if (forward == 1) {/*printf("converting back\n"); */ convert2(nonhex);}
	if (forward == 0 && back == 0) {/*printf("converting forward\n"); */convertF(hex);}

	//char hex[] = "0f717dc1";
}
void convert2(char *nonhex) {
	map <string, char> mp;

	int len = (int)strlen(nonhex);

	mp["00"] = '0'; mp["01"] = '1'; mp["02"] = '2'; mp["03"] = '3'; mp["04"] = '4';
	mp["05"] = '5'; mp["06"] = '6'; mp["07"] = '7'; mp["08"] = '8'; mp["09"] = '9';
	mp["10"] = 'a'; mp["11"] = 'b'; mp["12"] = 'c'; mp["13"] = 'd'; mp["14"] = 'e';
	mp["15"] = 'f';


	char ans[1000]; ans[0] = '\0';
	for (int kkk = 0; kkk < len; kkk += 2) {
		 string bla = "ZZ";
		 bla[0] = nonhex[kkk];
		 bla[1] = nonhex[kkk+1];
		 bla[2] = '\0';
		 
		 if (mp.find(bla) != mp.end()) {
			 sprintf(ans, "%s%c", ans, mp[bla]);
		 } else {
			 printf("bad input: %s\n", bla);
			 exit(0);
		}
	 }

	 printf("tohex: [%s]\n", ans);
}
void convertF(char *hex) {
	map <char, string> mp;
	mp['0'] = "00"; mp['1'] = "01"; mp['2'] = "02"; mp['3'] = "03"; mp['4'] = "04";
	mp['5'] = "05"; mp['6'] = "06"; mp['7'] = "07"; mp['8'] = "08"; mp['9'] = "09";
	mp['a'] = "10"; mp['b'] = "11"; mp['c'] = "12"; mp['d'] = "13"; mp['e'] = "14";
	mp['f'] = "15";

	int len = (int)strlen(hex);
	char ans2[SZ] = ""; 
	for (int jj = 0; jj < len; jj++) {
		if (mp.find(hex[jj]) != mp.end()) {
			sprintf(ans2, "%s%s", ans2, mp[hex[jj]].c_str());
		} else {
			printf("bad input: %s\n", mp[hex[jj]].c_str());
			exit(0);
		}
	}
	//assert((int)strcmp(ans1, ans2) == 0);
	int pos = 0;
	do {
		//printf("%d", mp[hex[pos]]);
		pos++;
	} while (hex[pos] != '\0');
	printf("fromhex: %s\n", ans2);
}

// Program to illustrate the getopt()
// function in C geekforgeeks


/*
   int main(int argc, char *argv[])
   {
   int opt; 

// put ':' in the starting of the 
// string so that program can  
//distinguish between '?' and ':'  
while((opt = getopt(argc, argv, ":if:lrx")) != -1)  
{  
switch(opt)  
{  
case 'i':  
case 'l':  
case 'r':  
printf("option: %c\n", opt);  
break;  
case 'f':  
printf("filename: %s\n", optarg);  
break;  
case ':':  
printf("option needs a value\n");  
break;  
case '?':  
printf("unknown option: %c\n", optopt); 
break;  
}  
}  

// optind is for the extra arguments 
// which are not parsed 
for(; optind < argc; optind++){      
printf("extra arguments: %s\n", argv[optind]);  
} 

return 0; 
} 
*/
//void convertBack(char *hex) {
//	char ans[1000];
//	ans[0] = '\0';
//	//union? of char and int
//	union asdf {
//		int ccI;
//		char ccD;
//	};
//	map <string, asdf> mp;
//	union asdf var_asdf;
//	/*
//	   var_asdf.ccI = 0;
//	   mp["00"] = var_asdf; 
//	   var_asdf.ccI = 1;
//	   mp["02"] = (asdf)1;
//	   */
//
//	var_asdf.ccI = 0;   mp["00"] = var_asdf;
//	printf("sizeof mp[00] is %d\n", sizeof(mp["00"]));
//	printf("%d --- \n", mp["00"]);
//	var_asdf.ccI = 1;   mp["01"] = var_asdf; 
//	var_asdf.ccI = 2;   mp["02"] = var_asdf;
//	var_asdf.ccI = 3;   mp["03"] = var_asdf;
//	var_asdf.ccI = 4;   mp["04"] = var_asdf;
//	var_asdf.ccI = 5;   mp["05"] = var_asdf;
//	var_asdf.ccI = 6;   mp["06"] = var_asdf;
//	var_asdf.ccI = 7;   mp["07"] = var_asdf;
//	var_asdf.ccI = 8;   mp["08"] = var_asdf;
//	var_asdf.ccI = 9;   mp["09"] = var_asdf;
//	var_asdf.ccD = 'a'; mp["10"] = var_asdf;
//	printf("sizeof mp[a] is %d\n", sizeof(mp["11"]));
//	printf("%c --- \n", mp["11"]);
//	printf("%d --- \n", mp["11"]);
//	var_asdf.ccD = 'b'; mp["11"] = var_asdf;
//	var_asdf.ccD = 'c'; mp["12"] = var_asdf;
//	var_asdf.ccD = 'd'; mp["13"] = var_asdf;
//	var_asdf.ccD = 'e'; mp["14"] = var_asdf;
//	var_asdf.ccD = 'f'; mp["15"] = var_asdf;
//	/*
//	   mp["10"] = (var_asdf.ccD='a'); mp["11"] = 'b'; mp["12"] = 'c'; mp["13"] = 'd'; mp["14"] = 'e'; mp["15"] = 'f';
//	   */
//	int len = (int)strlen(hex);
//	#define typeof(x) _Generic((x), \
//    int: "int", \
//    float: "float", \
//    double: "double", \
//    char: "char", \
//    default: "unknown")
//	     /*
//#define func_typeof(x) _Generic((x), \
// int: "int", \
// float: "float", \
// double: "double", \
// char: "char", \
// default: "unknown")
// */
//	     //int blablah; printf("%s \n", typeof(blablah));
//	 for (int kkk = 0; kkk < len; kkk += 2) {
//		 //printf("hex[kkk] is %c\n", hex[kkk]);
//		 string bla = "ZZ";
//		 bla[0] = hex[kkk];
//		 bla[1] = hex[kkk+1];
//		 bla[2] = '\0';
//		 //printf("bla: %s %c\n", bla.c_str(), mp[bla]);
//		 //ans[(kkk/2)] = mp[bla]; ans[(kkk/2)+1] = '\0';
//			 if (sizeof(mp[bla]) == 4) {
//				 printf("sizeof is 4\n");
//				 printf("mp[bla] is %d\n", mp[bla]);
//			 sprintf(ans, "%s%d", ans, mp[bla]);
//		 }  else {
//				 printf("sizeof is %d\n", sizeof(mp[bla]));
//				 printf("mp[bla] is %c\n", mp[bla]);
//			 sprintf(ans, "%s%c", ans, mp[bla]);
//		 }
//		 /*
//			 if (strcmp(func_typeof(mp[bla]), "int") == 0) {
//			 printf("%d", mp[bla]);
//		 }  else if (strcmp(func_typeof(mp[bla]), "char") == 0) {
//			 printf("%c", mp[bla]);
//		 }
//		 */
//	 }
//	 for (int kkk = 0; kkk < len; kkk += 2) {
//		 //printf("hex[kkk] is %c\n", hex[kkk]);
//		 string bla = "ZZ";
//		 bla[0] = hex[kkk];
//		 bla[1] = hex[kkk+1];
//		 bla[2] = '\0';
//
//	 printf("backfromhex: [%s]\n", ans);
//}
	/*
	char ans1[SZ] = "";

	for (int ii = 0; ii < len; ii++) {
		//printf("%d ", mp[hex[ii]]);
		sprintf(ans1, "%s%02d", ans1, mp[hex[ii]].c_str());
	}
	//printf("\n");
	//*/

/*
		if (isdigit(hex[jj])) {
			//printf("%02d ", hex[jj]-48);
		} else if (islower(hex[jj])) {
			//printf("%02d ", hex[jj] - 97);
		} else if (isupper(hex[jj])) {
			//printf("%02d ", hex[jj] - 65);
		}
		*/
