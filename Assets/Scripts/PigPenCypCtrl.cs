using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PigPenCypCtrl : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshPro> textMeshObjects = new List<TextMeshPro>(); // List to store TextMeshPro objects

    private Dictionary<string, TextMeshPro> textMeshMap = new Dictionary<string, TextMeshPro>();

    public PersonalRandmValueManager personalRandmValueManager;

    public int seed = 0;
    private int offset = 0;

    public TextMeshPro passwordText;


    void Start()
    {

        // generate a random number between 0-10 with the seed and set the offset to that number
        offset = (int)personalRandmValueManager.GetRandomValue("PigPen_offset");
        Debug.Log($"Offset: {offset}");

        // Populate the dictionary using the name of each TextMeshPro object as the key
        foreach (var textMesh in textMeshObjects)
        {
            if (textMesh != null && !textMeshMap.ContainsKey(textMesh.name))
            {
                textMeshMap.Add(textMesh.name, textMesh);
            }
            else if (textMesh == null)
            {
                Debug.LogWarning("A null TextMeshPro object was found in the list. Skipping.");
            }
            else
            {
                Debug.LogWarning($"Duplicate key found: {textMesh.name}. Skipping.");
            }
        }

        string passwordtxt = personalRandmValueManager.GetRandomValue("PigPen_password").ToString();

        // passwordText.text = passwordtxt;

        // for ech charactor in passwordtxt , get the ascii value of the character and reduce the offset from it and create a new passwordtxt to show and set that to the passwordText
        string newpasswordtxt = "";
        foreach (char c in passwordtxt)
        {
            int ascii = (int)c;
            char newchar = (char)((((ascii + offset) + 26) % 26) + 'A');
            newpasswordtxt += newchar;
        }

        passwordText.text = newpasswordtxt;
        Debug.Log($"Shown Password: {passwordtxt} -> {newpasswordtxt}");

        // Set the the text of each index with the offset
        for (int i = 0; i < textMeshObjects.Count; i++)
        {
            int index = int.Parse(textMeshObjects[i].name) - 1;
            string letter = ((char)((((index + offset) + 26) % 26) + 'A')).ToString();

            SetText(textMeshObjects[i].name, letter);
        }


    }


    void SetText(string key, string text)
    {
        if (textMeshMap.ContainsKey(key))
        {
            textMeshMap[key].text = text;

            Debug.Log($"Text for index: {key} is set to: {text}");
        }
        else
        {
            Debug.LogWarning($"Key not found: {key}");
        }
    }


}
