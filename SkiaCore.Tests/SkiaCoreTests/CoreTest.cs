using System;
using Xunit;
using SkiaCore;

namespace SkiaCoreTests
{
    /// <summary>
    /// Tests for the Core module. Assertion may need to be
    /// performed on the UI thread.
    /// </summary>
    public class CoreTest
    {
        [Fact]
        public void CheckIfWindowInitialized()
        {
            Assert.Equal(Core.Window, IntPtr.Zero);

            Core.Initialize(500, 500, "SkiaTest");
            Core.ExecuteOnUIThread(() => Assert.NotEqual(Core.Window, IntPtr.Zero));            
        }

        [Fact]
        public void CheckIfGraphicsInitialized()
        {
            Core.Initialize(500, 500, "SkiaTest");
            Core.ExecuteOnUIThread(
                () => 
                {
                    Assert.NotNull(GraphicsRenderer.Surface);
                }
            );
        }
    }
}
