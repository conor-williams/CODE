for i in 'GTK3Agg' 'GTK3Cairo' 'GTK4Agg' 'GTK4Cairo' 'MacOSX' 'nbAgg' 'QtAgg' 'QtCairo' 'Qt5Agg' 'Qt5Cairo' 'TkAgg' 'TkCairo' 'WebAgg' 'WX' 'WXAgg' 'WXCairo' 'agg' 'cairo' 'pdf' 'pgf' 'ps' 'svg' 'template'
do
	echo $i
	python 3dline.py $i
done
