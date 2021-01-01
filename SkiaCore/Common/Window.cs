using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Arqan;

namespace SkiaCore.Common
{
    public static class Window
    {
        public static void MoveWindow(int posX, int posY)
        {
            GLFW.glfwSetWindowPos(Core.Window, posX, posY);
        }
    }
}
