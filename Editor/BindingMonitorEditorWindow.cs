using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityMVVM.Binding;

namespace UnityMVVM.Editor
{
    public class BindingMonitorEditorWindow : EditorWindow
    {
        Color ViewModelColor = Color.green;
        Color PropertyColor = Color.yellow;
        Color ViewColor = Color.cyan;
        Color GameObjectColor = Color.white;
        Color FieldColor = Color.red;
        Color EventColor = Color.magenta;
        Color MethodColor = Color.blue;

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
            window.titleContent = new GUIContent("Binding Monitor");
            window.Show();
        }

        private bool ApplyFilters(DataBindingBase arg)
        {
            var oneWay = arg as OneWayDataBinding;
            var dataBinding = arg as DataBinding;

            if (oneWay)
            {
                if (_filterBy == FilterType.None) return true;
                if (oneWay.Connection == null) return false;
                return _filterBy == FilterType.Bound ? oneWay.Connection.IsBound : !oneWay.Connection.IsBound;
            }
            else if (dataBinding)
            {
                if (_filterBy == FilterType.None) return true;
                if (dataBinding.Connection == null) return false;
                return _filterBy == FilterType.Bound ? dataBinding.Connection.IsBound : !dataBinding.Connection.IsBound;
            }
            else return false;
        }

        private void DrawOneWay(OneWayDataBinding item)
        {
            var fmt = "";
            fmt += "{0}.{1}{7}{5}{3}.{4}{8}{9}{6}  ";

            item.SrcPropertyPath = item.SrcPropertyPath.Replace("--", "");
            item.DstPropertyPath = item.DstPropertyPath.Replace("--", "");

            var bindingStr = string.Format(
                fmt,                                                                                            // Format
                item.ViewModelName.Color(ViewModelColor),                                                                             // {0}
                item.SrcPropertyName.Color(PropertyColor),                                                                           // {1}
                item.gameObject.name.Color(GameObjectColor),                                                                           // {2}
                item._dstView.GetType().Name.Color(ViewColor),                                                                   // {3} 
                item.DstPropertyName.Color(PropertyColor),                                                                           // {4}
                (item is TwoWayDataBinding) ? "<->" : "->",                                                     // {5}
                (item is TwoWayDataBinding) ? "." + (item as TwoWayDataBinding)._dstChangedEventName.Color(EventColor) : "",            // {6}
                item.SrcPropertyPath.Length > 0 ? "." + item.SrcPropertyPath.Color(FieldColor) : "",                              // {7}
                item.DstPropertyPath.Length > 0 ? "." + item.DstPropertyPath.Color(FieldColor) : "",                              // {8}
               (item is TwoWayDataBinding) ? " -- " + item._dstView.GetType().Name.Color(ViewColor) : ""                         // {9}
                );

            var goStr = string.Format("<color=white><b>{0}</b></color>", item.gameObject.name);

            bool contains = ignoreCase ? bindingStr.ToLower().Contains(filter.ToLower()) : bindingStr.Contains(filter);

            if (string.IsNullOrEmpty(filter) || contains)
            {
                if (GUILayout.Button("", GUIStyles.BindingButton))
                    Selection.activeGameObject = item.gameObject;


                var btnRect = GUILayoutUtility.GetLastRect();
                var goLabelRect = new Rect(btnRect);

                goLabelRect.width /= 4;

                GUI.Box(goLabelRect, goStr, GUIStyles.LabelLeft);

                btnRect.x = goLabelRect.width;
                btnRect.width -= goLabelRect.width;
                GUI.Box(btnRect, bindingStr, GUIStyles.LabelLeft);
            }
        }

        private void DrawBinding(DataBinding item)
        {
            item.SrcPropertyPath = item.SrcPropertyPath.Replace("--", "");
            item.DstPropertyPath = item.DstPropertyPath.Replace("--", "");

            var isTwoWay = item.BindingMode == Enums.BindingMode.TwoWay;

            StringBuilder sb = new StringBuilder();
            sb.Append(item.ViewModelName.Color(ViewModelColor));
            sb.Append(item.SrcPropertyName.Color(PropertyColor));

            var bindingStr = $"{item.ViewModelName.Color(ViewModelColor)}" +
                $"." +
                $"{item.SrcPropertyName.Color(PropertyColor)}" +
                $"{(string.IsNullOrEmpty(item.SrcPropertyPath) ? "" : "." + item.SrcPropertyPath.Color(FieldColor))}" +
                $"{(item.BindingMode == Enums.BindingMode.OneWay ? " -->" : "")}" +
                $"{(item.BindingMode == Enums.BindingMode.TwoWay ? " <--> " : "")}" +
                $"{(item.BindingMode == Enums.BindingMode.OneWayToSource ? " <-- " : "")}" +
                $"{item.DstView.GetType().Name.Color(ViewColor)}." +
                $"{item.DstPropertyName.Color(PropertyColor)}" +
                $"{(string.IsNullOrEmpty(item.DstPropertyPath) ? "" : "." + item.DstPropertyPath)}" +
                $"{(item.BindingMode == Enums.BindingMode.TwoWay ? " -- " + item.DstChangedEventName.Color(EventColor) : "") }";


            var goStr = item.gameObject.name.Color(Color.white);

            bool contains = ignoreCase ? bindingStr.ToLowerInvariant().Contains(filter.ToLowerInvariant()) : bindingStr.Contains(filter);

            if (string.IsNullOrEmpty(filter) || contains)
            {
                if (GUILayout.Button("", GUIStyles.BindingButton))
                    Selection.activeGameObject = item.gameObject;


                var btnRect = GUILayoutUtility.GetLastRect();
                var goLabelRect = new Rect(btnRect);

                goLabelRect.width /= 4;

                GUI.Box(goLabelRect, goStr, GUIStyles.LabelLeft);

                btnRect.x = goLabelRect.width;
                btnRect.width -= goLabelRect.width;
                GUI.Box(btnRect, bindingStr, GUIStyles.LabelLeft);
            }
        }


        private void DrawBindings(DataBindingBase item)
        {
            if (item is OneWayDataBinding)
                DrawOneWay(item as OneWayDataBinding);
            else if (item is DataBinding)
                DrawBinding(item as DataBinding);
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

            var bindings = FindObjectsOfType<DataBindingBase>().OrderBy(ApplyOrder).Where(ApplyFilters);

            var eventPropBindings = FindObjectsOfType<EventPropertyBinding>();
            var eventBindings = FindObjectsOfType<EventBinding>();

            GUILayout.Label(string.Format("Data Bindings: {0}", bindings.Count()), EditorStyles.boldLabel);

            DrawHeader();

            onewWayScrollPos = EditorGUILayout.BeginScrollView(onewWayScrollPos, GUILayout.MaxHeight(600));

            bindings.ToList().ForEach(DrawBindings);


            EditorGUILayout.EndScrollView();

            GUILayout.Label(string.Format("Event Bindings: {0}", eventPropBindings.Count() + eventBindings.Count()), EditorStyles.boldLabel);

            eventPropScrollPos = EditorGUILayout.BeginScrollView(eventPropScrollPos, GUILayout.MaxHeight(400));

            eventPropBindings.ToList().ForEach(DrawEventPropertyBindings);
            eventBindings.ToList().ForEach(DrawEventBindings);

            EditorGUILayout.EndScrollView();

            var conns = BindingMonitor.Connections;

            GUILayout.Label(string.Format("Active Connections: {0}", conns.Count()), EditorStyles.boldLabel);

            connectionScrollPos = EditorGUILayout.BeginScrollView(connectionScrollPos, GUILayout.MaxHeight(600));

            foreach (var item in conns)
            {
                var str = string.Format("<b>{0}</b> Src: <b>{1}/{2}</b> Dst: <b>{3}/{4}</b> Bound: <b>{5}</b>", item.Owner, item.SrcTarget.propertyOwner.GetType().Name, item.SrcTarget.propertyName, item.DstTarget.propertyOwner.GetType().Name, item.DstTarget.propertyName, item.IsBound);

                bool contains = ignoreCase ? str.ToLower().Contains(filter.ToLower()) : str.Contains(filter);

                if (string.IsNullOrEmpty(filter) || contains)
                    if (GUILayout.Button(str, GUIStyles.BindingButton))
                        Selection.activeGameObject = (item.DstTarget.propertyOwner as MonoBehaviour).gameObject;
            }


            EditorGUILayout.EndScrollView();

            if (GUILayout.Button("Reset"))
                BindingMonitor.Reset();
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("GameObject".Color(GameObjectColor).Bold(), GUIStyles.LabelLeft);
            GUILayout.Label(
                 string.Format(
                "{1}.{2}.{3}<->{4}.{5}.{6} -- {7}",
                "GameObject".Color(GameObjectColor).Bold(),
                "ViewModel".Color(ViewModelColor),
                "SrcProperty".Color(PropertyColor),
                "SrcField".Color(FieldColor),
                "TargetView".Color(ViewColor),
                "TargetProperty".Color(PropertyColor),
                "TargetField".Color(FieldColor),
                "DestChangeEvent".Color(EventColor)

                ), GUIStyles.LabelLeft);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawEventBindings(EventBinding item)
        {
            var fmt = "";
            fmt += "{0}.{1}->{2}.{3} ";

            var bindingStr = string.Format(
                fmt,
                item.SrcView.GetType().Name.Color(ViewColor),
                item.SrcEventName.Color(EventColor),
                item.ViewModelName.Color(ViewModelColor),
               (item.DstMethodName + "()").Color(MethodColor),
                item.gameObject.name.Color(GameObjectColor));
            var goStr = string.Format("{0}", item.gameObject.name.Color(GameObjectColor).Bold());
            var lineStr = bindingStr.Concat(goStr).ToString();
            var contains = ignoreCase ? lineStr.ToLowerInvariant().Contains(filter.ToLowerInvariant()) : lineStr.Contains(filter);

            if (string.IsNullOrEmpty(filter) || contains)
            {
                if (GUILayout.Button("", GUIStyles.BindingButton))
                    Selection.activeGameObject = item.gameObject;

                var btnRect = GUILayoutUtility.GetLastRect();
                var goLabelRect = new Rect(btnRect);

                goLabelRect.width /= 4;

                GUI.Box(goLabelRect, goStr, GUIStyles.LabelLeft);

                btnRect.x = goLabelRect.width;
                btnRect.width -= goLabelRect.width;
                GUI.Box(btnRect, bindingStr, GUIStyles.LabelLeft);
            }
        }

        private void DrawEventPropertyBindings(EventPropertyBinding item)
        {
            var goStr = string.Format("{0}", item.gameObject.name.Color(GameObjectColor).Bold());

            item.DstPath.Replace("--", "");

            var argFmt = "";
            switch (item.ArgType)
            {
                case UnityMVVM.Types.EventArgType.None:
                    break;
                case UnityMVVM.Types.EventArgType.Property:
                    argFmt += string.Format("[Property] {0}.{1}", item.SrcView.GetType().Name.Color(ViewColor), item.SrcPropName.Color(PropertyColor));
                    break;
                case UnityMVVM.Types.EventArgType.String:
                    argFmt += string.Format("[String] \"{0}\"", item.StringArg);
                    break;
                case UnityMVVM.Types.EventArgType.Int:
                    argFmt += "[int] " + item.IntArg;
                    break;
                case UnityMVVM.Types.EventArgType.Float:
                    argFmt += "[float] " + item.FloatArg;
                    break;
                case UnityMVVM.Types.EventArgType.Bool:
                    argFmt += "[bool] " + item.BoolArg;
                    break;
                default:
                    break;
            }

            var fmt = "";
            fmt += "{0}.{1}->{2}.{3}{4} -- {5}";
            var bindingStr = string.Format(
                fmt,
                item.SrcView.GetType().Name.Color(ViewColor),
                item.SrcEventName.Color(EventColor),
                item.ViewModelName.Color(ViewModelColor),
                item.DstPropName.Color(PropertyColor),
                item.DstPath.Length > 0 ? "." + item.DstPath : "",
                argFmt
                );
            var lineStr = bindingStr.Concat(goStr).ToString();

            var contains = ignoreCase ? lineStr.ToLowerInvariant().Contains(filter.ToLowerInvariant()) : lineStr.Contains(filter);
            if (string.IsNullOrEmpty(filter) || contains)
            {
                if (GUILayout.Button("", GUIStyles.BindingButton))
                    Selection.activeGameObject = item.gameObject;

                var btnRect = GUILayoutUtility.GetLastRect();
                var goLabelRect = new Rect(btnRect);

                goLabelRect.width /= 4;

                GUI.Box(goLabelRect, goStr, GUIStyles.LabelLeft);

                btnRect.x = goLabelRect.width;
                btnRect.width -= goLabelRect.width;
                GUI.Box(btnRect, bindingStr, GUIStyles.LabelLeft);
            }
        }

        private object ApplyOrder(DataBindingBase arg)
        {
            return _orderBy == OrderType.GameObject ? arg.gameObject.name : arg.ViewModelName;
        }
    }

    public static class StringExt
    {
        public static string Color(this string str, Color c)
        {
            return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(c), str);
        }

        public static string Bold(this string str)
        {
            return string.Format("<b>{0}</b>", str);
        }
    }
}