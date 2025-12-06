int motorPin = 3;

void setup()
{
    pinMode(motorPin, OUTPUT);
    Serial.begin(9600);
}

void loop()
{
    for (int speed = 0; speed <= 255; speed += 3)
    {
        analogWrite(motorPin, speed);
        delay(20);
    }

    delay(500);

    for (int speed = 255; speed >= 0; speed -= 3)
    {
        analogWrite(motorPin, speed);
        delay(20);
    }

    delay(1000);
}