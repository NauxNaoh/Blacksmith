using System.Linq;
#if UNITY_EDITOR
using Object = UnityEngine.Object;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace FangsExtensionTool
{
    public enum ResourceType
    {
        //Enum value must always start at 0 and move forward plus 1
        Scene = 0,
        Script = 1,
        Prefab = 2,
        Texture2D = 3,
        Material = 4,
    }

    public class CustomTools : EditorWindow
    {
        private int _curLayer;
        private string[] _mainOptions = new[] {"Optimize Texture2D", "Find Asset"};
        private int _previousOption = -1; // Biến để lưu trạng thái trước đó của tùy chọn
        private int _previousSubOption = -1; // Biến để lưu trạng thái trước đó của tùy chọn


        //Option 1: Optimize Sprite field
        private string _pathOptimizeSprite = "Assets";
        private string[] _spritePaths = new string[0];
        private TextureImporterType _textureType = TextureImporterType.Sprite;
        private bool _genMipMap;
        private bool _alphaIsTransparency = true;
        private BuildTargetGroup _buildTarget = BuildTargetGroup.Android;
        private TextureImporterFormat _importerFormat = TextureImporterFormat.ASTC_6x6;


        //Option 2: Find Asset field
        private string _pathFindAsset = "Assets";
        private int _curFindLayer;
        private string[] _findAssetOptions = new[] {"Find All Asset", "Find Asset On Scene", "Find Asset Not Use"};
        private ResourceType _chooseResource = ResourceType.Texture2D;
        private ResourceType[] _displayedType;
        private Vector2 _scrollPosition = Vector2.zero;

        //sub 01
        private int _selectedResourceSub_01;
        private List<string> _assetPathsSub_01 = new List<string>();
        private List<bool> _choosePathsSub_01 = new List<bool>();
        private List<string> _lstDeletePath = new List<string>();

        //sub 02
        private int _selectedResourceSub_02;
        private List<string> _assetPathsSub_02 = new List<string>();
        private List<bool> _choosePathsSub_02 = new List<bool>();
        private List<GameObject> _lstObjectFound = new List<GameObject>();

        //Sub 03
        private int _selectedResourceSub_03;
        //private List<string> _assetPathsSub_03 = new List<string>();
        private List<string> _assetPathsSub_03_OnAssets = new List<string>();
        private HashSet<string> _assetPathsSub_03_OnScene = new HashSet<string>();
        private List<bool> _choosePathsSub_03 = new List<bool>();
        

        [MenuItem("Tools/" + nameof(CustomTools))]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<CustomTools>(nameof(CustomTools));
        }


        private void OnGUI()
        {
            GUILayout.Space(10);
            _curLayer = GUILayout.Toolbar(_curLayer, _mainOptions);
            GUILayout.Space(10);

            ResetMainOption();
            SetStyleGUI();

            switch (_curLayer)
            {
                case 0:
                    OptionOptimizeSprite();
                    break;
                case 1:
                    OptionFindAsset();
                    break;
            }

            GUILayout.Space(10);
        }

        void ResetMainOption()
        {
            if (_curLayer == _previousOption) return;
            if (GUIUtility.keyboardControl != 0)
            {
                GUIUtility.keyboardControl = 0;
            }

            _previousOption = _curLayer;
        }

        void SetStyleGUI()
        {
//            customTextFieldStyle.normal.background = null; // Loại bỏ nền
//            customTextFieldStyle.normal.textColor = Color.white; // Điều chỉnh màu chữ
//            customTextFieldStyle.focused.textColor = Color.cyan;
        }

        #region Main Option 1

        /// <summary>
        /// Main Option 1: Optimize Sprite
        /// </summary>
        private void OptionOptimizeSprite()
        {
            EditorGUILayout.BeginHorizontal();
            _pathOptimizeSprite = EditorGUILayout.TextField("Folder Path", _pathOptimizeSprite);

            if (GUILayout.Button("Browser", GUILayout.Width(100)))
            {
                var selectedPath = EditorUtility.OpenFolderPanel("Select Folder", _pathOptimizeSprite, "");
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    int assetsGameIndex = selectedPath.IndexOf("Assets", StringComparison.Ordinal);

                    _pathOptimizeSprite =
                        assetsGameIndex != -1 ? selectedPath.Substring(assetsGameIndex) : selectedPath;
                }
            }

            EditorGUILayout.EndHorizontal();


            _textureType = (TextureImporterType) EditorGUILayout.EnumPopup($"Texture Type", _textureType);
            _genMipMap = EditorGUILayout.Toggle("Generate Mip Maps", _genMipMap);
            _alphaIsTransparency = EditorGUILayout.Toggle("Alpha Is Transparency", _alphaIsTransparency);


            _buildTarget = (BuildTargetGroup) EditorGUILayout.EnumPopup($"{nameof(BuildTargetGroup)}", _buildTarget);
            _importerFormat = (TextureImporterFormat) EditorGUILayout.EnumPopup("Format", _importerFormat);

            if (GUILayout.Button(_mainOptions[_curLayer]))
            {
                //find texture2d
                _spritePaths = ToolFindAssets.FindAssetPathsWithPath(ResourceType.Texture2D, _pathOptimizeSprite);
                //optimize
                ToolOptimizerTexture2D.OptimizeSprite(_buildTarget, _spritePaths, _textureType, _genMipMap,
                    _alphaIsTransparency, _importerFormat);
            }
        }

        #endregion

        #region Main Option 2

        private void OptionFindAsset()
        {
            _curFindLayer = GUILayout.Toolbar(_curFindLayer, _findAssetOptions);
            GUILayout.Space(10);


            ResetSubOption();
            switch (_curFindLayer)
            {
                case 0:
                    SubOptionFindAll();
                    break;
                case 1:
                    SubOptionFindOnScene();
                    break;
                case 2:
                    SubOptionFindAssetsNotUse();
                    break;
            }
        }

        void ResetSubOption()
        {
            if (_curFindLayer == _previousSubOption) return;
            if (GUIUtility.keyboardControl != 0)
            {
                GUIUtility.keyboardControl = 0;
            }

            _previousSubOption = _curFindLayer;
        }

        void SetDataResourcePopupEnum(ref int selected, bool isCustom)
        {
            if (isCustom)
            {
                _displayedType = new[]
                    {ResourceType.Texture2D, ResourceType.Prefab, ResourceType.Material};
            }
            else
            {
                _displayedType = new ResourceType[Enum.GetNames(typeof(ResourceType)).Length];
                for (int i = 0; i < _displayedType.Length; i++)
                {
                    _displayedType[i] = (ResourceType) i;
                }
            }

            selected = EditorGUILayout.Popup("Selected Resource", selected,
                _displayedType.Select(type => type.ToString()).ToArray());
            _chooseResource = _displayedType[selected];
        }

        #region Sub 01

        void SubOptionFindAll()
        {
            _pathFindAsset = EditorGUILayout.TextField("Folder Path", _pathFindAsset);
            SetDataResourcePopupEnum(ref _selectedResourceSub_01, false);

            if (GUILayout.Button(_findAssetOptions[_curFindLayer]))
            {
                _choosePathsSub_01 = new List<bool>();
                FindAllAssets();
            }

            EditorGUILayout.Space(20);
            DrawScrollViewFindOnAssets(_choosePathsSub_01, _assetPathsSub_01);
            DrawDeleteBtn(_choosePathsSub_01,_assetPathsSub_01);
        }

        void FindAllAssets()
        {
            _assetPathsSub_01 = ToolFindAssets.FindAssetPathsWithPath(_chooseResource, _pathFindAsset).ToList();
            _choosePathsSub_01 = new List<bool>();
            for (int i = 0; i < _assetPathsSub_01.Count; i++)
            {
                _choosePathsSub_01.Add(false);
            }
        }

        private void DrawScrollViewFindOnAssets(IList<bool> lstChoose, IReadOnlyList<string> lstPathFound)
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            for (int i = 0; i < lstPathFound.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                bool previousState = lstChoose[i];
                if (lstChoose[i] = GUILayout.Toggle(lstChoose[i], $"{i}.  {lstPathFound[i]}"))
                {
                    if (!previousState)
                    {
                        var selectObj = AssetDatabase.LoadAssetAtPath(lstPathFound[i], typeof(Object));
                        Selection.activeObject = selectObj;
                        EditorGUIUtility.PingObject(selectObj);
                        Selection.activeObject = null;

                        _lstDeletePath.Add(lstPathFound[i]);
                    }
                    else
                    {
                        _lstDeletePath.Remove(lstPathFound[i]);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawDeleteBtn(IList<bool> lstBoolChoose, List<string> lstPath)
        {
            if (GUILayout.Button("Delete Asset Choose"))
            {
                if (!CheckHadChooseAsset(lstBoolChoose)) return;

                bool result = EditorUtility.DisplayDialog("Warning!!", "Do you really want to delete asset choose?!",
                    "Accept!",
                    "Cancel");
                if (!result) return;
                for (int i = lstPath.Count - 1; i >= 0; i--)
                {
                    if (lstBoolChoose[i])
                    {
                        AssetDatabase.DeleteAsset(lstPath[i]);
                        lstPath.Remove(lstPath[i]);
                        lstBoolChoose.Remove(lstBoolChoose[i]);
                    }
                }

                AssetDatabase.Refresh();
            }
        }

        bool CheckHadChooseAsset(IEnumerable<bool> lstBoolChoose)
        {
            return lstBoolChoose.Any(state => state);
        }

        #endregion


        #region Sub 02

        void SubOptionFindOnScene()
        {
            EditorGUILayout.TextField("Folder Path", "Assets/...", GUIStyle.none);
            SetDataResourcePopupEnum(ref _selectedResourceSub_02, true);
            if (GUILayout.Button(_findAssetOptions[_curFindLayer]))
            {
                FindOnScene();
            }

            DrawScrollViewFindOnScene(_choosePathsSub_02, _assetPathsSub_02, _lstObjectFound);
        }


        void FindOnScene()
        {
            var (enumerable, gameObjects) = ToolFindAssets.FindAssetPathsInScene(_chooseResource);
            _assetPathsSub_02 = enumerable.ToList();
            _lstObjectFound = gameObjects.ToList();
            _choosePathsSub_02 = new List<bool>();
            for (int i = 0; i < _assetPathsSub_02.Count; i++)
            {
                _choosePathsSub_02.Add(false);
            }
        }


        private void DrawScrollViewFindOnScene(IList<bool> lstChoose, IReadOnlyList<string> lstPathFound,
            IReadOnlyList<GameObject> lstObjFound)
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            for (int i = 0; i < lstPathFound.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Find", GUILayout.MaxWidth(40), GUILayout.MinWidth(40)))
                {
                    Selection.activeObject = lstObjFound[i];
                    EditorGUIUtility.PingObject(lstObjFound[i]);
                }

                bool previousState = lstChoose[i];
                if (lstChoose[i] = GUILayout.Toggle(lstChoose[i], $"{i}.  {lstPathFound[i]}"))
                {
                    if (!previousState)
                    {
                        var selectObj = AssetDatabase.LoadAssetAtPath(lstPathFound[i], typeof(Object));
                        Selection.activeObject = selectObj;
                        EditorGUIUtility.PingObject(selectObj);
                        Selection.activeObject = null;

                        _lstDeletePath.Add(lstPathFound[i]);
                    }
                    else
                    {
                        _lstDeletePath.Remove(lstPathFound[i]);
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        #endregion

        #region Sub 03

        void SubOptionFindAssetsNotUse()
        {
            SetDataResourcePopupEnum(ref _selectedResourceSub_03, true);
            if (GUILayout.Button(_findAssetOptions[_curFindLayer]))
            {
                FindNotUse();
            }
            DrawScrollViewFindOnAssets(_choosePathsSub_03, _assetPathsSub_03_OnAssets);
            DrawDeleteBtn(_choosePathsSub_03,_assetPathsSub_03_OnAssets);
        }

        void FindNotUse()
        {
            //find on "Assets/..."
            _assetPathsSub_03_OnAssets = ToolFindAssets.FindAssetPathsWithPath(_chooseResource, "Assets").ToList();
            
            // find on Scene
            var (enumerable, gameObjects) = ToolFindAssets.FindAssetPathsInScene(_chooseResource);
            _assetPathsSub_03_OnScene = new HashSet<string>(enumerable.ToList());
            //_lstObjectFound = gameObjects.ToList();

            
            Debug.LogError(_assetPathsSub_03_OnAssets.Count);
            Debug.LogError(_assetPathsSub_03_OnScene.Count);
            
            for (int i = _assetPathsSub_03_OnAssets.Count-1; i >= 0; i--)
            {
                if (_assetPathsSub_03_OnScene.Contains(_assetPathsSub_03_OnAssets[i]))
                {
                    _assetPathsSub_03_OnAssets.Remove(_assetPathsSub_03_OnAssets[i]);
                }
            }
            
            _choosePathsSub_03 = new List<bool>();
            for (int i = 0; i < _assetPathsSub_03_OnAssets.Count; i++)
            {
                _choosePathsSub_03.Add(false);
            }
            
        }

        #endregion

        #endregion
    }
}


#endif