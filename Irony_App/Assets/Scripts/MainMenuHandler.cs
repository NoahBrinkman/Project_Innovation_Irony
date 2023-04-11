using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MainMenuHandler : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputField;
    
    // Start is called before the first frame update
    void Start()
    {
        inputField.onValueChanged.AddListener(delegate(string arg0) { editPassCode(); });
    }

    private void editPassCode()
    {
        MobileNetworkClient.Instance.EditPasscode(inputField.text);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
