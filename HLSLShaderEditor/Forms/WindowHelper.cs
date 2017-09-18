using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;


namespace HLSLShaderEditor.Forms
{
    public static class WindowHelper
    {
        private static OpenTK.GameWindow m_CurrentWindow;

        public static void OnRunHideForm(GameWindow gameWindow)
        {
            OpenTK.GameWindow OTKWindow = GetForm(gameWindow);
            OTKWindow.VisibleChanged += OTKWindow_VisibleChanged;
            m_CurrentWindow = OTKWindow;
        }

        private static void OTKWindow_VisibleChanged(object sender, EventArgs e)
        {
            if (m_CurrentWindow.Visible == true)
                m_CurrentWindow.Visible = false;
        }

        public static OpenTK.GameWindow GetForm(this GameWindow gameWindow)
        {
            //Type type = typeof(Microsoft.Xna.Framework.OpenTKGameWindow);
            //System.Reflection.FieldInfo field = type.GetField("window", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //if (field != null)
            //    return field.GetValue(gameWindow) as OpenTK.GameWindow;
            return null;
        }
    }

    public class WindowHandleInfo
    {
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private IntPtr _MainHandle;

        public WindowHandleInfo(IntPtr handle)
        {
            this._MainHandle = handle;
        }

        public List<IntPtr> GetAllChildHandles()
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        [DllImport("user32.dll",
           EntryPoint = "GetParent",
           CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd); 

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }
    }


}
