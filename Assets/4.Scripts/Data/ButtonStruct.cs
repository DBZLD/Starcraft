using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ButtonStruct", menuName = "ScirptableObjects/ButtonStruct", order = 1)]
public class ButtonStruct : ScriptableObject
{
    public ButtonStructNum buttonNum;               //버튼 번호
    public Sprite buttonNormalImage;                //버튼 기본 이미지
    public Sprite buttonPressedImage;               //버튼 눌렸을때 이미지
    public KeyCode buttonHotKey;                    //버튼 단축키

    public int needMineral;
    public int needVespeneGas;
    public int needSupply;
    public int needTime;
    public int needEnergy;                          
    public GameObject objectPrefab;                 //오브젝트 프리팹

    [TextArea] public string buttonIfno;            //버튼 설명
}
