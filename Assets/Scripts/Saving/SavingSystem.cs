using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {


        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());
            }
     
        }


        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Loading from " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState (formatter.Deserialize(stream));
            }
          
        }

        private object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(SavableEntity savable in FindObjectsOfType<SavableEntity>())
            {
                    state[savable.GetUniqueIdentifier()] = savable.CaptureState();
            }
            return state;
        }
        private void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (SavableEntity savable in FindObjectsOfType<SavableEntity>())
            {
               savable.RestoreState(stateDict[savable.GetUniqueIdentifier()]);
            }
        
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
           
        }
    }
}

