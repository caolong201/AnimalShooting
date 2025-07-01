using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

namespace RevivalofNature.Common
{
    public class DataCalculation : MonoBehaviour
    {

        /// <summary>
        /// 最終3桁（小数点第2まで）の数字に変換
        /// </summary>
        /// <param name="totalMoney"></param>
        public static float ToMoneyParseFloat(float totalMoney){
            int _digit = 0;
            float tempMoney = totalMoney;
            //桁変換
            while(tempMoney >= 1000){
                tempMoney =  tempMoney / Mathf.Pow(10, 3);
                _digit ++;
            }

            return tempMoney;
        }

        /// <summary>
        /// 1000で割った桁数を英数字(a-z)に変換
        /// </summary>
        /// <param name="totalMoney"></param>
        public static string ToDigitAlphabet(float totalMoney)
        {
            int _digit = 0;
            float tempMoney = totalMoney;
            //桁変換
            while(tempMoney >= 1000){
                tempMoney =  tempMoney / Mathf.Pow(10, 3);
                _digit ++;
            }

            string alphabet = "a";
            if (_digit < 1) return alphabet;
            alphabet = "";
            while(_digit > 0)
            {
                // A-Zの変換を0-25にするため1を引く
                //index--;
                // ASCIIではAは10進数で65
                alphabet = Convert.ToChar(_digit % 26 + 65) + alphabet; 
                _digit = _digit / 26;
            }
            return alphabet.ToLower();
        }

        /// <summary>
        /// floatの数字をUIに表示するようのStringテキストに変換
        /// </summary>
        /// <param name="totalMoney"></param>
        public static String ToMoneyParseFloatandDigit(float totalMoney){
            String retrunText = ToMoneyParseFloat(totalMoney).ToString("f2");
            retrunText = String.Concat(retrunText,ToDigitAlphabet(totalMoney));

            return retrunText;
        }

        /// <summary>
        /// StringテキストをFloat数字に変換
        /// 注意！！減算二桁英字に対応していない
        /// </summary>
        /// <param name="totalMoney"></param>
        public static float ToFloatParseMoney(String price){
            float _price =  float.Parse(Regex.Replace(price,@"[a-z]", ""));
            Char alphabet =  char.Parse(Regex.Replace(price,@"[^a-z]", ""));

            int _digit = Convert.ToInt32(char.ToUpper(alphabet)) - 65;
            float tempMoney = _price;
            //桁変換
            for(int i = 0; i < _digit;i++){
                tempMoney = tempMoney * Mathf.Pow(10, 3);
            }
            return tempMoney;
        }
    }
}
