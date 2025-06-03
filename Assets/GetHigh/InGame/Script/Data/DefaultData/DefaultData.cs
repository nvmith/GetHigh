
public enum ECharacters
{
    Haeseong,
    Eunha,
    Kuiper, 
    c2, 
    c3
}

// 기본적인 데이터 정보 저장하는 클래스 ex)선택 플레이어의 능력치

public class DefaultData
{
    // Characters
    private CharacterData characters;
    public CharacterData Characters => characters;

    // CurIndex
    private ECharacters enumC;

    public DefaultData()
    {
        characters = new CharacterData();
    }

    public void SettingValue(int index)
    {
        characters.UpdateData(index);
    }
}

// 기본 캐릭터 특성
public class CharacterData
{
    // 메인, 서브, 1, 2, 3 -> Characters, 이름 꼭 지어야함
    private int[] defaultHp = { 6, 4, 8};
    private int[] defaultAttackPower = { 12, 9, 14};
    private float[] defaultAimAccuracy = { 5, 7, 3};
    private float[] defaultBulletDistance = { 0.8f, 1.1f, 0.65f};
    private float[] defaultSpeed = { 3.5f, 4.5f, 3.0f};
    private float[] defaultAttackDealy = { 10, 13, 8};

    public void UpdateData(int i)
    {
        if (DataManager.Instacne.JsonClass._PlayerData.playerLock[i])
        GameManager.Instance.GetPlayerValue(defaultHp[i], defaultAttackPower[i],
            defaultAimAccuracy[i], defaultBulletDistance[i], defaultSpeed[i],
            defaultAttackDealy[i]);
    }
}
