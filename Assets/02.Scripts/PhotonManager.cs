using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; //리얼타임쪽에 활성화
using TMPro;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks     //버전2에서는 오버레이딩 해서 쓸수 있게 콜백함수를 만들었음
{
    private readonly string gameVersion = "v1.0"; //게임버전 중요함, 같은 버전끼리 들어와서 대전을 할 수 있도록 함, 다른버전끼리 들어오면 버그 생길 수 있음
    private string userId = "GeonHo"; //내 아이디 작성

    public TMP_InputField userIdText;
    public TMP_InputField roomNameText;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //방장이 씬을 로딩을 하면 나머지 클라이언트들에게 그 씬을 자동으로 로딩 시켜줌.

        //게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;
        //유저명 지정
        //PhotonNetwork.NickName = userId;

        //서버 접속 (가장 빠른 서버를 찾아서 접속함)
        PhotonNetwork.ConnectUsingSettings();

    }

    void Start()
    {
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0,100):00}");
        userIdText.text = userId;
        PhotonNetwork.NickName = userId;
    }

    public override void OnConnectedToMaster() //포톤 서버로 접속
    {
        Debug.Log("Connected to Photoon Sever!!");
        //이미 방이 만들어져 있으면 아무 방이나 들어가기
        //PhotonNetwork.JoinRandomRoom(); //랜덤한 룸에 접속 시도
        
        //로그인 다음 호출
        //룸 목록을 받으려고 하면 로비에 접속 해야됨
        //로비에 접속
        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby !!!");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code={returnCode}, msg={message}");

        //룸 속성을 설정
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;//로비에 입장한 상태에서 현재의 룸 목록을 받아올 수 있음. 그때 IsVisilb 이 true 가 된 방만 볼 수 있음
        ro.MaxPlayers = 30;




        //룸을 생성
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    //룸 생성 완료 콜백

    public override void OnCreatedRoom()
    {
        Debug.Log("방생성 완료");
    }

    //룸에 입장했을 때 호출되는 콜백함수
    public override void OnJoinedRoom()
    {
        Debug.Log("방 입장 완료");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);//현재 방에 입장한 룸의 이름


        //방장만 호출

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("BattleField"); //얘를 쓰면 잠깐 네트워크 통신을 끊음, 씬 넘어갈때 데이터 통신 받으면 이상한거 받을 수도 있음
        }
        

        ////통신이 가능한 주인공 캐릭터 (탱크) 생성 
        //Instantiate 자기 로컬에도 만들고 다른 유저 네트워크에도 탱크를 만들라고 메세지를 보내줌
        //Instantiate 룸에 있는 모든 사용자에게 탱크를 만듦
        //PhotonNetwork.Instantiate("Tank", new Vector3(0, 5.0f, 0),Quaternion.identity); //탱크가 빠지지 않게 Y값을 5로 한거임// 앱 아이디가 다르면 안보임
    }

    public void OnLoginClick()
    {   
        if(string.IsNullOrEmpty(userIdText.text))
        {
            userId = $"USER_{Random.Range(0,100):00}";
            userIdText.text = userId;
        }

        PlayerPrefs.SetString("USER_ID", userIdText.text);
        PhotonNetwork.NickName = userIdText.text;

        PhotonNetwork.JoinRandomRoom();
    }
}
