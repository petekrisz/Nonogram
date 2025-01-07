
  _   _                    _____                      
 | \ | |                  / ____|                     
 |  \| | ___  _ __   ___ | |  __  _ __   __ _ _ __ ___  
 | . ` |/ _ \| '_ \ / _ \| | |_ || '__| / _` | '_ ` _ \ 
 | |\  | (_) | | | | (_) | |__| || |   | (_| | | | | | |
 |_| \_|\___/|_| |_|\___/ \_____||_|    \__,_|_| |_| |_|
                                                     
                                                     

Installation Instructions:
--------------------------
1. Run `setup.exe` to start the installation process.
2. The game will be installed in the your D: drive, but this can be changed during installation process if needed.
   Default installation path: `D:\NonoGram`
3. However, keep in mind that installing into Program folders is not remommended since Operation System could prevent database modification to be      updated upon exit in .csv files due to write permission restrictions. If you do not have a drive D: install the upp in your User folder.

Configuring SQL Server in INI File:
-----------------------------------
1. Open the `config.ini` file located in the installation directory.
2. Update the SQL server login details as shown below:

```ini
[Database]
Server=localhost
Port=3306
Username=your_username (by default it is set to root)
Password=your_password (by default it is empty)


ABOUT NONOGRAM PUZZLE

A nonogram, also known as picross, griddler, or Hanjie, is a logic puzzle where you fill a grid with black and empty (or colored) squares based on numerical clues to reveal a hidden picture. The numbers beside and above the grid indicate the lengths of consecutive filled squares in the respective rows and columns.

Basic Rules:

Numbers Meaning:
*If you see numbers like {3, 2} next to a row or column, it means you need to color three consecutive squares, leave at least one empty square, and then color two more consecutive squares.
*There must be at least one empty square between different blocks of filled squares.
Filling Squares:
*You can fill a square if you are sure it belongs to a specific number. Sometimes this is obvious, but other times it is not.
*For example, if a row has 20 squares and the number next to it is 16, you can be sure that squares 5-16 will be filled, even if you don't know the exact positions of squares 1-4 and 17-20.
*You can mark a square with an 'x' or leave it empty if you are sure it is not part of any sequence.
Logical Filling:
*Use the numbers beside and above the grid to deduce which squares to fill or leave empty.
*Generally, start with rows or columns with large numbers or many numbers, as these provide more clues.

Enjoy playing NonoGram!

NonoGram Team