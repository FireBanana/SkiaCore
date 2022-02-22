using System;
using System.Collections.Generic;
using System.Text;
using Arqan;

namespace SkiaCore.GL
{
    public class Events
    {
        private readonly IntPtr             _window;

        private Action                      _closeCallback;
        private Action<int, int>            _framebufferResizeCallback;

        //===================================================================
        // The delegates are cached, otherwise GC automatically removes them.
        //===================================================================
        private readonly GLFW.GLFWframebuffersizefun _frameBufferFunction;
        private readonly GLFW.GLFWwindowclosefun     _windowcloseFunction;

        internal Events(IntPtr winPtr)
        {
            _window = winPtr;

            _frameBufferFunction = (window, width, height) =>
            {
                _framebufferResizeCallback?.Invoke(width, height);
                GLInterface.ActivateContext(_window);
                GL10.glViewport(0, 0, width, height);
            };

            // TODO Add Window removal from list
            _windowcloseFunction = (win) => _closeCallback?.Invoke();

            GLFW.glfwSetWindowCloseCallback
                (
                    _window,
                    _windowcloseFunction
                );

            GLFW.glfwSetFramebufferSizeCallback
                (
                    _window,
                    _frameBufferFunction
                );
        }

        public void SetWindowCloseCallback(Action cb) => _closeCallback = cb;
        public void SetFramebufferResizeCallback(Action<int, int> cb)
            => _framebufferResizeCallback = cb;
    }
}
