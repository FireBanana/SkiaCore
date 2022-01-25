using Arqan;
using SkiaCore.Common;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using SkiaCore;

namespace SkiaCore.GL
{
    internal static class GLInterface
    {
        static readonly float[] vertices =
        {
          1f,  1f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f,
          1f, -1f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f,
         -1f, -1f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f, 1.0f,
         -1f,  1f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f
        };

        static uint[] indices =
        {
         0, 1, 3,
         1, 2, 3
        };

        static uint[] VBO = { 0 };
        static uint[] VAO = { 0 };
        static uint[] EBO = { 0 };
        static uint[] texture = { 0 };

        internal static void InitializeWindow()
        {
            GLFW.glfwWindowHint(GLFW.GLFW_CONTEXT_VERSION_MAJOR, 3);
            GLFW.glfwWindowHint(GLFW.GLFW_CONTEXT_VERSION_MINOR, 3);
            GLFW.glfwWindowHint(GLFW.GLFW_OPENGL_PROFILE, GLFW.GLFW_OPENGL_CORE_PROFILE);
        }

        internal static void SetUpOptions(SkiaCoreOptions options)
        {
            GLFW.glfwWindowHint(GLFW.GLFW_DECORATED, options.IsBorderless ? GLFW.GLFW_FALSE : GLFW.GLFW_TRUE);
            GLFW.glfwWindowHint(GLFW.GLFW_RESIZABLE, options.IsNotResizable ? GLFW.GLFW_FALSE : GLFW.GLFW_TRUE);
        }

        internal static void CreateProgram(IntPtr surfacePtr, int width, int height)
        {
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

            GL10.glTexImage2D(GL11.GL_TEXTURE_2D, 0, GL11.GL_RGB, width, height, 0, GL12.GL_BGRA, GL11.GL_UNSIGNED_BYTE, surfacePtr);

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

            GL20.glDeleteShader(vertexShader);
            GL20.glDeleteShader(fragmentShader);

            GLFW.glfwSwapInterval(1);
        }

        internal static void Draw(IntPtr win)
        {
            GL11.glDrawElements(GL11.GL_TRIANGLES, 6, GL11.GL_UNSIGNED_INT, IntPtr.Zero);
            GLFW.glfwSwapBuffers(win);
        }

        internal static void RenderSurface(IntPtr textPtr, int width, int height)
        {
            GL10.glTexImage2D(
                GL11.GL_TEXTURE_2D, 0, GL11.GL_RGB,
                width, height, 0, GL12.GL_BGRA,
                GL11.GL_UNSIGNED_BYTE, textPtr
                );
        }

        internal static void Poll()
        {
            GLFW.glfwPollEvents();
        }

        internal static void ActivateContext(IntPtr win)
        {
            GLFW.glfwMakeContextCurrent(win);
        }

        internal static IntPtr CreateWindowContext(int width, int height, string title)
        {
            var win_ptr = GLFW.glfwCreateWindow(
                width, height, Encoding.ASCII.GetBytes(title), IntPtr.Zero, IntPtr.Zero);
            
            GLFW.glfwMakeContextCurrent(win_ptr);
            GL10.glViewport(0, 0, width, height);
            
            return win_ptr;
        }

        internal static void DestroyWindow(IntPtr window)
        {
            GLFW.glfwDestroyWindow(window);
        }

        internal static uint GetError()
        {
            return GL10.glGetError();
        }
    }
}
