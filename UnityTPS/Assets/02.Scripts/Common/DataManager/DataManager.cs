using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using DataInfo;

// �����͸� �����ϱ� ���ؼ� BinaryFormatter�� ����� ��ü ����ȭ(Serialization)�ؾ��Ѵ�.
// ��ü ����ȭ�� ��ü�� ����̳� ���� �Ǵ� �޸𸮿� �����ϱ� ���ؼ�
// �Ϸ��� ����Ʈ�� ��ȯ�ϴ°��� ���Ѵ�.
// ���� �̾߱� ���ڸ� �޸𸮻� ����� �ִ� ����� �����͸� �Ϸķ� ��迭 �� �����ϱ�
// ������ �����͸� �ٲٴ°��� ���Ѵ�.

// ��ü�� ����ȭ �ϴ� �Լ��� Serialize�̰�, �� ����ȭ �ϴ� �Լ��� Deserialize�̴�.
// ������ �� �� Serialize, ������ ���� ���� Deserialize
public class DataManager : MonoBehaviour
{
    // ������ ����� �������� ���
    private string dataPath;

    // ���� ���� ��ο� ���� �� ����
    public void Initialized()
    {
        dataPath = Application.persistentDataPath + "/GameData.dat";
    }

    public void Save(GameData gameData)
    {
        // ���̳ʸ� ���� ������ ���� BinaryFormatter ����
        BinaryFormatter bf = new BinaryFormatter();
        // ������ ������ ���� ���� ����
        FileStream file = File.Create(dataPath);

        // ���Ͽ� ������ Ŭ������ ������ �Ҵ�
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.speed = gameData.speed;
        data.damage = gameData.damage;
        data.equipItem = gameData.equipItem;

        // BinaryFormatter�� ����� ���Ͽ� ������ ���
        bf.Serialize(file, data);
        file.Close();
    }

    public GameData Load()
    {
        // ���Ͽ��� �����͸� �����ϴ� �Լ�
        if(File.Exists(dataPath))
        {
            // ���̳ʸ� ���� �������� ���� BinaryFormatter ����
            BinaryFormatter bf = new BinaryFormatter();
            // ������ �ε带 ���� ���� ����
            FileStream file = File.Open(dataPath, FileMode.Open);

            // GameData Ŭ������ ���Ϸκ��� ���� �����͸� ���
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            return data;
        }
        else
        {
            GameData data = new GameData();
            return data;
        }
    }
}
