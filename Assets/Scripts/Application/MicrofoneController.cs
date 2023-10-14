using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;
using Whisper;
using TMPro;

public class MicrofoneController : MonoBehaviour
{
    public WhisperManager whisper;
    public MicrophoneRecord microphoneRecord;
    public bool streamSegments = true;

    [Header("UI")]
    [SerializeField] private Button recordButton;
    [SerializeField] private TextMeshProUGUI recordTextButton;
    [SerializeField] private Toggle translateToggle;
    [SerializeField] private Toggle vadToggle;

    private void Awake()
    {
        whisper.OnNewSegment += OnNewSegment;

        microphoneRecord.OnRecordStop += OnRecordStop;

        recordButton.onClick.AddListener(OnButtonPressed);
        //translateToggle.isOn = whisper.translateToEnglish;
        //translateToggle.onValueChanged.AddListener(OnTranslateChanged);

        //vadToggle.isOn = microphoneRecord.vadStop;
        //vadToggle.onValueChanged.AddListener(OnVadChanged);
    }

    private void OnVadChanged(bool vadStop)
    {
        microphoneRecord.vadStop = vadStop;
    }

    private void OnButtonPressed()
    {
        if (!microphoneRecord.IsRecording)
        {
            microphoneRecord.StartRecord();
            //recordButton.interactable = false;
            recordTextButton.text = "Verificar";
        }
        else
        {
            microphoneRecord.StopRecord();
            //recordButton.interactable = true;
            recordTextButton.text = "Pressione para falar";
        }
    }

    private async void OnRecordStop(AudioChunk recordedAudio)
    {
        //recordButton.interactable = true;
        recordTextButton.text = "Verificar";

        var sw = new Stopwatch();
        sw.Start();

        var res = await whisper.GetTextAsync(recordedAudio.Data, recordedAudio.Frequency, recordedAudio.Channels);
        if (res == null)
            return;
        
        var text = res.Result;
    }

    private void OnTranslateChanged(bool translate)
    {
        whisper.translateToEnglish = translate;
    }

    private void OnNewSegment(WhisperSegment segment)
    {
        if (!streamSegments)
            return;

         // segment.Text; -> spokenText
    }
}
