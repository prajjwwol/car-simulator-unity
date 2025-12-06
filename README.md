# SimDash - Unity Arduino Car Simulation

This project connects a Unity car game with Arduino Uno to display speed on a 3-digit 7-segment display and control a fan motor.

## Project Deliverables
- Refer to Miro Board: https://miro.com/app/board/uXjVJs10bzI=/

## Components

- **Unity**: Simple car game with steering wheel input
- **Arduino Uno**: 7-segment display (5641AS) and motor control
- **Display**: 5641AS 4-digit 7-segment display (common cathode) - using 3 digits
- **Motor**: 5V fan motor

## Hardware Setup

![Circuit Diagram](Circuit/simdash.png)

### 1. Connect 5641AS 7-Segment Display (3 digits) to Arduino:

**Segment Pins** (connect all 8 segments):
- Segment pins connected to: **9, 2, 3, 5, 6, 8, 7, 4**
  - Pin 9 → Segment a
  - Pin 2 → Segment b
  - Pin 3 → Segment c
  - Pin 5 → Segment d
  - Pin 6 → Segment e
  - Pin 8 → Segment f
  - Pin 7 → Segment g
  - Pin 4 → Segment dp (decimal point)

**Digit Pins** (only connect first 3 digits):
- Digit 1 (leftmost) → Pin **10**
- Digit 2 (middle) → Pin **11**
- Digit 3 (rightmost) → Pin **12**
- Digit 4 → **Not connected** (pin 13 is used for motor)

### 2. Connect 5V Motor to Arduino:
- Motor positive to digital pin **13** (PWM)
- Motor negative to GND via transistor for proper current handling


### 3. Connect Arduino to computer via USB for serial communication.

## Software Setup

### Arduino

1. Install **SevSeg** library in Arduino IDE (Sketch > Include Library > Manage Libraries > Search for "SevSeg")
2. Upload `ArduinoCode/simdash.ino` to Arduino Uno for Unity integration
   - Or upload `ArduinoCode/simdash_test/simdash_test.ino` for standalone testing without Unity
3. Note the COM port (e.g., COM3 on Windows, /dev/ttyACM0 on Linux)

### Unity

1. Open the project in Unity (2020+ recommended)
2. In `SerialCommunicator.cs`, update `portName` to match your Arduino port (e.g., "COM3" for Windows or "/dev/ttyACM0" for Linux)
3. Open the `Assets/Scenes/CarDrivingScene.unity` scene
4. The scene includes:
   - **Car**: A cube with CarController and Rigidbody components
   - **Ground**: A large plane for driving
   - **Main Camera**: Positioned above and behind for a good view
   - **Directional Light**: Provides scene lighting
   - **GameManager**: Contains SerialCommunicator for Arduino connection

## Running the Project

### With Unity Integration:
1. Upload `simdash.ino` and note the port
2. Open Unity project
3. Run the scene
4. Use steering wheel to control car
5. Speed will be sent to Arduino, updating display and fan

### Standalone Testing (Without Unity):
1. Upload `simdash_test.ino`
2. Open Serial Monitor (9600 baud)
3. Watch the display count from 0-100 and back
4. Feel the motor speed up and slow down automatically

## Notes

- 3-digit display shows speed 0-100
- Motor PWM is mapped from speed (0-100 → 0-255 PWM)
- Pin 13 used for motor control (4th digit not connected)
- Display uses SevSeg library with common cathode configuration
- Ensure serial port permissions on Linux/Mac (`sudo usermod -a -G dialout $USER`)
