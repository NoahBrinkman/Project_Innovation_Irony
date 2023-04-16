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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && SubstanceBrightness <= 1)
        {
            toBright = true;
        }

        if (toBright)
        {
            UpdateSubstance();
        }
    }

    public void UpdateSubstance()
    {
        float brightnessDiff = 1 - SubstanceBrightness;

        
        if(currentTimer < BrightTimer)
        {
            SubstanceBrightness += 0.1f;
            currentTimer += Time.deltaTime;
        }


        mySubstance.SetInputFloat("moltenlevel" , SubstanceBrightness);
        mySubstance.RenderAsync();
    }
}
