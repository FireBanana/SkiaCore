using SkiaCore;
using SkiaCore.Components;
using System;
using System.Collections.Generic;
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
            Core.Initialize(800, 600, "SkiaTest");

            var c1 = new TestComponent(10, 10);
            var c2 = new TestComponent(10, 10);
            var c3 = new TestComponent(10, 10);
            var c4 = new TestComponent(5, 5);

            Core.AddRenderComponent(c1);
            Core.AddRenderComponent(c2);
            Core.AddRenderComponent(c3, c2);
            Core.AddRenderComponent(c4, c3);

            Core.Recalculate();

            Core.ExecuteOnUIThread(() =>
            {
                Assert.Equal(0, c1.X);
                Assert.Equal(0, c1.Y);

                Assert.Equal(0, c2.X);
                Assert.Equal(0, c2.Y);

                Assert.Equal(0, c3.X);
                Assert.Equal(0, c3.Y);

                Assert.Equal(0, c4.X);
                Assert.Equal(0, c4.Y);
            });
        }

        [Fact]
        public void CheckIfTreeGenerates_Right()
        {
            Core.Initialize(800, 600, "SkiaTest");

            var c1 = new TestComponent(10, 10);
            var c2 = new TestComponent(10, 10);
            var c3 = new TestComponent(10, 10);
            var c4 = new TestComponent(5, 5);

            c1.SetJustifyContent(SkiaCore.Common.Justify.FlexEnd);
            c1.SetFlexDirection(SkiaCore.Common.FlexDirection.Column);
            c1.SetAlignItems(SkiaCore.Common.Alignment.FlexEnd);

            Core.AddRenderComponent(c1);
            Core.AddRenderComponent(c2);
            Core.AddRenderComponent(c3, c2);
            Core.AddRenderComponent(c4, c3);

            Core.Recalculate();

            Core.ExecuteOnUIThread(() =>
            {
                Assert.Equal(0, c1.X);
                Assert.Equal(0, c1.Y);

                Assert.Equal(790, c2.X);
                Assert.Equal(590, c2.Y);

                Assert.Equal(790, c3.X);
                Assert.Equal(590, c3.Y);

                Assert.Equal(790, c4.X);
                Assert.Equal(590, c4.Y);
            });
        }
    }
}
