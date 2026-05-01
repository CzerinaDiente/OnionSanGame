using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChangeWavesProperty
{
    //No need to put in Update as it will call every frame, it only needs to be called once
    public static void RandomizeProperty(Wave _wave, int _waveNum)
    {
        //_wave.waveName = "Wave " + _waveNum.ToString();
        _wave.waveName = "Wave " + NumToWords(_waveNum);
        _wave.noOfEnemies = Random.Range(15, 30);
        _wave.spawnInterval = Random.Range(0.5f, 1.0f);
        return;
    }

    public static string NumToWords(int number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + NumToWords(Mathf.Abs(number));

        string[] units =
        {
                "", "One", "Two", "Three", "Four", "Five",
                "Six", "Seven", "Eight", "Nine", "Ten",
                "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen",
                "Sixteen", "Seventeen", "Eighteen", "Nineteen"
            };

        string[] tens =
        {
                "", "", "Twenty", "Thirty", "Forty",
                "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
            };

        string words = "";

        if ((number / 1000) > 0)
        {
            words += NumToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "";

            if (number < 20)
                words += units[number];
            else
            {
                words += tens[number / 10];
                if ((number % 10) > 0)
                    words += " " + units[number % 10];
            }
        }

        return words.Trim();
    }
}
