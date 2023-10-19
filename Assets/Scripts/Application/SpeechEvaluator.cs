using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SpeechEvaluator
{
    private readonly string textCorrectResult       = "<color=green>{0}</color>";
    private readonly string textWrongResult         = "<color=red>{0}</color>";

    private StringSpeechCompare stringSpeechCompare = new StringSpeechCompare();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="spokenText"></param>
    /// <returns>Return the percent (up to 1) of accuracy and the text colored with hit and misses</returns>
    public (float, string) CompareSpeech(string text, string spokenText)
    {
        string[] textWords = text.Split();
        string[] spokenTextWords = spokenText.Split();

        return ComparePhrases(textWords, spokenTextWords);
    }

    public (float, string) ComparePhrases(string[] textWords, string[] spokenTextWords)
    {
        float rightWords = 0;
        bool[] rightWordsId = new bool[spokenTextWords.Length];

        if (textWords.Length == spokenTextWords.Length)
        {
            for (int i = 0; i < textWords.Length; i++)
            {
                if (MatchWord(textWords[i], spokenTextWords[i]))
                {
                    rightWords++;
                    rightWordsId[i] = true;
                }
                else
                {
                    rightWordsId[i] = false;
                }
            }
        }
        else
        {
            bool unmatchAlmostUncompletely;
            int min = Mathf.Min(textWords.Length, spokenTextWords[j]);
            for (int i = 0; i < min; i++)
            {
                if (MatchWord(textWords[i], spokenTextWords[j], out unmatchAlmostUncompletely))
                {
                    rightWords++;
                }
            }
        }

        return (rightWords / textWords.Length, GenerateResultText(textWords, rightWordsId));
    }

    private bool MatchWord(string correct, string spoken)
    {
        float percentWrong = stringSpeechCompare.LevenshteinDistance(correct, spoken) / correct.Length;

        return percentWrong < 0.25f;
    }

    private bool MatchWord(string correct, string spoken, out bool unmatchAlmostUncompletely)
    {
        float percentWrong = stringSpeechCompare.LevenshteinDistance(correct, spoken) / correct.Length;

        unmatchAlmostUncompletely = percentWrong > 0.85f;
        return percentWrong < 0.25f;
    }

    private string GenerateResultText(string[] textWords, bool[] rightWordsId)
    {
        StringBuilder resultText = new StringBuilder("");

        for (int i = 0; i < textWords.Length; i++)
        {
            if (i > 0)
                resultText.Append(" ");
            resultText.Append(string.Format(rightWordsId[i] ? textCorrectResult : textWrongResult, textWords[i]));
        }

        return resultText.ToString();
    }
}
