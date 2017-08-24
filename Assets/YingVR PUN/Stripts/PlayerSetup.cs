using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

    [Header("UI")]
    public Text m_NameText;

    [Header("PhotonNetwork")]
    [Space]
    [SyncVar]
    public Color m_Color;

    [SyncVar]
    public string m_PlayerName = "";

    public override void OnStartClient()
    {
        base.OnStartClient();

        m_NameText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(m_Color) + ">" + m_PlayerName + "</color>";
    }

    // Use this for initialization
    void Start () {

}
	
	// Update is called once per frame
	void Update () {
	    //if(!isLocalPlayer)
     //   {
     //       m_NameText.transform.parent.LookAt(Camera.main.transform);
     //   }
	}

}
