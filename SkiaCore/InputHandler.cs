using Arqan;
using SkiaCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkiaCore
{
    internal static class InputHandler
    {
        static double mouseX, mouseY;
        static bool isLeftMousePressed;
        static List<IInteractableComponent> _components = new List<IInteractableComponent>();
        static IntPtr _window;

        public static void Initialize(IntPtr window)
        {
            _window = window;
            GLFW.glfwSetMouseButtonCallback(window, (wind, button, action, mods) =>
            {
                if (button == GLFW.GLFW_MOUSE_BUTTON_LEFT)
                {
                    if (action == GLFW.GLFW_PRESS)
                    {
                        isLeftMousePressed = true;
                    }
                    else if (action == GLFW.GLFW_RELEASE)
                    {
                        isLeftMousePressed = false;
                    }
                }
            });

            GLFW.glfwSetCursorPosCallback(window, (wind, x, y) =>
            {
                mouseX = x;
                mouseY = y;
            });
        }

        public static void Update()
        {
            foreach (var component in _components)
            {
                if (isLeftMousePressed && CheckIfInsideComponent(component))
                {
                    component.OnClick();
                }
            }
        }

        static bool CheckIfInsideComponent(IInteractableComponent component)
        {
            if (mouseX > component.X &&
                mouseX < component.X + component.Width &&
                mouseY > component.Y &&
                mouseY < component.Y + component.Height)
                return true;
            else
                return false;
        }

        public static void AddComponent(IInteractableComponent _input)
        {
            _components.Add(_input);
        }
    }
}