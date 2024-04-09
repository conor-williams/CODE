sed 's/\S\S\+//g' input | tsort | xargs -n 1 echo -n; echo
