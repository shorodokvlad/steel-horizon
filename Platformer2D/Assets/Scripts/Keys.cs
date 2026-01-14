using UnityEngine;

public class Key : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {   
            ObjectiveManager.instance.UpdateKeyCount();
            Destroy(gameObject); 
        }
    }
}