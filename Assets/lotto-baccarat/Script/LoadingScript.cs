using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
public class LoadingScript : MonoBehaviour
{
    
    IEnumerator Start()
    {
        LeanTween.scale(gameObject, new Vector3(2, 2, 2), 0.3f).setEaseInOutBounce().setLoopPingPong();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
