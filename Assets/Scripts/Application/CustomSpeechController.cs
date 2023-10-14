using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Whisper;

public class CustomSpeechController : MonoBehaviour
{
    private readonly string textToSpeakFormat = "           {0}";

    [SerializeField] private MicrofoneController microfoneController;
    [Space]
    [SerializeField] private TextMeshProUGUI textToSpeak;
    [SerializeField] private TextMeshProUGUI resultText;

    private SpeechEvaluator speechEvaluator = new SpeechEvaluator();

    private void Awake()
    {
        microfoneController.whisper.OnNewSegment += OnNewSegment;
    }

    /// <summary>
    /// When user spoke at microfone
    /// </summary>
    /// <param name="segment"></param>
    private void OnNewSegment(WhisperSegment segment)
    {
        float result = speechEvaluator.CompareSpeech(textToSpeak.text, segment.Text);

        if( result >= 0.9f)
        {
            // TODO: Play PS
            resultText.text = "Excelente, sua pronuncia é ótima!";
        }
        else if( result >= 0.7f)
        {
            // TODO: Play PS
            resultText.text = "Muito bom! Foi quase perfeito!";
        }
        else
        {
            resultText.text = "Você está no caminho certo, ouça o audio de exemplo e tente novamente!";
        }
    }

    //public string word1;
    //public string word2;

    //StringSpeechCompare stringSpeechCompare = new StringSpeechCompare(); 

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.A))
    //    {
    //        Debug.Log(stringSpeechCompare.LevenshteinDistance(word1, word2));
    //    }
    //}
}
