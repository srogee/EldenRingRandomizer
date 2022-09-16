using Newtonsoft.Json;
using System.IO;

namespace EldenRingItemRandomizer
{
    internal static class JSON
    {
        /// <summary>
        /// Tries to parse the contents of a text file as the templated class.
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="filename">Text file path</param>
        /// <returns>Templated class, default constructed on error</returns>
        public static T ParseFile<T>(string filename, out bool success) where T : class, new()
        {
            try
            {
                return Parse<T>(File.ReadAllText(filename, System.Text.Encoding.UTF8), out success);
            }
            catch (System.Exception)
            {
                success = false;
                return new T();
            }
        }

        /// <summary>
        /// Tries to parse the contents of a text file as the templated class.
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="filename">Text file path</param>
        /// <returns>Templated class, default constructed on error</returns>
        public static T ParseFile<T>(string filename) where T : class, new()
        {
            return ParseFile<T>(filename, out _);
        }

        /// <summary>
        /// Tries to parse a string as the templated class.
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="serializedData">Serialized JSON</param>
        /// <returns>Templated class, default constructed on error</returns>
        public static T Parse<T>(string serializedData, out bool success) where T : class, new()
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(serializedData);
                if (obj == null)
                {
                    throw new System.Exception();
                }
                success = true;
                return obj;
            }
            catch (System.Exception)
            {
                success = false;
                return new T();
            }
        }

        /// <summary>
        /// Tries to parse a string as the templated class.
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="serializedData">Serialized JSON</param>
        /// <returns>Templated class, default constructed on error</returns>
        public static T Parse<T>(string serializedData) where T : class, new()
        {
            return Parse<T>(serializedData, out _);
        }

        /// <summary>
        /// Converts an object to JSON serialized data.
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="objectToSerialize">Object reference</param>
        /// <returns>Serialized JSON</returns>
        public static string Stringify<T>(T objectToSerialize) where T : class
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        /// <summary>
        /// Converts an object to JSON serialized data.
        /// </summary>
        /// <typeparam name="T">Class</typeparam>
        /// <param name="objectToSerialize">Object reference</param>
        /// <returns>Serialized JSON</returns>
        public static void SaveToFile<T>(string filename, T objectToSerialize) where T : class
        {
            File.WriteAllText(filename, Stringify(objectToSerialize), System.Text.Encoding.UTF8);
        }
    }
}
