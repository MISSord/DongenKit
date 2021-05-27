using UnityEngine;
using UnityEngine.UI;

public class InteractionCanvas : MonoBehaviour
{
    //Button
    public string interaction { get { return InputSettings.InteractionKey.ToString(); } }

    [Header("Componetns")]
    public Text InteractionText;

    public void Init()
    {
        InteractionText = transform.GetComponentInChildren<Text>();
        InteractionText.text = interaction; //Assign the text in the UI to our button from InputSettings
    }
}
