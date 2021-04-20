using Arqan;
using SkiaCore.Components;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkiaCore
{
    public static class Core
    {
        public delegate Component RenderFunction(SKSurface surface);
        private static SKSurface Surface { get; set; }

        static internal int Width = 800;
        static internal int Height = 600;

        internal static IntPtr Window;
        internal static int IdCounter = 0;

        public struct SkiaCoreOptions
        {
            public bool IsBorderless;
            public bool IsNotResizable;
            public SKColor BackgroundColor;
        } 

        static ConcurrentQueue<Action> _dispatcherQueue = new ConcurrentQueue<Action>();

        public static void Initialize(int width, int height, string title, SkiaCoreOptions options = new SkiaCoreOptions())
        {
            var thread = new Thread(() =>
            {
                Run(width, height, title, options);
            });
            thread.Start();

        }

        static void Run(int width, int height, string title, SkiaCoreOptions options)
        {
            Width = width;
            Height = height;

            var imageInfo = new SKImageInfo(Width, Height);

            using (SKSurface _surface = SKSurface.Create(imageInfo))
            {
                Surface = _surface;
                var canvas = _surface.Canvas;

                GLInitializer.InitializeWindow();

                SetUpOptions(options);

                Window = GLInitializer.CreateWindowContext(width, height, title);

                #region CALLBACKS
                GLFW.glfwSetWindowCloseCallback(Window, (win) => { GLFW.glfwTerminate(); Environment.Exit(0); });
                #endregion

                GLInitializer.Execute(_surface);

                GraphicsRenderer.Initialize(_surface, options.BackgroundColor);
                InputHandler.Initialize(Window);

                while (GLFW.glfwWindowShouldClose(Window) == 0)
                {
                    if(_dispatcherQueue.Count > 0)
                    {
                        Action res;

                        if (_dispatcherQueue.TryDequeue(out res))
                            res.Invoke();
                    }                        

                    GraphicsRenderer.Update();
                    InputHandler.Update();

                    GL11.glDrawElements(GL11.GL_TRIANGLES, 6, GL11.GL_UNSIGNED_INT, IntPtr.Zero);
                    GLFW.glfwSwapBuffers(Window);
                    GLFW.glfwWaitEvents(); //Pauses execution until event received
                }

            }

        }

        static void SetUpOptions(SkiaCoreOptions options)
        {
            GLFW.glfwWindowHint(GLFW.GLFW_DECORATED, options.IsBorderless ? GLFW.GLFW_FALSE : GLFW.GLFW_TRUE);
            GLFW.glfwWindowHint(GLFW.GLFW_RESIZABLE, options.IsNotResizable ? GLFW.GLFW_FALSE : GLFW.GLFW_TRUE);
        }
        
        public static void AddRenderComponent(RenderFunction func)
        {
            _dispatcherQueue.Enqueue(new Action(() =>
            {
                var component = func(Surface);
                GraphicsRenderer.AddComponent(component);

                if (component is InteractableComponent)
                    InputHandler.AddComponent(component as InteractableComponent);
            }));
        }

    }
}
