using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MeunUI : MonoBehaviour
{
	public DictionaryUI dictUi;
	public CharacterImage characterImage;

    private void Start()
    {
		GameManager.Instance.dictionaryUI = dictUi;
    }

    public void GameStart()
	{
        if (DataManager.Instacne.JsonClass._PlayerData.playerLock[characterImage.CurIndex])
        {
            DataManager.Instacne.DefaultData.SettingValue(characterImage.CurIndex);
            GameManager.Instance.playerCheck = true; //이거 안 써도 될듯
            GameManager.Instance.selectCharacter = (ECharacters)characterImage.CurIndex;

            GameManager.Instance.UpdateDiaryDate((int)GameManager.Instance.selectCharacter + 14); // 기본 총 다이어리 등록
            SceneManager.LoadScene(1);
        }

        /*if (GameManager.Instance.playerCheck)
		{
			GameManager.Instance.UpdateDiaryDate((int)GameManager.Instance.selectCharacter + 14); // 기본 총 다이어리 등록
			SceneManager.LoadScene(1);
		}*/
	}
}
