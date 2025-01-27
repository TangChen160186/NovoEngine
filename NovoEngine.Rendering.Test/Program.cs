using OpenTK.Platform;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Core.Utility;
using System;

namespace Pal2Sample;

class Sample
{
    public static void Main(string[] args)
    {
        #region MyRegion

        EventQueue.EventRaised += EventRaised;

        ToolkitOptions options = new ToolkitOptions();

        options.ApplicationName = "OpenTK tutorial";
        options.Logger = new ConsoleLogger();

        Toolkit.Init(options);

        OpenGLGraphicsApiHints contextSettings = new OpenGLGraphicsApiHints()
        {
            // Here different options of the opengl context can be set.
            Version = new Version(4, 1),
            Profile = OpenGLProfile.Core,
            DebugFlag = true,
            DepthBits = ContextDepthBits.Depth24,
            StencilBits = ContextStencilBits.Stencil8,
        };

        WindowHandle window = Toolkit.Window.Create(contextSettings);

        OpenGLContextHandle glContext = Toolkit.OpenGL.CreateFromWindow(window);

        // Show window
        Toolkit.Window.SetTitle(window, "OpenTK window");
        Toolkit.Window.SetSize(window, new Vector2i(800, 600));
        Toolkit.Window.SetMode(window, WindowMode.Normal);

        // The the current opengl context and load the bindings.
        Toolkit.OpenGL.SetCurrentContext(glContext);
        GLLoader.LoadBindings(Toolkit.OpenGL.GetBindingsContext(glContext));

        #endregion





        while (true)
        {
            // This will process events for all windows and
            // post those events to the event queue.
            Toolkit.Window.ProcessEvents(false);
            if (Toolkit.Window.IsWindowDestroyed(window))
            {
                break;
            }

            GL.ClearColor(new Color4<Rgba>(64 / 255f, 0, 127 / 255f, 255));
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            Toolkit.OpenGL.SwapBuffers(glContext);
        }
    }

    static void EventRaised(PalHandle? handle, PlatformEventType type, EventArgs args)
    {
        if (args is CloseEventArgs closeArgs)
        {
            // Destroy the window that the user wanted to close.
            Toolkit.Window.Destroy(closeArgs.Window);
        }
    }
}

