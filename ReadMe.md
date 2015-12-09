# App Finder

AppFinder is a small application with a simple purpose in mind. Easily creating shortcuts for the portable apps on your USB.

##### Using App Finder:

1. First clone the repository and build the application. After that, just copy the build folder somewhere on your PC. <br> <br>
    ![Step 1](http://i.imgur.com/A24pIPG.jpg) <br> <br>
2. Now that we have that covered, run the application. You will recieve a prompt as seen below: <br> <br>
    ![Step 2-1](http://i.imgur.com/l0CZgiT.jpg)       <br> <br>
**Phase 1:** Now the application will start chewing through your USB drives and it will search for any portable apps installed (it searches for applications ending in 'Portable.exe' as per convention, all the applications found on [PortableApps](http://portableapps.com/) or any other portable app provider respect this convention).<br>
**Phase 2:** After *Phase 1*, it will extract all of the icons in the folder 'Icons' that can be found in the 'AppFinder' root folder.<br>
**Phase 3:** After *Phase 2*, it will generate a folder on your desktop, named 'AppFinder - Portable Shortcuts' which will contain all of the portable app shortcuts. *NOTE:* when the generation is done, you will recieve a notification as show below:<br> <br>
   ![Step 2-2](http://i.imgur.com/BZjgZjk.jpg) <br> <br>
3. After Step 2 is complete you can find all of your the shortcuts to your portable apps in the folder 'AppFinder - Portable Shortcuts' on your Desktop.<br> <br>
  ![Step 3](http://i.imgur.com/llmBWFJ.jpg) <br><br>
  You can now see that all of the shortcuts point to the AppFinder application with two arguments: '-f' and '-d'.<br><br>
  ```  C:\AppFinder\AppFinder.exe -f "2048Portable.exe" -d "Portable Apps\Games\2048" ```<br><br>
 '-f' arg -> application executable.
 '-d' arg -> the application root directory, excludes the drive letter, searching within all the removable devices to determine if the application can be found.
 
***Self Healing Feature***<br><br>
Take the following scenario into consideration: you remove the application or you restructure the file contents of your USB device. AppFinder will search for the application executable through all of your removable devices and regenerate the shortcut if it cannot be found on first run.
