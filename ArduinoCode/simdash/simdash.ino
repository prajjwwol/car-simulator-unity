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
}

void loop()
{
  // Check for serial data
  if (Serial.available() > 0)
  {
    String data = Serial.readStringUntil('\n');
    speed = data.toInt();
    // Clamp speed to 0-999 for 3-digit display
    speed = constrain(speed, 0, 999);

    // Map speed to motor PWM (0-255)
    int motorSpeed = map(speed, 0, 100, 0, 255); // Assuming speed 0-100
    analogWrite(MOTOR_PIN, motorSpeed);
  }

  // Display speed on 3-digit 7-segment display (with 2 decimal places like 9.99)
  sevseg.setNumber(speed, 2);
  sevseg.refreshDisplay();
}