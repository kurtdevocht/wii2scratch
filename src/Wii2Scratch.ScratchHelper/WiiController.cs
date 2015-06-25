using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WiimoteLib;

namespace Wii2Scratch.ScratchHelper
{
    public class WiiController
    {
        int m_index;
        Wiimote m_wiimote;

        private double m_rotX;
        private double m_rotY;
        private double m_rotZ;

        private List<IRState> m_irStates = new List<IRState>();

        public WiiController(int index, Wiimote wiimote)
        {
            if (index < 1 || index > 4)
            {
                throw new ArgumentOutOfRangeException("index", "Index should be 1..4");
            }

            if (wiimote == null)
            {
                throw new ArgumentNullException("wiimote");
            }

            for (int i = 0; i < 4; i++)
            {
                m_irStates.Add(new IRState());
            }

            m_index = index;
            m_wiimote = wiimote;
            m_wiimote.WiimoteChanged += m_wiimote_WiimoteChanged;
            m_wiimote.Connect();

            if (m_wiimote.WiimoteState.ExtensionType != ExtensionType.BalanceBoard)
            {
                m_wiimote.SetReportType(InputReport.IRExtensionAccel, IRSensitivity.Maximum, true);
            }

            m_wiimote.SetLEDs(m_index);
        }

        public int Index
        {
            get
            {
                return m_index;
            }
        }

        public bool ButtonA
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.A;
            }
        }

        public bool ButtonB
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.B;
            }
        }

        public bool ButtonUp
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.Up;
            }
        }

        public bool ButtonRight
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.Right;
            }
        }

        public bool ButtonDown
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.Down;
            }
        }

        public bool ButtonLeft
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.Left;
            }
        }

        public bool Button1
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.One;
            }
        }

        public bool Button2
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.Two;
            }
        }

        public bool ButtonPlus
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.Plus;
            }
        }

        public bool ButtonMin
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.Minus;
            }
        }

        public bool ButtonHome
        {
            get
            {
                return m_wiimote.WiimoteState.ButtonState.Home;
            }
        }

        public double RotationX
        {
            get
            {
                return m_rotX;
            }
        }

        public double RotationY
        {
            get
            {
                return m_rotY;
            }
        }

        public double RotationZ
        {
            get
            {
                return m_rotZ;
            }
        }

        public int BatteryPercent
        {
            get
            {
                return (int)(m_wiimote.WiimoteState.Battery + 0.5);
            }
        }

        public List<IRState> IRStates
        {
            get
            {
                return m_irStates;
            }
        }

        public void SetLed(int index, bool state)
        {
            var l1 = index == 1 ? state : m_wiimote.WiimoteState.LEDState.LED1;
            var l2 = index == 2 ? state : m_wiimote.WiimoteState.LEDState.LED2;
            var l3 = index == 3 ? state : m_wiimote.WiimoteState.LEDState.LED3;
            var l4 = index == 4 ? state : m_wiimote.WiimoteState.LEDState.LED4;

            m_wiimote.SetLEDs(l1, l2, l3, l4);
        }

        public void SetRumble(bool state)
        {
            m_wiimote.SetRumble(state);
        }

        void m_wiimote_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            this.UpdateRotations();
            this.UpdateIR();
        }

        private void UpdateIR()
        {
            var irs = m_wiimote.WiimoteState.IRState.IRSensors;

            for (int i = 0; i < m_irStates.Count; i++)
            {
                bool found = false;
                float x = 0.0f;
                float y = 0.0f;

                if (irs.Length > i)
                {
                    found = irs[i].Found;
                    x = irs[i].Position.X;
                    y = irs[i].Position.Y;
                }

                m_irStates[i].Found = found;
                m_irStates[i].XPos = (int)(480 * x - 240 + 0.5);
                m_irStates[i].YPos = (int)(360 * y - 180 + 0.5);
            }
        }

        private void UpdateRotations()
        {
            var acc = m_wiimote.WiimoteState.AccelState.Values;
            var atanX = 0.0;
            if (acc.Y != 0.0 || acc.Z != 0.0)
            {
                atanX = System.Math.Atan(acc.X / Math.Sqrt(acc.Y * acc.Y + acc.Z * acc.Z)) * 180.0 / System.Math.PI;
            }

            var atanY = 0.0;
            if (acc.X != 0.0 || acc.Z != 0.0)
            {
                atanY = System.Math.Atan(acc.Y / Math.Sqrt(acc.X * acc.X + acc.Z * acc.Z)) * 180.0 / System.Math.PI;
            }

            var atanZ = 0.0;
            if (acc.Y != 0.0 || acc.X != 0.0)
            {
                atanZ = System.Math.Atan(acc.Z / Math.Sqrt(acc.Y * acc.Y + acc.X * acc.X)) * 180.0 / System.Math.PI;
            }

            // Debug.WriteLine( "{0:0.0}; {1:0.0}; {2:0.0}", atanX, atanY, atanZ );

            if (Math.Abs(acc.X) < Math.Abs(acc.Y))
            {
                // Portrait
                if (acc.Y > 0)
                {
                    m_rotZ = -atanX - 90;
                }
                else
                {
                    m_rotZ = atanX + 90;
                }
            }
            else
            {
                // Landscape
                if (acc.X < 0)
                {
                    m_rotZ = -atanY;
                }
                else
                {
                    m_rotZ = atanY + 180;
                }
            }

            if (Math.Abs(acc.Y) > Math.Abs(acc.Z))
            {
                // Portrait
                if (atanY < 0)
                {
                    m_rotX = 90 - atanZ;
                }
                else
                {
                    m_rotX = -90 + atanZ;
                }
            }
            else
            {
                // Flat
                if (atanZ < 0)
                {
                    m_rotX = 180 + atanY;
                }
                else
                {
                    m_rotX = -atanY;
                }
            }

            if (Math.Abs(acc.Z) > Math.Abs(acc.X))
            {
                //Flat
                if (atanZ < 0)
                {
                    m_rotY = atanX + 90;
                }
                else
                {
                    m_rotY = -atanX - 90;
                }
            }
            else
            {
                // Portrait
                if (atanX > 0)
                {
                    m_rotY = atanZ + 180;
                }
                else
                {
                    m_rotY = -atanZ;
                }
            }
        }
    }
}
