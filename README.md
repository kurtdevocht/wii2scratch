# wii2scratch
Control you Scratch projects with one or more Wii controllers!
Windows only...

##Step 1 - Make sure .Net framework 4.5.1 is installed
Installed by default on Windows 8.1 and higher. If needed, it can be installed from http://www.microsoft.com/en-US/download/details.aspx?id=40779

##Step 2 - Connect 1 or more Wii controllers to your pc
Not guaranteed to work on all pc's... You need bluetooth and a compatible driver.

1. Go to bluetooth settings and make sure your pc is discovering devices;
2. Press and hold the '1' and '2' button of your Wii controller simultaneous. An 'Input device - Ready to pair' should appear in the list;
3. Click on the input device and click the 'Pair' button. If a passcode is requested, leave it empty and click 'Next';
4. The device should be named 'Nintendo RVL-CNT-01'. Status should be 'Connected'.

##Step 3 - Run the Scratch helper application
Build the Scratch helper application (Visual studio 2013) or run the binary 'w2s.exe' that can be found in the bin\release folder. It should detect your Wii controller and start listening for http requests sent by Scratch. Can be verified by browsing to http://localhost:15002/poll

##Step 4 - Import blocks in Scratch
This only works in the offline Scratch editor (https://scratch.mit.edu/scratch2download/).

1. Hold the shift-key and click the 'File' menu;
2. Click 'Import experimental HTTP extension';
3. Import the file 'Wii2Scratch.s2e';
4. In the 'More blocks' section a sub-section 'Wii2Scratch' should appear. A green light should indicate a succesful connection with the helper application;
5. the blocks should be self-explaining :-)


Have fun!
