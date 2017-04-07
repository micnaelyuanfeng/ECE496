using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.VisualBasic;
using Leap;

namespace LeapPanel
{
    public partial class Form1 : Form, ILeapEventDelegate
    {
        /**********************************/
        /****** Events controllers ********/
        private Controller controller;
        private LeapEventListener listener;
        /**********************************/

        /**********************************/
        /***** Networking Variables *******/
        private TcpClient tcpclnt;
        private string command;
        private Byte[] data;
        public String serverIP = "10.249.238.10";
        public int serverPort = 2050;
        public int videoPort = 8080;
        /**********************************/

        /**********************************/
        /******* Leap Coordinates *********/
        private Vector palmPositionRaw;
        public Vector palmPositionSmooth;
        private Vector palmPositionLast = new Vector();
        public float handGrap;
        private float handGrapLast;
        public float handRoll;
        private float handRollLast;
        /**********************************/

        /**********************************/
        /******** Frame Smoothing *********/
        private int SMOOTHING_FRAMES = 10;
        private Queue<Vector> palmPositionHistory = new Queue<Vector>();
        /**********************************/

        /**********************************/
        /********** Robotic Arm ***********/
        private int LENGTH1 = 160;
        private int LENGTH2 = 200;
        /**********************************/

        /**********************************/
        /******** Vehicle Driving *********/
        public int drivingInstruction;
        private int drivingInstructionLast;
        /**********************************/
        public Form1()
        {
            InitializeComponent();
            palmPositionLast.x = 0;
            palmPositionLast.y = 0;
            palmPositionLast.z = 0;
            handGrapLast = 0;
            handRollLast = 0;
            drivingInstruction = 0;
            drivingInstructionLast = 0;
            // Display Server Information
            ServerIPTextbox.Text = serverIP;
            InstructionPort.Text = serverPort.ToString();
            VideoPort.Text = videoPort.ToString();

            // Initialize Server Connection
            try
            {
                tcpclnt = new TcpClient();
                tcpclnt.Connect(serverIP, serverPort);
       
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to server, reason : " + ex.Message);
            }
        
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            //this.KeyPreview = true;
            //this.KeyDown += onKeyDown;

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.KeyUp += new KeyEventHandler(Form1_KeyUp);
            controller.AddListener(listener);
        }

        void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs ke)
        {
            var handler = new keyEventDelegate(setArrowKeyTextBox);
            if (ke.KeyCode == Keys.Up)
            {
                drivingInstruction = 1;
                if (InvokeRequired)
                    this.Invoke(handler, this.up_key);
                else
                    setArrowKeyTextBox(this.up_key);
            }
            else if (ke.KeyCode == Keys.Down)
            {
                drivingInstruction = 2;
                if (InvokeRequired)
                    this.Invoke(handler, this.down_key);
                else
                    setArrowKeyTextBox(this.down_key);
            }
            else if (ke.KeyCode == Keys.Left)
            {
                drivingInstruction = 3;
                if (InvokeRequired)
                    this.Invoke(handler, this.left_key);
                else
                    setArrowKeyTextBox(this.left_key);
            }
            else if (ke.KeyCode == Keys.Right)
            {
                drivingInstruction = 4;
                if (InvokeRequired)
                    this.Invoke(handler, this.right_key);
                else
                    setArrowKeyTextBox(this.right_key);
            }
            else if (ke.KeyCode == Keys.Space)
            {
                drivingInstruction = 0;
                if (InvokeRequired)
                    this.Invoke(handler, this.stop_key);
                else
                    setArrowKeyTextBox(this.stop_key);
            }
            ke.SuppressKeyPress = true;
        }

        void Form1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs ke)
        {
            var handler = new keyEventDelegate(clearArrowKeyTextBox);
            if (ke.KeyCode == Keys.Up)
            {
                drivingInstruction = 0;
                if (InvokeRequired)
                    this.Invoke(handler, this.up_key);
                else
                    clearArrowKeyTextBox(this.up_key);
            }
            else if (ke.KeyCode == Keys.Down)
            {
                drivingInstruction = 0;
                if (InvokeRequired)
                    this.Invoke(handler, this.down_key);
                else
                    clearArrowKeyTextBox(this.down_key);
            }
            else if (ke.KeyCode == Keys.Left)
            {
                drivingInstruction = 0;
                if (InvokeRequired)
                    this.Invoke(handler, this.left_key);
                else
                    clearArrowKeyTextBox(this.left_key);
            }
            else if (ke.KeyCode == Keys.Right)
            {
                drivingInstruction = 0;
                if (InvokeRequired)
                    this.Invoke(handler, this.right_key);
                else
                    clearArrowKeyTextBox(this.right_key);
            }
            else if (ke.KeyCode == Keys.Space)
            {
                drivingInstruction = 0;
                if (InvokeRequired)
                    this.Invoke(handler, this.stop_key);
                else
                    clearArrowKeyTextBox(this.stop_key);
            }
            ke.SuppressKeyPress = true;
        }
        private delegate void keyEventDelegate(TextBox target);

        private void setArrowKeyTextBox(TextBox target)
        {
            target.Text = "Pressed";
        }

        private void clearArrowKeyTextBox(TextBox target)
        {
            target.Text = "";
        }

        delegate void LeapEventDelegate(string EventName);
        public void LeapEventNotification(string EventName)
        {
            if (!this.InvokeRequired)
            {
                switch (EventName)
                {
                    case "onInit":
                        break;
                    case "onConnect":
                        connectHandler();
                        break;
                    case "onFrame":
                        handMotion(this.controller.Frame());
                        break;
                }
            }
            else
            {
                BeginInvoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }

       

        public void connectHandler()
        {
            this.controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            this.controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
        }

        public void detectGesture(Leap.Frame frame)
        {
            GestureList gestures = frame.Gestures();

            for(int i = 0; i < gestures.Count(); i++)
            {
                Gesture gesture = gestures[i];

                switch(gesture.Type)
                {
                    case Gesture.GestureType.TYPE_INVALID:
                        break;

                    case Gesture.GestureType.TYPE_CIRCLE:
                        break;

                    case Gesture.GestureType.TYPE_SWIPE:
                        break;

                    case Gesture.GestureType.TYPE_KEY_TAP:
                        break;

                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        break;
                }
            }
        }

        public void handMotion(Leap.Frame frame)
        {
            Hand hand;
            if (drivingInstruction != drivingInstructionLast)
            {
                int M1Motor = 0, M2Motor = 0;
                if (drivingInstruction == 1)
                {
                    M1Motor = 1;
                    M2Motor = 1;
                }
                else if (drivingInstruction == 2)
                {
                    M1Motor = 2;
                    M2Motor = 2;
                }
                else if (drivingInstruction == 3)
                {
                    M1Motor = 1;
                    M2Motor = 2;
                }
                else if (drivingInstruction == 4)
                {
                    M1Motor = 2;
                    M2Motor = 1;
                }
                M1Instruction.Text = (M1Motor).ToString();
                M2Instruction.Text = (M2Motor).ToString();
                command = "3" + ":" + (M1Motor).ToString() + ":" + (M2Motor).ToString();
                if (tcpclnt.Connected)
                {
                    NetworkStream stream = tcpclnt.GetStream();
                    if (command != null)
                    {
                        testBox.Text = command;
                        data = System.Text.Encoding.ASCII.GetBytes(command);
                        stream.Write(data, 0, data.Length);
                    }
                }
                drivingInstructionLast = drivingInstruction;
            }
            else if (frame.Hands.Count > 0)
            {
                // Hand detected.
                hand = frame.Hands[0];
                string handName = hand.IsLeft ? "Left hand" : "Right hand";
                whichHand.Text = handName;
                if (hand.IsValid)
                {
                    // Hand is detected and valid
                    palmPositionRaw = hand.PalmPosition;
                    //palmPositionRaw = hand.StabilizedPalmPosition;
                    handGrap = hand.GrabStrength;
                    if(handGrap <= 0.25)
                    {
                        handGrap = 0;
                    }else if(handGrap >0.25 && handGrap < 0.75)
                    {
                        handGrap = (float)0.5;
                    }else if (handGrap >= 0.75)
                    {
                        handGrap = 1;
                    }

                    handRoll = hand.PalmNormal.Roll;
                    if (handRoll <= 0.5 && handRoll >= -0.5)
                    {
                        handRoll = 0;
                    }
                    else if (handRoll > 0.5 && handRoll < 1.05)
                    {
                        handRoll = (float)0.785398;
                    }
                    else if (handRoll >= 1.05)
                    {
                        handRoll = (float)1.5708;
                    }
                    else if (handRoll < -0.5 && handRoll > -1.05)
                    {
                        handRoll = (float)-0.785398;
                    }
                    else if (handRoll <= -1.05)
                    {
                        handRoll = (float)-1.5708;
                    }

                    palmPositionRaw.y = palmPositionRaw.y - 150;
                    if (palmPositionRaw.y > 415)
                    {
                        palmPositionRaw.y = 415;
                    }
                    if (palmPositionRaw.y < 0)
                    {
                        palmPositionRaw.y = 0;
                    }

                    palmPositionRaw.z = -1* palmPositionRaw.z + 200;
                    if (palmPositionRaw.z > 800)
                    {
                        palmPositionRaw.z = 800;
                    }
                    if (palmPositionRaw.z < 0)
                    {
                        palmPositionRaw.z = 0;
                    }

                    SmoothingCoordinates(palmPositionRaw);
                    SmoothingHistoryManagement(palmPositionRaw);
                    
                    if(drivingInstruction == 0 && handGrap != handGrapLast)
                    {
                        // Adjust Gripper First
                        command = "2" + ":" + ((int)ToDegrees(handRoll)).ToString() + ":" + (handGrap).ToString();
                        handGrapLast = handGrap;
                        grapVal.Text = handGrap.ToString();
                        palmRoll.Text = handRoll.ToString();
                    }
                    else if(drivingInstruction == 0 && (palmPositionSmooth.x != palmPositionLast.x || palmPositionSmooth.y != palmPositionLast.y || palmPositionSmooth.z != palmPositionLast.z))
                    {
                        float baseServo = calculateBaseAngle(palmPositionSmooth.x, palmPositionSmooth.z);
                        float shoulderServo = calculateInverseKinematics1(palmPositionSmooth.y, palmPositionSmooth.z);
                        float elbowServo = calculateInverseKinematics2(palmPositionSmooth.y, palmPositionSmooth.z);
                        if(!float.IsNaN(baseServo) && !float.IsNaN(shoulderServo) && !float.IsNaN(elbowServo))
                        {
                            float baseServoTrans = baseServo;
                            float shoulderServoTrans = shoulderServo + 65;
                            float elbowServoTrans = 155 - elbowServo;

                            if(shoulderServoTrans <= 65)
                            {
                                shoulderServoTrans = 65;
                            }else if (shoulderServoTrans >= 245)
                            {
                                shoulderServoTrans = 245;
                            }

                            if (elbowServoTrans <= 30)
                            {
                                elbowServoTrans = 30;
                            }
                            else if (elbowServoTrans >= 250)
                            {
                                elbowServoTrans = 250;
                            }

                            command = "1" + ":" + ((int)baseServoTrans).ToString()
                                   + ":" + ((int)shoulderServoTrans).ToString()
                                   + ":" + ((int)elbowServoTrans).ToString()
                                   + ":" + ((int)ToDegrees(handRoll)).ToString()
                                   + ":" + (handGrap).ToString();

                            baseServoText.Text = ((int)baseServo).ToString();
                            baseServoTransText.Text = ((int)baseServoTrans).ToString();
                            shoulderServoText.Text = ((int)shoulderServo).ToString();
                            shoulderServoTransText.Text = ((int)shoulderServoTrans).ToString();
                            elbowServoText.Text = ((int)elbowServo).ToString();
                            elbowServoTransText.Text = ((int)elbowServoTrans).ToString();
                        }
                        
                        else
                        {
                            command = null;
                        }

                        if (tcpclnt.Connected)
                        {
                            NetworkStream stream = tcpclnt.GetStream();
                            if (command != null)
                            {
                                data = System.Text.Encoding.ASCII.GetBytes(command);
                                stream.Write(data, 0, data.Length);
                            }
                        }


                        x_displacement.Text = palmPositionSmooth.x.ToString();
                        y_displacement.Text = palmPositionSmooth.y.ToString();
                        z_displacement.Text = palmPositionSmooth.z.ToString();
                        grapVal.Text = handGrap.ToString();
                        palmRoll.Text = ToDegrees(handRoll).ToString();
                    }
                   
                }   
            }
           
        }

        public void SmoothingCoordinates(Vector latestHandPosition)
        {
            if (palmPositionHistory.Count <= 0)
            {
                palmPositionSmooth = latestHandPosition;
            }

            float x = 0, y = 0, z = 0;
            int periods = palmPositionHistory.Count;
            Vector[] palmPositionHistoryArray = palmPositionHistory.ToArray();
            for (int i = 0; i < periods; i++)
            {
               x += latestHandPosition.x + palmPositionHistoryArray[i].x;
               y += latestHandPosition.y + palmPositionHistoryArray[i].y;
               z += latestHandPosition.z + palmPositionHistoryArray[i].z;
            }
      
            periods += 1; // To incldue the current frame      
            palmPositionSmooth.x = x / periods;
            palmPositionSmooth.y = y / periods;
            palmPositionSmooth.z = z / periods;
        }

        public void SmoothingHistoryManagement(Vector latestHandPosition)
        {
            palmPositionHistory.Enqueue(latestHandPosition);
            if (palmPositionHistory.Count > SMOOTHING_FRAMES)
            {
                palmPositionHistory.Dequeue();
            }
        }

        public float ToDegrees(float Radian)
        {
            float degrees;
            degrees = Radian * 180 / (float)Math.PI;
            return degrees;
        }


        public float calculateBaseAngle(float x, float z)
        {
            double angle = Math.Atan(x / z);
            return 90 - ToDegrees((float)angle);
        }

        public float calculateInverseKinematics1(float y, float z)
        {
            double hypotenuse = Math.Sqrt(Math.Pow(y, 2) + Math.Pow(z, 2));
            double a = Math.Atan(y / z);
            double b = Math.Acos((Math.Pow(LENGTH1, 2) + Math.Pow(hypotenuse, 2) - Math.Pow(LENGTH2, 2)) / (2 * LENGTH1 * hypotenuse));
            return ToDegrees((float)(a + b));
        }

        public float calculateInverseKinematics2(float y, float z)
        {
            double hypotenuse = Math.Sqrt(Math.Pow(y, 2) + Math.Pow(z, 2));
            double c = Math.Acos((Math.Pow(LENGTH2, 2) + Math.Pow(LENGTH1, 2) - Math.Pow(hypotenuse, 2)) / (2 * LENGTH1 * LENGTH2));
            return 180 - ToDegrees((float)c);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }

    public interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }

    public class LeapEventListener : Listener
    {
        ILeapEventDelegate eventDelegate;

        public LeapEventListener(ILeapEventDelegate delegateObject)
        {
            this.eventDelegate = delegateObject;
        }
        public override void OnInit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onInit");
        }
        public override void OnConnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onConnect");
        }
        public override void OnDeviceChange(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onDevice");
        }
        public override void OnFrame(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onFrame");
        }
        public override void OnExit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onExit");
        }
        public override void OnDisconnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onDisconnect");
        }
    }
}
