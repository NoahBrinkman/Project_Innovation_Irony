using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadSwipeInput : MonoBehaviour
{

    [SerializeField, Tooltip("How many pixels does the finger/cursor need to move to trigger the event of a swipe")] 
    private Vector2 minimumDistance = new Vector2(20, 20);

    [SerializeField] private bool onlyOneSwipeDetectable = true;

    private Vector2 startPosition, endPosition;


    public Action OnSwipeLeft;

    public Action OnSwipeRight;

    public Action OnSwipeUp;

    public Action OnSwipeDown;
   
    private static ReadSwipeInput _instance;

    public static ReadSwipeInput Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;
    }
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPosition = touch.position;
                    break;
                case  TouchPhase.Ended:
                    endPosition = touch.position;
                    DetectSwipes();
                    startPosition = new Vector2();
                    endPosition = new Vector2();
                    break;
            }
        }
    }

    private void DetectSwipes()
    {
        if (startPosition.x - endPosition.x <= -minimumDistance.x)
        {
            OnSwipeRight?.Invoke();
            if (onlyOneSwipeDetectable) return;
        }

        if (startPosition.x - endPosition.x >= minimumDistance.x)
        {
            OnSwipeLeft?.Invoke();
            if (onlyOneSwipeDetectable) return;
        }
        
        if (startPosition.y - endPosition.y <= -minimumDistance.y)
        {
            OnSwipeUp?.Invoke();
            if (onlyOneSwipeDetectable) return;
        }

        if (startPosition.y - endPosition.y >= minimumDistance.y)
        {
            OnSwipeLeft?.Invoke();
            if (onlyOneSwipeDetectable) return;
        }
        
    }
    
}
