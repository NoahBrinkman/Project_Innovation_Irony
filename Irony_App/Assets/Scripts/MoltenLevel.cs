using UnityEngine;
using Adobe.Substance.Runtime;

public class MoltenLevel : MonoBehaviour
{
    public SubstanceRuntimeGraph mySubstance;

    // Start is called before the first frame update
    void Start()
    {       
        mySubstance.SetInputFloat("moltenlevel", 0.0f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateSubstance();
        }
    }

    public void UpdateSubstance()
    {
        mySubstance.SetInputFloat("moltenlevel" , 0.7f);
        mySubstance.RenderAsync();
    }
}
