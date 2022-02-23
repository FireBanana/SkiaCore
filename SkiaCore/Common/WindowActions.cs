using SkiaCore;
using Arqan;

namespace SkiaCore.Common
{
    //Todo: Move to events
    public static class WindowActions
    {
        public static void MoveWindow(Window win, int posX, int posY)
        {
            GLFW.glfwSetWindowPos(win.WindowPointer, posX, posY);
        }

        public static Point GetSize(Window win)
        {
            int w = 0, h = 0;
            GLFW.glfwGetWindowSize(win.WindowPointer, ref w, ref h);
            return new Point(w, h);
        }
    }
}
