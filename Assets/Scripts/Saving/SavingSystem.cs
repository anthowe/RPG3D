﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

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
                byte[] bytes = Encoding.UTF8.GetBytes("¡Hola Mando!");
                stream.Write(bytes, 0, bytes.Length);
            }
     
        }

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            print("Loading from " + path);
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                print (Encoding.UTF8.GetString(buffer));

            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
           // print("File is " + saveFile);
           // "/Users/anthonyhowe/Library/Application Support/DefaultCompany/RPG"
        }
    }
}

