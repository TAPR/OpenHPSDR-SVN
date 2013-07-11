README.TXT   July 10, 2013    Tom McDermott, N5EG

The SVN repository  ./trunk/N5EG/GRC3.7 contains the gnuradio module for Hermes/Metis for GRC Version 3.7.

At this time, there appears to be a bug in GRC3.7 where the emitted python code references the wrong subdirectory for firdes. This means that band pass filters don't seem to work in GRC3.7.

Testing on GRC3.7 is pretty limited until Gnuradio fixes the bug.


The SVN repository  ./trunk/N5EG/GRC3.6 contains the gnuradio module for Hermes/Metis for GRC Version 3.6 (tested up through 3.6.5).

Right now, gnuradio only runs on linux. As far as I know, there is no windows
or Mac version available.

Gnuradio is structured to deploy modules by building from source, there is no
way that I know to distribute just a binary (but I'm not an expert).

The document "How to build Gnuradio Hermes-Metis.pdf" describes how do do the build.

The directory gr-hpsdr must be held intact, but you can put it where ever you
would like on your system.

The directory grc_applications contains some samples to show how to use the 
HermesNB module in Gnu Radio Companion (GRC) the gui front end for gnuradio.




