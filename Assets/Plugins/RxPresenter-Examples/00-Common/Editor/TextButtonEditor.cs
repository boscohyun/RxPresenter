using UnityEditor;
using UnityEditor.UI;

namespace Boscohyun.RxPresenter.Examples.Editor
{
    [CustomEditor(typeof(TextButton))]
    public class TextButtonEditor : ButtonEditor
    {
        private SerializedProperty _text;

        protected override void OnEnable()
        {
            base.OnEnable();
            _text = serializedObject.FindProperty("text");
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.PropertyField(_text);
        }
    }
}
