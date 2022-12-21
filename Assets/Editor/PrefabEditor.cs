using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Editor
{
    public class PrefabEditor : EditorWindow
    {
        #region --- Const ---

        private const string NAME_TITLE = "Name:";
        private const string SPRITE_FILED_NAME = "Sprite";
        private const string SPRITE_FOLDER_NAME = "Sprites";
        private const string MATERIAL_FILED_NAME = "Material";
        private const string MATERIALS_FOLDER_NAME = "Materials";
        private const string TRANSFORM_POSITION_FILED_NAME = "Position";
        private const string TRANSFORM_ROTATION_FILED_NAME = "Rotation";
        private const string TRANSFORM_SCALE_FILED_NAME = "Scale";
        private const string X_VECTOR_NAME = "X ";
        private const string Y_VECTOR_NAME = "Y ";
        private const string Z_VECTOR_NAME = "Z ";
        private const string SEMI_COLLUM_SIGN = ": ";
        private const string DROPMENU_BUTTON_NAME = "Select Option";
        private const string START_BUTTON_NAME = "Start";
        private const string ERROR_TITLE = "Error";
        private const string OK_BUTTON_NAME = "OK";

        #endregion
        
        
        #region --- Members ---

        private static Sprite _sprite;
        private string _spriteName;
        private static Material _material;
        private string _materialName;
        private string _option = "";
        private float _xPosition;
        private float _yPosition;
        private float _zPosition;
        private float _xRotation;
        private float _yRotation;
        private float _zRotation;
        private float _xScale;
        private float _yScale;
        private float _zScale;
        private int _editorMode;
        
        private bool _shouldLookAtResourcesFolder = true;

        #endregion Members


        #region --- Editor Methods ---
        
        private void OnGUI()
        {
            HandleWindowPosition();
            CreateMenu();
            HandleMenuSelection();
            GUILayout.Space(21);

            if (GUILayout.Button(START_BUTTON_NAME))
            {
                Set(_option);
            }
        }

        #endregion Editor Methods


        #region --- Private Methods ---
        
        [MenuItem("Tools/Open Prefabs Editor")]
        private static void Init()
        {
            PrefabEditor window = (PrefabEditor)GetWindow(typeof(PrefabEditor));
            window.Show();
        }

        private void HandleWindowPosition()
        {
            int value = GUI.skin.window.padding.bottom;
            GUI.skin.window.padding.bottom = -20;
            Rect windowRect1 = GUILayoutUtility.GetRect(1, 17);
            windowRect1.x += 4;
            windowRect1.width -= 7;
            GUI.skin.window.padding.bottom = value;
        }

        private void CreateMenu()
        {
            if (GUILayout.Button(DROPMENU_BUTTON_NAME))
            {
                GenericMenu menu = new GenericMenu();

                AddMenuItem(menu, "Option/Sprite", SPRITE_FILED_NAME);
                AddMenuItem(menu, "Option/Material", MATERIAL_FILED_NAME);
                AddMenuItem(menu, "Option/Transform/Position", TRANSFORM_POSITION_FILED_NAME);
                AddMenuItem(menu, "Option/Transform/Rotation", TRANSFORM_ROTATION_FILED_NAME);
                AddMenuItem(menu, "Option/Transform/Scale", TRANSFORM_SCALE_FILED_NAME);

                menu.ShowAsContext();
            }
        }

        private void AddMenuItem(GenericMenu menu, string menuPath, string option)
        {
            menu.AddItem(new GUIContent(menuPath), _option.Equals(option), OnOptionSelected, option);
        }

        private void HandleMenuSelection()
        {
            switch (_option)
            {
                case SPRITE_FILED_NAME:
                    _shouldLookAtResourcesFolder = EditorGUILayout.Toggle("Resources Folder", _shouldLookAtResourcesFolder);
                    _spriteName = EditorGUILayout.TextField(SPRITE_FILED_NAME + " " + NAME_TITLE, _spriteName);
                    break;
                case MATERIAL_FILED_NAME:
                    _shouldLookAtResourcesFolder = EditorGUILayout.Toggle("Resources Folder", _shouldLookAtResourcesFolder);
                    _materialName = EditorGUILayout.TextField(MATERIAL_FILED_NAME + " " + NAME_TITLE, _materialName);
                    break;
                case TRANSFORM_POSITION_FILED_NAME:
                    _xPosition = EditorGUILayout.FloatField(X_VECTOR_NAME + TRANSFORM_POSITION_FILED_NAME + SEMI_COLLUM_SIGN, _xPosition);
                    _yPosition = EditorGUILayout.FloatField(Y_VECTOR_NAME + TRANSFORM_POSITION_FILED_NAME + SEMI_COLLUM_SIGN, _yPosition);
                    _zPosition = EditorGUILayout.FloatField(Z_VECTOR_NAME + TRANSFORM_POSITION_FILED_NAME + SEMI_COLLUM_SIGN, _zPosition);
                    break;
                case TRANSFORM_ROTATION_FILED_NAME:
                    _xRotation = EditorGUILayout.FloatField(X_VECTOR_NAME + TRANSFORM_ROTATION_FILED_NAME + SEMI_COLLUM_SIGN, _xRotation);
                    _yRotation = EditorGUILayout.FloatField(Y_VECTOR_NAME + TRANSFORM_ROTATION_FILED_NAME + SEMI_COLLUM_SIGN, _yRotation);
                    _zRotation = EditorGUILayout.FloatField(Z_VECTOR_NAME + TRANSFORM_ROTATION_FILED_NAME + SEMI_COLLUM_SIGN, _zRotation);
                    break;
                case TRANSFORM_SCALE_FILED_NAME:
                    _xScale = EditorGUILayout.FloatField(X_VECTOR_NAME + TRANSFORM_SCALE_FILED_NAME + SEMI_COLLUM_SIGN, _xScale);
                    _yScale = EditorGUILayout.FloatField(Y_VECTOR_NAME + TRANSFORM_SCALE_FILED_NAME + SEMI_COLLUM_SIGN, _yScale);
                    _zScale = EditorGUILayout.FloatField(Z_VECTOR_NAME + TRANSFORM_SCALE_FILED_NAME + SEMI_COLLUM_SIGN, _zScale);
                    break;
            }
        }
        
        private void Set(string selectedOption)
        {
            switch (selectedOption)
            {
                case SPRITE_FILED_NAME:
                {
                    _sprite = _shouldLookAtResourcesFolder ? GetComponentFromResources<Sprite>(SPRITE_FOLDER_NAME, _spriteName) : GetComponentFromFolder<Sprite>(SPRITE_FOLDER_NAME);
                
                    if (_sprite == null)
                    {
                        EditorUtility.DisplayDialog(ERROR_TITLE, "Cannot find the sprite in project", OK_BUTTON_NAME);
                
                        return;
                    }
                
                    foreach (GameObject go in Selection.gameObjects)
                    {
                        go.GetComponentInChildren<Image>().sprite = _sprite;
                    }

                    break;
                }
                case MATERIAL_FILED_NAME:
                {
                    _material = GetComponentFromResources<Material>(MATERIALS_FOLDER_NAME, _materialName);
                
                    if (_material == null)
                    {
                        EditorUtility.DisplayDialog(ERROR_TITLE, "Cannot find the material in project", OK_BUTTON_NAME);
                
                        return;
                    }
                
                    foreach (GameObject go in Selection.gameObjects)
                    {
                        go.GetComponentInChildren<MeshRenderer>().sharedMaterial = _material;
                    }

                    break;
                }
                default:
                {
                    foreach (GameObject go in Selection.gameObjects)
                    {
                        switch (selectedOption)
                        {
                            case TRANSFORM_POSITION_FILED_NAME:
                                go.GetComponentInChildren<Transform>().Translate(new Vector3(_xPosition, _yPosition, _zPosition));
                                break;
                            case TRANSFORM_ROTATION_FILED_NAME:
                                go.GetComponentInChildren<Transform>().Rotate(new Vector3(_xRotation, _yRotation, _yScale));
                                break;
                            case TRANSFORM_SCALE_FILED_NAME:
                                go.GetComponentInChildren<Transform>().localScale = new Vector3(_xScale, _yScale, _zScale);
                                break;
                            default:
                                EditorUtility.DisplayDialog(ERROR_TITLE, "Please select an option", OK_BUTTON_NAME);
                                return;
                        }
                    }

                    break;
                }
            }
        }

        private T GetComponentFromResources<T>(string folderName, string componentName) where T : Object
        {
            return Resources.Load<T>(folderName + "/" + componentName);
        }
        
        private T GetComponentFromFolder<T>(string folderName) where T : Object
        {
            Debug.LogError(AssetDatabase.GetAssetPath(_sprite));
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GetAssetPath(_sprite));
        }

        #endregion Private Methods


        #region --- Event Handler ---

        private void OnOptionSelected(object option)
        {
            _option = (string)option;
        }

        #endregion Event Handler
    }
}
