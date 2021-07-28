using UnityEngine;
using UnityEngine.UI;

public class InteractionCanvas : MonoBehaviour
{
    //Button
    public string interaction { get { return InputSettings.InteractionKey.ToString(); } }

    [Header("Componetns")]
    private Text InteractionText;
    private Transform m_InterTextGameObj;

    public void Init()
    {
        m_InterTextGameObj = transform.GetChild(0);
        InteractionText = m_InterTextGameObj.GetComponent<Text>();
        InteractionText.text = interaction; //Assign the text in the UI to our button from InputSettings
        m_InterTextGameObj.gameObject.SetActive(false);

        MessageServer.AddListener<Vector3>(EventType.ShowInteractionKey, ShowInter);
        MessageServer.AddListener(EventType.CloseInteractionKey, CloseInter);
        MessageServer.AddListener<int>(EventType.ShowDamage, ShowDamage);
    }

    private void ShowDamage(int m_damage)
    {
        //GameObject damageText = ObjectManager.Instance.GetObject(BaseData.DamageText);
        //DamageText m_text = damageText.GetComponent<DamageText>();
        //m_text.ShowDamage(m_damage);
    }

    private void ShowInter(Vector3 item)
    {
        m_InterTextGameObj.position = item + new Vector3(0, 1.3f, 0);
        m_InterTextGameObj.gameObject.SetActive(true);
    }

    private void CloseInter()
    {
        m_InterTextGameObj.gameObject.SetActive(false);
    }
}
