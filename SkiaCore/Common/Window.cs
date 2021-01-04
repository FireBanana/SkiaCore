using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static Point GetSize()
        {
            int w = 0, h = 0;
            GLFW.glfwGetWindowSize(Core.Window, ref w, ref h);
            return new Point(w, h);
        }
    }
}
