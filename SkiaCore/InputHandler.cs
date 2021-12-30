using Arqan;
using SkiaCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SkiaCore
{
    public static class InputHandler
    {
        public static double MouseX, MouseY;

        static List<InteractableComponent> _components = new List<InteractableComponent>();
        static IntPtr _window;

        static InteractableComponent _currentMouseTargetedComponent = null;
        static InteractableComponent _currentSelectedComponent = null;

        internal static void Initialize(IntPtr window)
        {
            _window = window;
            GLFW.glfwSetMouseButtonCallback(window, (wind, button, action, mods) =>
            {
                if (button == GLFW.GLFW_MOUSE_BUTTON_LEFT)
                {
                    if (action == GLFW.GLFW_PRESS)
                    {
                        if (_currentMouseTargetedComponent != null)
                        {
                            _currentSelectedComponent = _currentMouseTargetedComponent;
                            _currentSelectedComponent.OnClick();
                        }
                    }
                    else if (action == GLFW.GLFW_RELEASE)
                    {
                        if (_currentSelectedComponent != null)
                        {
                            _currentSelectedComponent.OnRelease();                            
                        }

                        _currentSelectedComponent = null;
                    }
                }
            });

            GLFW.glfwSetCursorPosCallback(window, (wind, x, y) =>
            {
                MouseX = x;
                MouseY = y;

                for(int i = _components.Count - 1; i >= 0; i--)
                {
                    if (_components[i].IsUnderCursor((int)x, (int)y))
                    {
                        if (_currentMouseTargetedComponent == null)
                        {
                            _currentMouseTargetedComponent = _components[i];
                            _components[i].OnMouseEnter();
                        }
                        else if (_currentMouseTargetedComponent != _components[i])
                        {
                            _currentMouseTargetedComponent.OnMouseExit();
                            _components[i].OnMouseEnter();
                            _currentMouseTargetedComponent = _components[i];
                        }

                        return;
                    }
                }

                if (_currentMouseTargetedComponent != null)
                {
                    _currentMouseTargetedComponent.OnMouseExit();
                    _currentMouseTargetedComponent = null;
                }
            });

            GLFW.glfwSetKeyCallback(window, (wind, key, scancode, action, mods) =>
            {
                
                {
                    switch (action)
                    {
                        case GLFW.GLFW_PRESS:
                            break;
                        case GLFW.GLFW_REPEAT:
                            break;
                        case GLFW.GLFW_RELEASE:
                            break;
                    }
                }
            });
        }

        internal static void Update()
        {

        }

        internal static void AddComponent(InteractableComponent _input)
        {
            _components.Add(_input);
        }
    }
}