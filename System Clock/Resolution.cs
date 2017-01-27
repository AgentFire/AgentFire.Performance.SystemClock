using System;
using System.Runtime.InteropServices;

namespace AgentFire.Performance.SystemClock
{
    /// <summary>
    /// Represents a reliable way to change system clock's resolution.
    /// </summary>
    /// <remarks>
    /// A resolution change is not persistent and rolls back to its previous value (but not always though) once the calling process is closed.
    /// If another process makes a call to change the resolution, then if that process' desired value is more precise than this process', then the system clock will be changed.
    /// If another process' desired value is less precise, then it will not be changed until this process is closed.
    /// </remarks>
    public static class Resolution
    {
        private const string NtDll = "ntdll.dll";

        [DllImport(NtDll, SetLastError = true)]
        private static extern int NtQueryTimerResolution(out int MinimumResolution, out int MaximumResolution, out int CurrentResolution);

        [DllImport(NtDll, SetLastError = true)]
        private static extern int NtSetTimerResolution(int DesiredResolution, bool SetResolution, out int CurrentResolution);

        static Resolution()
        {
            int min, max, dummy;
            NtQueryTimerResolution(out min, out max, out dummy);
            Min = TimeSpan.FromTicks(min);
            Max = TimeSpan.FromTicks(max);
        }

        /// <summary>
        /// Gets system minimum clock resolution. (Slowest, consumes less CPU, provides poorer performance).
        /// </summary>
        public static TimeSpan Min { get; }

        /// <summary>
        /// Gets system maximum clock resolution. (Fastest, consumes more CPU, provides better performance).
        /// </summary>
        public static TimeSpan Max { get; }

        /// <summary>
        /// Gets system current clock resolution.
        /// </summary>
        public static TimeSpan Current
        {
            get
            {
                int current;
                int dummy;
                NtQueryTimerResolution(out dummy, out dummy, out current);
                return TimeSpan.FromTicks(current);
            }
        }

        /// <summary>
        /// Tries to set desired resolution. Returns true if any change has been made, otherwise false.
        /// </summary>
        /// <param name="clockResolution">Desired resolution</param>
        public static bool TrySet(TimeSpan clockResolution)
        {
            int result;
            TimeSpan current = Current;
            NtSetTimerResolution((int)clockResolution.Ticks, true, out result);
            return result == current.Ticks;
        }
    }
}
