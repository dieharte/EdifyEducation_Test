using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;

public class CustomSpeechController : MonoBehaviour
{
    [Header("Controller")]
    [SerializeField] private MicrofoneController microfoneController;

    [Space]
    [SerializeField] private Button recordButton;
    [SerializeField] private TextMeshProUGUI recordTextButton;

    [Space]
    [SerializeField] private TMP_InputField inputTextToSpeak;
    [SerializeField] private TextMeshProUGUI textToSpeak;
    [SerializeField] private TextMeshProUGUI resultText;

    private SpeechEvaluator speechEvaluator = new SpeechEvaluator();
    private State currentState = State.Ready;

    private enum State
    {
        Ready,
        Recording,
        Verifying,
        VoiceNotRecognized,
        VerifyingError
    }

    private void Awake()
    {
        recordButton.onClick.AddListener(OnRecordButtonPressed);
        microfoneController.whisper.OnNewSegment += OnNewSegment;
        microfoneController.record.OnRecordStop += OnRecordStop;
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

        SetState(State.Ready);        
    }

    private void OnRecordButtonPressed()
    {
        if (!microfoneController.record.IsRecording)
        {
            SetState(State.Recording);
        }
        else
        {
            SetState(State.Verifying);
        }
    }

    private async void OnRecordStop(AudioChunk recordedAudio)
    {
        var res = await microfoneController.whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
        if (string.IsNullOrEmpty(res.Result))
        {
            SetState(State.VoiceNotRecognized);
            return;
        }
        else if (res == null)
        {
            SetState(State.VerifyingError);
            return;
        }
    }

    private void SetState(State newState)
    {
        if(newState == State.Ready)
        {
            inputTextToSpeak.interactable = true;
            recordButton.interactable = true;
            recordTextButton.text = "Pressione para falar";
        } 
        else if (newState == State.Recording)
        {
            microfoneController.OnRecordStart();
            recordTextButton.text = "Verificar";
        }
        else if (newState == State.Verifying)
        {
            microfoneController.OnRecordEnd();
            inputTextToSpeak.interactable = false;
            recordButton.interactable = false;
            recordTextButton.text = "Verificando";
        }
        else if (newState == State.VoiceNotRecognized)
        {
            SetState(State.Ready);
            resultText.text = "Nenhum som foi reconhecido. Tente novamente";
        }
        else if (newState == State.VerifyingError)
        {
            SetState(State.Ready);
            resultText.text = "Houve um erro no processamento do som. Tente novamente";
        }

        currentState = newState;
    }
}
