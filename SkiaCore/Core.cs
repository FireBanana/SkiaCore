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

        public static void Initialize
            (int width, int height, string title,
            SkiaCoreOptions options = new SkiaCoreOptions())
        {
            var thread = new Thread(() =>
            {
                InitializeInternal(width, height, title, options);
            });

            thread.Start();
        }

        private static void SetUpInterfaces(int width, int height, string title, SkiaCoreOptions options)
        {
            GraphicsRenderer.Initialize(width, height);
            GLInterface.InitializeWindow();

            Window = GLInterface.CreateWindowContext(width, height, title);

            Events.Initialize(Window);
            InputHandler.Initialize(Window);

            GLInterface.SetUpOptions(options);
            GLInterface.CreateProgram(GraphicsRenderer.Surface);
        }

        private static void InitializeInternal(int width, int height,
            string title, SkiaCoreOptions options)
        {
            SetUpInterfaces(width, height, title, options);

            while (GLFW.glfwWindowShouldClose(Window) == 0)
            {
                UIQueue.CallDispatch();

                GraphicsRenderer.Update();
                InputHandler.Update();

                GLInterface.Draw(Window);
                GLInterface.Poll();
            }
        }

        public static void AddRenderComponent(Component component, Component parent = null)
        {
            UIQueue.AddToQueue(() =>
            {
                GraphicsRenderer.AddComponent(component, parent);

                if (component is InteractableComponent)
                    InputHandler.AddComponent(component as InteractableComponent);
            });
        }

        public static void Recalculate()
        {
            UIQueue.AddToQueue(() =>
            {
                GraphicsRenderer.UpdateLayout();
            });
        }

        internal static void ExecuteOnUIThread(Action action)
        {
            UIQueue.AddToQueue(action);
        }
    }
}
