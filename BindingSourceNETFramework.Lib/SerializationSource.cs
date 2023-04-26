using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using BindingSourceNETFramework.Lib;
using System.Collections.Generic;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace BindingProject.Lib
{
    /// <summary>
    /// Wrestlers information serialization class
    /// </summary>
    public class DataSerialization
    {
        /// <summary>
        /// Serialize list of wrestlers to binary file
        /// </summary>
        /// <param name="bjjWrestler">List of wrestlers to serialize</param>
        /// <param name="fi">Location where to serialize</param>
        public static void SaveDataBin(List<BjjWrestler> bjjWrestlers, FileInfo fi)
        {
            if (File.Exists(fi.FullName) == true)
            {
                var stream = new System.IO.FileStream(fi.FullName, FileMode.Open);
                IFormatter fmt = new BinaryFormatter();
                fmt.Serialize(stream, bjjWrestlers);
                stream.Close();
            }
            else
            {
                var stream = new System.IO.FileStream(String.Format(fi.FullName + "\\newfile.bin"), FileMode.Create);
                IFormatter fmt = new BinaryFormatter();
                fmt.Serialize(stream, bjjWrestlers);
                stream.Close();
            }
        }
        /// <summary>
        /// Deserialize list of wrestlers from binary file
        /// </summary>
        /// <param name="bjjWrestler">List of wrestlers got after deserialization</param>
        /// <param name="fi">Location from where to serialize</param>
        public static void LoadDataBin(ref List<BjjWrestler> bjjWrestlers, FileInfo fi)
        {
            var stream = new System.IO.FileStream(fi.FullName, FileMode.Open);
            IFormatter fmt = new BinaryFormatter();
            try
            {
                bjjWrestlers = (List<BjjWrestler>)fmt.Deserialize(stream);
            }
            catch (Exception)
            {
                bjjWrestlers = new List<BjjWrestler>(1);
            }
            stream.Close();
        }
        /// <summary>
        /// Serialize list of wrestlers to json file
        /// </summary>
        /// <param name="bjjWrestler">List of wrestlers to serialize</param>
        /// <param name="fi">Location where to serialize</param>
        public static void SaveDataJson(List<BjjWrestler> bjjWrestlers, FileInfo fi)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(bjjWrestlers);
            if (File.Exists(String.Format(fi.FullName + "\\new_file.json")) == true)
            {
                File.SetAttributes(fi.FullName, FileAttributes.Normal);
                File.Delete(fi.FullName);
                File.WriteAllText(fi.FullName, json);
            }
            else
            {
                
                using (StreamWriter writer = new StreamWriter(String.Format(fi.FullName + "\\new_file.json"), false))
                {
                    writer.WriteLineAsync(json);
                }
                //FileStream fs = new FileStream(String.Format(fi.FullName + "new_file.json"), FileMode.Create);
                //fs.Write(json,0, json.Length);
                //fs.Close();
                //File.SetAttributes(String.Format(fi.FullName + "new_file.json"), FileAttributes.Normal);
                //File.WriteAllText(String.Format(fi.FullName + "new_file.json"), json);
            }
        }
        /// <summary>
        /// Deserialize list of wrestlers from json file
        /// </summary>
        /// <param name="bjjWrestler">List of wrestlers got after deserialization</param>
        /// <param name="fi">Location from where to serialize</param>
        public static void LoadDataJson(ref List<BjjWrestler> bjjWrestlers, FileInfo fi)
        {
            bjjWrestlers = JsonConvert.DeserializeObject<List<BjjWrestler>>(File.ReadAllText(fi.FullName));
        }

    }
}