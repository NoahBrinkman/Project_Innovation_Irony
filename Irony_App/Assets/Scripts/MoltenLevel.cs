using UnityEngine;
using Adobe.Substance.Runtime;

public class MoltenLevel : MonoBehaviour
{
    public SubstanceRuntimeGraph mySubstance;
    [SerializeField] private float BrightTimer = 1;
    private float currentTimer = 0f;
    [HideInInspector] public float SubstanceBrightness = 0.0f;
    bool toBright;

    // Start is called before the first frame update
    void Start()
    {       
        mySubstance.SetInputFloat("moltenlevel", 0.0f);
    }


    public void UpdateSubstance(float brightnessFloat)
    {
 

        mySubstance.SetInputFloat("moltenlevel" , brightnessFloat);
        mySubstance.RenderAsync();
    }
}
