using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime; //리얼타임쪽에 활성화

public class PhotonManager : MonoBehaviourPunCallbacks     //버전2에서는 오버레이딩 해서 쓸수 있게 콜백함수를 만들었음
{
    private readonly string gameVersion = "v1.0"; //게임버전 중요함, 같은 버전끼리 들어와서 대전을 할 수 있도록 함, 다른버전끼리 들어오면 버그 생길 수 있음
    private string UserId = "GeonHo"; //내 아이디 작성

    void Awake()
    {
        //게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;
        //유저명 지정
        PhotonNetwork.NickName = UserId;

        //서버 접속 (가장 빠른 서버를 찾아서 접속함)
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster() //포톤 서버로 접속
    {
        Debug.Log("Connected to Photoon Sever!!");
        //이미 방이 만들어져 있으면 아무 방이나 들어가기
        PhotonNetwork.JoinRandomRoom(); //랜덤한 룸에 접속 시도

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"code={returnCode}, msg={message}");

        //룸을 생성
        PhotonNetwork.CreateRoom("My Room");
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
    }

    
}
