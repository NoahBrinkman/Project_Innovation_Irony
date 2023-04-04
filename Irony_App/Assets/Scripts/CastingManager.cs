using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingManager : MonoBehaviour
{

    [SerializeField] private float minimumRotationalInput;
    [SerializeField] private float minimumaximumRotationalInput;
    [SerializeField] private float pouringAdditionMultiplier;
    [SerializeField] private List<metals> moltenMetalsInForge;
    [SerializeField] private CastMold currentlyChosenMold;

  
    
    void Start()
    {
        Input.gyro.enabled = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyChosenMold != null)
        {
            float rotationalInput = Mathf.Abs(Input.gyro.attitude.z);
            //Debug.Log(Input.gyro.attitude.x + " , " + Input.gyro.attitude.y + " , "+ Input.gyro.attitude.z );
            if ( rotationalInput >  minimumRotationalInput && rotationalInput < minimumaximumRotationalInput)
            {
                currentlyChosenMold.Fill(rotationalInput);
            }
        }
    }


    public void SelectCurrentMold(CastMold mold)
    {
        if (currentlyChosenMold == mold)
        {
            mold.MoveToStartPosition();
            currentlyChosenMold = null;
        }
        else
        {
            if(currentlyChosenMold != null)
                currentlyChosenMold.MoveToStartPosition();
            
            mold.MoveToPouringPosition();
            currentlyChosenMold = mold;
        }
    }
    
}
