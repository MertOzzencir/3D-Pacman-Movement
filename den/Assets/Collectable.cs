using UnityEditor;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if(player != null)
        {
            Destroy(gameObject);
        }
    }
}
