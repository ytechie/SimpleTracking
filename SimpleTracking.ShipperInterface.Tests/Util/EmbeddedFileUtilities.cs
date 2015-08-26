using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SimpleTracking.ShipperInterface.Util
{
    /// <summary>
    ///		Utilities for working with files that are embedded
    ///		in .NET assemblies.
    /// </summary>
    public static class EmbeddedFileUtilities
    {
        /// <summary>
        ///		Reads an embedded text file from the specified assembly.
        /// </summary>
        /// <param name="asm">
        ///		The assembly that contains the embedded text document.
        /// </param>
        /// <param name="namespaceName">
        ///		The namespace used to organize the embedded text document.
        /// </param>
        /// <param name="fileName">
        ///		The full file name of the embedded text document.
        /// </param>
        /// <returns>
        ///		The contents of the embedded file.
        /// </returns>
        /// <exception cref="FileNotFoundException">
        ///		Raised when the embedded resource could not be found.
        /// </exception>
        public static string ReadEmbeddedTextFile(Assembly asm, string namespaceName, string fileName)
        {
            var resourceName = namespaceName + "." + fileName;
            using (var readStream = asm.GetManifestResourceStream(resourceName))
            {
                if (readStream == null)
                {
                    var failMsg = string.Format("Could not find embedded file: {0}", resourceName);
                    Debug.WriteLine(failMsg);
                    foreach (var resource in asm.GetManifestResourceNames())
                    {
                        Debug.WriteLine(string.Format("Found Resource: {0}", resource));
                    }
                    throw new Exception(failMsg);
                }

                using (var reader = new StreamReader(readStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}