using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    
    [CustomEditor(typeof(LoadManager))]
    public class CustomSaveDataInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Reset Save File"))
            {
               ResetSave();
            }

            // GUILayout.TextArea("Enter level you want to start with");
        }

        private void ResetSave()
        {
            File.Delete(DataSaver.SavePath + "\\" +DataSaver.SaveFileName);
        }
    }
}