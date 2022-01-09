using Arqan;
using SkiaCore.Common;
using SkiaCore.Components;
using SkiaCore.GL;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("SkiaCoreTests")]

namespace SkiaCore
{
    public static class Core
    {
        internal static IntPtr Window;
        internal static int IdCounter = 0;

        private static readonly ConcurrentQueue<Action> _dispatcherQueue = new ConcurrentQueue<Action>();

        public static void Initialize
            (int width, int height, string title, SkiaCoreOptions options = new SkiaCoreOptions())
        {
            var thread = new Thread(() =>
            {
                Run(width, height, title, options);
            });

            thread.Start();
        }

        private static void Run(int width, int height, string title, SkiaCoreOptions options)
        {
            GraphicsRenderer.Initialize(width, height);

            var canvas = GraphicsRenderer.Surface.Canvas;

            GLInitializer.InitializeWindow();
            GLInitializer.SetUpOptions(options);

            Window = GLInitializer.CreateWindowContext(width, height, title);

            Events.Initialize(Window);

            GLInitializer.CreateProgram(GraphicsRenderer.Surface);

            InputHandler.Initialize(Window);

            while (GLFW.glfwWindowShouldClose(Window) == 0)
            {
                if (_dispatcherQueue.Count > 0)
                {
                    if (_dispatcherQueue.TryDequeue(out Action res))
                        res.Invoke();
                }

                GraphicsRenderer.Update();
                InputHandler.Update();

                GL11.glDrawElements(GL11.GL_TRIANGLES, 6, GL11.GL_UNSIGNED_INT, IntPtr.Zero);
                GLFW.glfwSwapBuffers(Window);

                GLFW.glfwWaitEvents(); //Pauses execution until event received
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
                GraphicsRenderer.UpdateLayout();
            }));
        }

        internal static void ExecuteOnUIThread(Action action)
        {
            _dispatcherQueue.Enqueue(action);
        }
    }
}
