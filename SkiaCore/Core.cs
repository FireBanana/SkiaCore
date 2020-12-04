using Arqan;
using SkiaCore.Components;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkiaCore
{
    public static class Core
    {
        public delegate Component RenderFunction(SKSurface surface);
        private static SKSurface Surface { get; set; }

        static internal int Width = 800;
        static internal int Height = 600;

        static float[] vertices =
        {
         1f,  1f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
         1f, -1f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
         -1f, -1f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f,
         -1f, 1f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f
        };

        static uint[] indices =
        {
         0, 1, 3,
         1, 2, 3
        };

        static ConcurrentQueue<Action> _dispatcherQueue = new ConcurrentQueue<Action>();

        public static void Initialize(int width, int height, string title)
        {
            var thread = new Thread(() =>
            {
                Run(width, height, title);
            });
            thread.Start();

        }

        static void Run(int width, int height, string title)
        {
            Width = width;
            Height = height;

            var imageInfo = new SKImageInfo(Width, Height);

            using (SKSurface _surface = SKSurface.Create(imageInfo))
            {
                Surface = _surface;
                var canvas = _surface.Canvas;

                GLFW.glfwInit();
                GL10.glHint(GLFW.GLFW_CONTEXT_VERSION_MAJOR, 3);
                GL10.glHint(GLFW.GLFW_CONTEXT_VERSION_MINOR, 3);
                GL10.glHint(GLFW.GLFW_OPENGL_PROFILE, GLFW.GLFW_OPENGL_CORE_PROFILE);
                GL10.glHint(GLFW.GLFW_RESIZABLE, GLFW.GLFW_FALSE);

                var window = GLFW.glfwCreateWindow(Width, Height, Encoding.ASCII.GetBytes(title), IntPtr.Zero, IntPtr.Zero);
                GLFW.glfwMakeContextCurrent(window);
                GL10.glViewport(0, 0, Width, Height);

                uint[] VBO = { 0 };
                uint[] VAO = { 0 };
                uint[] EBO = { 0 };
                uint[] texture = { 0 };

                GL30.glGenVertexArrays(1, VAO);
                GL30.glBindVertexArray(VAO[0]);

                GL15.glGenBuffers(1, VBO);
                GL15.glBindBuffer(GL15.GL_ARRAY_BUFFER, VBO[0]);
                GL15.glBufferData(GL15.GL_ARRAY_BUFFER, vertices.Length * sizeof(float), vertices, GL15.GL_STATIC_DRAW);

                GL15.glGenBuffers(1, EBO);
                GL15.glBindBuffer(GL15.GL_ELEMENT_ARRAY_BUFFER, EBO[0]);
                GL15.glBufferData(GL15.GL_ELEMENT_ARRAY_BUFFER, indices.Length * sizeof(uint), indices, GL15.GL_STATIC_DRAW);

                GL11.glGenTextures(1, texture);
                GL11.glBindTexture(GL11.GL_TEXTURE_2D, texture[0]);

                GL20.glVertexAttribPointer(0, 3, GL11.GL_FLOAT, false, 8 * sizeof(float), IntPtr.Zero);
                GL20.glEnableVertexAttribArray(0);
                GL20.glVertexAttribPointer(1, 3, GL11.GL_FLOAT, false, 8 * sizeof(float), new IntPtr(3 * sizeof(float)));
                GL20.glEnableVertexAttribArray(1);
                GL20.glVertexAttribPointer(2, 2, GL11.GL_FLOAT, false, 8 * sizeof(float), new IntPtr(6 * sizeof(float)));
                GL20.glEnableVertexAttribArray(2);

                GL10.glTexParameteri(GL11.GL_TEXTURE_2D, GL11.GL_TEXTURE_WRAP_S, GL12.GL_CLAMP_TO_EDGE);
                GL10.glTexParameteri(GL11.GL_TEXTURE_2D, GL11.GL_TEXTURE_WRAP_T, GL12.GL_CLAMP_TO_EDGE);
                GL10.glTexParameteri(GL11.GL_TEXTURE_2D, GL11.GL_TEXTURE_MIN_FILTER, GL11.GL_LINEAR);
                GL10.glTexParameteri(GL11.GL_TEXTURE_2D, GL11.GL_TEXTURE_MAG_FILTER, GL11.GL_LINEAR);

                var dataPointer = _surface.PeekPixels().GetPixels();
                GL10.glTexImage2D(GL11.GL_TEXTURE_2D, 0, GL11.GL_RGB, Width, Height, 0, GL12.GL_BGRA, GL11.GL_UNSIGNED_BYTE, dataPointer);

                GL30.glGenerateMipmap(GL11.GL_TEXTURE_2D);

                uint vertexShader = GL20.glCreateShader(GL20.GL_VERTEX_SHADER);
                var shaderSource = @"#version 330 core
                                     layout(location = 0) in vec3 aPos;
                                     layout(location = 1) in vec3 aColor;
                                     layout(location = 2) in vec2 aTexCoord;
                                     out vec3 ourColor;
                                     out vec2 TexCoord;

                                     void main()
                                     {
                                         gl_Position = vec4(aPos, 1.0);
                                         ourColor = aColor;
                                         TexCoord = aTexCoord;
                                     }";

                GL20.glShaderSource(vertexShader, 1, new string[] { shaderSource }, new int[] { shaderSource.Length });
                GL20.glCompileShader(vertexShader);

                uint fragmentShader = GL20.glCreateShader(GL20.GL_FRAGMENT_SHADER);
                shaderSource = @"#version 330 core
                                 out vec4 FragColor;
                                 in vec3 ourColor;
                                 in vec2 TexCoord;
                                 uniform sampler2D ourTexture;

                                 void main(){
	                                 FragColor = texture(ourTexture, TexCoord);
                                 }";

                GL20.glShaderSource(fragmentShader, 1, new string[] { shaderSource }, new int[] { shaderSource.Length });
                GL20.glCompileShader(fragmentShader);

                uint shaderProgram = GL20.glCreateProgram();
                GL20.glAttachShader(shaderProgram, vertexShader);
                GL20.glAttachShader(shaderProgram, fragmentShader);
                GL20.glLinkProgram(shaderProgram);
                GL20.glUseProgram(shaderProgram);

                GLFW.glfwSwapInterval(1);

                GraphicsRenderer.Initialize(_surface);
                InputHandler.Initialize(window);

                while (GLFW.glfwWindowShouldClose(window) == 0)
                {
                    if(_dispatcherQueue.Count > 0)
                    {
                        Action res;

                        if (_dispatcherQueue.TryDequeue(out res))
                            res.Invoke();
                    }                        

                    GraphicsRenderer.Update();
                    InputHandler.Update();

                    GL11.glDrawElements(GL11.GL_TRIANGLES, 6, GL11.GL_UNSIGNED_INT, IntPtr.Zero);
                    GLFW.glfwSwapBuffers(window);
                    GLFW.glfwWaitEvents(); //Pauses execution until event received
                }

            }

        }

        public static void AddRenderComponent(RenderFunction func)
        {
            _dispatcherQueue.Enqueue(new Action(() =>
            {
                var component = func(Surface);
                GraphicsRenderer.AddComponent(component);

                if (component is InteractableComponent)
                    InputHandler.AddComponent(component as InteractableComponent);
            }));
        }

    }
}
