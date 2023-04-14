using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using shared;

public class CastingManager : MonoBehaviour
{

    [SerializeField] private float minimumRotationalInput;
    [SerializeField] private float minimumaximumRotationalInput;
    [SerializeField] private float pouringAdditionMultiplier;
    [SerializeField] private List<Metal> moltenMetalsInForge;
    [SerializeField] private CastMold currentlyChosenMold;

    private List<Recipe> recipeBacklog = new List<Recipe>();

    void Start()
    {
        Input.gyro.enabled = true;
        ReadSwipeInput.Instance.OnSwipeRight += OnswipeRight;
        if (MobileNetworkClient.Instance != null)
        {
            MobileNetworkClient.Instance.OnRecipeReceived += OnRecipeReceived;
            MobileNetworkClient.Instance.OnMetalReceived += OnMetalReceived;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (currentlyChosenMold != null)
        {
            if (MobileNetworkClient.Instance == null)
            {
                float rotationalInput = Mathf.Abs(Input.gyro.attitude.z);
                if ( rotationalInput >  minimumRotationalInput && rotationalInput < minimumaximumRotationalInput)
                {
                    currentlyChosenMold.Fill(rotationalInput);
                            
                }
            }
            if(recipeBacklog.Count == 0) return;
                //Debug.Log(Input.gyro.attitude.x + " , " + Input.gyro.attitude.y + " , "+ Input.gyro.attitude.z );
                for (int i = 0; i < recipeBacklog.Count; i++)
                {
                    if (recipeBacklog[i].item == currentlyChosenMold.myItem && moltenMetalsInForge.ContainsAll(recipeBacklog[0].metalRecipe))
                    {
                        float rotationalInput = Mathf.Abs(Input.gyro.attitude.z);
                        if ( rotationalInput >  minimumRotationalInput && rotationalInput < minimumaximumRotationalInput)
                        {
                            currentlyChosenMold.Fill(rotationalInput);
                            
                        }
                        break;
                    }
                }
        }
    }

    void OnMetalReceived(SendMetalResponse message)
    {
        if (message.to == MinigameRoom.Casting)
        {
            moltenMetalsInForge.Add(message.metal);
        }
    }
    
    private void OnswipeRight()
    {
        if (currentlyChosenMold != null)
        {
            if (currentlyChosenMold.fillValue > +currentlyChosenMold.targetFillValue - currentlyChosenMold.fillMargin)
            {
                for (int i = 0; i < recipeBacklog.Count; i++)
                {
                    if (recipeBacklog[i].item == currentlyChosenMold.myItem)
                    {
                        foreach (Metal metal in recipeBacklog[i].metalRecipe)
                        {
                            moltenMetalsInForge.Remove(metal);
                        }
                        break;
                    }
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

    private void OnRecipeReceived(Recipe r)
    {
        recipeBacklog.Add(r);
    }
    
    public void SendToolToServer(Item item, int grade)
    {
        //TODO: Send message with this data;
        for (int i = 0; i < recipeBacklog.Count; i++)
        {
            if (recipeBacklog[i].item == item)
            {
                FinishItemRequest request = new FinishItemRequest();
                request.recipe = recipeBacklog[i];
                MobileNetworkClient.Instance.channel.SendMessage(request);
                recipeBacklog.Remove(recipeBacklog[i]);
                
            }
        }

    }
}
