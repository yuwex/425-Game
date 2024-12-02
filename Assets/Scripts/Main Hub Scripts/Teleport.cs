using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("FlatPlaneTestScene");
            // GameManager.Instance.SceneTransition();
        }
    }

}
