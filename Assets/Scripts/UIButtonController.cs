using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonController : MonoBehaviour
{
    public bool isPressed;          
    public float pressValue = 0;   
    public float sensitivity = 2f; 
    // Start is called before the first frame update
    void Start()
    {
        addListener();              
    }

    // Update is called once per frame
    void Update()
    {
        // update pressvalue
        if (isPressed)
        {
            pressValue += sensitivity * Time.deltaTime;
        }
        else
        {
            pressValue -= sensitivity * Time.deltaTime;
        }
        pressValue = Mathf.Clamp01(pressValue);
    }
    void addListener()
    {
        //create a new event trigger
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

        //add listener to pointer down action
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => { isPressed = true; });

        //add listener to pointer up action
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => { isPressed = false; });

        //add the EventTrigger to the button
        trigger.triggers.Add(pointerDown);
        trigger.triggers.Add(pointerUp);


    }

    
}
