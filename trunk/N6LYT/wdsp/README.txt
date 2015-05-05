source code version r3457

To build wdsp you will need to install pthreads and fttw3.


There is a simple Makefile than is used to build the shared library libwdsp.so. From the source directory simply run the command:

make


You can also build the shared library libwdspj.so which contains the Java JNI interface code to be able to use wdsp from a Java application. You will need to install the Java JDK and make sure that the JAVA_HOME environment variable is pointing to the install location of the JDK. Simply run the command:

make java


Once successfully built you can run the following command to install the shared library and header fileinto /usr/local:

sudo make install


