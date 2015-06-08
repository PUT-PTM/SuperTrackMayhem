# SuperTrackMayhem

##Overview
# SuperTrackMayhem 
is a simple racing game created as a university project for Microprocessors lab at the Poznan University of Technology. The car can be controlled by both a  keyboard and an accelerometer built into the STM32F407 Discovery board.

## Description
This repository consists of two main parts:

### STM32 project
A CooCox IDE project used for the controller part. Written in C, its main purpose is to send accelerometer data to the PC running the game.

### Unity project
The game itself including the STM connector, and one sample level.

### Tools
[CooCox IDE](www.coocox.com) v1.7.7
[GNU ARM Toolchain](https://launchpad.net/gcc-arm-embedded) v4.8.2014q3
[Unity Engine](https://unity3d.com/) v5.0.2f1
[Blender](http://www.blender.org/) v2.74
[Gimp](http://www.gimp.org/) v2.8.14


### How to run
Builds will be available soon.

A virtual COM Port is used to establish connection between the board and the PC. The only virtual COM Port driver this project was tested with (under Window 8.1) is available under:
http://www.st.com/web/en/catalog/tools/PF257938

### How to compile
1. Build the STM32 Project and upload it to the STM32F407 board (CooCox IDE project files are provided).
2. Build the Unity project using the Unity 5 Editor.
3. Edit/create the configuration file in the game directory ("SuperTrackMayhem.config"). Sample config file is available in the UnityProject folder.

### Future improvements
No future improvements are planned for the game. Feel free to create additional levels, improve the camera work, tweak the driving model and hack the game code to your heart's content.

### Attributions
The STM project is based upon 
https://github.com/xenovacivus/STM32DiscoveryVCP

### License
SuperTrackMayhem is released under the [MIT License](http://opensource.org/licenses/MIT). 

### Credits 
Project made by *Jacek Przemieniecki* and *Bartosz Litwiniuk*.

*The project was conducted during the Microprocessor Lab course held by the Institute of Control and Information Engineering, Poznan University of Technology.*
Supervisor: [Michał Fularz](https://github.com/Michal-Fularz)

