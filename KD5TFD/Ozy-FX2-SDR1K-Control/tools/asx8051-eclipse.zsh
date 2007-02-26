#  outfile opts  infile
echo 'outfile:' $1
echo 'opts:' $2
echo 'infile: ' $3
ofile=$1
srcin=$3
echo 'ofile: ' $ofile
echo 'outdir: ' ${ofile%/*}
echo 'srcname: ' ${srcin##*/}
mkdir -p ${ofile%/*}
cp $3 ${ofile%/*}
asx8051 $2 ${ofile%/*}/${srcin##*/}
