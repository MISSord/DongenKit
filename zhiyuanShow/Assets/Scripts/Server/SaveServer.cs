using UnityEngine;
using UnityEngine.SceneManagement;


public class SaveServer : MonoBehaviour
{
    public static void Save()
    {
        PlayerPrefs.SetInt(BaseData.Hp, GameManager.Instance.playState.HP.max);
        PlayerPrefs.SetInt(BaseData.Money, GameManager.Instance.playState.money);
        PlayerPrefs.SetInt(BaseData.Bottles, GameManager.Instance.playState.bottles);
        PlayerPrefs.SetString(BaseData.GameLevel, SceneManager.GetActiveScene().name);
    }

    public static void Load()
    {
        GameManager.Instance.playState.HP = new DoubleInt(PlayerPrefs.GetInt(BaseData.Hp),
            PlayerPrefs.GetInt(BaseData.Hp));
        GameManager.Instance.playState.money = PlayerPrefs.GetInt(BaseData.Money);
        GameManager.Instance.playState.bottles = PlayerPrefs.GetInt(BaseData.Bottles);
    }
}
