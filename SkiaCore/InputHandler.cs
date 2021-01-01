﻿using Arqan;
using SkiaCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SkiaCore
{
    public static class InputHandler
    {
        public static double MouseX, MouseY;
        static bool isLeftMousePressed, isLeftMouseReleased; //TODO: Turn to struct
        static List<InteractableComponent> _components = new List<InteractableComponent>();
        static IntPtr _window;

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
                        isLeftMousePressed = true;
                    }
                    else if (action == GLFW.GLFW_RELEASE)
                    {
                        isLeftMouseReleased = true;
                    }
                }
            });

            GLFW.glfwSetCursorPosCallback(window, (wind, x, y) =>
            {
                MouseX = x;
                MouseY = y;
            });
        }

        internal static void Update()
        {
            foreach (var component in _components)
            {
                if (CheckIfInsideComponent(component))
                {
                    if (!component.WasMouseIn)
                    {
                        component.WasMouseIn = true;
                        component.OnMouseEnter();
                    }

                    if (isLeftMousePressed)
                    {
                        component.OnClick();
                        _currentSelectedComponent = component;
                        isLeftMousePressed = false;
                    }
                    else if (isLeftMouseReleased)
                    {
                        component.OnRelease();
                        _currentSelectedComponent = null;
                        isLeftMouseReleased = false;
                    }
                }
                else if (component.WasMouseIn || _currentSelectedComponent != null)
                {
                    if (component.WasMouseIn)
                    {
                        component.WasMouseIn = false;
                        component.OnMouseExit();
                    }

                    if(_currentSelectedComponent != null && isLeftMouseReleased)
                    {
                        isLeftMouseReleased = false;
                        _currentSelectedComponent.OnRelease();
                        _currentSelectedComponent = null;
                    }
                }
            }
        }

        static bool CheckIfInsideComponent(InteractableComponent component)
        {
            if (MouseX > component.X &&
                MouseX < component.X + component.Width &&
                MouseY > component.Y &&
                MouseY < component.Y + component.Height)
                return true;
            else
                return false;
        }

        internal static void AddComponent(InteractableComponent _input)
        {
            _components.Add(_input);
        }
    }
}