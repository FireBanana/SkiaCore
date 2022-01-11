using System;
using Xunit;
using SkiaCore;
using SkiaCore.Common;
using System.Threading;
using System.Reflection;

namespace SkiaCoreTests
{
    public class CoreTest
    {
        private void CallInit()
        {
            Assert.Equal(IntPtr.Zero, Core.Window);

            var mInfo = typeof(Core)
                .GetMethod("SetUpInterfaces", BindingFlags.Static | BindingFlags.NonPublic);

            mInfo.Invoke(null, new object[] { 50, 50, "Test", new SkiaCoreOptions() });
        }

        [Fact]
        public void CheckIfWindowInitialized()
        {
            CallInit();

            Assert.Equal((uint)0, Arqan.GL10.glGetError());
            Assert.NotEqual(IntPtr.Zero, Core.Window);
        }

        [Fact]
        public void CheckIfGraphicsInitialized()
        {
            CallInit();

            Assert.NotNull(GraphicsRenderer.Surface);

        }
    }
}
