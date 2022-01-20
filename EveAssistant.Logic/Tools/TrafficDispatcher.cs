using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using EveAssistant.Common.Device;
using EveAssistant.Common.Devices.UserInterface.Mouse;
using EveAssistant.Common.Patterns;
using EveAssistant.Common.UserInterface;
using EveAssistant.Common.WinAPI;

namespace EveAssistant.Logic.Tools
{
    public class TrafficDispatcher
    {

        static readonly object Lock = new object();

        private static void BringToFront(IntPtr hWnd)
        {
            Thread.Sleep(50);

            User32.SetForegroundWindow(hWnd);

            Thread.Sleep(50);
        }

        public static void ShowWindow(IntPtr hWnd)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                User32.SetForegroundWindow(hWnd);

                Thread.Sleep(200);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void EmulationPressKey(IntPtr wndHandle, string key)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                Thread.Sleep(50);

                BringToFront(wndHandle);

                Thread.Sleep(50);

                // Ctrl+v example
                //SendKeys.SendWait("^{v}");

                SendKeys.SendWait("{" + key + "}");

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void EmulationPressCrlKey(IntPtr wndHandle, string key)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                Thread.Sleep(50);

                BringToFront(wndHandle);

                Thread.Sleep(50);

                SendKeys.SendWait("^{" + key + "}");

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void EmulationPressAltKey(IntPtr wndHandle, string key)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                Thread.Sleep(50);

                BringToFront(wndHandle);

                Thread.Sleep(50);

                SendKeys.SendWait("%{" + key + "}");

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void EmulationPressCrlShiftKey(IntPtr wndHandle, string key)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                Thread.Sleep(50);

                BringToFront(wndHandle);

                Thread.Sleep(50);

                SendKeys.SendWait("^+{" + key + "}");

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void EmulationPressShiftKey(IntPtr wndHandle, string key)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                Thread.Sleep(50);

                BringToFront(wndHandle);

                Thread.Sleep(50);

                SendKeys.SendWait("+{" + key + "}");

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void PressKey(IntPtr wndHandle, string key)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                Thread.Sleep(50);

                BringToFront(wndHandle);

                Thread.Sleep(50);

                SendKeys.SendWait(key);

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void EmulationPressKeyComplex(IntPtr wndHandle, string key)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                Thread.Sleep(50);

                BringToFront(wndHandle);

                Thread.Sleep(50);

                SendKeys.SendWait("" + key + "");

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void ContextMenuClick(IDevice device, Point coordinates, string menu)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                BringToFront(device.IntPtr);

                Thread.Sleep(200);

                // Step 1 - Activate context menu RightClickOnPoint
                RightClickOnPoint(device.IntPtr, new Point(coordinates.X, coordinates.Y));

                Thread.Sleep(500);

                // Step 2 - Search context menu element
                var result = device.FindObjectInScreen(menu, 0.2);

                Thread.Sleep(200);

                if (result.IsFound)
                {
                    // Step 3 - Use 'Stack All' context menu element
                    ClickOnPoint(device.IntPtr, result.PositionCenter);
                }
                

                Thread.Sleep(200);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }


        }
  
        public static void ClickOnPoint(IntPtr wndHandle, Point clientPoint)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                BringToFront(wndHandle);

                // get screen coordinates
                User32.ClientToScreen(wndHandle, ref clientPoint);

                //HumanWindMouse.Move(clientPoint.X, clientPoint.Y);
                User32.SetCursorPos(clientPoint.X, clientPoint.Y);

                Thread.Sleep(50);

                Worker.mouse_event(0x2, 0, 0, 0, 0);
                Thread.Sleep(100);
                Worker.mouse_event(0x4, 0, 0, 0, 0);

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }


        }

        public static void LeftClick(IntPtr wndHandle, Point clientPoint)
        {
            // get screen coordinates
            User32.ClientToScreen(wndHandle, ref clientPoint);

            Thread.Sleep(50);

            User32.SetForegroundWindow(wndHandle);

            Cursor.Position = clientPoint;

            Thread.Sleep(50);

            Worker.mouse_event(0x2, 0, 0, 0, 0);
            Thread.Sleep(100);
            Worker.mouse_event(0x4, 0, 0, 0, 0);
            Thread.Sleep(50);
        }

        public static void RightClickOnPoint(IntPtr wndHandle, Point clientPoint)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                BringToFront(wndHandle);

                // get screen coordinates
                User32.ClientToScreen(wndHandle, ref clientPoint);

                // set cursor on coords, and press mouse
                //HumanWindMouse.Move(clientPoint.X, clientPoint.Y);
                User32.SetCursorPos(clientPoint.X, clientPoint.Y);

                Thread.Sleep(50);

                Worker.mouse_event(0x0008, 0, 0, 0, 0);
                Thread.Sleep(100);
                Worker.mouse_event(0x0010, 0, 0, 0, 0);

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }


        }

        public static void MoveCursorToRandomPoint(IntPtr wndHandle)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                BringToFront(wndHandle);

                var randomInt = new Random((int)DateTime.Now.Ticks);

                var window = GetControlSize(wndHandle);

                var x = (window.Width / 2) + randomInt.Next(-10, 10);
                var y = (window.Height / 2) + randomInt.Next(-10, 10);

                MoveCursorPoint(wndHandle, new Point(x, y));

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void MoveCursorDelta(IntPtr wndHandle, int deltaX, int deltaY)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                BringToFront(wndHandle);

                var randomInt = new Random((int)DateTime.Now.Ticks);

                var window = GetControlSize(wndHandle);

                var x = Cursor.Position.X + deltaX + randomInt.Next(-10, 10);
                var y = Cursor.Position.Y + deltaY + randomInt.Next(-10, 10);

                MoveCursorPoint(new Point(x, y));

                Thread.Sleep(200);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }
        }

        public static Size GetControlSize(IntPtr hWnd)
        {
            var windowRect = new User32.Rect();
            User32.GetWindowRect(hWnd, ref windowRect);

            var cSize = new Size
            {
                Width = windowRect.right - windowRect.left,
                Height = windowRect.bottom - windowRect.top
            };

            return cSize;
        }

        public static void MoveCursorPoint(IntPtr wndHandle, Point clientPoint)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                // get screen coordinates
                User32.ClientToScreen(wndHandle, ref clientPoint);

                // set cursor on coords, and press mouse
                //HumanWindMouse.Move(clientPoint.X, clientPoint.Y);
                //HumanWindMouse.Move(clientPoint.X, clientPoint.Y);
                User32.SetCursorPos(clientPoint.X, clientPoint.Y);

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void MoveCursorPoint(Point clientPoint)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                // set cursor on coords
                HumanWindMouse.Move(clientPoint.X, clientPoint.Y);

                Thread.Sleep(50);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static Point CoordinatesClientToScreen(IntPtr wndHandle, Point clientPoint)
        {
            User32.ClientToScreen(wndHandle, ref clientPoint);

            var point = new Point(clientPoint.X, clientPoint.Y);

            // get screen coordinates
            return point;
        }

        public static void FastRightClickOnPoint(IntPtr wndHandle, Point clientPoint)
        {
            // get screen coordinates
            User32.ClientToScreen(wndHandle, ref clientPoint);

            // set cursor on coords, and press mouse
            HumanWindMouse.Move(clientPoint.X, clientPoint.Y);

            Thread.Sleep(50);

            Worker.mouse_event(0x0008, 0, 0, 0, 0);

            Thread.Sleep(100);

            Worker.mouse_event(0x0010, 0, 0, 0, 0);

            Thread.Sleep(50);


        }

        public static void FastClickOnPoint(IntPtr wndHandle, Point clientPoint)
        {
            // get screen coordinates
            User32.ClientToScreen(wndHandle, ref clientPoint);

            User32.SetCursorPos(clientPoint.X, clientPoint.Y);

            //HumanWindMouse.Move(clientPoint.X, clientPoint.Y);

            Thread.Sleep(50);

            Worker.mouse_event(0x2, 0, 0, 0, 0);
            Thread.Sleep(100);
            Worker.mouse_event(0x4, 0, 0, 0, 0);

            Thread.Sleep(50);


        }

        public static void FastDrugAndDrop(IDevice device, Point coordinatesFrom, Point coordinatesTo, Action<string> logger)
        {
            User32.ClientToScreen(device.IntPtr, ref coordinatesFrom);
            User32.ClientToScreen(device.IntPtr, ref coordinatesTo);

            HumanWindMouse.Move(coordinatesFrom.X, coordinatesFrom.Y);

            Thread.Sleep(500);

            Worker.mouse_event(0x2, 0, 0, 0, 0); //left down

            Thread.Sleep(100);

            Worker.mouse_event(0x2, 0, 0, 0, 0); //left down

            Thread.Sleep(100);

            HumanWindMouse.Move(coordinatesTo.X, coordinatesTo.Y);

            Thread.Sleep(500);

            Worker.mouse_event(0x4, 0, 0, 0, 0); //left up

            Thread.Sleep(500);

            HumanWindMouse.Move(coordinatesTo.X, coordinatesTo.Y);


        }

        public static void MoveCargoToStationHangar(IDevice device, Point oreHoldCoordinates, Point itemHangarCoordinates, Point shipCargoFirstItemCoordinates)
        {
            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                BringToFront(device.IntPtr);
                Thread.Sleep(200);

                //logger("Stack all items in ore cargo.");
                ContextMenuClick(device, oreHoldCoordinates, Types.ContextMenuStackAll);

                Thread.Sleep(200);

                //logger(" Focus on ore hold.");
                FastClickOnPoint(device.IntPtr, new Point(oreHoldCoordinates.X, oreHoldCoordinates.Y));

                Thread.Sleep(200);

                //logger(" Select all items.");
                ContextMenuClick(device, oreHoldCoordinates, Types.ContextMenuSelectAll);

                //Thread.Sleep(200);

                Thread.Sleep(200);

                //logger(" Move ore to item hangar.");
                FastDrugAndDrop(device, shipCargoFirstItemCoordinates, itemHangarCoordinates, device.Logger);

                Thread.Sleep(200);

                //logger(" Stack all items in hangar.");
                ContextMenuClick(device, itemHangarCoordinates, Types.ContextMenuStackAll);

                Thread.Sleep(200);
            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }

        public static void DragAndDrop(IDevice device, Point coordinatesFrom, Point coordinatesTo)
        {
            //logger(" MoveOreToStationHangar.");

            var lockWasTaken = false;
            var temp = Lock;

            try
            {
                Monitor.Enter(temp, ref lockWasTaken);

                BringToFront(device.IntPtr);

                //logger(" Focus on ore hold.");
                FastClickOnPoint(device.IntPtr, new Point(coordinatesFrom.X, coordinatesFrom.Y));

                FastDrugAndDrop(device, coordinatesFrom, coordinatesTo, device.Logger);

                Thread.Sleep(200);


            }
            finally
            {
                if (lockWasTaken)
                {
                    Monitor.Exit(temp);
                }
            }

        }
    }

}