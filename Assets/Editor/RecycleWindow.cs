using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RecycleWindow : EditorWindow
{
    [System.Serializable]
    public class Entity
    {
        public string content { private set; get; }
        public int height { private set; get; }

        public Entity(string pContent, int pHeight)
        {
            content = string.Format("{0} test content. height = {1}", pContent, pHeight);
            height = pHeight;
        }

        public void Draw(int pIndex, Rect pPosition, Vector2 pScrollViewPos)
        {
            var tPosition = new Rect(pScrollViewPos, pPosition.size);
            var tDrawRect = EditorGUILayout.GetControlRect(true, height);
            if (!tPosition.Overlaps(tDrawRect))
                return;

            //以下绘制只能通过GUI的相关方法来操作，不能使用Layout相关的方法

            var tColor = (pIndex & 1) == 1 ? Color.grey : Color.black;
            tColor.a = 0.6F;
            GUI.backgroundColor = tColor;
            EditorGUI.HelpBox(tDrawRect, content, MessageType.None);
            var tButtonRect = tDrawRect;
            tButtonRect.position = new Vector2(tDrawRect.position.x + 250, tDrawRect.position.y);
            tButtonRect.size = new Vector2(150, height);
            if (GUI.Button(tButtonRect, '\u2708'.ToString() + "click me"))
            {
                Debug.Log(content);
            }
            GUI.backgroundColor = Color.white;
        }
    }

    List<Entity> mEntitys;
    Vector2 mScrollViewPos;

    [MenuItem("Tools/RecycleWindow")]
    static void Open()
    {
        GetWindow<RecycleWindow>().Show();
    }

    void Awake()
    {
        mEntitys = new List<Entity>();
        for (int i = 0; i < 10000; i++)
        {
            mEntitys.Add(new Entity(i.ToString(), Random.Range(20, 80)));
        }
    }

    void OnGUI()
    {
        mScrollViewPos = EditorGUILayout.BeginScrollView(mScrollViewPos);
        EditorGUILayout.BeginVertical();
        for (int i = 0, imax = mEntitys.Count; i < imax; i++)
        {
            mEntitys[i].Draw(i, position, mScrollViewPos);
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }
}
