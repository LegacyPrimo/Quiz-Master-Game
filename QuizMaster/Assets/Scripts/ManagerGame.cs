using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerGame : MonoBehaviour
{
    QuizScript quizObject;
    EndScreen endscreenObject;

    private void Awake()
    {
        quizObject = FindObjectOfType<QuizScript>();
        endscreenObject = FindObjectOfType<EndScreen>();
    }

    // Start is called before the first frame update
    void Start()
    {
        quizObject.gameObject.SetActive(true);
        endscreenObject.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (quizObject.isComplete == true) 
        {
            quizObject.gameObject.SetActive(false);
            endscreenObject.gameObject.SetActive(true);
            endscreenObject.ShowScore();
        }
    }

    public void ReplayLevel() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
