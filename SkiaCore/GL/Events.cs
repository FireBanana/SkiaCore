using System;
using System.Collections.Generic;
using System.Text;
using Arqan;

namespace SkiaCore.GL
{
    public static class Events
    {
        private static IntPtr _window;

        private static Action _closeCallback;
        private static Action<int, int> _framebufferResizeCallback;

        internal static void Initialize(IntPtr winPtr)
        {
            _window = winPtr;

            GLFW.glfwSetWindowCloseCallback
                (
                _window, 
                (win) => { _closeCallback?.Invoke(); Environment.Exit(0); }
                );

            GLFW.glfwSetFramebufferSizeCallback
                (
                _window,
                (window, width, height) => 
                    {
                        _framebufferResizeCallback?.Invoke(width, height);
                        GL10.glViewport(0, 0, width, height);
                    }
                );
        }

        public static void SetWindowCloseCallback(Action cb) => _closeCallback = cb;
        public static void SetFramebufferResizeCallback(Action<int, int> cb)
            => _framebufferResizeCallback = cb;
    }
}
