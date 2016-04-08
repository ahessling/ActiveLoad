# Active Load with microcontroller
This repository contains the hardware design, the firmware and the GUI software of my active load device.

Features:
* Power supply from around 3.3V to 40V
* Should be powered by device under test
* Alternative power supply via USB
* Maximum dissipated power: Around 12W permanently at room temperature
* Maximum current: 3A
* Robust (protected from overvoltage, overcurrent and overtemperature conditions)
* Device under test should be connected via banana plugs or screw terminals
* A PC should be able to monitor the actual current and control the desired current via USB
* Small display to monitor actual and setpoint current
* Rotary encoder to change setpoint current
* Small (5×10 cm)
* Measure temperature at heatsink

The whole design process with a lot of details are written down [in my blog](http://blog.andrehessling.de/2016/02/07/project-active-load-with-microcontroller-part-1-requirements/).
