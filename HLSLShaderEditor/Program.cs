using System;
using HLSLShaderEditor.Forms;

namespace HLSLShaderEditor
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            EditorWindow window = new EditorWindow();
            window.Show();
            using (var game = new Game1(window.GetPanelHandle()))
                game.Run();
        }
    }
}
