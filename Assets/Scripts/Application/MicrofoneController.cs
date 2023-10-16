using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Whisper.Utils;
using Whisper;
using TMPro;
using System;

public class MicrofoneController : MonoBehaviour
{
    public WhisperManager whisper;
    public MicrophoneRecord record;

    public void OnRecordStart()
    {
        record.StartRecord();
    }

    public void OnRecordEnd()
    {
        record.StopRecord();
    }
}
