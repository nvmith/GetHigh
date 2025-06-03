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
            GameManager.Instance.playerCheck = true; //�̰� �� �ᵵ �ɵ�
            GameManager.Instance.selectCharacter = (ECharacters)characterImage.CurIndex;

            GameManager.Instance.UpdateDiaryDate((int)GameManager.Instance.selectCharacter + 14); // �⺻ �� ���̾ ���
            SceneManager.LoadScene(1);
        }

        /*if (GameManager.Instance.playerCheck)
		{
			GameManager.Instance.UpdateDiaryDate((int)GameManager.Instance.selectCharacter + 14); // �⺻ �� ���̾ ���
			SceneManager.LoadScene(1);
		}*/
	}
}
