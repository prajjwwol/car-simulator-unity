#include "SevSeg.h"

// Define pin for motor (PWM capable)
// Using pin 13 since we only use 3 digits now
#define MOTOR_PIN 13

SevSeg sevseg;
int speed = 0;

void setup()
{
  Serial.begin(9600);

  // Setup 7-segment display with 3 digits only
  byte numDigits = 3;
  byte digitPins[] = {10, 11, 12}; // Removed pin 13, now free for motor
  byte segmentPins[] = {9, 2, 3, 5, 6, 8, 7, 4};
  bool resistorsOnSegments = true;
  bool updateWithDelaysIn = true;
  byte hardwareConfig = COMMON_CATHODE;

  sevseg.begin(hardwareConfig, numDigits, digitPins, segmentPins, resistorsOnSegments);
  sevseg.setBrightness(90);

  // Setup motor pin
  pinMode(MOTOR_PIN, OUTPUT);
  
  Serial.println("Simdash Test - Speed simulation starting...");
}

void loop()
{
  // Simulate speed increasing from 0 to 100
  for (speed = 0; speed <= 100; speed++)
  {
    // Map speed to motor PWM (0-255)
    int motorSpeed = map(speed, 0, 100, 0, 255);
    analogWrite(MOTOR_PIN, motorSpeed);
    
    // Display speed on 3-digit 7-segment display as plain number (e.g., 50)
    sevseg.setNumber(speed, 0);
    
    // Refresh display multiple times for smooth display
    for (int i = 0; i < 50; i++)
    {
      sevseg.refreshDisplay();
      delay(1);
    }
    
    Serial.print("Speed: ");
    Serial.println(speed);
  }
  
  delay(1000);
  
  // Simulate speed decreasing from 100 to 0
  for (speed = 100; speed >= 0; speed--)
  {
    // Map speed to motor PWM (0-255)
    int motorSpeed = map(speed, 0, 100, 0, 255);
    analogWrite(MOTOR_PIN, motorSpeed);
    
    // Display speed on 3-digit 7-segment display as plain number
    sevseg.setNumber(speed, 0);
    
    // Refresh display multiple times for smooth display
    for (int i = 0; i < 50; i++)
    {
      sevseg.refreshDisplay();
      delay(1);
    }
    
    Serial.print("Speed: ");
    Serial.println(speed);
  }
  
  delay(2000);
}
