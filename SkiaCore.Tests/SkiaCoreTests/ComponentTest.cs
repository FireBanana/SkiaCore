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
        private void CallInit()
        {
            Assert.Equal(IntPtr.Zero, Core.Window);

            var mInfo = typeof(Core)
                .GetMethod("SetUpInterfaces", BindingFlags.Static | BindingFlags.NonPublic);

            mInfo.Invoke(null, new object[] { 800, 600, "Test", new SkiaCoreOptions() });
        }

        [Fact]
        public void CheckIfTreeGenerates_Default()
        {
            CallInit();

            var c1 = new TestComponent(10, 10);
            var c2 = new TestComponent(10, 10);
            var c3 = new TestComponent(10, 10);
            var c4 = new TestComponent(5, 5);

            GraphicsRenderer.AddComponent(c1);
            GraphicsRenderer.AddComponent(c2);
            GraphicsRenderer.AddComponent(c3, c2);
            GraphicsRenderer.AddComponent(c4, c3);

            GraphicsRenderer.UpdateLayout();

            Assert.Equal(0, c1.X);
            Assert.Equal(0, c1.Y);

            Assert.Equal(0, c2.X);
            Assert.Equal(0, c2.Y);

            Assert.Equal(0, c3.X);
            Assert.Equal(0, c3.Y);

            Assert.Equal(0, c4.X);
            Assert.Equal(0, c4.Y);
        }

        [Fact]
        public void CheckIfTreeGenerates_Right()
        {
            CallInit();

            var c1 = new TestComponent(10, 10);
            var c2 = new TestComponent(10, 10);
            var c3 = new TestComponent(10, 10);
            var c4 = new TestComponent(5, 5);

            c1.SetJustifyContent(SkiaCore.Common.Justify.FlexEnd);
            c1.SetFlexDirection(SkiaCore.Common.FlexDirection.Column);
            c1.SetAlignItems(SkiaCore.Common.Alignment.FlexEnd);

            GraphicsRenderer.AddComponent(c1);
            GraphicsRenderer.AddComponent(c2);
            GraphicsRenderer.AddComponent(c3, c2);
            GraphicsRenderer.AddComponent(c4, c3);

            GraphicsRenderer.UpdateLayout();

            Assert.Equal(0, c1.X);
            Assert.Equal(0, c1.Y);

            Assert.Equal(790, c2.X);
            Assert.Equal(590, c2.Y);

            Assert.Equal(790, c3.X);
            Assert.Equal(590, c3.Y);

            Assert.Equal(790, c4.X);
            Assert.Equal(590, c4.Y);

        }
    }
}
