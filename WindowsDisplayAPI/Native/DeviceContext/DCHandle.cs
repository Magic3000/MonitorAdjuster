using System;
using System.Runtime.InteropServices;

namespace WindowsDisplayAPI.Native.DeviceContext
{
    internal class DCHandle : SafeHandle
    {
        private readonly bool _created;

        private DCHandle(IntPtr handle, bool created) : base(handle, true)
        {
            _created = created;
        }

        public override bool IsInvalid
        {
            get => handle == IntPtr.Zero;
        }

        public static DCHandle CreateFromDevice(string screenName, string devicePath)
        {
            return new DCHandle(
                DeviceContextApi.CreateDC(screenName, devicePath, null, IntPtr.Zero),
                true
            );
        }

        public static DCHandle CreateFromScreen(string screenName)
        {
            return CreateFromDevice(screenName, screenName);
        }

        public static DCHandle CreateFromWindow(IntPtr windowHandle)
        {
            return new DCHandle(
                DeviceContextApi.GetDC(windowHandle),
                true
            );
        }

        public static DCHandle CreateGlobal()
        {
            return new DCHandle(
                DeviceContextApi.CreateDC("DISPLAY", null, null, IntPtr.Zero),
                true
            );
        }

        private static int handleExceptions = 0;
        protected override bool ReleaseHandle()
        {
            var toRet = true;
            try
            {
                toRet = _created
                    ? DeviceContextApi.DeleteDC(this)
                    : DeviceContextApi.ReleaseDC(IntPtr.Zero, this);
            }
            catch (Exception exc)
            {
                //handleExceptions++;
                //Console.WriteLine($"Exception in ReleaseHandle: {exc.Message}");
            }
            //Console.WriteLine("ReleaseHandle returning: " + toRet);
            return toRet;
        }
    }
}