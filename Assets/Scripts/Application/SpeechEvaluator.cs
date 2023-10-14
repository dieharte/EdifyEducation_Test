using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SpeechEvaluator
{
    private StringSpeechCompare stringSpeechCompare = new StringSpeechCompare();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="spokenText"></param>
    /// <returns>Return the percent (up to 1) of accuracy</returns>
    public float CompareSpeech(string text, string spokenText)
    {
        string[] textWords =  text.Split();
        string[] spokenTextWords = spokenText.Split();

        return ComparePhrases(textWords, spokenTextWords);
    }

    public string GenerateNewPhrase()
    {
        return string.Empty;
    }


    public float ComparePhrases(string[] textWords, string[] spokenTextWords)
    {
        float rightWords = 0;

        if(textWords.Length == spokenTextWords.Length)
        {
            for (int i = 0; i < textWords.Length; i++)
            {
                if(MatchWord(textWords[i], spokenTextWords[i]))
                {
                    rightWords++;
                }
            }
        }
        else
        {
            int j = 0;
            bool unmatchAlmostUncompletely;
            for (int i = 0; i < textWords.Length; i++)
            {                
                if (MatchWord(textWords[i], spokenTextWords[j], out unmatchAlmostUncompletely))
                {
                    rightWords++;
                }
                else if(unmatchAlmostUncompletely)
                {
                    i--;
                }

                j++;
                if (j >= spokenTextWords.Length)
                    break;
            }
        }

        return rightWords / textWords.Length;
    }

    public bool MatchWord(string correct, string spoken)
    {
        float percentWrong = stringSpeechCompare.LevenshteinDistance(correct, spoken) / correct.Length;

        return percentWrong < 0.25f;
    }

    public bool MatchWord(string correct, string spoken, out bool unmatchAlmostUncompletely)
    {
        float percentWrong = stringSpeechCompare.LevenshteinDistance(correct, spoken) / correct.Length;

        unmatchAlmostUncompletely = percentWrong > 0.85f;
        return percentWrong < 0.25f;
    }
}
