using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EXRImporter
{

    public class EXRFile : System.IDisposable
    {
        System.IntPtr mHandle;
        int mWidth;
        int mHeight;

        public int width { get => mWidth; }
        public int height { get => mHeight; }

        [DllImport("EXRImporterPlugin")]
        extern static System.IntPtr OpenEXR(string path, out int width, out int height);

        [DllImport("EXRImporterPlugin")]
        extern static void CloseEXR(System.IntPtr handle);

        [DllImport("EXRImporterPlugin")]
        extern static bool CopyRGBAData(System.IntPtr handle, System.IntPtr dest, int destLen);

        public EXRFile(string path)
        {
            mHandle = OpenEXR(path, out mWidth, out mHeight);
        }

        public unsafe bool CopyRGBAData(NativeArray<Color> result)
        {
            if (result.IsCreated && mHandle != System.IntPtr.Zero)
            {
                return CopyRGBAData(mHandle, (System.IntPtr)result.GetUnsafePtr(), sizeof(Color) * mWidth * mHeight);
            }
            return false;
        }

        public void Dispose()
        {
            if (mHandle != System.IntPtr.Zero)
            {
                CloseEXR(mHandle);
                mHandle = System.IntPtr.Zero;
            }
        }
    }

}
