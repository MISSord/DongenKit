using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Text m_DamageText;

    public void ShowDamage(int Damage)
    {
        m_DamageText = transform.GetComponent<Text>();
        m_DamageText.text = Damage.ToString();
        transform.gameObject.SetActive(true);
        StartCoroutine(CloseDamage());
    }

    IEnumerator CloseDamage()
    {
        yield return new WaitForSeconds(1.5f);
        transform.gameObject.SetActive(false);
        ObjectManager.Instance.PushObject(transform.gameObject);
    }
}
