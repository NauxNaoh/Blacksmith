using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime
{
    public class LoadingHandler : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(LoadingRoutine());
        }

        IEnumerator LoadingRoutine()
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(1);
        }
    }
}