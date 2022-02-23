using Arqan;
using SkiaCore.Common;
using SkiaCore.Components;
using SkiaCore.GL;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkiaCore
{
    public class Window
    {
        internal IntPtr WindowPointer { get; private set; }

        internal readonly int Width;
        internal readonly int Height;
        internal readonly string Title;

        private readonly SkiaCoreOptions _options;
        private readonly UIQueue _queue;

        private GraphicsRenderer _renderer;

        private Events _eventSystem;
        //private readonly InputHandler _inputHandler;

        internal Window(int width, int height, string title,
            SkiaCoreOptions options = new SkiaCoreOptions())
        {
            Width = width;
            Height = height;

            Title = title;

            _options = options;

            _queue = new UIQueue();
        }

        public void AddRenderComponent(Component component, Component parent = null)
        {
            _queue.AddToQueue(() =>
            {
                _renderer.AddComponent(component, parent);

                if (component is InteractableComponent)
                    InputHandler.AddComponent(component as InteractableComponent);
            });
        }

        public void Recalculate()
        {
            _queue.AddToQueue(() =>
            {
                _renderer.UpdateLayout();
            });
        }

        public void ExecuteOnUIThread(Action action)
        {
            _queue.AddToQueue(action);
        }

        internal void SetUpInterfaces()
        {
            _renderer = new GraphicsRenderer(Width, Height);            

            GLInterface.InitializeWindow();

            WindowPointer = GLInterface.CreateWindowContext(Width, Height, Title);

            _eventSystem = new Events(WindowPointer);
            _eventSystem.SetWindowCloseCallback(() =>
            {
                GLInterface.DestroyWindow(WindowPointer);
            });

            _renderer.RegisterOnResize(_eventSystem);
            //InputHandler.Initialize(WindowPointer);

            GLInterface.SetUpOptions(_options);
            GLInterface.CreateProgram(_renderer.Surface.PeekPixels().GetPixels(), Width, Height);
        }

        internal void Update()
        {
            GLInterface.ActivateContext(WindowPointer);

            if (GLFW.glfwWindowShouldClose(WindowPointer) == 0)
            {              
                _queue.CallDispatch();

                _renderer.Update();
                //InputHandler.Update();

                GLInterface.Draw(WindowPointer);
                GLInterface.Poll();
            }
        }
    
        // TODO set close callback to remove from list
    }
}
