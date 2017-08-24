using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraModel : MonoBehaviour {

    public Camera mycamera;

	void Update () {
        transform.position = mycamera.transform.position;
        transform.localRotation = mycamera.transform.localRotation;
	}
}
