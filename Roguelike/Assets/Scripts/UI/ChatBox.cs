using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChatBox : MonoBehaviour
{
    public GameObject[] Chats;
    public bool ChatCheck = true;


    void Update()
    {
        if (ChatCheck)
        {
            StartCoroutine(ChatCoroutine());
        }
    }

    protected IEnumerator ChatCoroutine() {
        ChatBoxSetup(Random.Range(0, 8));
        yield return new WaitForSeconds(2.0f);
        ChatCheck = true;
    }

    void ChatBoxSetup(int i)
    {
        var clone = Instantiate(Chats[i], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        clone.transform.parent = transform;
        Debug.Log("채팅");
        Destroy(clone, 2.0f);
        ChatCheck = false;
    }
}
