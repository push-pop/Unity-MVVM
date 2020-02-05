using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;

public class BindingMonitorEditorWindow : EditorWindow
{
    public enum OrderType
    {
        GameObject,
        ViewModel
    }

    public enum FilterType
    {
        None,
        Bound,
        Unbound
    }

    public enum Type
    {
        OneWay,
        TwoWay,
        Event,
        EventProp,
        Activator,
        Visibility,
        All
    }

    [SerializeField]
    BindTarget src = new BindTarget(new object(), "testProp");

    public Vector2 onewWayScrollPos = Vector2.zero;
    public Vector2 twoWayScrollPos = Vector2.zero;
    public Vector2 eventScrollPos = Vector2.zero;
    public Vector2 eventPropScrollPos = Vector2.zero;
    public Vector2 connectionScrollPos = Vector2.zero;

    public string filter = "";
    public bool ignoreCase = true;

    public OrderType _orderBy = OrderType.ViewModel;
    public FilterType _filterBy = FilterType.None;

    [MenuItem("Unity-MVVM/Binding Monitor")]
    static void Init()
    {
        var window = (BindingMonitorEditorWindow)EditorWindow.GetWindow(typeof(BindingMonitorEditorWindow));

        window.Show();
    }

    private void Awake()
    {
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Filter:", GUILayout.MaxWidth(50));

        EditorGUIUtility.labelWidth = 80;
        GUILayout.ExpandWidth(false);

        filter = GUILayout.TextField(filter, 25, GUILayout.MaxWidth(800));
        ignoreCase = GUILayout.Toggle(ignoreCase, "Ignore Case", options: GUILayout.MaxWidth(100));
        _orderBy = (OrderType)EditorGUILayout.EnumPopup("Oder By:", _orderBy);
        EditorGUILayout.EndHorizontal();

        var bindings = FindObjectsOfType<DataBindingBase>().OrderBy(e => _orderBy == OrderType.GameObject ? e.gameObject.name : e.ViewModelName);

        var oneWays = bindings.Where(e => e is OneWayDataBinding /*&& !(e is TwoWayDataBinding)*/).Select(e => e as OneWayDataBinding).Where(
            e =>
            {
                if (_filterBy == FilterType.None) return true;
                if (e.Connection == null) return false;
                return _filterBy == FilterType.Bound ? e.Connection.IsBound : !e.Connection.IsBound;

            });
        var twoWays = bindings.Where(e => e is TwoWayDataBinding).Select(e => e as TwoWayDataBinding);

        var eventPropBindings = FindObjectsOfType<EventPropertyBinding>();
        var eventBindings = FindObjectsOfType<EventBinding>();

        GUILayout.Label(string.Format("Data Bindings: {0}", oneWays.Count()), EditorStyles.boldLabel);

        onewWayScrollPos = EditorGUILayout.BeginScrollView(onewWayScrollPos, GUILayout.MaxHeight(600));

        var buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.richText = true;
        buttonStyle.active.background = buttonStyle.normal.background;
        buttonStyle.margin = new RectOffset(0, 0, 0, 0);
        buttonStyle.stretchWidth = true;

        var bindingLabelStyle = new GUIStyle(GUI.skin.label)
        {
            richText = true,
            alignment = TextAnchor.MiddleRight
        };

        var labelStyle = new GUIStyle(bindingLabelStyle)
        { alignment = TextAnchor.MiddleLeft };


        foreach (var item in oneWays)
        {
            var fmt = "";
            fmt += "{0}.{1}{5}{3}.{4}{6}  ";

            var bindingStr = string.Format(fmt, item.ViewModelName, item.SrcPropertyName, item.gameObject.name, item._dstView.GetType().Name, item.DstPropertyName, (item is TwoWayDataBinding) ? "<->" : "->", (item is TwoWayDataBinding) ? " -- " + (item as TwoWayDataBinding)._dstChangedEventName : "");

            var goStr = string.Format("<color=blue><b>{0}</b></color>", item.gameObject.name);

            bool contains = ignoreCase ? bindingStr.ToLower().Contains(filter.ToLower()) : bindingStr.Contains(filter);

            if (string.IsNullOrEmpty(filter) || contains)
            {
                if (GUILayout.Button("", buttonStyle))
                    Selection.activeGameObject = item.gameObject;


                var btnRect = GUILayoutUtility.GetLastRect();
                var goLabelRect = new Rect(btnRect);

                goLabelRect.width /= 4;

                GUI.Box(goLabelRect, goStr, labelStyle);

                btnRect.x = goLabelRect.width;
                btnRect.width -= goLabelRect.width;
                GUI.Box(btnRect, bindingStr, labelStyle);
            }


        }

        EditorGUILayout.EndScrollView();

        GUILayout.Label(string.Format("Event Bindings: {0}", eventPropBindings.Count() + eventBindings.Count()), EditorStyles.boldLabel);

        eventPropScrollPos = EditorGUILayout.BeginScrollView(eventPropScrollPos, GUILayout.MaxHeight(400));
        foreach (var item in eventPropBindings)
        {
            var goStr = string.Format("<color=blue><b>{0}</b></color>", item.gameObject.name);

            var fmt = "";
            fmt += "{0}.{1}->{2}{3}()  ";
            var bindingStr = string.Format(fmt, item._srcView.GetType().Name, item.SrcEventName, item.ViewModelName, item.DstPropName, item.gameObject.name);

            if (string.IsNullOrEmpty(filter) || bindingStr.Contains(filter))
            {
                if (GUILayout.Button("", buttonStyle))
                    Selection.activeGameObject = item.gameObject;

                var btnRect = GUILayoutUtility.GetLastRect();
                var goLabelRect = new Rect(btnRect);

                goLabelRect.width /= 4;

                GUI.Box(goLabelRect, goStr, labelStyle);

                btnRect.x = goLabelRect.width;
                btnRect.width -= goLabelRect.width;
                GUI.Box(btnRect, bindingStr, labelStyle);
            }
        }

        foreach (var item in eventBindings)
        {
            var fmt = "";
            fmt += "{0}.{1}->{2}{3}()  ";

            var bindingStr = string.Format(fmt, item._srcView.GetType().Name, item.SrcEventName, item.ViewModelName, item.DstMethodName, item.gameObject.name);
            var goStr = string.Format("<color=blue><b>{0}</b></color>", item.gameObject.name);

            if (string.IsNullOrEmpty(filter) || bindingStr.Contains(filter))
            {
                if (GUILayout.Button("", buttonStyle))
                    Selection.activeGameObject = item.gameObject;

                var btnRect = GUILayoutUtility.GetLastRect();
                var goLabelRect = new Rect(btnRect);

                goLabelRect.width /= 4;

                GUI.Box(goLabelRect, goStr, labelStyle);

                btnRect.x = goLabelRect.width;
                btnRect.width -= goLabelRect.width;
                GUI.Box(btnRect, bindingStr, labelStyle);
            }
        }
        EditorGUILayout.EndScrollView();


        var conns = BindingMonitor.Connections;

        GUILayout.Label(string.Format("Active Connections: {0}", conns.Count()), EditorStyles.boldLabel);

        connectionScrollPos = EditorGUILayout.BeginScrollView(connectionScrollPos, GUILayout.MaxHeight(600));

        foreach (var item in conns)
        {
            var str = string.Format("<b>{0}</b> Src: <b>{1}/{2}</b> Dst: <b>{3}/{4}</b> Bound: <b>{5}</b>", item.Owner, item.SrcTarget.propertyOwner.GetType().Name, item.SrcTarget.propertyName, item.DstTarget.propertyOwner.GetType().Name, item.DstTarget.propertyName, item.IsBound);

            bool contains = ignoreCase ? str.ToLower().Contains(filter.ToLower()) : str.Contains(filter);

            if (string.IsNullOrEmpty(filter) || contains)
                if (GUILayout.Button(str, buttonStyle))
                    Selection.activeGameObject = (item.DstTarget.propertyOwner as MonoBehaviour).gameObject;
        }


        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Reset"))
            BindingMonitor.Reset();
    }
}
