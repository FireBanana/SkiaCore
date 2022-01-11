using Arqan;
using SkiaCore.Common;
using SkiaCore.GL;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaCore
{
    internal class Window
    {
        internal readonly IntPtr WindowPointer;
        internal readonly int Width;
        internal readonly int Height;
        internal readonly string Title;

        private readonly SkiaCoreOptions _options;
        private readonly GraphicsRenderer _renderer;
        private readonly UIQueue _queue;
        //private readonly InputHandler _inputHandler;
        //private readonly EventSystem _eventSystem;

        internal Window(int width, int height, string title,
            SkiaCoreOptions options = new SkiaCoreOptions())
        {
            Width = width;
            Height = height;

            Title = title;

            _options = options;

            _renderer = new GraphicsRenderer();
            _queue = new UIQueue();
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

        internal void Update()
        {
            while (GLFW.glfwWindowShouldClose(WindowPointer) == 0)
            {
                UIQueue.CallDispatch();

                GraphicsRenderer.Update();
                InputHandler.Update();

                GLInterface.Draw(WindowPointer);
                GLInterface.Poll();
            }
        }

        public void AddRenderComponent(Component component, Component parent = null)
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
