using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace CommonFileConverter.Serializers
{
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
    internal class GCSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        private GCHandle gcHandle;

        public GCSafeHandle(byte[] rawdataHolder) : base(true)
        {
            gcHandle = GCHandle.Alloc(rawdataHolder, GCHandleType.Pinned);
            this.SetHandle(gcHandle.AddrOfPinnedObject());
        }

        public void RawSerialize(object serializable)
        {
            Marshal.StructureToPtr(serializable, handle, false);
        }

        public object RawDeserialize(Type objectType)
        {
            return Marshal.PtrToStructure(handle, objectType);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        override protected bool ReleaseHandle()
        {
            try
            {
                gcHandle.Free();
                return true;
            }
            catch (InvalidOperationException)
            {
                return true; // The handle was freed or never initialized.
            }
            catch
            {
                return false;
            }
        }
    }
}
