using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Utilities.BetterHierarchy.BetterHierarchyPreferences;
using Type = System.Type;

namespace Utilities.BetterHierarchy
{
    [InitializeOnLoad]
    public static class BetterHierarchyIconDisplayer
    {
        private static bool hierarchyHasFocus;
        private static EditorWindow hierarchyEditorWindow;
        private static readonly HashSet<int> additionalSelectedInstanceIDs;
        private const float HIERARCHY_ICON_WIDTH = 18.5f;

        [Obsolete("Obsolete")]
        static BetterHierarchyIconDisplayer()
        {
			additionalSelectedInstanceIDs = new HashSet<int>();
            EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyWindowItemOnGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
            OnSettingsChangedEvents -= EditorApplication.RepaintHierarchyWindow;
            OnSettingsChangedEvents += EditorApplication.RepaintHierarchyWindow;
        }

        private static void OnEditorUpdate()
        {
            if (!hierarchyEditorWindow && IsHierarchyWindowFocused())
            {
                hierarchyEditorWindow = EditorWindow.GetWindow(
                    Type.GetType($"{nameof(UnityEditor)}.SceneHierarchyWindow,{nameof(UnityEditor)}"));
            }

            hierarchyHasFocus = EditorWindow.focusedWindow
                && EditorWindow.focusedWindow == hierarchyEditorWindow;
            
			additionalSelectedInstanceIDs.Clear();
        }
        
        private static bool IsHierarchyWindowFocused()
        {
            EditorWindow focusedWindow = EditorWindow.focusedWindow;
            Type sceneHierarchyWindowType = Type.GetType($"{nameof(UnityEditor)}.SceneHierarchyWindow,{nameof(UnityEditor)}");
            
            return focusedWindow != null && focusedWindow.GetType() == sceneHierarchyWindowType;
        }

        [Obsolete("Obsolete")]
        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            if (!IsEnabled)
                return;

            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (obj == null)
                return;

            if (IsShowDefaultIconForPrefab(obj))
                return;

            Component[] components = obj.GetComponents<Component>();
            int componentsLength = components != null ? components.Length : 0;

            if (components == null || componentsLength == 0)
                return;

            GUIContent content = GetTopComponentContent(components);

            if (content == null || content.image == null)
                return;

            ScriptIconType iconType = GetIconType();
            
            if (IsOverridePrefabIcons && IsPrefab(obj))
                iconType = OverridenPrefabIcons;

            HierarchyObjectStatus objectStatus = SetupHierarchyObjectStatus();
            UpdateSelectedObjectsList(objectStatus);

            switch (iconType)
            {
                case ScriptIconType.UnityDefault:
                    return;
                case ScriptIconType.BigIcon:
                    ClearOriginalIcon(objectStatus, selectionRect);
                    break;
                case ScriptIconType.SmallIcon:
                default:
                    break;
            }
            
            DrawIcon(selectionRect, content, obj, iconType);

            HierarchyObjectStatus SetupHierarchyObjectStatus()
            {
				const float HIERARCHY_EXPAND_ICON_WIDTH = 11f;
				const float HIERARCHY_EXPAND_ICON_X_OFFSET = HIERARCHY_EXPAND_ICON_WIDTH + 3f;
				
                HierarchyObjectStatus objectStatus = new HierarchyObjectStatus();
                
                Rect entireRowRect = selectionRect;
                entireRowRect.x = 0;
                entireRowRect.width = short.MaxValue;
                Rect expandChildrenIconRect = selectionRect;
                expandChildrenIconRect.x -= HIERARCHY_EXPAND_ICON_X_OFFSET;
                expandChildrenIconRect.width = HIERARCHY_EXPAND_ICON_WIDTH;

                objectStatus.IsSelected = Selection.instanceIDs.Contains(instanceID);
                objectStatus.IsHovered = entireRowRect.Contains(Event.current.mousePosition);
                objectStatus.IsDropDownHovered = expandChildrenIconRect.Contains(Event.current.mousePosition);

                return objectStatus;
            }

            void UpdateSelectedObjectsList(HierarchyObjectStatus objectStatus)
            {
                // If the object is selected or hovered with mouse down, add it to the additional selected list.
                // This list is cleared once per frame in OnEditorUpdate.
                if (objectStatus.IsSelected || (objectStatus.IsDropDownHovered && MouseStatus.IsMouseDown))
                {
                    additionalSelectedInstanceIDs.Add(instanceID);
                }
                else
                {
                    additionalSelectedInstanceIDs.Remove(instanceID);
                }
            }

            ScriptIconType GetIconType()
            {
                if (componentsLength > 2 && !IsShowAlwaysFirstScriptIcon)
                {
                    if (HasDefaultScriptIcon(content))
                        return ContainsNonUnityScriptsType;
                    
                    if (!HasCustomScripts(components, out Component component))
                        return ContainsOnlyUnityScriptsType;

                    if (component)
                        content = GetContent(component.GetType(), component);
                    else
                        content = GetContent(typeof(Component));
                    
                    return ContainsNonUnityScriptsType;
                }
                
                if (componentsLength == 2 || (componentsLength > 2 && IsShowAlwaysFirstScriptIcon))
                {
                    if (HasDefaultScriptIcon(content))
                        return ContainsSingleScriptType;

                    return ContainsOnlyUnityScriptsType;
                }
                
                return ContainsNoScriptsType;
            }
            
            static GUIContent GetTopComponentContent(Component[] components)
            {
                Component component = GetTopComponent(components);
                Type componentType = component ? component.GetType() : typeof(Component);
                
                return GetContent(componentType, component);
            }

            static bool IsShowDefaultIconForPrefab(GameObject obj)
            {
                return (IsOverridePrefabIcons && OverridenPrefabIcons == ScriptIconType.UnityDefault && IsPrefab(obj));
            }
        }


        private static bool IsPrefab(GameObject obj)
        {
            return (PrefabUtility.GetCorrespondingObjectFromOriginalSource(obj) != null);
        }

        private static Component GetTopComponent(IReadOnlyList<Component> components)
        {
            Component component = components.Count > 1 ? components[1] : components[0];
            bool isComponentMissing = (component == null);

            if (isComponentMissing)
                return component;
            
            var componentType = component.GetType();

            if (!IsShowAlwaysFirstScriptIcon && componentType == typeof(CanvasRenderer))
                component = components[components.Count - 1];

            return component;
        }

        private static void DrawIcon(Rect selectionRect, GUIContent objContent, GameObject obj, ScriptIconType iconType)
        {
            if (iconType == ScriptIconType.SmallIcon)
            {
                selectionRect.width = HIERARCHY_ICON_WIDTH / 2;
                selectionRect.height = HIERARCHY_ICON_WIDTH / 2;
                selectionRect.position += new Vector2(HIERARCHY_ICON_WIDTH / 2 - selectionRect.width / 2, HIERARCHY_ICON_WIDTH / 2 - selectionRect.height / 2);
            }

            Color originalColor = GUI.color;
            {
                if (!obj.activeInHierarchy)
                {
                    Color transparentIconColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);
                    GUI.color = transparentIconColor;
                }

                EditorGUI.LabelField(selectionRect, objContent);
            }
            GUI.color = originalColor;
        }

        private static GUIContent GetContent(Type type, Component component = null)
        {
            GUIContent content = EditorGUIUtility.ObjectContent(component, type);
            content.text = null;
            content.tooltip = IsEnableTooltipsOnHierarchyObject ? type.Name : "";

            return content;
        }

        [Obsolete("Obsolete")]
        private static void ClearOriginalIcon(HierarchyObjectStatus hierarchyObjectStatus, Rect selectionRect)
        {
            int selectedAmount = Selection.instanceIDs.Length > 1 ? Selection.instanceIDs.Length : additionalSelectedInstanceIDs.Count;
            Color color = UnityEditorBackgroundColor.Get(hierarchyObjectStatus, hierarchyHasFocus, selectedAmount);
            Rect backgroundRect = selectionRect;
            backgroundRect.width = HIERARCHY_ICON_WIDTH;
            EditorGUI.DrawRect(backgroundRect, color);
        }


        private static bool HasCustomScripts(IReadOnlyList<Component> components, out Component customComponent)
        {
            const int MAX_COMPONENT_CHECKS = 10;
            const int FIRST_COMPONENT_INDEX = 1;
            int componentsCount = components.Count;
            customComponent = null;

            // If there are too many scripts, assume there's a custom one to avoid excessive checks.
            if (componentsCount > MAX_COMPONENT_CHECKS + FIRST_COMPONENT_INDEX)
            {
                // In this scenario, we can't reliably determine the *first* custom component without iterating
                // so we just return true and leave customComponent as null, or set it to the first non-Unity component if found.
                // For now, we'll just indicate that custom scripts are likely present.
                return true;
            }

            for (int i = FIRST_COMPONENT_INDEX; i < componentsCount; i++)
            {
                var component = components[i];
                if (component == null) continue; // Skip null components

                bool isCustomScript = !IsNamespaceUnityRelated(component);

                if (isCustomScript)
                {
                    customComponent = component;
                    return true;
                }
            }

            return false;
        }

        private static bool HasDefaultScriptIcon(GUIContent content)
        {
            string imageName = content.image.name;

            if (string.IsNullOrEmpty(imageName))
                return false;
            
            return imageName.EndsWith("Script Icon");
        }

        private static bool IsNamespaceUnityRelated(Component component)
        {
            if (component == null)
                return false;
            
            string namespaceStr = component.GetType().Namespace;

            if (string.IsNullOrEmpty(namespaceStr))
                return false;

            bool isNativeUnityRelated;

            if (UnityScriptDetectionType == UnityNativeScriptsDetectionType.Unity)
                isNativeUnityRelated = namespaceStr.Contains("Unity");
            else if (UnityScriptDetectionType == UnityNativeScriptsDetectionType.UnityEngine)
                isNativeUnityRelated = namespaceStr.Equals(nameof(UnityEngine)) 
                                       || namespaceStr.Equals(nameof(UnityEditor)) 
                                       || namespaceStr.StartsWith(nameof(UnityEngine) + ".") 
                                       || namespaceStr.StartsWith(nameof(UnityEditor) + ".");
            else
                isNativeUnityRelated = false;

            return (isNativeUnityRelated || namespaceStr.StartsWith("TMPro"));
        }
    }
}