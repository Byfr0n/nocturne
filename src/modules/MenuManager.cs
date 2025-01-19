using UnityEngine;
using System.Collections.Generic;
using System;
namespace nocturne
{
    public class MenuManager : MonoBehaviour
    {
        private Vector2 scrollPosition;
        private static float menuWidth = 300f;
        private static float menuHeight = 400f;
        private bool isDragging;
        private Vector2 menuPosition = new Vector2(Screen.width / 2 - menuWidth / 2, Screen.height / 2 - menuHeight / 2);
        private Dictionary<string, bool> categoryStates = new Dictionary<string, bool>();
        private bool menuVisible = false;
        private Color accentColor = new Color(0.4f, 0.7f, 1f);
        private bool isPickerOpen = false;
        private Rect pickerRect = new Rect(0, 0, 200, 200);
        private Color currentColor = Color.white;
        private float h = 0, s = 1, v = 1;
        private Vector2 huePickerPos;
        private Vector2 satValPickerPos;
        private Texture2D hueTex;
        private Texture2D satValTex;
        private Dictionary<Color, Texture2D> colorTextures = new Dictionary<Color, Texture2D>();
        private GUIStyle labelStyle;
        private GUIStyle buttonStyle;
        private GUIStyle toggleStyle;
        private GUIStyle titleStyle;
        void OnDestroy()
        {
            foreach (var texture in colorTextures.Values)
            {
                Destroy(texture);
            }
            colorTextures.Clear();
        }
        void InitializeStyles()
        {
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle();
                labelStyle.normal.textColor = Color.white;
                labelStyle.fontSize = 12;
                labelStyle.margin = new RectOffset(5, 5, 5, 5);
            }
            if (buttonStyle == null)
            {
                buttonStyle = new GUIStyle();
                buttonStyle.normal.textColor = Color.white;
                buttonStyle.normal.background = Utils.GetColorTexture(new Color(0.2f, 0.2f, 0.2f, 0.9f));
                buttonStyle.hover.background = Utils.GetColorTexture(new Color(0.3f, 0.3f, 0.3f, 0.9f));
                buttonStyle.active.background = Utils.GetColorTexture(accentColor);
                buttonStyle.padding = new RectOffset(10, 10, 5, 5);
                buttonStyle.margin = new RectOffset(5, 5, 5, 5);
            }
            if (toggleStyle == null)
            {
                toggleStyle = new GUIStyle();
                toggleStyle.normal.textColor = Color.white;
                toggleStyle.padding = new RectOffset(25, 10, 5, 5);
                toggleStyle.margin = new RectOffset(5, 5, 5, 5);
            }
            if (titleStyle == null)
            {
                titleStyle = new GUIStyle();
                titleStyle.normal.textColor = Color.white;
                titleStyle.fontSize = 14;
                titleStyle.fontStyle = FontStyle.Bold;
                titleStyle.alignment = TextAnchor.MiddleCenter;
            }
        }

        CursorLockMode previousLock;
        bool previousVis;

        void LateUpdate()
        {
            if (menuVisible)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(ModuleVars.menuKey))
            {
                menuVisible = !menuVisible;

                if (!menuVisible) {
                    Cursor.lockState = previousLock;
                    Cursor.visible = previousVis;
                }

                if (menuVisible)
                {
                    previousLock = Cursor.lockState;
                    previousVis = Cursor.visible;
                }
            }
        }

        void OnGUI()
        {
            if (!menuVisible) return;
            InitializeStyles();
            GUI.DrawTexture(new Rect(menuPosition.x, menuPosition.y, menuWidth, menuHeight),
            Utils.GetColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.95f)));
            GUILayout.BeginArea(new Rect(menuPosition.x, menuPosition.y, menuWidth, menuHeight));
            DrawTitleBar();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            DrawContent();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        private void DrawTitleBar()
        {
            Rect titleBarRect = new Rect(menuPosition.x, menuPosition.y, menuWidth, 25);
            GUI.DrawTexture(titleBarRect, Utils.GetColorTexture(new Color(0.15f, 0.15f, 0.15f, 1f)));
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Label("nocturne v" + Loader.version, titleStyle, GUILayout.Height(25));
            GUILayout.EndHorizontal();
        }
        public Color someColorValue = Color.white;
        private void DrawContent()
        {
            GUILayout.Space(5);
            /*
            DrawCategory("Combat", () => {
                DrawToggle("Criticals", "Guarantees critical hits", ref someToggleValue);
                DrawToggle("Anti KB", "Prevents knockback", ref someToggleValue);
                DrawButton(
    "My Button",
    "Button Description",
    () => Debug.Log("Button clicked!")
);
                DrawColorPicker("ESP Color", "Choose the ESP color", ref someColorValue);
                DrawSlider("Speed", "Attack speed multiplier", ref someSliderValue, 0f, 2f);
            });
            DrawCategory("Movement", () => {
                DrawToggle("Speed", "Increases movement speed", ref someToggleValue);
                DrawToggle("Fly", "Enables flight mode", ref someToggleValue);
                DrawSlider("Height", "Maximum flight height", ref someSliderValue, 0f, 10f);
            });
            DrawCategory("Render", () => {
                DrawToggle("ESP", "See through walls", ref someToggleValue);
                DrawToggle("Tracers", "Draw lines to players", ref someToggleValue);
            });
            */
            DrawCategory("Sigma", () => {
                DrawToggle("Skbidi", "Makes you skibidi", ref someToggleValue);
            });
        }
        private bool someToggleValue = false;
        private float someSliderValue = 1f;
        void DrawCategory(string name, System.Action content)
        {
            if (!categoryStates.ContainsKey(name))
                categoryStates[name] = false;
            if (GUILayout.Button((categoryStates[name] ? "▼ " : "► ") + name, buttonStyle))
                categoryStates[name] = !categoryStates[name];
            if (categoryStates[name])
            {
                GUILayout.BeginVertical();
                content?.Invoke();
                GUILayout.EndVertical();
            }
        }
        void DrawToggle(string name, string description, ref bool value)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            if (GUILayout.Button(
                (value ? "✓ " : "   ") + name,
                buttonStyle))
            {
                value = !value;
            }
            GUILayout.Label(description, labelStyle);
            GUILayout.EndVertical();
        }
        void DrawButton(string name, string description, Action onClick)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            if (GUILayout.Button(name, buttonStyle))
            {
                onClick();
            }
            GUILayout.Label(description, labelStyle);
            GUILayout.EndVertical();
        }
        void DrawSlider(string name, string description, ref float value, float min, float max)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Label(name, labelStyle);
            value = GUILayout.HorizontalSlider(value, min, max, GUILayout.ExpandWidth(true));
            GUILayout.Label($"{description} ({value:F2})", labelStyle);
            GUILayout.EndVertical();
        }
        GUIStyle colorPrevStyle = new GUIStyle();
        public void DrawColorPicker(string name, string description, ref Color color)
        {
            var colorStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(2, 2, 2, 2)
            };
            GUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label(name, titleStyle);
                GUILayout.BeginHorizontal();
                {
                    colorPrevStyle.normal.background = Utils.GetColorTexture(color);
                    GUILayout.Box("", colorPrevStyle, GUILayout.Width(25), GUILayout.Height(25));
                    if (GUILayout.Button("Pick color", GUILayout.Width(80)))
                    {
                        isPickerOpen = !isPickerOpen;
                        if (isPickerOpen)
                        {
                            currentColor = color;
                            Color.RGBToHSV(currentColor, out h, out s, out v);
                            InitializeTextures();
                        }
                    }
                }
                GUILayout.EndHorizontal();
                if (isPickerOpen)
                {
                    pickerRect = GUILayout.Window(
                        GUIUtility.GetControlID(FocusType.Passive),
                        pickerRect,
                        DrawPickerWindow,
                        "Pick a color"
                    );
                }
                if (currentColor != color && isPickerOpen)
                {
                    color = currentColor;
                }
            }
            GUILayout.EndVertical();
        }
        private void DrawPickerWindow(int windowID)
        {
            const int padding = 10;
            const int hueWidth = 20;
            const int pickerSize = 150;
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            Rect satValRect = GUILayoutUtility.GetRect(pickerSize, pickerSize);
            if (Event.current.type == EventType.Repaint)
            {
                GUI.DrawTexture(satValRect, satValTex);
            }
            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
            {
                if (satValRect.Contains(Event.current.mousePosition))
                {
                    Vector2 coords = Event.current.mousePosition - new Vector2(satValRect.x, satValRect.y);
                    s = Mathf.Clamp01(coords.x / satValRect.width);
                    v = Mathf.Clamp01(1f - (coords.y / satValRect.height));
                    currentColor = Color.HSVToRGB(h, s, v);
                    Event.current.Use();
                }
            }
            if (Event.current.type == EventType.Repaint)
            {
                Vector2 circlePos = new Vector2(s * satValRect.width + satValRect.x, (1f - v) * satValRect.height + satValRect.y);
                float circleSize = 8f;
                Rect circleRect = new Rect(circlePos.x - circleSize / 2, circlePos.y - circleSize / 2, circleSize, circleSize);
                GUI.color = Color.black;
                GUI.DrawTexture(circleRect, Texture2D.whiteTexture);
                circleRect = new Rect(circleRect.x + 1, circleRect.y + 1, circleSize - 2, circleSize - 2);
                GUI.color = currentColor;
                GUI.DrawTexture(circleRect, Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
            GUILayout.Space(padding);
            Rect hueRect = GUILayoutUtility.GetRect(hueWidth, pickerSize);
            if (Event.current.type == EventType.Repaint)
            {
                GUI.DrawTexture(hueRect, hueTex);
            }
            if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
            {
                if (hueRect.Contains(Event.current.mousePosition))
                {
                    h = Mathf.Clamp01(1f - ((Event.current.mousePosition.y - hueRect.y) / hueRect.height));
                    currentColor = Color.HSVToRGB(h, s, v);
                    UpdateSatValTexture();
                    Event.current.Use();
                }
            }
            if (Event.current.type == EventType.Repaint)
            {
                float hueIndicatorY = hueRect.y + (1f - h) * hueRect.height;
                Rect indicatorRect = new Rect(hueRect.x, hueIndicatorY - 1, hueRect.width, 2);
                GUI.color = Color.black;
                GUI.DrawTexture(indicatorRect, Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
        private void InitializeTextures()
        {
            if (hueTex == null)
            {
                hueTex = new Texture2D(1, 64);
                for (int i = 0; i < 64; i++)
                {
                    hueTex.SetPixel(0, i, Color.HSVToRGB((float)i / 64f, 1, 1));
                }
                hueTex.Apply();
            }
            UpdateSatValTexture();
        }
        private void UpdateSatValTexture()
        {
            if (satValTex == null)
            {
                satValTex = new Texture2D(64, 64);
            }
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    satValTex.SetPixel(x, y, Color.HSVToRGB(h, (float)x / 64f, (float)y / 64f));
                }
            }
            satValTex.Apply();
        }
    }
}