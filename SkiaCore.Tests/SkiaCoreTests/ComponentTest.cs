using SkiaCore;
using SkiaCore.Common;
using SkiaCore.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xunit;

namespace SkiaCoreTests
{
    public class TestComponent : Component
    {
        public TestComponent(int width, int height, params object[] args) : base(width, height, args)
        {
        }
    }

    public class ComponentTest
    {
        [Fact]
        public void CheckIfTreeGenerates_Default()
        {
            var win = Core.CreateWindow(800, 600, "test");

            var c1 = new TestComponent(10, 10);
            var c2 = new TestComponent(10, 10);
            var c3 = new TestComponent(10, 10);
            var c4 = new TestComponent(5, 5);

            win.AddRenderComponent(c1);
            win.AddRenderComponent(c2);
            win.AddRenderComponent(c3, c2);
            win.AddRenderComponent(c4, c3);

            win.Recalculate();

            win.ExecuteOnUIThread(() => Assert.Equal(0, c1.X));
            win.ExecuteOnUIThread(() => Assert.Equal(0, c1.Y));

            win.ExecuteOnUIThread(() => Assert.Equal(0, c2.X));
            win.ExecuteOnUIThread(() => Assert.Equal(0, c2.Y));

            win.ExecuteOnUIThread(() => Assert.Equal(0, c3.X));
            win.ExecuteOnUIThread(() => Assert.Equal(0, c3.Y));

            win.ExecuteOnUIThread(() => Assert.Equal(0, c4.X));
            win.ExecuteOnUIThread(() => Assert.Equal(0, c4.Y));
        }

        [Fact]
        public void CheckIfTreeGenerates_Right()
        {
            var win = Core.CreateWindow(800, 600, "test");

            var c1 = new TestComponent(10, 10);
            var c2 = new TestComponent(10, 10);
            var c3 = new TestComponent(10, 10);
            var c4 = new TestComponent(5, 5);

            c1.SetJustifyContent(SkiaCore.Common.Justify.FlexEnd);
            c1.SetFlexDirection(SkiaCore.Common.FlexDirection.Column);
            c1.SetAlignItems(SkiaCore.Common.Alignment.FlexEnd);

            win.AddRenderComponent(c1);
            win.AddRenderComponent(c2);
            win.AddRenderComponent(c3, c2);
            win.AddRenderComponent(c4, c3);

            win.Recalculate();

            win.ExecuteOnUIThread(() => Assert.Equal(0, c1.X));
            win.ExecuteOnUIThread(() => Assert.Equal(0, c1.Y));

            win.ExecuteOnUIThread(() => Assert.Equal(790, c2.X));
            win.ExecuteOnUIThread(() => Assert.Equal(590, c2.Y));

            win.ExecuteOnUIThread(() => Assert.Equal(790, c3.X));
            win.ExecuteOnUIThread(() => Assert.Equal(590, c3.Y));

            win.ExecuteOnUIThread(() => Assert.Equal(790, c4.X));
            win.ExecuteOnUIThread(() => Assert.Equal(590, c4.Y));

        }
    }
}
