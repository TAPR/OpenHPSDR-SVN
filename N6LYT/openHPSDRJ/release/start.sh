SYSTEM=`uname -s`
echo "System is $SYSTEM"
if [ -d lib ];
then
    if [ "$SYSTEM" == "Linux" ]; then
        export LD_LIBRARY_PATH=./lib/linux:$LD_LIBRARY_PATH
    elif [ "$SYSTEM" == "Darwin" ]; then
        export DYLD_LIBRARY_PATH=./lib/mac:$DYLD_LIBRARY_PATH
    fi
    java -jar openHPSDRJ.jar
else
    echo "Directory lib does not exist."
fi
