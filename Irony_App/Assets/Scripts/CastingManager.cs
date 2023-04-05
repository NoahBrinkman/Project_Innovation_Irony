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
        ReadSwipeInput.Instance.OnSwipeRight += OnswipeRight;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentlyChosenMold != null)
        {
            if (moltenMetalsInForge.ContainsAll(currentlyChosenMold.myRecipe.metalRecipe))
            {
                float rotationalInput = Mathf.Abs(Input.gyro.attitude.z);
                //Debug.Log(Input.gyro.attitude.x + " , " + Input.gyro.attitude.y + " , "+ Input.gyro.attitude.z );
                if ( rotationalInput >  minimumRotationalInput && rotationalInput < minimumaximumRotationalInput)
                {
                    currentlyChosenMold.Fill(rotationalInput);
                    
                }
                
            }
            else
            {
                Debug.Log("no");
            }
        }
    }

    private void OnswipeRight()
    {
        if (currentlyChosenMold != null)
        {
            if (currentlyChosenMold.fillValue > +currentlyChosenMold.targetFillValue - currentlyChosenMold.fillMargin)
            {
                foreach (var metal in currentlyChosenMold.myRecipe.metalRecipe)
                {
                    moltenMetalsInForge.Remove(metal);
                }
                StartCoroutine(currentlyChosenMold.SendOffScreen());
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

    public void SendToolToServer(Item item, int grade)
    {
        //TODO: Send message with this data;
    }
}
