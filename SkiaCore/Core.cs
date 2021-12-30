using Arqan;
using SkiaCore.Common;
using SkiaCore.Components;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkiaCore
{
    public static class Core
    {
        private static SKSurface Surface { get; set; }

        static internal int Width = 800;
        static internal int Height = 600;

        internal static IntPtr Window;
        internal static int IdCounter = 0;

        static ConcurrentQueue<Action> _dispatcherQueue = new ConcurrentQueue<Action>();

        public static void Initialize(int width, int height, string title, SkiaCoreOptions options = new SkiaCoreOptions())
        {
            var thread = new Thread(() =>
            {
                Run(width, height, title, options);
            });
            thread.Start();

        }

        private static void Run(int width, int height, string title, SkiaCoreOptions options)
        {
            Width = width;
            Height = height;

            var imageInfo = new SKImageInfo(Width, Height);

            using (SKSurface _surface = SKSurface.Create(imageInfo))
            {
                Surface = _surface;
                var canvas = _surface.Canvas;

                GLInitializer.InitializeWindow();
                GLInitializer.SetUpOptions(options);

                Window = GLInitializer.CreateWindowContext(width, height, title);

                #region CALLBACKS

                //TODO Move
                GLFW.glfwSetWindowCloseCallback(Window, (win) => { GLFW.glfwTerminate(); Environment.Exit(0); });

                #endregion

                GLInitializer.CreateProgram(_surface);

                GraphicsRenderer.Initialize(_surface, width, height, options.BackgroundColor);
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
        
        public static void AddRenderComponent(Component component, Component parent = null)
        {
            _dispatcherQueue.Enqueue(new Action(() =>
            {
                GraphicsRenderer.AddComponent(component, parent);

                if (component is InteractableComponent)
                    InputHandler.AddComponent(component as InteractableComponent);
            }));
        }

        public static void Recalculate()
        {
            _dispatcherQueue.Enqueue(new Action(() =>
            {
                GraphicsRenderer.RecalculateTree();
            }));
        }
    }
}
