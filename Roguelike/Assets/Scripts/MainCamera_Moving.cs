using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera_Moving : MonoBehaviour {

    public GameObject player;

    public float offsetX = 0f;
    public float offsetY = 3.5f;
    public float offsetZ = -3f;

    Vector3 cameraPosition;

    void LateUpdate() {
        cameraPosition.x = player.transform.position.x + offsetX;
        cameraPosition.y = player.transform.position.y + offsetY;
        cameraPosition.z = player.transform.position.z + offsetZ;

        transform.position = cameraPosition;
    }

}
