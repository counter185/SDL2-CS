using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SDL2;
using static SDL2.SDL;

namespace SDL2
{
    public static class SDL_gpu
    {
        #region SDL2#-gpu Variables

        private const string nativeLibName = "SDL2_gpu";

        #endregion

        #region pointer types
        public class GPU_Target : SDL2.SDL.PointerWrapper
        {
            public GPU_Target_Data data => Marshal.PtrToStructure<GPU_Target_Data>(this.p);
            public GPU_Target(IntPtr handle) : base(handle)
            {
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct GPU_Target_Data
        {
            public IntPtr renderer;
            public IntPtr context_target;
            public IntPtr image;
            public IntPtr data;
            public short w, h;
            public short base_w, base_h;
            public GPU_Rect clip_rect;
            public SDL_Color color;
            public GPU_Rect viewport;

            public int matrix_mode;
            public GPU_MatrixStack_Data projection_matrix;
            public GPU_MatrixStack_Data view_matrix;
            public GPU_MatrixStack_Data model_matrix;

            public GPU_Camera camera;

            public bool using_virtual_resolution;
            public bool use_clip_rect;
            public bool use_color;
            public bool use_camera;

            public uint depth_function;

            public IntPtr context;
            public int refcount;
            public bool use_depth_test;
            public bool use_depth_write;
            public bool is_alias;

            byte p;

        }
        [StructLayout(LayoutKind.Sequential)]
        public struct GPU_MatrixStack_Data
        {
            uint storage_size;
            uint size;
            IntPtr matrix;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct GPU_Camera
        {
            float x, y, z;
            float angle;
            float zoom_x, zoom_y;
            float z_near, z_far;  // z clipping planes
            bool use_centered_origin;  // move rotation/scaling origin to the center of the camera's view

            byte p0, p1, p2, p3, p4, p5, p6;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GPU_ErrorObject
        {
            IntPtr function;
            IntPtr details;
            int error;

            byte p0, p1, p2, p3;
        }
        public class GPU_Image : SDL2.SDL.PointerWrapper
        {
            public GPU_Image(IntPtr handle) : base(handle)
            {
            }
        }
        #endregion

        #region enums
        public const int GPU_INIT_ENABLE_VSYNC = 0x1;
        public const int GPU_INIT_DISABLE_VSYNC = 0x2;
        public const int GPU_INIT_DISABLE_DOUBLE_BUFFER = 0x4;
        public const int GPU_INIT_DISABLE_AUTO_VIRTUAL_RESOLUTION = 0x8;
        public const int GPU_INIT_REQUEST_COMPATIBILITY_PROFILE = 0x10;
        public const int GPU_INIT_USE_ROW_BY_ROW_TEXTURE_UPLOAD_FALLBACK = 0x20;
        public const int GPU_INIT_USE_COPY_TEXTURE_UPLOAD_FALLBACK = 0x40;

        public const uint GPU_VERTEX_SHADER = 0;
        public const uint GPU_FRAGMENT_SHADER = 1;
        public const uint GPU_PIXEL_SHADER = 1;
        public const uint GPU_GEOMETRY_SHADER = 2;

        public const uint GPU_FORMAT_LUMINANCE = 1;
        public const uint GPU_FORMAT_LUMINANCE_ALPHA = 2;
        public const uint GPU_FORMAT_RGB = 3;
        public const uint GPU_FORMAT_RGBA = 4;
        public const uint GPU_FORMAT_ALPHA = 5;
        public const uint GPU_FORMAT_RG = 6;
        public const uint GPU_FORMAT_YCbCr422 = 7;
        public const uint GPU_FORMAT_YCbCr420P = 8;
        public const uint GPU_FORMAT_BGR = 9;
        public const uint GPU_FORMAT_BGRA = 10;
        public const uint GPU_FORMAT_ABGR = 11;
        #endregion

        #region structs

        [StructLayout(LayoutKind.Sequential)]
        public struct GPU_Rect
        {
            public float x, y, w, h;
            public GPU_Rect(float x, float y, float w, float h)
            {
                this.x = x;
                this.y = y;
                this.w = w;
                this.h = h;
            }
            public GPU_Rect(SDL.SDL_Rect rect)
            {
                this.x = rect.x;
                this.y = rect.y;
                this.w = rect.w;
                this.h = rect.h;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GPU_ShaderBlock
        {
            public int position_loc, texcoord_loc, color_loc, modelViewProjection_loc;
        }
        #endregion

        #region SDL2#-gpu Functions

        [DllImport(nativeLibName, EntryPoint = "GPU_Init", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr INTERNAL_GPU_Init(int w, int h, int flags);
        public static GPU_Target GPU_Init(int w, int h, int flags) => new GPU_Target(INTERNAL_GPU_Init(w, h, flags));

        [DllImport(nativeLibName, EntryPoint = "GPU_Clear", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Clear(IntPtr target);
        public static void GPU_Clear(GPU_Target target) => GPU_Clear(target.p);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_ClearRGBA(IntPtr target, byte r, byte g, byte b, byte a);
        public static void GPU_ClearRGBA(GPU_Target target, byte r, byte g, byte b, byte a) => GPU_ClearRGBA(target.p, r, g, b, a);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetShapeBlending(bool enable);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float GPU_SetLineThickness(float thickness);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float GPU_GetLineThickness();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Pixel(IntPtr target, float x, float y, SDL.SDL_Color color);
        public static void GPU_Pixel(GPU_Target target, float x, float y, SDL.SDL_Color color) => GPU_Pixel(target.p, x, y, color);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Line(IntPtr target, float x1, float y1, float x2, float y2, SDL.SDL_Color color);
        public static void GPU_Line(GPU_Target target, float x1, float y1, float x2, float y2, SDL.SDL_Color color) => GPU_Line(target.p, x1, y1, x2, y2, color);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Rectangle(IntPtr target, float x1, float y1, float x2, float y2, SDL.SDL_Color color);
        public static void GPU_Rectangle(GPU_Target target, float x1, float y1, float x2, float y2, SDL.SDL_Color color) => GPU_Rectangle(target.p, x1,y1,x2,y2, color);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_RectangleFilled(IntPtr target, float x1, float y1, float x2, float y2, SDL.SDL_Color color);
        public static void GPU_RectangleFilled(GPU_Target target, float x1, float y1, float x2, float y2, SDL.SDL_Color color) => GPU_RectangleFilled(target.p, x1, y1, x2, y2, color);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Blit(IntPtr image, ref GPU_Rect src_rect, IntPtr target, float x, float y);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Blit(IntPtr image, IntPtr src_rect, IntPtr target, float x, float y);
        public static void GPU_Blit(GPU_Image image, GPU_Rect? src_rect, GPU_Target target, float x, float y)
        {
            if (src_rect == null)
            {
                GPU_Blit(image.p, IntPtr.Zero, target.p, x, y);
            }
            else
            {
                GPU_Rect rect = src_rect.Value;
                GPU_Blit(image.p, ref rect, target.p, x, y);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitTransform(IntPtr image, ref GPU_Rect src_rect, IntPtr target, float x, float y, float degrees, float x_scale, float y_scale);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitTransform(IntPtr image, IntPtr src_rect, IntPtr target, float x, float y, float degrees, float x_scale, float y_scale);
        public static void GPU_BlitTransform(GPU_Image image, GPU_Rect? src_rect, GPU_Target target, float x, float y, float degrees, float x_scale, float y_scale)
        {
            if (src_rect == null)
            {
                GPU_BlitTransform(image.p, IntPtr.Zero, target.p, x, y, degrees, x_scale, y_scale);
            }
            else
            {
                GPU_Rect rect = src_rect.Value;
                GPU_BlitTransform(image.p, ref rect, target.p, x, y, degrees, x_scale, y_scale);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitTransformX(IntPtr image, ref GPU_Rect src_rect, IntPtr target, float x, float y, float pivot_x, float pivot_y, float degrees, float scaleX, float scaleY);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitTransformX(IntPtr image, IntPtr src_rect, IntPtr target, float x, float y, float pivot_x, float pivot_y, float degrees, float scaleX, float scaleY);
        public static void GPU_BlitTransformX(GPU_Image image, GPU_Rect? src_rect, GPU_Target target, float x, float y, float pivot_x, float pivot_y, float degrees, float scaleX, float scaleY)
        {
            if (src_rect == null)
            {
                GPU_BlitTransformX(image.p, IntPtr.Zero, target.p, x,y, pivot_x, pivot_y, degrees, scaleX, scaleY);
            }
            else
            {
                GPU_Rect rect = src_rect.Value;
                GPU_BlitTransformX(image.p, ref rect, target.p, x, y, pivot_x, pivot_y, degrees, scaleX, scaleY);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitRect(IntPtr image, ref GPU_Rect src_rect, IntPtr target, ref GPU_Rect dest_rect);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitRect(IntPtr image, IntPtr src_rect, IntPtr target, ref GPU_Rect dest_rect);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitRect(IntPtr image, ref GPU_Rect src_rect, IntPtr target, IntPtr dest_rect);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_BlitRect(IntPtr image, IntPtr src_rect, IntPtr target, IntPtr dest_rect);
        public static void GPU_BlitRect(GPU_Image image, GPU_Rect? src_rect, GPU_Target target, GPU_Rect? dest_rect)
        {
            if (src_rect == null)
            {
                if (dest_rect == null)
                {
                    GPU_BlitRect(image.p, IntPtr.Zero, target.p, IntPtr.Zero);
                }
                else
                {
                    GPU_Rect rect = dest_rect.Value;
                    GPU_BlitRect(image.p, IntPtr.Zero, target.p, ref rect);
                }
            }
            else
            {
                GPU_Rect rect = src_rect.Value;
                if (dest_rect == null)
                {
                    GPU_BlitRect(image.p, ref rect, target.p, IntPtr.Zero);
                }
                else
                {
                    GPU_Rect destRect = dest_rect.Value;
                    GPU_BlitRect(image.p, ref rect, target.p, ref destRect);
                }
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_Flip(IntPtr target);
        public static void GPU_Flip(GPU_Target target) => GPU_Flip(target.p);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_FlushBlitBuffer();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetWindowResolution(ushort w, ushort h);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_SetFullscreen(bool enable_fullscreen, bool use_desktop_resolution);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GPU_GetFullscreen();

        #region SDL2-GPU  SHADER STUFF

        [DllImport(nativeLibName, EntryPoint = "GPU_CompileShader", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern uint INTERNAL_GPU_CompileShader(uint shader_type, byte* shader_source);
        public static unsafe uint GPU_CompileShader(uint shader_type, string shader_source)
        {
            using (var shaderSourceUTF8 = new StringW(shader_source))
            {
                return INTERNAL_GPU_CompileShader(
                    shader_type,
                    shaderSourceUTF8.utf8
                );
            }
        }

        [DllImport(nativeLibName, EntryPoint = "GPU_LoadShaderBlock", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern GPU_ShaderBlock INTERNAL_GPU_LoadShaderBlock(uint program_object, byte* position_name, byte* texcoord_name, byte* color_name, byte* modelViewMatrix_name);
        public static unsafe GPU_ShaderBlock GPU_LoadShaderBlock(uint program_object, string position_name, string texcoord_name, string color_name, string modelViewMatrix_name)
        {
            using (var positionNameUTF8 = new StringW(position_name))
            using (var texcoordNameUTF8 = new StringW(texcoord_name))
            using (var colorNameUTF8 = new StringW(color_name))
            using (var modelViewMatrixNameUTF8 = new StringW(modelViewMatrix_name))
            {
                return INTERNAL_GPU_LoadShaderBlock(
                    program_object,
                    positionNameUTF8.utf8,
                    texcoordNameUTF8.utf8,
                    colorNameUTF8.utf8,
                    modelViewMatrixNameUTF8.utf8
                );
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint GPU_LinkShaders(uint shader_object1, uint shader_object2);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetShaderBlock(GPU_ShaderBlock block);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_ShaderBlock GPU_GetShaderBlock();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_ActivateShaderProgram(uint program_object, IntPtr block);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_ActivateShaderProgram(uint program_object, ref GPU_ShaderBlock block);
        public static void GPU_ActivateShaderProgram(uint program_object, GPU_ShaderBlock? block)
        {
            if (block == null)
            {
                GPU_ActivateShaderProgram(program_object, IntPtr.Zero);
            } else
            {
                GPU_ShaderBlock blockValue = block.Value;
                GPU_ActivateShaderProgram(program_object, ref blockValue);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_DeactivateShaderProgram();

        [DllImport(nativeLibName, EntryPoint = "GPU_GetShaderMessage", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_GPU_GetShaderMessage();
        public static string GPU_GetShaderMessage() => UTF8_ToManaged(INTERNAL_GPU_GetShaderMessage());

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int GPU_GetAttributeLocation(uint program_object, byte* attrib_name);
        public static unsafe int GPU_GetAttributeLocation(uint program_object, string attrib_name)
        {
            using (var attribNameUTF8 = new StringW(attrib_name))
            {
                return GPU_GetAttributeLocation(program_object, attribNameUTF8.utf8);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static unsafe extern int GPU_GetUniformLocation(uint program_object, byte* uniform_name);
        public static unsafe int GPU_GetUniformLocation(uint program_object, string uniform_name)
        {
            using (var attribNameUTF8 = new StringW(uniform_name))
            {
                return GPU_GetUniformLocation(program_object, attribNameUTF8.utf8);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetShaderImage(IntPtr image, int location, int image_uint);
        public static void GPU_SetShaderImage(GPU_Image image, int location, int image_uint) => GPU_SetShaderImage(image.p, location, image_uint);


        //set one value
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetUniformi(int location, int value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetUniformui(int location, int value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetUniformf(int location, float value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetAttributei(int location, int value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetAttributeui(int location, int value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetAttributef(int location, float value);

        //set multiple values
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GPU_SetUniformiv(int location, int num_elements_per_value, int num_values, IntPtr values);
        public static unsafe void GPU_SetUniformiv(int location, int num_elements_per_value, int[] values)
        {
            fixed (int* valuesPtr = values)
            {
                GPU_SetUniformiv(location, values.Length, 1, (IntPtr)valuesPtr);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GPU_SetUniformuiv(int location, int num_elements_per_value, int num_values, IntPtr values);
        public static unsafe void GPU_SetUniformuiv(int location, int num_elements_per_value, uint[] values)
        {
            fixed (uint* valuesPtr = values)
            {
                GPU_SetUniformuiv(location, values.Length, 1, (IntPtr)valuesPtr);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GPU_SetUniformfv(int location, int num_elements_per_value, int num_values, IntPtr values);
        public static unsafe void GPU_SetUniformfv(int location, int num_elements_per_value, float[] values)
        {
            fixed (float* valuesPtr = values)
            {
                GPU_SetUniformfv(location, values.Length, 1, (IntPtr)valuesPtr);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GPU_SetAttributeiv(int location, int num_elements, IntPtr values);
        public static unsafe void GPU_SetAttributeiv(int location, int[] values)
        {
            fixed (int* valuesPtr = values)
            {
                GPU_SetAttributeiv(location, values.Length, (IntPtr)valuesPtr);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GPU_SetAttributeuiv(int location, int num_elements, IntPtr values);
        public static unsafe void GPU_SetAttributeuiv(int location, uint[] values)
        {
            fixed (uint* valuesPtr = values)
            {
                GPU_SetAttributeuiv(location, values.Length, (IntPtr)valuesPtr);
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GPU_SetAttributefv(int location, int num_elements, IntPtr values);
        public static unsafe void GPU_SetAttributefv(int location, float[] values)
        {
            fixed (float* valuesPtr = values)
            {
                GPU_SetAttributeuiv(location, values.Length, (IntPtr)valuesPtr);
            }
        }




        #endregion

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetRGBA(IntPtr image, byte r, byte g, byte b, byte a);
        public static void GPU_SetRGBA(GPU_Image image, byte r, byte g, byte b, byte a) => GPU_SetRGBA(image.p, r, g, b, a);

        [DllImport(nativeLibName, EntryPoint = "GPU_CreateImage", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr INTERNAL_GPU_CreateImage(ushort w, ushort h, uint format);
        public static GPU_Image GPU_CreateImage(ushort w, ushort h, uint format) => new GPU_Image(INTERNAL_GPU_CreateImage(w, h, format));

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern IntPtr GPU_LoadImage(byte* filename);
        public static unsafe GPU_Image GPU_LoadImage(string filename)
        {
            using (var str = new StringW(filename))
            {
                return new GPU_Image(GPU_LoadImage(str.utf8));
            }
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GPU_CopyImageFromSurface(IntPtr surface);
        public static GPU_Image GPU_CopyImageFromSurface(SDL.SDL_SurfacePtr surface) => new GPU_Image(GPU_CopyImageFromSurface(surface.p));

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern IntPtr GPU_LoadImage_RW(IntPtr rwops, bool free_rwops);
        public static unsafe GPU_Image GPU_LoadImage_RW(SDL_RWOpsPtr rwops, bool free_rwops)
        {
            return new GPU_Image(GPU_LoadImage_RW(rwops.p, free_rwops));
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_FreeImage(IntPtr image);
        public static void GPU_FreeImage(GPU_Image image) => GPU_FreeImage(image.p);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr GPU_LoadTarget(IntPtr image);
        public static GPU_Target GPU_LoadTarget(GPU_Image image) => new GPU_Target(GPU_LoadTarget(image.p));

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_FreeTarget(IntPtr target);
        public static void GPU_FreeTarget(GPU_Target target) => GPU_FreeTarget(target.p);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern GPU_ErrorObject GPU_PopErrorCode();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GPU_SetErrorQueueMax(uint max);

        #endregion
    }
}
