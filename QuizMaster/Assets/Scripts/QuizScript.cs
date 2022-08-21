using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizScript : MonoBehaviour
{
    [Header("Question Objects")]
    private QuestionScriptableObject questionObject;
    [SerializeField] private List<QuestionScriptableObject> questionObjects = new List<QuestionScriptableObject>();
    [SerializeField] private TextMeshProUGUI questionText;

    [Header("Answer Objects")]
    [SerializeField] private GameObject[] answerObjects;
    [SerializeField] private int correctAnswerIndex;
    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;
    [SerializeField] private bool isAnsweredEarly = true;

    [Header("Timer Objects")]
    [SerializeField] private Image timerImage;
    private TimerScript timer;

    [Header("Score Manager")]
    [SerializeField] TextMeshProUGUI scoreText;
    private ScoreKeeper scoreKeeper;

    [Header("Progress Bar")]
    [SerializeField] Slider progressBar;

    public bool isComplete;

    private void Awake()
    {
        timer = FindObjectOfType<TimerScript>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        progressBar.maxValue = questionObjects.Count;
        progressBar.value = 0;
        GetQuestion();
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        
        if (timer.loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            isAnsweredEarly = false;
            GetQuestion();
            timer.loadNextQuestion = false;
        }

        else if (!isAnsweredEarly && !timer.isAnsweringQuestion) 
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    private void WriteQuestions() 
    {
        questionText.text = questionObject.GetQuestion();
        
        for (int i = 0; i < answerObjects.Length; i++)
        {
            TextMeshProUGUI buttonText = answerObjects[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = questionObject.GetAnswer(i);
        }
    }

    private void GetQuestion()
    {
        if (questionObjects.Count > 0) 
        {
            SetButtonState(true);
            SetButtonSprites();
            GetRandomQuestion();
            WriteQuestions();
            progressBar.value++;
            scoreKeeper.IncrementQuestionSeen();
        }
        
    }

    private void GetRandomQuestion() 
    {
        int random = Random.Range(0, questionObjects.Count);
        questionObject = questionObjects[random];

        if (questionObjects.Contains(questionObject)) 
        {
            questionObjects.Remove(questionObject);
        }
        
    }

    private void SetButtonState(bool state) 
    {
        for (int i = 0; i < answerObjects.Length; i++) 
        {
            Button answerButton = answerObjects[i].GetComponent<Button>();
            answerButton.interactable = state;
        }
    }

    private void SetButtonSprites() 
    {
        for (int i = 0; i < answerObjects.Length; i++) 
        {
            Button answerButtons = answerObjects[i].GetComponent<Button>();
            answerButtons.image.sprite = defaultAnswerSprite;
        }
    }

    public void OnAnswerSelected(int index) 
    {
        isAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculateScore() + "%";
    }

    private void DisplayAnswer(int index) 
    {
        Image buttonImage;

        if (index == questionObject.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            buttonImage = answerObjects[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }

        else
        {
            correctAnswerIndex = questionObject.GetCorrectAnswerIndex();
            string correctAnswer = questionObject.GetAnswer(correctAnswerIndex);
            questionText.text = "Wrong answer, the correct answer is:\n" + correctAnswer;

            buttonImage = answerObjects[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }
}
