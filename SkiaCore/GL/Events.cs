using System;
using System.Collections.Generic;
using System.Text;
using Arqan;

//======================================================================
// Important notes: Make sure that all the callback functions are cached
// otherwise the GC will remove them. Also make the context for the
// window current in each callback otherwise undefined behavior will 
// occur
//======================================================================

namespace SkiaCore.GL
{
    public class Events
    {
        private readonly IntPtr                      _window;

        private Action                               _closeCallback;
        private Action<int, int>                     _framebufferResizeCallback;

        private readonly GLFW.GLFWframebuffersizefun _frameBufferFunction;
        private readonly GLFW.GLFWwindowclosefun     _windowcloseFunction;

        internal Events(IntPtr winPtr)
        {
            _window = winPtr;

            _frameBufferFunction = (window, width, height) =>
            {
                GLInterface.ActivateContext(_window);
                _framebufferResizeCallback?.Invoke(width, height);
                GL10.glViewport(0, 0, width, height);
            };

            // TODO Add Window removal from list
            _windowcloseFunction = (win) =>
            {
                GLInterface.ActivateContext(_window);
                _closeCallback?.Invoke();
            };

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
