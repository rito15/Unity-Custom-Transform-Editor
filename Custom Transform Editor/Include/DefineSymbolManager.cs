#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

// 날짜 : 2021-03-12 PM 6:41:03
// 작성자 : Rito

namespace Rito.EditorPlugins
{
    public static class DefineSymbolManager
    {
        public struct DefineSymbolData
        {
            public BuildTargetGroup buildTargetGroup; // 현재 빌드 타겟 그룹
            public string fullSymbolString;           // 현재 빌드 타겟 그룹에서 정의된 심볼 문자열 전체
            public Regex symbolRegex;

            public DefineSymbolData(string symbol)
            {
                buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
                fullSymbolString = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
                symbolRegex = new Regex(@"\b" + symbol + @"\b(;|$)");
            }
        }

        /// <summary> 심볼이 이미 정의되어 있는지 검사 </summary>
        public static bool IsSymbolAlreadyDefined(string symbol)
        {
            DefineSymbolData dsd = new DefineSymbolData(symbol);

            return dsd.symbolRegex.IsMatch(dsd.fullSymbolString);
        }

        /// <summary> 심볼이 이미 정의되어 있는지 검사 </summary>
        public static bool IsSymbolAlreadyDefined(string symbol, out DefineSymbolData dsd)
        {
            dsd = new DefineSymbolData(symbol);

            return dsd.symbolRegex.IsMatch(dsd.fullSymbolString);
        }

        /// <summary> 특정 디파인 심볼 추가 </summary>
        public static void AddDefineSymbol(string symbol)
        {
            // 기존에 존재하지 않으면 끝에 추가
            if (!IsSymbolAlreadyDefined(symbol, out var dsd))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(dsd.buildTargetGroup, $"{dsd.fullSymbolString};{symbol}");
            }
        }

        /// <summary> 특정 디파인 심볼 제거 </summary>
        public static void RemoveDefineSymbol(string symbol)
        {
            // 기존에 존재하면 제거
            if (IsSymbolAlreadyDefined(symbol, out var dsd))
            {
                string strResult = dsd.symbolRegex.Replace(dsd.fullSymbolString, "");

                PlayerSettings.SetScriptingDefineSymbolsForGroup(dsd.buildTargetGroup, strResult);
            }
        }
    }
}

#endif