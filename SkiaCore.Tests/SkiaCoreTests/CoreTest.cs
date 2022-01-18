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
        [Fact]
        public void CheckIfWindowInitialized()
        {
            var win = Core.CreateWindow(800, 600, "test");

            win.ExecuteOnUIThread(() => Assert.False(win.GetWindowError()));
            win.ExecuteOnUIThread(() => Assert.NotEqual(IntPtr.Zero, win.WindowPointer));
        }
    }
}
