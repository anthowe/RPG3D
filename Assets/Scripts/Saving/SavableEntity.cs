using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
public class SavableEntity : MonoBehaviour
    {
        [SerializeField] string uniqueIdentifier = "";

        public string GetUniqueIdentifier()
        {
                return uniqueIdentifier;
        }
        public object CaptureState()
        {
            
            return new SerializableVector3(transform.position);
        }
        public void RestoreState(object state)
        {
            print("Restoring state for " + GetUniqueIdentifier());
        }
#if UNITY_EDITOR
        private void Update() {
            print("Editing ");
            if (Application.IsPlaying(gameObject)) return;
           
            if(string.IsNullOrEmpty(gameObject.scene.path))return;

            
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if(string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

               
            }
#endif  
    }
}