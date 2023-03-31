using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using DataInfo;

// 데이터를 저장하기 위해서 BinaryFormatter를 사용해 객체 직렬화(Serialization)해야한다.
// 객체 직렬화란 객체를 통신이나 파일 또는 메모리에 저장하기 위해서
// 일련의 바이트로 변환하는것을 말한다.
// 쉽게 이야기 하자면 메모리상에 흩어져 있는 저장된 데이터를 일렬로 재배열 해 전송하기
// 용이한 데이터를 바꾸는것을 말한다.

// 객체를 직렬화 하는 함수는 Serialize이고, 역 직렬화 하는 함수는 Deserialize이다.
// 파일을 쓸 때 Serialize, 파일을 읽을 때는 Deserialize
public class DataManager : MonoBehaviour
{
    // 파일이 저장될 물리적인 경로
    private string dataPath;

    // 파일 저장 경로와 파일 명 지정
    public void Initialized()
    {
        dataPath = Application.persistentDataPath + "/GameData.dat";
    }

    public void Save(GameData gameData)
    {
        // 바이너리 파일 포맷을 위한 BinaryFormatter 생성
        BinaryFormatter bf = new BinaryFormatter();
        // 데이터 저장을 위한 파일 생성
        FileStream file = File.Create(dataPath);

        // 파일에 저장할 클래스의 데이터 할당
        GameData data = new GameData();
        data.killCount = gameData.killCount;
        data.hp = gameData.hp;
        data.speed = gameData.speed;
        data.damage = gameData.damage;
        data.equipItem = gameData.equipItem;

        // BinaryFormatter를 사용해 파일에 데이터 기록
        bf.Serialize(file, data);
        file.Close();
    }

    public GameData Load()
    {
        // 파일에서 데이터를 추출하는 함수
        if(File.Exists(dataPath))
        {
            // 바이너리 파일 언포맷을 위한 BinaryFormatter 생성
            BinaryFormatter bf = new BinaryFormatter();
            // 데이터 로드를 위한 파일 생성
            FileStream file = File.Open(dataPath, FileMode.Open);

            // GameData 클래스에 파일로부터 읽은 데이터를 기록
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
