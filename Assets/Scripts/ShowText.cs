using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowText : MonoBehaviour
{

    public GameObject car;
    private CarController car_script;
    private int carSpeed;

    public string textValue = "";
    public TMP_Text textElement;
    
    // Flashing parameters
    public float flashSpeed = 2f;
    private float flashTimer = 0f;
    private bool isVisible = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        car_script = car.GetComponent<CarController>();
        textValue = "Warning! Slow down!";
    }

    // Update is called once per frame
    void Update()
    {
        carSpeed = car_script.displaySpeed;
        if(carSpeed > 50)
        {
            // Flash the text
            flashTimer += Time.deltaTime * flashSpeed;
            if(flashTimer >= 1f)
            {
                flashTimer = 0f;
                isVisible = !isVisible;
                textElement.enabled = isVisible;
            }
            textElement.text = textValue;
        }
        else
        {
            textElement.text = "";
            textElement.enabled = true;
            flashTimer = 0f;
            isVisible = true;
        }
    }
}
