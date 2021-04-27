using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;

public class TankCtrl : MonoBehaviour
{
    private Transform tr;
    public float speed = 10.0f;
    private PhotonView pv;

    public Transform firePos;
    public GameObject cannon;

    public Transform cannonMesh;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        pv = GetComponent<PhotonView>();
        
        if (pv.IsMine)
        {
        Camera.main.GetComponent<SmoothFollow>().target = tr.Find("CamPivot").transform; //카메라가 자기 탱크
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -5.0f, 0);
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true; //운동역학으로 됨 ,리직바디는 자체 활성화 비활성화(enable)는 없음
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            //앞뒤
            tr.Translate(Vector3.forward * Time.deltaTime * speed * v);
            tr.Rotate(Vector3.up * Time.deltaTime * 100.0f * h);

            //포탄 발사 로직
            if (Input.GetMouseButtonDown(0))
            {
                pv.RPC("Fire",RpcTarget.AllViaServer, null);    
            }

            //포신 회전 설정
            float r = Input.GetAxis("Mouse ScrollWheel");
            cannonMesh.Rotate(Vector3.right * Time.deltaTime * r * 100.0f);
        }
        
    }

    [PunRPC]
    void Fire()
    {
        Instantiate(cannon, firePos.position, firePos.rotation);
    }
}
