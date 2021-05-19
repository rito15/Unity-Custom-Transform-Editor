#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;

// 날짜 : 2021-02-18 AM 4:41:01
// 작성자 : Rito

namespace Rito.EditorPlugins
{
    [InitializeOnLoad]
    public static class TransformEditorHelper
    {
        public static string TexturePath => FolderPath + @"\Editor Resources\Refresh.png";
        public static string FolderPath { get; private set; }
        static TransformEditorHelper()
        {
#if RITO_USE_CUSTOM_TRANSFORM_EDITOR
            InitFolderPath();

            // Load Adv Foldout Value
            TransformEditor.LoadGlobalFoldOutValue(PlayerPrefs.GetInt(GlobalPrefName, 0) == 1 ? true : false);
#endif
        }

        private static void InitFolderPath([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            FolderPath = System.IO.Path.GetDirectoryName(sourceFilePath);
            int rootIndex = FolderPath.IndexOf(@"Assets\");
            if (rootIndex > -1)
            {
                FolderPath = FolderPath.Substring(rootIndex, FolderPath.Length - rootIndex);
            }
        }

        public static (object rotationGUI, MethodInfo OnEnable, MethodInfo RotationField) GetTransformRotationGUI()
        {
            object target;
            MethodInfo onEnableMethod;
            MethodInfo rotationFieldMethod;

            Type targetType = Type.GetType("UnityEditor.TransformRotationGUI, UnityEditor");
            target = Activator.CreateInstance(targetType);

            onEnableMethod = targetType.GetMethod (
                "OnEnable",
                new Type[] { typeof(UnityEditor.SerializedProperty), typeof(GUIContent) }
            );
            rotationFieldMethod = targetType.GetMethod("RotationField", new Type[] { });

            return (target, onEnableMethod, rotationFieldMethod);
        }
        /***********************************************************************
        *                               PlayerPrefs
        ***********************************************************************/
#region .
        private const string GlobalPrefName = "TE_GlobalFoldOut";

        public static void SaveGlobalFoldOutPref(bool value) => PlayerPrefs.SetInt(GlobalPrefName, value ? 1 : 0);

#endregion
        /***********************************************************************
        *                               Menu Item
        ***********************************************************************/
        #region .
        private const string MenuItemTitle = "Tools/Rito/Use Custom Transform Editor";
        public const string DefineSymbolName = "RITO_USE_CUSTOM_TRANSFORM_EDITOR";

        private static bool MenuItemChecked
        {
            get => PlayerPrefs.GetInt(MenuItemTitle, 0) == 1 ? true : false;
            set => PlayerPrefs.SetInt(MenuItemTitle, value ? 1 : 0);
        }

        [MenuItem(MenuItemTitle, false)]
        private static void UseCustomTransformEditor()
        {
            MenuItemChecked = !MenuItemChecked;

            // 디파인 심볼 변경
            if (MenuItemChecked)
            {
                DefineSymbolManager.AddDefineSymbol(DefineSymbolName);
            }
            else
            {
                DefineSymbolManager.RemoveDefineSymbol(DefineSymbolName);
            }
        }

        [MenuItem(MenuItemTitle, true)]
        private static bool UseCustomTransformEditor_Validate()
        {
            Menu.SetChecked(MenuItemTitle, MenuItemChecked);

            return true;
        }

        #endregion
    }
}

#endif