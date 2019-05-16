﻿using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ImageViewer
{
    public class SetDefaults
    {
        public static class FileAssociations
        {
            [DllImport("Shell32.dll")]
            public static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

            public const int SHCNE_ASSOCCHANGED = 0x8000000;
            public const int SHCNF_FLUSH = 0x1000;

            public static bool RegistryValueExists()
            {
                RegistryKey root = Registry.ClassesRoot.OpenSubKey(@"\ImageViewer", false);

                return root != null;
            }

            public static void SetAssociation()
            {
                string[] extensions = { ".bmp", ".gif", ".jpeg", ".jpg", ".png" };

                foreach (string ext in extensions)
                {
                    SetKeyValue(ext, "ImageViewer");
                }
                
                SetKeyValue("ImageViewer", "ImageFileViewer");
                SetKeyValue(@"ImageViewer\DefaultIcon", "\"" + Application.ExecutablePath + "\"");
                SetKeyValue(@"ImageViewer\shell\open\command", "\"" + Application.ExecutablePath + "\" \"%1\"");

                SHChangeNotify(SHCNE_ASSOCCHANGED, SHCNF_FLUSH, IntPtr.Zero, IntPtr.Zero);
            }

            private static void SetKeyValue(string keyPath, string value)
            {
                using (var key = Registry.ClassesRoot.CreateSubKey(keyPath))
                {
                    if (key.GetValue(null) as string != value)
                    {
                        key.SetValue(null, value);
                    }
                }
            }
        }
    }
}
