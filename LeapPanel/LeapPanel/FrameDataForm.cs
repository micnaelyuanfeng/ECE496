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
using Microsoft.VisualBasic;
using System.Diagnostics;
using Leap;

namespace LeapPanel
{
    public partial class Form1 : Form, ILeapEventDelegate
    {
        private Controller controller;
        private LeapEventListener listener;

        private Process faceRecProcess;

        private int gesture_motion;

        //wifi vars
        private TcpClient tcpclnt;
        private string command;
        //private NetworkStream stream;
        private Byte[] data;

        //three fingers: index, middle, pinkle
        private Finger[] finger_object = new Finger[3];
        private Bone[,] bone_object = new Bone[3, 4];
        private bool[] finger_exist = new bool[3];

        private bool start_track = true;
        private float time_track_1;
        //private float time_track_2;
        private double track_resolution = 250000;
        private int sensitivity = 3;
        //private double track_resolution = 250000;
        private bool if_send = true;

        private bool initial_f1 = true;
        private bool initial_f2 = true;
        private bool initial_f3 = true;

        //one finger move var block
        private Vector bone0_x;
        private Vector bone1_x;
        private Vector bone2_x;
        private Vector bone11_x_reference; //normal to x
        private Vector bone21_x_reference;
        private Vector bone31_x_reference;
       // private bool update_x = true; // only take once and set false, expcet lost tracking and back

        private double last_degree_x_f011 = 0;
        private double last_degree_y_f01 = 0;
        private double last_degree_z_f01 = 0;
        private double last_degree_x_f012 = 0;
        private double last_degree_x_f013 = 0;
        private double last_degree_y_f02 = 0;
        private double last_degree_z_f02 = 0;

        private double last_degree_x_f111 = 0;
        private double last_degree_x_f112 = 0;
        private double last_degree_x_f113 = 0;

        private double last_degree_y_f11 = 0;
        private double last_degree_z_f11 = 0;   
        private double last_degree_y_f12 = 0;
        private double last_degree_z_f12 = 0;

        private double last_degree_x_f211 = 0;
        private double last_degree_x_f212 = 0;
        private double last_degree_x_f213 = 0;

        private double last_degree_y_f21 = 0;
        private double last_degree_z_f21 = 0;
        private double last_degree_y_f22 = 0;
        private double last_degree_z_f22 = 0;

        private int command_degree_f01 = 0;
        private int command_degree_f02 = 0;
        private int command_degree_f03 = 0;
        private int last_degree_f01 = 0;
        private int last_degree_f02 = 0;
        private int last_degree_f03 = 0;

        private int command_degree_f11 = 0;
        private int command_degree_f12 = 0;
        private int command_degree_f13 = 0;
        private int last_degree_f11 = 0;
        private int last_degree_f12 = 0;
        private int last_degree_f13 = 0;

        private int command_degree_f21 = 0;
        private int command_degree_f22 = 0;
        private int command_degree_f23 = 0;
        private int last_degree_f21 = 0;
        private int last_degree_f22 = 0;
        private int last_degree_f23 = 0;

        private double[] velocity_lookup = new double[20];

        public Form1()
        {
            InitializeComponent();

            ServerIPTextbox.Text = "192.168.1.244";
            PortNumber.Text = "2050";

            textBox13.Text = track_resolution.ToString();
            //wifi connection
            try
            {
                tcpclnt = new TcpClient();
                //Console.WriteLine("Connecting to the server[{0}]", "192.168.1.244");
                tcpclnt.Connect("192.168.1.244", 2050);
                //MessageBox.Show("Connected");
                //textBox5.Text = "Connected";
                //richTextBox.AppendText("Connected" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to server, reason : " +
                    ex.Message);
            }

            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
        }

        delegate void LeapEventDelegate(string EventName);
        public void LeapEventNotification(string EventName)
        {
            if (!this.InvokeRequired)
            {
                switch (EventName)
                {
                    case "onInit":
                        //MessageBox.Show("Leap Motion Conneted");
                        break;
                    case "onConnect":
                        connectHandler();
                        break;
                    case "onFrame":
                        //detectGesture(this.controller.Frame());//get latest frame     
                        //display_flag = true;
                       /* while (greeting(this.controller.Frame()) != 1)
                        {
                            richTextBox.Text= "Need Swipe to Activate";
                        }*/
                        fingerMotion(this.controller.Frame());
                        degree_conversion();
                        break;
                }
            }
            else
            {
                BeginInvoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }

        public int greeting(Frame frame)
        {
            detectGesture(frame);
            richTextBox.AppendText("Hello, Please use Swipe motion to activate" + Environment.NewLine);

            if(gesture_motion == 1)
                return 0;
            else
                return 1;
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
                        //fingerMotion(this.controller.Frame());
                        //degree_conversion();
                        gesture_motion = 10;
                        break;

                    case Gesture.GestureType.TYPE_CIRCLE:
                        //fingerMotion(this.controller.Frame());
                        //degree_conversion();
                        gesture_motion = 10;
                        richTextBox.AppendText("CircleGesture detected" + Environment.NewLine);
                        break;

                    case Gesture.GestureType.TYPE_SWIPE:
                        richTextBox.AppendText("SwipeGesture detected" + Environment.NewLine);
                        //fingerMotion(this.controller.Frame());
                        //degree_conversion();
                        gesture_motion = 1;
                        break;

                    case Gesture.GestureType.TYPE_KEY_TAP:
                        //fingerMotion(this.controller.Frame());
                        //degree_conversion();
                        richTextBox.AppendText("Key Tap detected" + Environment.NewLine);
                        gesture_motion = 10;
                        break;

                    case Gesture.GestureType.TYPE_SCREEN_TAP:
                        //fingerMotion(this.controller.Frame());
                        //degree_conversion();
                        richTextBox.AppendText("Screen Tap detected" + Environment.NewLine);
                        gesture_motion = 10;
                        break;
                }
            }

 
        }

        public void handMotion(Leap.Frame frame)
        {
            //get hand roll degree
        }

        public void fingerMotion(Leap.Frame frame)
        {
            //create fingers and their bones
            //bone[0, 0...2]: finger 0 bone base to tip
            //finger 0 is index, finger 1 is middle, finger 2 is pinkle
            richTextBox.Text = frame.Timestamp.ToString();

            if(start_track)
            {
                start_track = false;
                time_track_1 = frame.Timestamp;
                //richTextBox2.Text = time_track_1.ToString() + "haha" + start_track.ToString();
            }
            
            if((frame.Timestamp - time_track_1) > track_resolution)
            {
                //ServerIPTextbox.Text = if_send.ToString();
                //send info
               if_send = send_validation();

                //ServerIPTextbox.Text = if_send.ToString();

                start_track = true;

                if (if_send)
                {
                    if_send = false;
                    //update_x = true;
                   // finger_x_plane();
                   // ServerIPTextbox.Text = if_send.ToString();

                    degree_validation();


                    //finger 0(1)
                    if(finger_exist[0] == true)
                        command = "0" + "-" + "0" + "-" + ((int)command_degree_f01).ToString()
                                   + "-" + "1" + "-" + ((int)command_degree_f02).ToString() + "-" + "2"
                                   + "-" + ((int)command_degree_f03).ToString();
                    else
                        command = "0" + "-" + "0" + "-" + ((int)last_degree_f01).ToString()
                                   + "-" + "1" + "-" + ((int)last_degree_f02).ToString() + "-" + "2"
                                   + "-" + ((int)last_degree_f03).ToString();
                    //add finger 1 and finger 2 data

                    textBox4.Text = command;

                    System.Threading.Thread.Sleep(70);

                    if (tcpclnt.Connected)
                    {
                        NetworkStream stream = tcpclnt.GetStream();
                        data = System.Text.Encoding.ASCII.GetBytes(textBox4.Text);
                        stream.Write(data, 0, data.Length);
                    }

                    //finger 1(2)
                    if(finger_exist[1] == true)
                        command = "1" + "-" + "0" + "-" + ((int)command_degree_f11).ToString()
                                   + "-" + "1" + "-" + ((int)command_degree_f12).ToString() + "-" + "2"
                                   + "-" + ((int)command_degree_f13).ToString();
                    else
                        command = "1" + "-" + "0" + "-" + ((int)last_degree_f11).ToString()
                              + "-" + "1" + "-" + ((int)last_degree_f12).ToString() + "-" + "2"
                              + "-" + ((int)last_degree_f13).ToString();

                    textBox11.Text = command;

                    if (tcpclnt.Connected)
                    {
                        NetworkStream stream = tcpclnt.GetStream();
                        data = System.Text.Encoding.ASCII.GetBytes(textBox11.Text);
                        stream.Write(data, 0, data.Length);
                    }

                    System.Threading.Thread.Sleep(70);

                    //finger 2(3)
                    if(finger_exist[2] == true)
                        command = "2" + "-" + "0" + "-" + ((int)command_degree_f21).ToString()
                                   + "-" + "1" + "-" + ((int)command_degree_f22).ToString() + "-" + "2"
                                   + "-" + ((int)command_degree_f23).ToString();
                    else
                        command = "2" + "-" + "0" + "-" + ((int)last_degree_f21).ToString()
                                   + "-" + "1" + "-" + ((int)last_degree_f22).ToString() + "-" + "2"
                                   + "-" + ((int)last_degree_f23).ToString();

                    textBox12.Text = command;

                    if (tcpclnt.Connected)
                    {
                        NetworkStream stream = tcpclnt.GetStream();
                        data = System.Text.Encoding.ASCII.GetBytes(textBox12.Text);
                        stream.Write(data, 0, data.Length);
                    }

                    System.Threading.Thread.Sleep(70);
                }

                //finger_x_plane();

                last_degree_f01 = command_degree_f01;
                last_degree_f02 = command_degree_f02;
                last_degree_f03 = command_degree_f03;

                last_degree_f11 = command_degree_f11;
                last_degree_f12 = command_degree_f12;
                last_degree_f13 = command_degree_f13;

                last_degree_f21 = command_degree_f21;
                last_degree_f22 = command_degree_f22;
                last_degree_f23 = command_degree_f23;

                richTextBox2.Text = frame.Timestamp.ToString();
                
            }

            for(int i = 0; i < 3; i++)
            {
                finger_exist[i] = false;
            }

            /*******************************************************
             * update degree block
             * 
            *******************************************************/
            foreach (Finger finger in frame.Fingers)
            {
                //get finger type
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_INDEX:
                        finger_object[0] = finger;
                        finger_exist[0] = true;
                        break;

                    case Finger.FingerType.TYPE_MIDDLE:
                        finger_object[1] = finger;
                        finger_exist[1] = true;
                        break;

                    case Finger.FingerType.TYPE_PINKY:
                        finger_object[2] = finger;
                        finger_exist[2] = true;
                        break;
                }

                //get bones of finger
                foreach (Bone.BoneType boneType in (Bone.BoneType[])Enum.GetValues(typeof(Bone.BoneType)))
                {
                    Bone bone = finger.Bone(boneType);

                    switch(bone.Type)
                    {
                        case Bone.BoneType.TYPE_METACARPAL:
                            bone_assignment(bone, finger);
                            break;

                        case Bone.BoneType.TYPE_PROXIMAL:
                            bone_assignment(bone, finger);
                            break;

                        case Bone.BoneType.TYPE_INTERMEDIATE:
                            bone_assignment(bone, finger);
                            break;

                        case Bone.BoneType.TYPE_DISTAL:
                            bone_assignment(bone, finger);
                            break;
                    }
                }
            }
         }

        public bool send_validation()
        {
            if (Math.Abs(last_degree_f01 - command_degree_f01) >= sensitivity)
                return true;
            if (Math.Abs(last_degree_f02 - command_degree_f02) >= sensitivity)
                return true;
            if (Math.Abs(last_degree_f03 - command_degree_f03) >= sensitivity)
                return true;
            if (Math.Abs(last_degree_f11 - command_degree_f11) >= sensitivity)
                return true;
            if (Math.Abs(last_degree_f12 - command_degree_f12) >= sensitivity)
                return true;
            if (Math.Abs(last_degree_f13 - command_degree_f13) >= sensitivity)
                return true;
            if (Math.Abs(last_degree_f21 - command_degree_f21) >= sensitivity)
                return true;
            if (Math.Abs(last_degree_f22 - command_degree_f22) >= sensitivity)
                return true;
            if (Math.Abs(last_degree_f23 - command_degree_f23) >= sensitivity)
                return true;

            return false;
        }

        public void degree_validation()
        {
            int last_digit;

            last_digit = command_degree_f02 % 10;
            command_degree_f02 = command_degree_f02 - last_digit;

            if (last_digit >= 3)
                command_degree_f02 += 5;

            last_digit = command_degree_f03 % 10;
            command_degree_f03 = command_degree_f03 - last_digit;

            if (last_digit >= 3)
                command_degree_f03 += 5;

            last_digit = command_degree_f12 % 10;
            command_degree_f12 = command_degree_f12 - last_digit;

            if (last_digit >= 3)
                command_degree_f12 += 5;

            last_digit = command_degree_f13 % 10;
            command_degree_f13 = command_degree_f13 - last_digit;

            if (last_digit >= 3)
                command_degree_f13 += 5;

            last_digit = command_degree_f22 % 10;
            command_degree_f22 = command_degree_f22 - last_digit;

            if (last_digit >= 3)
                command_degree_f22 += 5;

            last_digit = command_degree_f23 % 10;
            command_degree_f23 = command_degree_f23 - last_digit;

            if (last_digit >= 3)
                command_degree_f23 += 5;

            last_digit = command_degree_f01 % 10;
            command_degree_f01 = command_degree_f01 - last_digit;

            if (last_digit >= 3)
                command_degree_f01 += 5;

            last_digit = command_degree_f11 % 10;
            command_degree_f11 = command_degree_f11 - last_digit;

            if (last_digit >= 3)
                command_degree_f11 += 5;

            last_digit = command_degree_f21 % 10;
            command_degree_f21 = command_degree_f21 - last_digit;

            if (last_digit >= 3)
                command_degree_f21 += 5;

           
        }

        public void bone_assignment(Bone bone, Finger finger)
        {

            if(bone.Type == Bone.BoneType.TYPE_METACARPAL)
            {
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_INDEX:
                        bone_object[0, 0] = bone;
                        break;

                    case Finger.FingerType.TYPE_MIDDLE:
                        bone_object[1, 0] = bone;
                        break;

                    case Finger.FingerType.TYPE_PINKY:
                        bone_object[2, 0] = bone;
                        break;
                }
            }
            else if(bone.Type == Bone.BoneType.TYPE_PROXIMAL)
            {
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_INDEX:
                        bone_object[0, 1] = bone;
                        break;

                    case Finger.FingerType.TYPE_MIDDLE:
                        bone_object[1, 1] = bone;
                        break;

                    case Finger.FingerType.TYPE_PINKY:
                        bone_object[2, 1] = bone;
                        break;
                }
            }
            else if(bone.Type == Bone.BoneType.TYPE_INTERMEDIATE)
            {
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_INDEX:
                        bone_object[0, 2] = bone;
                        break;

                    case Finger.FingerType.TYPE_MIDDLE:
                        bone_object[1, 2] = bone;
                        break;

                    case Finger.FingerType.TYPE_PINKY:
                        bone_object[2, 2] = bone;
                        break;
                }
            }
            else if(bone.Type == Bone.BoneType.TYPE_DISTAL)
            {
                switch (finger.Type)
                {
                    case Finger.FingerType.TYPE_INDEX:
                        bone_object[0, 3] = bone;
                        break;

                    case Finger.FingerType.TYPE_MIDDLE:
                        bone_object[1, 3] = bone;
                        break;

                    case Finger.FingerType.TYPE_PINKY:
                        bone_object[2, 3] = bone;
                        break;
                }
            }
        }

        public void finger_x_plane()
        {
            Matrix f0, f1, f2;
 
            //update_x = false;

            f0 = bone_object[0, 0].Basis;
            f1 = bone_object[1, 0].Basis;
            f2 = bone_object[2, 0].Basis;

            bone11_x_reference = f0.xBasis;
            bone21_x_reference = f1.xBasis;
            bone31_x_reference = f2.xBasis;
        }

        public void degree_conversion()
        {
            //x base is not useful in calculating relative reference
            Matrix bone0, bone1, bone2, bone3;

            Vector bone1_x, bone0_y, bone0_z;
            Vector bone1_y, bone1_z;
            Vector bone2_y, bone2_z;
            Vector bone3_y, bone3_z;
            Vector b1_x_norm, b0_y_norm, b0_z_norm;
            Vector b1_y_norm, b1_z_norm;
            Vector b2_y_norm, b2_z_norm;
            Vector b3_y_norm, b3_z_norm;

            double[] degree;
            degree = new double[2];

            if ((finger_object[0] != null || finger_object[1] != null || finger_object[2] != null))
            {
                finger_x_plane();
                //update_x = false;
            }


            if (finger_object[0] != null)
            {
                if(initial_f1)
                {
                    initial_f1 = false;
                    //finger 0 or index finger degree
                    bone0 = bone_object[0, 0].Basis;
                    bone1 = bone_object[0, 1].Basis;
                    bone2 = bone_object[0, 2].Basis;
                    bone3 = bone_object[0, 3].Basis;

                    bone1_x = bone1.xBasis;
                    bone0_y = bone0.yBasis;
                    bone1_y = bone1.yBasis;
                    bone2_y = bone2.yBasis;
                    bone3_y = bone3.yBasis;

                    bone0_z = bone0.zBasis;
                    bone1_z = bone1.zBasis;
                    bone2_z = bone2.zBasis;
                    bone3_z = bone3.zBasis;

                    b1_x_norm = bone1_x.Normalized;

                    b0_y_norm = bone0_y.Normalized;
                    b1_y_norm = bone1_y.Normalized;
                    b2_y_norm = bone2_y.Normalized;
                    b3_y_norm = bone3_y.Normalized;

                    b0_z_norm = bone0_z.Normalized;
                    b1_z_norm = bone1_z.Normalized;
                    b2_z_norm = bone2_z.Normalized;
                    b3_z_norm = bone3_z.Normalized;

                    //richTextBox.AppendText("get value" + Environment.NewLine);

                    double dotProduct_x_0 = b1_z_norm.Dot(bone21_x_reference);
                    double dotProduct_y_0 = b0_y_norm.Dot(b1_y_norm);
                    double dotProduct_y_1 = b1_y_norm.Dot(b2_y_norm);
                    double dotProduct_z_0 = b0_z_norm.Dot(b1_z_norm);
                    double dotProduct_z_1 = b1_z_norm.Dot(b2_z_norm);

                    last_degree_x_f011 = Math.Acos(dotProduct_x_0) * (180 / Math.PI);
                    last_degree_y_f01 = Math.Acos(dotProduct_y_0) * (180 / Math.PI);
                    last_degree_y_f02 = Math.Acos(dotProduct_y_1) * (180 / Math.PI);
                    last_degree_z_f01 = Math.Acos(dotProduct_z_0) * (180 / Math.PI);
                    last_degree_z_f02 = Math.Acos(dotProduct_z_1) * (180 / Math.PI);

                    command_degree_f01 = (int)last_degree_x_f011;
                    command_degree_f02 = 180 - (int)(0.6 * last_degree_y_f01 + 0.4 * last_degree_z_f01);
                    command_degree_f03 = 180 - (int)(0.6 * last_degree_y_f02 + 0.4 * last_degree_z_f02);


                    textBox1.Text = command_degree_f02.ToString();
                    textBox5.Text = command_degree_f01.ToString();
                    textBox7.Text = command_degree_f03.ToString();

                }
                else
                {
                    //finger 0 or index finger degree
                    bone0 = bone_object[0, 0].Basis;
                    bone1 = bone_object[0, 1].Basis;
                    bone2 = bone_object[0, 2].Basis;
                    bone3 = bone_object[0, 3].Basis;

                    bone1_x = bone1.xBasis;

                    bone0_y = bone0.yBasis;
                    bone1_y = bone1.yBasis;
                    bone2_y = bone2.yBasis;
                    bone3_y = bone3.yBasis;

                    bone0_z = bone0.zBasis;
                    bone1_z = bone1.zBasis;
                    bone2_z = bone2.zBasis;
                    bone3_z = bone3.zBasis;

                    b1_x_norm = bone1_x.Normalized;

                    b0_y_norm = bone0_y.Normalized;
                    b1_y_norm = bone1_y.Normalized;
                    b2_y_norm = bone2_y.Normalized;
                    b3_y_norm = bone3_y.Normalized;

                    b0_z_norm = bone0_z.Normalized;
                    b1_z_norm = bone1_z.Normalized;
                    b2_z_norm = bone2_z.Normalized;
                    b3_z_norm = bone3_z.Normalized;

                    //richTextBox.AppendText("get value" + Environment.NewLine);
                    double dotProduct_x_0 = b1_z_norm.Dot(bone11_x_reference);
                    double dotProduct_x_1 = b2_z_norm.Dot(bone11_x_reference);
                    double dotProduct_x_2 = b3_z_norm.Dot(bone11_x_reference);

                    double dotProduct_y_0 = b0_y_norm.Dot(b1_y_norm);
                    double dotProduct_y_1 = b1_y_norm.Dot(b2_y_norm);
                    double dotProduct_z_0 = b0_z_norm.Dot(b1_z_norm);
                    double dotProduct_z_1 = b1_z_norm.Dot(b2_z_norm);

                    last_degree_x_f011 = Math.Acos(dotProduct_x_0) * (180 / Math.PI);
                    last_degree_x_f012 = Math.Acos(dotProduct_x_1) * (180 / Math.PI);
                    last_degree_x_f013 = Math.Acos(dotProduct_x_2) * (180 / Math.PI);

                    last_degree_y_f01 = Math.Acos(dotProduct_y_0) * (180 / Math.PI);
                    last_degree_y_f02 = Math.Acos(dotProduct_y_1) * (180 / Math.PI);
                    last_degree_z_f01 = Math.Acos(dotProduct_z_0) * (180 / Math.PI);
                    last_degree_z_f02 = Math.Acos(dotProduct_z_1) * (180 / Math.PI);

                    command_degree_f01 = (int) (0.3 * last_degree_x_f011 + 0.3 * last_degree_x_f012 + 0.4 * last_degree_x_f013);

                    if (command_degree_f01 > 90)
                        command_degree_f01 = command_degree_f01 + 5;
                    else
                        command_degree_f01 = command_degree_f01 - 5;

                    command_degree_f02 = 180 - (int) (0.6 * last_degree_y_f01 + 0.4 * last_degree_z_f01);
                    command_degree_f03 = 180 - (int) (0.6 * last_degree_y_f02 + 0.4 * last_degree_z_f02);
                    //Vector speed = finger_object[0].TipVelocity;
                    //float speed_1 = speed.Magnitude;
                    //richTextBox.AppendText(bone_object[0, 0].Type + Environment.NewLine);
                    //richTextBox.AppendText(bone_object[0, 1].Type + Environment.NewLine);
                    //richTextBox.AppendText(bone_object[0, 2].Type + Environment.NewLine);
                    //richTextBox.AppendText(bone_object[0, 3].Type + Environment.NewLine);
                    textBox1.Text = command_degree_f02.ToString();
                    textBox5.Text = command_degree_f01.ToString();
                    textBox7.Text = command_degree_f03.ToString();
                }
               
            }

            if (finger_object[1] != null)
            {
                if (initial_f2)
                {
                    initial_f2 = false;
                 
                    //finger 0 or index finger degree
                    bone0 = bone_object[1, 0].Basis;
                    bone1 = bone_object[1, 1].Basis;
                    bone2 = bone_object[1, 2].Basis;
                    bone3 = bone_object[1, 3].Basis;

                    bone1_x = bone1.xBasis;

                    bone0_y = bone0.yBasis;
                    bone1_y = bone1.yBasis;
                    bone2_y = bone2.yBasis;
                    bone3_y = bone3.yBasis;

                    bone0_z = bone0.zBasis;
                    bone1_z = bone1.zBasis;
                    bone2_z = bone2.zBasis;
                    bone3_z = bone3.zBasis;

                    b1_x_norm = bone1_x.Normalized;

                    b0_y_norm = bone0_y.Normalized;
                    b1_y_norm = bone1_y.Normalized;
                    b2_y_norm = bone2_y.Normalized;
                    b3_y_norm = bone3_y.Normalized;

                    b0_z_norm = bone0_z.Normalized;
                    b1_z_norm = bone1_z.Normalized;
                    b2_z_norm = bone2_z.Normalized;
                    b3_z_norm = bone3_z.Normalized;

                    //richTextBox.AppendText("get value" + Environment.NewLine);
                    double dotProduct_x_0 = b1_z_norm.Dot(bone21_x_reference);
                    double dotProduct_x_1 = b2_z_norm.Dot(bone21_x_reference);
                    double dotProduct_x_2 = b3_z_norm.Dot(bone21_x_reference);

                    double dotProduct_y_0 = b0_y_norm.Dot(b1_y_norm);
                    double dotProduct_y_1 = b1_y_norm.Dot(b2_y_norm);
                    double dotProduct_z_0 = b0_z_norm.Dot(b1_z_norm);
                    double dotProduct_z_1 = b1_z_norm.Dot(b2_z_norm);

                    last_degree_x_f111 = Math.Acos(dotProduct_x_0) * (180 / Math.PI);
                    last_degree_x_f112 = Math.Acos(dotProduct_x_1) * (180 / Math.PI);
                    last_degree_x_f113 = Math.Acos(dotProduct_x_2) * (180 / Math.PI);

                    last_degree_y_f11 = Math.Acos(dotProduct_y_0) * (180 / Math.PI);
                    last_degree_y_f12 = Math.Acos(dotProduct_y_1) * (180 / Math.PI);
                    last_degree_z_f11 = Math.Acos(dotProduct_z_0) * (180 / Math.PI);
                    last_degree_z_f12 = Math.Acos(dotProduct_z_1) * (180 / Math.PI);

                    command_degree_f11 = (int)(0.3 * last_degree_x_f111 + 0.3 * last_degree_x_f112 + 0.4 * last_degree_x_f113);
                    if (command_degree_f11 > 90)
                        command_degree_f11 = command_degree_f11 + 5;
                    else
                        command_degree_f11 = command_degree_f11 - 5;

                    command_degree_f12 = 180 - (int)(0.6 * last_degree_y_f11 + 0.4 * last_degree_z_f11);
                    command_degree_f13 = 180 - (int)(0.6 * last_degree_y_f12 + 0.4 * last_degree_z_f12);

                    textBox2.Text = command_degree_f12.ToString();
                    textBox6.Text = command_degree_f11.ToString();
                    textBox8.Text = command_degree_f13.ToString();
                }

                else
                {
                    //finger 0 or index finger degree
                    bone0 = bone_object[1, 0].Basis;
                    bone1 = bone_object[1, 1].Basis;
                    bone2 = bone_object[1, 2].Basis;
                    bone3 = bone_object[1, 3].Basis;

                    bone1_x = bone1.xBasis;

                    bone0_y = bone0.yBasis;
                    bone1_y = bone1.yBasis;
                    bone2_y = bone2.yBasis;
                    bone3_y = bone3.yBasis;

                    bone0_z = bone0.zBasis;
                    bone1_z = bone1.zBasis;
                    bone2_z = bone2.zBasis;
                    bone3_z = bone3.zBasis;

                    b1_x_norm = bone1_x.Normalized;

                    b0_y_norm = bone0_y.Normalized;
                    b1_y_norm = bone1_y.Normalized;
                    b2_y_norm = bone2_y.Normalized;
                    b3_y_norm = bone3_y.Normalized;

                    b0_z_norm = bone0_z.Normalized;
                    b1_z_norm = bone1_z.Normalized;
                    b2_z_norm = bone2_z.Normalized;
                    b3_z_norm = bone3_z.Normalized;

                    //richTextBox.AppendText("get value" + Environment.NewLine);
                    double dotProduct_x_0 = b1_z_norm.Dot(bone21_x_reference);
                    double dotProduct_x_1 = b2_z_norm.Dot(bone21_x_reference);
                    double dotProduct_x_2 = b3_z_norm.Dot(bone21_x_reference);

                    double dotProduct_y_0 = b0_y_norm.Dot(b1_y_norm);
                    double dotProduct_y_1 = b1_y_norm.Dot(b2_y_norm);
                    double dotProduct_z_0 = b0_z_norm.Dot(b1_z_norm);
                    double dotProduct_z_1 = b1_z_norm.Dot(b2_z_norm);

                    last_degree_x_f111 = Math.Acos(dotProduct_x_0) * (180 / Math.PI);
                    last_degree_x_f112 = Math.Acos(dotProduct_x_1) * (180 / Math.PI);
                    last_degree_x_f113 = Math.Acos(dotProduct_x_2) * (180 / Math.PI);

                    last_degree_y_f11 = Math.Acos(dotProduct_y_0) * (180 / Math.PI);
                    last_degree_y_f12 = Math.Acos(dotProduct_y_1) * (180 / Math.PI);
                    last_degree_z_f11 = Math.Acos(dotProduct_z_0) * (180 / Math.PI);
                    last_degree_z_f12 = Math.Acos(dotProduct_z_1) * (180 / Math.PI);

                    command_degree_f11 = (int)(0.3 * last_degree_x_f111 + 0.3 * last_degree_x_f112 + 0.4 * last_degree_x_f113);
                    if (command_degree_f11 > 90)
                        command_degree_f11 = command_degree_f11 + 5;
                    else
                        command_degree_f11 = command_degree_f11 - 5;

                    command_degree_f12 = 180 - (int)(0.6 * last_degree_y_f11 + 0.4 * last_degree_z_f11);
                    command_degree_f13 = 180 - (int)(0.6 * last_degree_y_f12 + 0.4 * last_degree_z_f12);

                    textBox6.Text = command_degree_f11.ToString();
                    textBox2.Text = command_degree_f12.ToString();
                    textBox8.Text = command_degree_f13.ToString();
                }
            }

            if (finger_object[2] != null)
            {
                if (initial_f3)
                {
                    initial_f3 = false;
                    bone0 = bone_object[2, 0].Basis;
                    bone1 = bone_object[2, 1].Basis;
                    bone2 = bone_object[2, 2].Basis;
                    bone3 = bone_object[2, 3].Basis;

                    bone1_x = bone1.xBasis;

                    bone0_y = bone0.yBasis;
                    bone1_y = bone1.yBasis;
                    bone2_y = bone2.yBasis;
                    bone3_y = bone3.yBasis;

                    bone0_z = bone0.zBasis;
                    bone1_z = bone1.zBasis;
                    bone2_z = bone2.zBasis;
                    bone3_z = bone3.zBasis;

                    b1_x_norm = bone1_x.Normalized;

                    b0_y_norm = bone0_y.Normalized;
                    b1_y_norm = bone1_y.Normalized;
                    b2_y_norm = bone2_y.Normalized;
                    b3_y_norm = bone3_y.Normalized;

                    b0_z_norm = bone0_z.Normalized;
                    b1_z_norm = bone1_z.Normalized;
                    b2_z_norm = bone2_z.Normalized;
                    b3_z_norm = bone3_z.Normalized;

                    //richTextBox.AppendText("get value" + Environment.NewLine);
                    double dotProduct_x_0 = b1_z_norm.Dot(bone31_x_reference);
                    double dotProduct_x_1 = b2_z_norm.Dot(bone31_x_reference);
                    double dotProduct_x_2 = b3_z_norm.Dot(bone31_x_reference);

                    double dotProduct_y_0 = b0_y_norm.Dot(b1_y_norm);
                    double dotProduct_y_1 = b1_y_norm.Dot(b2_y_norm);
                    double dotProduct_z_0 = b0_z_norm.Dot(b1_z_norm);
                    double dotProduct_z_1 = b1_z_norm.Dot(b2_z_norm);

                    last_degree_x_f211 = Math.Acos(dotProduct_x_0) * (180 / Math.PI);
                    last_degree_x_f212 = Math.Acos(dotProduct_x_1) * (180 / Math.PI);
                    last_degree_x_f213 = Math.Acos(dotProduct_x_2) * (180 / Math.PI);

                    last_degree_y_f21 = Math.Acos(dotProduct_y_0) * (180 / Math.PI);
                    last_degree_y_f22 = Math.Acos(dotProduct_y_1) * (180 / Math.PI);
                    last_degree_z_f21 = Math.Acos(dotProduct_z_0) * (180 / Math.PI);
                    last_degree_z_f22 = Math.Acos(dotProduct_z_1) * (180 / Math.PI);

                    command_degree_f21 = (int)(0.3 * last_degree_x_f111 + 0.3 * last_degree_x_f112 + 0.4 * last_degree_x_f113);
                    if (command_degree_f21 > 90)
                        command_degree_f21 = command_degree_f21 + 5;
                    else
                        command_degree_f21 = command_degree_f21 - 5;

                    command_degree_f22 = 180 - (int)(0.5 * last_degree_z_f21 + 0.5 * last_degree_y_f21);
                    command_degree_f23 = 180 - (int)(0.5 * last_degree_z_f22 + 0.5 * last_degree_y_f22);

                    textBox3.Text = command_degree_f22.ToString();
                    textBox10.Text = command_degree_f22.ToString();
                    textBox9.Text = command_degree_f23.ToString();

                }
                else
                {
                    bone0 = bone_object[2, 0].Basis;
                    bone1 = bone_object[2, 1].Basis;
                    bone2 = bone_object[2, 2].Basis;
                    bone3 = bone_object[2, 3].Basis;

                    bone1_x = bone1.xBasis;

                    bone0_y = bone0.yBasis;
                    bone1_y = bone1.yBasis;
                    bone2_y = bone2.yBasis;
                    bone3_y = bone3.yBasis;

                    bone0_z = bone0.zBasis;
                    bone1_z = bone1.zBasis;
                    bone2_z = bone2.zBasis;
                    bone3_z = bone3.zBasis;

                    b1_x_norm = bone1_x.Normalized;

                    b0_y_norm = bone0_y.Normalized;
                    b1_y_norm = bone1_y.Normalized;
                    b2_y_norm = bone2_y.Normalized;
                    b3_y_norm = bone3_y.Normalized;

                    b0_z_norm = bone0_z.Normalized;
                    b1_z_norm = bone1_z.Normalized;
                    b2_z_norm = bone2_z.Normalized;
                    b3_z_norm = bone3_z.Normalized;

                    //richTextBox.AppendText("get value" + Environment.NewLine);
                    double dotProduct_x_0 = b1_z_norm.Dot(bone31_x_reference);
                    double dotProduct_x_1 = b2_z_norm.Dot(bone31_x_reference);
                    double dotProduct_x_2 = b3_z_norm.Dot(bone31_x_reference);

                    double dotProduct_y_0 = b0_y_norm.Dot(b1_y_norm);
                    double dotProduct_y_1 = b1_y_norm.Dot(b2_y_norm);
                    double dotProduct_z_0 = b0_z_norm.Dot(b1_z_norm);
                    double dotProduct_z_1 = b1_z_norm.Dot(b2_z_norm);

                    last_degree_x_f211 = Math.Acos(dotProduct_x_0) * (180 / Math.PI);
                    last_degree_x_f212 = Math.Acos(dotProduct_x_1) * (180 / Math.PI);
                    last_degree_x_f213 = Math.Acos(dotProduct_x_2) * (180 / Math.PI);

                    last_degree_y_f21 = Math.Acos(dotProduct_y_0) * (180 / Math.PI);
                    last_degree_y_f22 = Math.Acos(dotProduct_y_1) * (180 / Math.PI);
                    last_degree_z_f21 = Math.Acos(dotProduct_z_0) * (180 / Math.PI);
                    last_degree_z_f22 = Math.Acos(dotProduct_z_1) * (180 / Math.PI);

                    command_degree_f21 = (int)(0.3 * last_degree_x_f211 + 0.3 * last_degree_x_f212 + 0.4 * last_degree_x_f213);
                    if (command_degree_f21 > 90)
                        command_degree_f21 = command_degree_f21 + 5;
                    else
                        command_degree_f21 = command_degree_f21 - 5;

                    command_degree_f22 = 180 - (int)(0.5 * last_degree_z_f21 + 0.5 * last_degree_y_f21);
                    command_degree_f23 = 180 - (int)(0.5 * last_degree_z_f22 + 0.5 * last_degree_y_f22);
                   
                    textBox3.Text = command_degree_f22.ToString();
                    textBox10.Text = command_degree_f21.ToString();
                    textBox9.Text = command_degree_f23.ToString();
                }
            }
        }

        public void mystoi()
        {
            track_resolution = Int64.Parse(textBox13.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mystoi();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void faceButton_Click(object sender,EventArgs e)
        {
            faceRecProcess = Process.Start("MultiFaceRec.exe");
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
