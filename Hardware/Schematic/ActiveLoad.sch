EESchema Schematic File Version 2
LIBS:power
LIBS:device
LIBS:transistors
LIBS:conn
LIBS:linear
LIBS:regul
LIBS:74xx
LIBS:cmos4000
LIBS:adc-dac
LIBS:memory
LIBS:xilinx
LIBS:microcontrollers
LIBS:dsp
LIBS:microchip
LIBS:analog_switches
LIBS:motorola
LIBS:texas
LIBS:intel
LIBS:audio
LIBS:interface
LIBS:digital-audio
LIBS:philips
LIBS:display
LIBS:cypress
LIBS:siliconi
LIBS:opto
LIBS:atmel
LIBS:contrib
LIBS:valves
LIBS:stm32f0
LIBS:bts117
LIBS:ActiveLoad-cache
EELAYER 25 0
EELAYER END
$Descr A3 16535 11693
encoding utf-8
Sheet 1 1
Title "Active Load (MCU controlled)"
Date ""
Rev "1"
Comp ""
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L STM32F072CB U?
U 1 1 569BF24D
P 7100 4150
F 0 "U?" H 5600 6050 60  0000 C CNN
F 1 "STM32F072C8" H 8350 2250 60  0000 C CNN
F 2 "LQFP48" H 7100 4150 40  0000 C CIN
F 3 "" H 7100 4150 60  0000 C CNN
	1    7100 4150
	1    0    0    -1  
$EndComp
$Comp
L BTS117 Q?
U 1 1 569BF4D2
P 3350 1900
F 0 "Q?" H 3550 1975 50  0000 L CNN
F 1 "BTS117" H 3550 1900 50  0000 L CNN
F 2 "TO-220" H 3550 1825 50  0000 L CIN
F 3 "" H 3350 1900 50  0000 L CNN
	1    3350 1900
	1    0    0    -1  
$EndComp
$Comp
L MCP6002 U?
U 1 1 569BF6A4
P 10250 1900
F 0 "U?" H 10250 2050 50  0000 L CNN
F 1 "MCP6002" H 10250 1750 50  0000 L CNN
F 2 "" H 10150 1950 50  0000 C CNN
F 3 "" H 10250 2050 50  0000 C CNN
	1    10250 1900
	1    0    0    -1  
$EndComp
$Comp
L MCP6002 U?
U 2 1 569BF725
P 10350 2600
F 0 "U?" H 10350 2750 50  0000 L CNN
F 1 "MCP6002" H 10350 2450 50  0000 L CNN
F 2 "" H 10250 2650 50  0000 C CNN
F 3 "" H 10350 2750 50  0000 C CNN
	2    10350 2600
	1    0    0    -1  
$EndComp
Text Notes 9600 1350 0    60   ~ 0
Use MCP6442 for better performance
$Comp
L HEATSINK HS?
U 1 1 569BF978
P 3500 1300
F 0 "HS?" H 3500 1500 50  0000 C CNN
F 1 "SK 129-50 STS" H 3500 1250 50  0000 C CNN
F 2 "Heatsinks:Heatsink_Fischer_SK129-STS_42x25mm_2xDrill2.5mm" H 3500 1300 50  0001 C CNN
F 3 "" H 3500 1300 50  0000 C CNN
	1    3500 1300
	1    0    0    -1  
$EndComp
$EndSCHEMATC
