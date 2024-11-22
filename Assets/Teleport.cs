using UnityEngine;

public class Teleport : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SceneTransition();
        }
    }

}
