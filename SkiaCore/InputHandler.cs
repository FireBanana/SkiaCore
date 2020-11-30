using Arqan;
using System;

namespace SkiaCore
{
    internal static class InputHandler
    {
        static double mouseX, mouseY;

        public static void Update(IntPtr window)
        {
            GLFW.glfwGetCursorPos(window, ref mouseX, ref mouseY);
        }
    }
}