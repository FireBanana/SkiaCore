using Arqan;
using SkiaCore.Common;
using SkiaCore.Components;
using SkiaCore.GL;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("SkiaCoreTests")]

namespace SkiaCore
{
    public static class Core
    {
        private static readonly List<Window> _windowList = new List<Window>(); 

        private static readonly ConcurrentQueue<Window> _windowQueue 
            = new ConcurrentQueue<Window>();

        public static void Initialize()
        {
            var thread = new Thread(() => 
            {
                GLFW.glfwInit(); // TODO Move to interface
                Update();
            });

            thread.Start();
        }

        public static Window CreateWindow
            (int width, int height, string title,
            SkiaCoreOptions options = new SkiaCoreOptions())
        {
            var win = new Window(width, height, title, options);
            
            _windowQueue.Enqueue(win);            

            return win;
        }

        private static void Update()
        {
            while (true)
            {
                if (_windowQueue.TryDequeue(out var window))
                {
                    window.SetUpInterfaces();
                    _windowList.Add(window);
                }

                foreach (var win in _windowList)
                {
                    win.Update();
                }
            }
        }
    }
}
