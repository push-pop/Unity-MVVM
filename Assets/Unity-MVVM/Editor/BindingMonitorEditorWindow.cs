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

        var oneWays = bindings.Where(e => e is OneWayDataBinding && !(e is TwoWayDataBinding)).Select(e => e as OneWayDataBinding).Where(
            e =>
            {
                if (_filterBy == FilterType.None) return true;
                if (e.Connection == null) return false;
                return _filterBy == FilterType.Bound ? e.Connection.IsBound : !e.Connection.IsBound;

            });
        var twoWays = bindings.Where(e => e is TwoWayDataBinding).Select(e => e as TwoWayDataBinding);

        var eventPropBindings = FindObjectsOfType<EventPropertyBinding>();
        var eventBindings = FindObjectsOfType<EventBinding>();

        GUILayout.Label(string.Format("OneWayBindings: {0}", oneWays.Count()), EditorStyles.boldLabel);

        onewWayScrollPos = EditorGUILayout.BeginScrollView(onewWayScrollPos, GUILayout.MaxHeight(600));
        var style = new GUIStyle(GUI.skin.label);
        style.richText = true;

        foreach (var item in oneWays)
        {
            var str = string.Format("<b>{0}</b> Src: <b>{1}:{2}</b> Dst: <b>{3}:{4}</b> Bound: <b>{5}</b>", item.gameObject.name, item.ViewModelName, item.SrcPropertyName, item._dstView.GetType().Name, item.DstPropertyName, item.Connection == null ? false : item.Connection.IsBound);

            bool contains = ignoreCase ? str.ToLower().Contains(filter.ToLower()) : str.Contains(filter);

            if (string.IsNullOrEmpty(filter) || contains)
                EditorGUILayout.LabelField(str, style);
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Label(string.Format("TwoWay Bindings: {0}", twoWays.Count()), EditorStyles.boldLabel);

        twoWayScrollPos = EditorGUILayout.BeginScrollView(twoWayScrollPos, GUILayout.MaxHeight(400));
        foreach (var item in twoWays)
        {
            var str = string.Format("<b>{0}</b> Src: <b>{1}/{2}</b> Dst: <b>{3}/{4}</b> Event: <b>{4}</b> Bound: <b>{5}</b>", item.gameObject.name, item.ViewModelName, item.SrcPropertyName, item._dstView.GetType().Name, item.DstPropertyName, item._dstChangedEventName, item.Connection == null ? false : item.Connection.IsBound);

            if (string.IsNullOrEmpty(filter) || str.Contains(filter))
                EditorGUILayout.LabelField(str, style);
        }
        EditorGUILayout.EndScrollView();


        GUILayout.Label(string.Format("Event Bindings: {0}", eventPropBindings.Count()), EditorStyles.boldLabel);

        eventPropScrollPos = EditorGUILayout.BeginScrollView(eventPropScrollPos, GUILayout.MaxHeight(400));
        foreach (var item in eventPropBindings)
        {
            var str = string.Format("<b>{0}</b> SrcEvent: <b>{1}/{2}</b> DstProp: <b>{3}/{4}</b>", item.gameObject.name, item._srcView.GetType().Name, item.SrcEventName, item.ViewModelName, item.DstPropName);

            if (string.IsNullOrEmpty(filter) || str.Contains(filter))
                EditorGUILayout.LabelField(str, style);
        }
        EditorGUILayout.EndScrollView();


        var conns = BindingMonitor.Connections;

        GUILayout.Label(string.Format("Active Connections: {0}", conns.Count()), EditorStyles.boldLabel);

        if (GUILayout.Button("Reset"))
            BindingMonitor.Reset();

        connectionScrollPos = EditorGUILayout.BeginScrollView(connectionScrollPos, GUILayout.MaxHeight(600));

        foreach (var item in conns)
        {
            var str = string.Format("<b>{0}</b> Src: <b>{1}/{2}</b> Dst: <b>{3}/{4}</b> Bound: <b>{5}</b>", item.Owner, item.SrcTarget.propertyOwner.GetType().Name, item.SrcTarget.propertyName, item.DstTarget.propertyOwner.GetType().Name, item.DstTarget.propertyName, item.IsBound);

            bool contains = ignoreCase ? str.ToLower().Contains(filter.ToLower()) : str.Contains(filter);

            if (string.IsNullOrEmpty(filter) || contains)
                EditorGUILayout.LabelField(str, style);
        }


        EditorGUILayout.EndScrollView();
    }
}
